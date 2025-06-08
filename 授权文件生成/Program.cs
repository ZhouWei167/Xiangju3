using System;
using System.Collections.Generic;
using System.Management;
using Reactor = License;

namespace LicenseGeneratorTest
{
    class Program
    {
        static string GetLocalHardwareID()
        {
            try
            {
                return Reactor.Status.GetHardwareID(true, true, true, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取本机硬件ID失败: {ex.Message}");
                return null;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Zircon Server 授权文件生成工具");
                Console.WriteLine("==============================");

                // 获取本机硬件ID
                string defaultHardwareId = GetLocalHardwareID();
                Console.Write($"请输入硬件ID（回车使用本机ID: {defaultHardwareId}）：");
                string hardwareId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(hardwareId))
                {
                    hardwareId = defaultHardwareId;
                }
                while (string.IsNullOrWhiteSpace(hardwareId))
                {
                    Console.Write("硬件ID不能为空，请重新输入: ");
                    hardwareId = Console.ReadLine();
                }

                Console.Write("请输入过期时间(yyyy-MM-dd): ");
                DateTime expirationDate;
                while (!DateTime.TryParse(Console.ReadLine(), out expirationDate))
                {
                    Console.Write("格式错误，请重新输入过期时间(yyyy-MM-dd): ");
                }

                Console.Write("请输入最大使用次数: ");
                int numberOfUses;
                while (!int.TryParse(Console.ReadLine(), out numberOfUses) || numberOfUses <= 0)
                {
                    Console.Write("必须为正整数，请重新输入最大使用次数: ");
                }

                Console.Write("请输入最大实例数: ");
                int numberOfInstances;
                while (!int.TryParse(Console.ReadLine(), out numberOfInstances) || numberOfInstances <= 0)
                {
                    Console.Write("必须为正整数，请重新输入最大实例数: ");
                }

                Console.Write("是否启用试用锁定(1=是,0=否): ");
                bool enableEvaluationLock = Console.ReadLine() == "1";

                Console.Write("是否启用过期时间锁定(1=是,0=否): ");
                bool enableExpirationDateLock = Console.ReadLine() == "1";

                Console.Write("是否启用使用次数锁定(1=是,0=否): ");
                bool enableNumberOfUsesLock = Console.ReadLine() == "1";

                Console.Write("是否启用实例数锁定(1=是,0=否): ");
                bool enableNumberOfInstancesLock = Console.ReadLine() == "1";

                Console.Write("是否启用硬件锁定(1=是,0=否): ");
                bool enableHardwareLock = Console.ReadLine() == "1";

                Console.Write("请输入试用期(天数): ");
                int evaluationTime;
                while (!int.TryParse(Console.ReadLine(), out evaluationTime) || evaluationTime < 0)
                {
                    Console.Write("必须为非负整数，请重新输入试用期(天数): ");
                }

                Console.Write("请输入试用类型(0=按天,1=按次数,2=按时长): ");
                int evaTypeInt;
                while (!int.TryParse(Console.ReadLine(), out evaTypeInt) || evaTypeInt < 0 || evaTypeInt > 2)
                {
                    Console.Write("请输入0、1或2: ");
                }
                var evaluationType = (Reactor.EvaluationType)evaTypeInt;

                Console.Write("请输入版本号: ");
                string version = Console.ReadLine();
                Console.Write("请输入客户名称: ");
                string customerName = Console.ReadLine();
                Console.Write("请输入产品名称: ");
                string productName = Console.ReadLine();
                DateTime issueDate = DateTime.Now;

                // 可选自定义数据
                var customData = new Dictionary<string, string>();
                Console.Write("是否添加自定义数据(1=是,0=否): ");
                if (Console.ReadLine() == "1")
                {
                    while (true)
                    {
                        Console.Write("请输入键(留空结束): ");
                        string key = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(key)) break;
                        Console.Write("请输入值: ");
                        string value = Console.ReadLine();
                        customData[key] = value;
                    }
                }

                // 打印所有参数供用户确认
                Console.WriteLine("\n即将生成授权文件，参数如下：");
                Console.WriteLine($"硬件ID: {hardwareId}");
                Console.WriteLine($"过期时间: {expirationDate}");
                Console.WriteLine($"最大使用次数: {numberOfUses}");
                Console.WriteLine($"最大实例数: {numberOfInstances}");
                Console.WriteLine($"启用试用锁定: {enableEvaluationLock}");
                Console.WriteLine($"启用过期时间锁定: {enableExpirationDateLock}");
                Console.WriteLine($"启用使用次数锁定: {enableNumberOfUsesLock}");
                Console.WriteLine($"启用实例数锁定: {enableNumberOfInstancesLock}");
                Console.WriteLine($"启用硬件锁定: {enableHardwareLock}");
                Console.WriteLine($"试用期: {evaluationTime}");
                Console.WriteLine($"试用类型: {evaluationType}");
                Console.WriteLine($"版本号: {version}");
                Console.WriteLine($"客户名称: {customerName}");
                Console.WriteLine($"产品名称: {productName}");
                Console.WriteLine($"签发日期: {issueDate}");
                if (customData.Count > 0)
                {
                    Console.WriteLine("自定义数据:");
                    foreach (var kv in customData)
                        Console.WriteLine($"  {kv.Key}: {kv.Value}");
                }
                Console.Write("确认生成授权文件？(y/n): ");
                if (!Console.ReadLine().Trim().ToLower().StartsWith("y"))
                {
                    Console.WriteLine("已取消。");
                    return;
                }

                // 创建许可证信息
                var licenseInfo = new Reactor.LicenseGenerator.LicenseInfo
                {
                    HardwareID = hardwareId,
                    ExpirationDate = expirationDate,
                    NumberOfUses = numberOfUses,
                    NumberOfInstances = numberOfInstances,
                    EnableEvaluationLock = enableEvaluationLock,
                    EnableExpirationDateLock = enableExpirationDateLock,
                    EnableNumberOfUsesLock = enableNumberOfUsesLock,
                    EnableNumberOfInstancesLock = enableNumberOfInstancesLock,
                    EnableHardwareLock = enableHardwareLock,
                    EvaluationTime = evaluationTime,
                    EvaluationType = evaluationType,
                    Version = version,
                    CustomerName = customerName,
                    ProductName = productName,
                    IssueDate = issueDate,
                    CustomData = customData
                };

                // 验证授权信息
                try
                {
                    licenseInfo.Validate();
                    Console.WriteLine("授权信息验证通过");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"授权信息验证失败: {ex.Message}");
                    return;
                }

                // 生成授权文件
                string outputPath = "license.license";
                Reactor.LicenseGenerator.GenerateLicenseFile(licenseInfo, outputPath);
                Console.WriteLine($"授权文件已生成: {outputPath}");

                // 验证许可证文件
                var loadedLicense = Reactor.LicenseGenerator.LoadLicenseFile(outputPath);
                Console.WriteLine("\n授权文件内容:");
                Console.WriteLine($"硬件ID: {loadedLicense.HardwareID}");
                Console.WriteLine($"过期时间: {loadedLicense.ExpirationDate}");
                Console.WriteLine($"使用次数: {loadedLicense.NumberOfUses}");
                Console.WriteLine($"实例数量: {loadedLicense.NumberOfInstances}");
                Console.WriteLine($"版本: {loadedLicense.Version}");
                Console.WriteLine($"客户名称: {loadedLicense.CustomerName}");
                Console.WriteLine($"产品名称: {loadedLicense.ProductName}");
                Console.WriteLine($"签发日期: {loadedLicense.IssueDate}");
                Console.WriteLine($"试用期: {loadedLicense.EvaluationTime}天");
                Console.WriteLine($"试用类型: {loadedLicense.EvaluationType}");
                if (loadedLicense.CustomData != null && loadedLicense.CustomData.Count > 0)
                {
                    Console.WriteLine("自定义数据:");
                    foreach (var item in loadedLicense.CustomData)
                    {
                        Console.WriteLine($"  {item.Key}: {item.Value}");
                    }
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