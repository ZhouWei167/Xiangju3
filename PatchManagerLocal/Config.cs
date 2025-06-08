using Library;

namespace PatchManager
{
    /// <summary>
    /// 配置路径
    /// </summary>
    [ConfigPath(@".\PatchManager.ini")]
    public static class Config
    {
        /// <summary>
        /// 端游干净的补丁目录
        /// </summary>
        [ConfigSection("Patcher")]
        public static string PcCleanClientPath { get; set; } = @"C:\Server\Clients\Patch Files\";
        /// <summary>
        /// 端游补丁保存目录
        /// </summary>
        public static string PcPatchSavePath { get; set; } = @"C:\Server\Patches\PC\";
        /// <summary>
        /// 手游干净的补丁目录
        /// </summary>
        [ConfigSection("MobPatcher")]
        public static string MobCleanClientPath { get; set; } = @"C:\Server\MobClients\Patch Files\";
        /// <summary>
        /// 手游补丁保存目录
        /// </summary>
        public static string MobPatchSavePath { get; set; } = @"C:\Server\Patches\Mobile\";
    }
}
