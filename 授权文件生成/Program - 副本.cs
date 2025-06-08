using System;
using System.Collections.Generic;

namespace LicenseGeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Zircon Server 授权文件生成工具");
                Console.WriteLine("==============================");
                
                // 获取当前机器的硬件ID
                string hardwareId = License.Status.GetHardwareID(true, true, true, true);
                Console.WriteLine($"当前机器硬件ID: {hardwareId}");

                // 创建许可证信息
                var licenseInfo = new License.LicenseGenerator.LicenseInfo
                {
                    HardwareID = hardwareId,
                    ExpirationDate = DateTime.Now.AddYears(1),
                    NumberOfUses = 1000,
                    NumberOfInstances = 1,
                    EnableEvaluationLock = true,
                    EnableExpirationDateLock = true,
                    EnableNumberOfUsesLock = true,
                    EnableNumberOfInstancesLock = true,
                    EnableHardwareLock = true,
                    EvaluationTime = 30,
                    EvaluationType = License.EvaluationType.Trial_Days,
                    Version = "1.0.0",
                    CustomerName = "测试客户",
                    ProductName = "Zircon Server",
                    IssueDate = DateTime.Now,
                    CustomData = new Dictionary<string, string>
                    {
                        { "Company", "测试公司" },
                        { "Department", "研发部" }
                    }
                };

                // 生成许可证文件
                string licensePath = $"{hardwareId}.license";
                License.LicenseGenerator.GenerateLicenseFile(licenseInfo, licensePath);
                Console.WriteLine($"\n许可证文件已生成: {licensePath}");

                // 验证许可证文件
                var loadedLicense = License.LicenseGenerator.LoadLicenseFile(licensePath);
                Console.WriteLine("\n许可证信息:");
                Console.WriteLine($"硬件ID: {loadedLicense.HardwareID}");
                Console.WriteLine($"过期时间: {loadedLicense.ExpirationDate}");
                Console.WriteLine($"使用次数: {loadedLicense.NumberOfUses}");
                Console.WriteLine($"实例数量: {loadedLicense.NumberOfInstances}");
                Console.WriteLine($"版本: {loadedLicense.Version}");
                Console.WriteLine($"客户名称: {loadedLicense.CustomerName}");
                Console.WriteLine($"产品名称: {loadedLicense.ProductName}");
                Console.WriteLine($"签发日期: {loadedLicense.IssueDate}");
                Console.WriteLine($"试用期: {loadedLicense.EvaluationTime}天");
                Console.WriteLine("\n自定义数据:");
                foreach (var item in loadedLicense.CustomData)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }

                Console.WriteLine("\n按任意键退出...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
                Console.WriteLine("\n按任意键退出...");
                Console.ReadKey();
            }
        }
    }
} 