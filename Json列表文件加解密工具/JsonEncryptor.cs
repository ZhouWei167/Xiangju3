using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly string KEY = "YourSecretKey123"; // 16字节密钥
    private static readonly string IV = "YourIVKey1234567";  // 16字节IV
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== JSON文件加密解密工具 ===");
            Console.WriteLine("1. 加密JSON文件");
            Console.WriteLine("2. 解密JSON文件");
            Console.WriteLine("3. 从网络获取并解密JSON");
            Console.WriteLine("4. 退出");
            Console.Write("请选择操作 (1-4): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EncryptJsonFile();
                    break;
                case "2":
                    DecryptJsonFile();
                    break;
                case "3":
                    await DownloadAndDecryptJson();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("无效的选择，请重试。");
                    break;
            }
        }
    }

    static async Task DownloadAndDecryptJson()
    {
        try
        {
            Console.Write("请输入网络JSON文件地址: ");
            string url = Console.ReadLine()?.Trim('"', ' ');

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("URL不能为空！");
                return;
            }

            Console.WriteLine("正在从网络获取文件...");
            client.Timeout = TimeSpan.FromSeconds(10); // 设置10秒超时
            string encryptedContent = await client.GetStringAsync(url);

            Console.WriteLine("正在解密文件内容...");
            string decryptedContent = DecryptString(encryptedContent);

            // 保存解密后的文件
            string outputPath = "decrypted_network.json";
            await File.WriteAllTextAsync(outputPath, decryptedContent);

            Console.WriteLine($"解密完成！解密后的文件保存在: {outputPath}");
            Console.WriteLine("解密后的内容：");
            Console.WriteLine(decryptedContent);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"网络请求失败: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解密过程中发生错误: {ex.Message}");
        }
    }

    static void EncryptJsonFile()
    {
        try
        {
            Console.Write("请输入要加密的JSON文件路径（直接输入文件名即可）: ");
            string inputPath = Console.ReadLine()?.Trim('"', ' '); // 移除引号和空格
            
            if (string.IsNullOrEmpty(inputPath))
            {
                Console.WriteLine("路径不能为空！");
                return;
            }

            // 获取当前目录
            string currentDirectory = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currentDirectory, inputPath);

            // 检查文件是否存在
            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"文件不存在！");
                Console.WriteLine($"当前目录: {currentDirectory}");
                Console.WriteLine($"尝试访问的完整路径: {fullPath}");
                Console.WriteLine("请确认文件名是否正确。");
                return;
            }

            string jsonContent = File.ReadAllText(fullPath);
            string encryptedContent = EncryptString(jsonContent);
            
            string outputPath = Path.ChangeExtension(fullPath, ".encrypted.json");
            File.WriteAllText(outputPath, encryptedContent);
            
            Console.WriteLine($"加密完成！加密后的文件保存在: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加密过程中发生错误: {ex.Message}");
        }
    }

    static void DecryptJsonFile()
    {
        try
        {
            Console.Write("请输入要解密的JSON文件路径（直接输入文件名即可）: ");
            string inputPath = Console.ReadLine()?.Trim('"', ' '); // 移除引号和空格
            
            if (string.IsNullOrEmpty(inputPath))
            {
                Console.WriteLine("路径不能为空！");
                return;
            }

            // 获取当前目录
            string currentDirectory = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currentDirectory, inputPath);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"文件不存在！");
                Console.WriteLine($"当前目录: {currentDirectory}");
                Console.WriteLine($"尝试访问的完整路径: {fullPath}");
                Console.WriteLine("请确认文件名是否正确。");
                return;
            }

            string encryptedContent = File.ReadAllText(fullPath);
            string decryptedContent = DecryptString(encryptedContent);
            
            string outputPath = Path.ChangeExtension(fullPath, ".decrypted.json");
            File.WriteAllText(outputPath, decryptedContent);
            
            Console.WriteLine($"解密完成！解密后的文件保存在: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解密过程中发生错误: {ex.Message}");
        }
    }

    static string EncryptString(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(KEY);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    static string DecryptString(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(KEY);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
} 