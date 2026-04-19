using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ProcessFileLog5GB
{
    public class Read
    {
        public static async Task Main(string[] agrs)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string filePath = "file5gb.txt";
            var watch = new Stopwatch();
            watch.Start();
            Read countwords = new Read();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File chưa tồn tại đang tạo file  ...");
                await WriteFile.GenerateRandomWordsFileAsync(filePath, 5000);
            }
            else
            {
                Console.WriteLine("File đã tồn tại ...");
            }
            Console.WriteLine("Đang xử lí.....");
            await countwords.CountWordsAsync(filePath);
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");

        }
        public async Task  CountWordsAsync(string filePath)
        {
            var wordCounts = new ConcurrentDictionary<string, int>();
            long totalWords = 0;
            Parallel.ForEach(File.ReadLines(filePath), line =>
            {
                
                string[] words = line.Split(new[] { ' '}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    wordCounts.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
                }
                 Interlocked.Add(ref totalWords, words.Length);
            });
            
            Console.WriteLine("--- KẾT QUẢ TẦN SUẤT TỪ ---");
            var sortedWordCounts = wordCounts.OrderByDescending(kvp => kvp.Value);

            foreach (var kvp in sortedWordCounts)
            {
                Console.WriteLine($"Từ '{kvp.Key}': {kvp.Value:N0} lần");
            }
            Console.WriteLine($"Tổng số từ: {totalWords:N0}");

        }

    }
}