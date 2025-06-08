using Library;
using Library.Network;
using Microsoft.AspNetCore.Mvc;
using Server.Web.Controllers;
using System.Web;
using SFile = System.IO.File;
using Microsoft.AspNetCore.Authorization;
namespace ServerWeb.Server.Controllers
{
    [AuthorFilter]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GameController : ControllerBase
    {
        /// <summary>
        /// 用户名验证
        /// </summary>
        public static string Author { get; set; } = "BlackDragon";
        /// <summary>
        /// 移动端版本
        /// </summary>
        public static string AppVersion { get; set; } = "1.0.0";
        /// <summary>
        /// 库文件
        /// </summary>
        public static Dictionary<string, MirLibrary> LibraryList = new Dictionary<string, MirLibrary>();

        private readonly ILogger<GameController> _logger;
        IWebHostEnvironment _environment;
        public GameController(ILogger<GameController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _environment = env;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/file/{path}/{name}")]
        public IActionResult GetFile(string path, string name)
        {
            _logger.LogInformation($"请求下载文件: /api/file/{path}/{name}");
            
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name)) 
            {
                _logger.LogWarning($"文件路径或名称为空: path={path}, name={name}");
                return new NotFoundResult();
            }

            path = HttpUtility.UrlDecode(path).Replace("_", "/");
            name = HttpUtility.UrlDecode(name);
            
            // 规范化路径，适应不同操作系统
            string fullPath = Path.Combine(Globals.ResoucePath, path, name).Replace("\\", "/");
            
            _logger.LogInformation($"完整文件路径: {fullPath}");
            
            // 检查文件是否存在
            if (!SFile.Exists(fullPath)) 
            {
                _logger.LogWarning($"文件不存在: {fullPath}");
                
                // 尝试检查大小写不敏感的匹配
                try 
                {
                    string directoryPath = Path.GetDirectoryName(fullPath);
                    if (Directory.Exists(directoryPath))
                    {
                        string fileName = Path.GetFileName(fullPath);
                        string[] files = Directory.GetFiles(directoryPath);
                        
                        foreach (string file in files)
                        {
                            if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogInformation($"找到匹配文件(大小写不敏感): {file}");
                                fullPath = file;
                                goto FileFound;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"尝试查找匹配文件时出错: {ex.Message}");
                }
                
                return new NotFoundResult();
            }
            
            FileFound:
            // 读取文件信息
            FileInfo fileMetaInfo = new FileInfo(fullPath);
            _logger.LogInformation($"文件大小: {fileMetaInfo.Length} bytes");
            
            // 创建文件读取流
            FileStream fileStream = SFile.OpenRead(fullPath);
            return File(fileStream, "application/octet-stream");
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/sound/{name}/{index?}")]
        public MicroSound? Sound(string name, int? index)
        {
            _logger.LogInformation($"请求音频文件: /api/sound/{name}/{index}");
            
            index = index ?? 1;
            var path = Path.Combine(Globals.ResoucePath, "Sound", name + ".wav").Replace("\\", "/");
            
            _logger.LogInformation($"音频文件路径: {path}");
            
            if (!SFile.Exists(path))
            {
                _logger.LogWarning($"音频文件不存在: {path}");
                
                // 尝试检查大小写不敏感的匹配
                try 
                {
                    string directoryPath = Path.GetDirectoryName(path);
                    if (Directory.Exists(directoryPath))
                    {
                        string fileName = Path.GetFileName(path);
                        string[] files = Directory.GetFiles(directoryPath);
                        
                        foreach (string file in files)
                        {
                            if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogInformation($"找到匹配音频文件(大小写不敏感): {file}");
                                path = file;
                                goto FileFound;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"尝试查找匹配音频文件时出错: {ex.Message}");
                }
                
                return null;
            }
            
            FileFound:
            try
            {
                var bytes = SFile.ReadAllBytes(path);
                var len = 1024 * 1024;
                
                _logger.LogInformation($"音频文件大小: {bytes.Length} bytes, 分块索引: {index}, 总分块数: {(bytes.Length % len == 0 ? bytes.Length / len : (bytes.Length / len + 1))}");
                
                return new MicroSound
                {
                    Bytes = bytes.Skip(len * (index.Value - 1)).Take(len).ToArray(),
                    Max = bytes.Length % len == 0 ? bytes.Length / len : (bytes.Length / len + 1),
                    Current = index.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"处理音频文件时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 返回资源文件信息
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/libimage/{path}/{name}/{index}")]
        public IActionResult? LibraryImage(string name, int index, string path)
        {
            _logger.LogInformation($"请求库图像: /api/libimage/{path}/{name}/{index}");
            
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path)) 
            {
                _logger.LogWarning($"文件路径或名称为空: path={path}, name={name}");
                return new NotFoundResult();
            }

            path = HttpUtility.UrlDecode(path).Replace("_", "/");
            name = HttpUtility.UrlDecode(name);
            
            // 规范化路径，适应不同操作系统
            string fullPath = Path.Combine(Globals.ResoucePath, path, name).Replace("\\", "/");
            
            _logger.LogInformation($"完整文件路径: {fullPath}");
            
            // 检查文件是否存在
            if (!SFile.Exists(fullPath)) 
            {
                _logger.LogWarning($"文件不存在: {fullPath}");
                
                // 尝试检查大小写不敏感的匹配
                try 
                {
                    string directoryPath = Path.GetDirectoryName(fullPath);
                    if (Directory.Exists(directoryPath))
                    {
                        string fileName = Path.GetFileName(fullPath);
                        string[] files = Directory.GetFiles(directoryPath);
                        
                        foreach (string file in files)
                        {
                            if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogInformation($"找到匹配文件(大小写不敏感): {file}");
                                fullPath = file;
                                goto FileFound;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"尝试查找匹配文件时出错: {ex.Message}");
                }
                
                return new NotFoundResult();
            }
            
            FileFound:
            var key = Path.Combine(path, name).Replace("\\", "/");
            
            // 先尝试从缓存中获取
            if (LibraryList.TryGetValue(key, out var library))
            {
                if (library != null)
                {
                    var image = library.GetImage(index);
                    if (image != null && image.Length > 0)
                    {
                        _logger.LogInformation($"成功获取库图像: {key}, 索引: {index}, 大小: {image.Length} bytes");
                        return File(image, "application/octet-stream");
                    }
                    else
                    {
                        _logger.LogWarning($"库图像为空或长度为0: {key}, 索引: {index}");
                    }
                }
                else
                {
                    _logger.LogWarning($"库文件对象为null: {key}");
                }
            }
            else
            {
                _logger.LogWarning($"在LibraryList中找不到库文件: {key}，尝试加载");
                
                // 如果缓存中没有，尝试加载
                try 
                {
                    library = new MirLibrary(fullPath);
                    LibraryList[key] = library;
                    
                    var image = library.GetImage(index);
                    if (image != null && image.Length > 0)
                    {
                        _logger.LogInformation($"成功加载并获取库图像: {key}, 索引: {index}, 大小: {image.Length} bytes");
                        return File(image, "application/octet-stream");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"加载库文件失败: {fullPath}, 错误: {ex.Message}");
                }
            }
            
            return new NotFoundResult();
        }

        /// <summary>
        /// 返回头信息
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/libheader/{path}/{name}")]
        public IActionResult Library(string name, string path)
        {
            _logger.LogInformation($"请求库文件头: /api/libheader/{path}/{name}");
            
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path)) 
            {
                _logger.LogWarning($"文件路径或名称为空: path={path}, name={name}");
                return new NotFoundResult();
            }

            path = HttpUtility.UrlDecode(path).Replace("_", "/");
            name = HttpUtility.UrlDecode(name);
            
            // 规范化路径，适应不同操作系统
            string fullPath = Path.Combine(Globals.ResoucePath, path, name).Replace("\\", "/");
            
            _logger.LogInformation($"完整文件路径: {fullPath}");
            
            // 检查文件是否存在
            if (!SFile.Exists(fullPath)) 
            {
                _logger.LogWarning($"文件不存在: {fullPath}");
                
                // 尝试检查大小写不敏感的匹配
                try 
                {
                    string directoryPath = Path.GetDirectoryName(fullPath);
                    if (Directory.Exists(directoryPath))
                    {
                        string fileName = Path.GetFileName(fullPath);
                        string[] files = Directory.GetFiles(directoryPath);
                        
                        foreach (string file in files)
                        {
                            if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogInformation($"找到匹配文件(大小写不敏感): {file}");
                                fullPath = file;
                                goto FileFound;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"尝试查找匹配文件时出错: {ex.Message}");
                }
                
                return new NotFoundResult();
            }
            
            FileFound:
            var key = Path.Combine(path, name).Replace("\\", "/");
            
            // 先尝试从缓存中获取
            if (LibraryList.TryGetValue(key, out var library))
            {
                if (library != null)
                {
                    var libraryHeader = library.GetLibraryHeader();
                    if (libraryHeader != null && libraryHeader.Length > 0)
                    {
                        _logger.LogInformation($"成功获取库文件头: {key}, 大小: {libraryHeader.Length} bytes");
                        return File(libraryHeader, "application/octet-stream");
                    }
                    else
                    {
                        _logger.LogWarning($"库文件头为空或长度为0: {key}");
                    }
                }
                else
                {
                    _logger.LogWarning($"库文件对象为null: {key}");
                }
            }
            else
            {
                _logger.LogWarning($"在LibraryList中找不到库文件: {key}，尝试加载");
                
                // 如果缓存中没有，尝试加载
                try 
                {
                    library = new MirLibrary(fullPath);
                    LibraryList[key] = library;
                    
                    var libraryHeader = library.GetLibraryHeader();
                    if (libraryHeader != null && libraryHeader.Length > 0)
                    {
                        _logger.LogInformation($"成功加载并获取库文件头: {key}, 大小: {libraryHeader.Length} bytes");
                        return File(libraryHeader, "application/octet-stream");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"加载库文件失败: {fullPath}, 错误: {ex.Message}");
                }
            }
            
            return new NotFoundResult();
        }
    }
}
