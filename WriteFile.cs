using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class WriteFile
{
    // Tạo một "kho" chứa các từ vựng. Càng nhiều từ thì file càng đa dạng.
    private static readonly string[] WordPool = 
    { 
        "Lam", "HaUI", "INFO", "ERROR", "WARN", "DEBUG", 
        "System", "Database", "Connection", "Timeout", 
        "Success", "Failed", "User", "Data", "Process", "2026",
        "API", "Request", "Response", "Parallel", "Async"
    };

    public static async Task GenerateRandomWordsFileAsync(string filePath, int targetMB)
    {
        Console.WriteLine($"Bắt đầu tạo file {targetMB}");
        
        long targetBytes = targetMB * 1024L * 1024L;
        long currentBytes = 0;
        
        // Khởi tạo bộ sinh số ngẫu nhiên
        Random rand = new Random();

        // Mở luồng ghi với buffer 64KB để tối ưu tốc độ đĩa
        using (StreamWriter writer = new StreamWriter(filePath, append: false, Encoding.UTF8, bufferSize: 65536))
        {
            while (currentBytes < targetBytes)
            {
                StringBuilder sb = new StringBuilder();
                
                // Gom 5.000 dòng vào một khối (Chunk) trước khi ghi
                for (int i = 0; i < 5000; i++)
                {
                    // Mỗi dòng sẽ có ngẫu nhiên từ 8 đến 15 từ
                    int wordsPerLine = rand.Next(8, 16); 
                    
                    for (int w = 0; w < wordsPerLine; w++)
                    {
                        // Bốc ngẫu nhiên 1 từ trong WordPool và nối thêm dấu cách
                        string randomWord = WordPool[rand.Next(WordPool.Length)];
                        sb.Append(randomWord).Append(" ");
                    }
                    
                    // Thêm dấu xuống dòng khi hết 1 câu
                    sb.AppendLine(); 
                }

                // Xuất mẻ gạch vừa trộn xong ra thành 1 cục Text
                string chunkData = sb.ToString();
                
                // Ghi bất đồng bộ xuống ổ cứng
                await writer.WriteAsync(chunkData);
                
                // Tính toán dung lượng thực tế vừa ghi (tính bằng Byte)
                int bytesWritten = Encoding.UTF8.GetByteCount(chunkData);
                currentBytes += bytesWritten;

                // Cập nhật tiến độ trên màn hình
                Console.Write($"\rĐã ghi: {currentBytes / (1024 * 1024)} MB / {targetMB} MB");
            }
        }
        
        Console.WriteLine("\nHoàn tất quá trình ghi file ngẫu nhiên!");
    }
}