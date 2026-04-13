using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Diagnostics;
namespace ProcessFileLog5GB
{
    public class Read
    {
        public static async Task Main(string[] agrs)
        {
            
            string filePath = "file5gb.txt";
            var watch = new Stopwatch();
            watch.Start();
            Read countwords = new Read();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist, creating file...");
                await WriteFile.GenerateRandomWordsFileAsync(filePath, 5000);
            }
            else
            {
                Console.WriteLine("File already exists, skipping file creation...");
            }
            Console.WriteLine("Processing.....");
            long result = await countwords.CountWordsAsync(filePath);
            watch.Stop();
            Console.WriteLine("Total words: " + result);
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
            
        }
        public async Task<long> CountWordsAsync(string filePath)
        {
            long totalWords = 0;
            await Task.Run(() =>
            {
                var lines = File.ReadLines(filePath);
                Parallel.ForEach(lines, line =>
                {
                    int wordCount = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .Count();
                    Interlocked.Add(ref totalWords, wordCount);
                });
            });
            return totalWords;

        }
    }

}