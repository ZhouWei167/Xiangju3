using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using MirDB;
using Server.DBModels;
using Server.Envir;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Library;
using Library.SystemModels;
using System.Drawing;
using Microsoft.Extensions.DependencyModel;

namespace Server.Views
{
    /// <summary>
    /// 数据库迁移工具视图类
    /// 
    /// 此工具用于将一个Users.db数据库中的用户数据迁移到另一个Users.db数据库中。
    /// 
    /// 主要功能：
    /// 1. 将源Users.db中的所有账号、角色数据迁移到当前运行的服务端数据库
    /// 2. 当遇到重名的账号、角色、宠物或公会时，自动添加自定义前缀
    /// 3. 保持所有原始数据的结构和绑定关系
    /// 4. 支持分区数据导入到主区功能
    /// 
    /// 使用方法参考README-DatabaseMigrate.txt文件
    /// </summary>
    /// <remarks>
    /// 修复日志：
    /// 
    /// 2023.10 数据库迁移工具修复
    /// 1. 类属性名称不匹配问题：
    ///    - 修复了AccountInfo的CreateDate/LastDate改为CreationDate/LastLogin
    ///    - 修复了CharacterInfo的Hair改为HairType，PositionX/PositionY改为CurrentLocation
    ///    - 修复了UserItem的CurrentDura/MaxDura改为CurrentDurability/MaxDurability
    ///    - 修复了BeltLink更改为CharacterBeltLink，FixedPoint更改为FixedPointInfo
    /// 
    /// 2. 集合引用错误：
    ///    - 修复了CharacterInfo类使用Items而非Inventory的问题，添加了灵活的适配
    ///    - 修正了UserQuest类使用QuestInfo而非QuestId的问题，增加了双向转换支持
    /// 
    /// 3. 物品迁移失败问题：
    ///    - 添加了MigrateItemInfos方法先迁移物品定义
    ///    - 增强了MigrateItemProperties方法的容错能力
    ///    - 修复了AddedStats属性复制问题
    /// 
    /// 4. 空引用异常：
    ///    - 增强了AutoFightConfig、UserAchievement和RefineInfo迁移的安全检查
    ///    - 改进了反射调用的错误处理
    /// 
    /// 2023.12 添加分区导入功能
    /// 1. 添加分区导入选项，允许用户将分区数据导入到主区
    /// 2. 使用账号删除方法的逻辑进行分区数据导入
    /// 3. 优化了数据导入流程，支持批量导入
    /// </remarks>
    public partial class DatabaseMigrateView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Session _sourceSession;
        private string _sourcePrefix = "导入_";
        private bool _isPartitionImport = true; // 是否为分区导入模式
        private DBCollection<ItemInfo> sourceItemInfos;
        private List<string> AccountRenameLog = new List<string>();
        private List<string> CharacterRenameLog = new List<string>();
        private List<string> GuildRenameLog = new List<string>();
        private List<string> CompanionRenameLog = new List<string>();

        // 检查对象是否有指定属性
        private bool HasProperty(object obj, string propertyName)
        {
            if (obj == null) return false;
            return obj.GetType().GetProperty(propertyName) != null;
        }

        // 安全地复制属性值
        private void SafeCopyProperty(object source, object target, string propertyName)
        {
            if (source == null || target == null) return;

            var sourceProperty = source.GetType().GetProperty(propertyName);
            var targetProperty = target.GetType().GetProperty(propertyName);

            if (sourceProperty != null && targetProperty != null)
            {
                try
                {
                    var value = sourceProperty.GetValue(source);
                    targetProperty.SetValue(target, value);
                }
                catch
                {
                    // 属性复制失败时不抛出异常
                }
            }
        }

        public DatabaseMigrateView()
        {
            InitializeComponent();

            if (SEnvir.Session == null)
            {
                XtraMessageBox.Show("请先启动服务端", "无法迁移", MessageBoxButtons.OK);
                return;
                //Close();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void SelectSourceDBButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "数据库文件|*.db";
                dialog.Title = "选择源数据库文件";
                dialog.Multiselect = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SourceDBPathTextBox.Text = dialog.FileName;
                }
            }
        }

        private void StartMigrateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SourceDBPathTextBox.Text))
            {
                XtraMessageBox.Show("请先选择源数据库文件", "无法操作", MessageBoxButtons.OK);
                return;
            }
            if (!File.Exists(SourceDBPathTextBox.Text))
            {
                XtraMessageBox.Show("选择的源数据库文件不存在", "无法操作", MessageBoxButtons.OK);
                return;
            }

            LogTextBox.Clear();
            Application.DoEvents();
            try
            {
                if (!string.IsNullOrEmpty(CustomPrefixTextBox.Text))
                {
                    _sourcePrefix = CustomPrefixTextBox.Text;
                }

                if (_isPartitionImport)
                {
                    LogTextBox.Text = "开始分区导入数据...\r\n";
                    Application.DoEvents();
                    InitializeSourceSession();
                    MigratePartitionData();
                    SEnvir.Session.Save(true, SessionMode.Server);
                    LogTextBox.Text += "分区数据导入完成！\r\n";
                    XtraMessageBox.Show("分区数据导入成功完成！", "导入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"操作过程中发生错误：{ex.Message}\r\n";
                XtraMessageBox.Show($"操作过程中发生错误：{ex.Message}", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeSourceSession()
        {
            LogTextBox.Text = "正在初始化源数据库...\r\n";
            Application.DoEvents();

            string sourceDbPath = Path.GetDirectoryName(SourceDBPathTextBox.Text);
            if (string.IsNullOrEmpty(sourceDbPath))
            {
                sourceDbPath = "./";
            }
            else
            {
                sourceDbPath += "/";
            }

            var assemblies = new Assembly[]
            {
                Assembly.GetAssembly(typeof(Library.SystemModels.ItemInfo)),
                Assembly.GetAssembly(typeof(Server.DBModels.AccountInfo))
            };

            //_sourceSession = new Session(SessionMode.Server, assemblies, SEnvir.Session.XOREncrypt, SEnvir.Session.XORKey, sourceDbPath)
            _sourceSession = new Session(SessionMode.Server, assemblies, Config.EnableDBEncryption, Config.DBPassword, sourceDbPath)
            {
                BackUpDelay = 0,
                IsMySql = Config.MySqlEnable,
                MySqlParms = new string[] { Config.MySqlUser, Config.MySqlPassword, Config.MySqlIP, Config.MySqlPort, },
            };
            _sourceSession.Init();

            // 尝试预加载重要信息
            try
            {
                // 初始化源数据库的ItemInfo集合
                sourceItemInfos = _sourceSession.GetCollection<ItemInfo>();
                //LogTextBox.Text += $"  ItemInfo集合初始化成功，共{sourceItemInfos.Binding.Count}个物品定义\r\n";

                // 检查UserItem结构
                var userItems = _sourceSession.GetCollection<UserItem>();
                //LogTextBox.Text += $"  UserItem集合检查: 共{userItems.Binding.Count}个物品实例\r\n";
            }
            catch (Exception ex)
            {
                //LogTextBox.Text += $"  警告: 预加载数据时出现问题: {ex.Message}\r\n  迁移过程中可能会跳过某些项目\r\n";
            }

            LogTextBox.Text += "源数据库初始化成功！\r\n";
        }

        // 分区数据导入方法
        private void MigratePartitionData()
        {
            LogTextBox.Text += "开始分区数据导入...\r\n";
            Application.DoEvents();

            // 1. 读取源数据库集合
            var sourceAccounts = _sourceSession.GetCollection<AccountInfo>();
            var sourceCharacters = _sourceSession.GetCollection<CharacterInfo>();
            var sourceGuilds = _sourceSession.GetCollection<GuildInfo>();
            var sourceCompanions = _sourceSession.GetCollection<UserCompanion>();

            // 2. 目标数据库现有集合
            var targetAccounts = SEnvir.Session.GetCollection<AccountInfo>();
            var targetCharacters = SEnvir.Session.GetCollection<CharacterInfo>();
            var targetGuilds = SEnvir.Session.GetCollection<GuildInfo>();
            var targetCompanions = SEnvir.Session.GetCollection<UserCompanion>();

            // 3. 创建映射字典
            Dictionary<AccountInfo, AccountInfo> accountMap = new Dictionary<AccountInfo, AccountInfo>();
            Dictionary<CharacterInfo, CharacterInfo> charMap = new Dictionary<CharacterInfo, CharacterInfo>();
            Dictionary<GuildInfo, GuildInfo> guildMap = new Dictionary<GuildInfo, GuildInfo>();
            Dictionary<UserCompanion, UserCompanion> compMap = new Dictionary<UserCompanion, UserCompanion>();

            // 3.1 现有名称集合
            HashSet<string> existAccountNames = new HashSet<string>(targetAccounts.Binding.Select(a => a.EMailAddress.ToLower()));
            HashSet<string> existCharacterNames = new HashSet<string>(targetCharacters.Binding.Select(c => c.CharacterName.ToLower()));
            HashSet<string> existGuildNames = new HashSet<string>(targetGuilds.Binding.Select(g => g.GuildName.ToLower()));
            HashSet<string> existCompanionNames = new HashSet<string>(targetCompanions.Binding.Where(c => !string.IsNullOrEmpty(c.Name)).Select(c => c.Name.ToLower()));

            // 4. 导入账号数据
            MigrateAccounts(sourceAccounts, targetAccounts, existAccountNames, accountMap);

            // 5. 导入角色数据
            int importedCharacters = 0;
            int skippedCharacters = 0;
            int renamedCharacters = 0;

            foreach (var srcCharacter in sourceCharacters.Binding)
            {
                if (srcCharacter.Account == null || !accountMap.ContainsKey(srcCharacter.Account))
                {
                    LogTextBox.Text += $"跳过无效账号的角色: {srcCharacter.CharacterName}\r\n";
                    skippedCharacters++;
                    continue;
                }

                var targetAccount = accountMap[srcCharacter.Account];
                string newName = srcCharacter.CharacterName;
                bool renamed = false;

                // 检查名称冲突
                if (existCharacterNames.Contains(newName.ToLower()))
                {
                    newName = _sourcePrefix + newName;
                    renamed = true;
                    renamedCharacters++;
                    CharacterRenameLog.Add($"\"{srcCharacter.CharacterName}\"\t\t\t\"{newName}\"");
                }
                existCharacterNames.Add(newName.ToLower());

                // 创建新角色
                var newCharacter = targetCharacters.CreateNewObject();
                CopyCharacter(srcCharacter, newCharacter, newName, accountMap);
                charMap[srcCharacter] = newCharacter;
                importedCharacters++;

                if (renamed)
                {
                    LogTextBox.Text += $"角色重命名: {srcCharacter.CharacterName} -> {newName}\r\n";
                }
                else
                {
                    LogTextBox.Text += $"导入新角色: {srcCharacter.CharacterName}\r\n";
                }
                Application.DoEvents();
            }

            LogTextBox.Text += $"角色处理完成: 导入 {importedCharacters} 个角色，其中 {renamedCharacters} 个被重命名，跳过 {skippedCharacters} 个无效角色\r\n";
            Application.DoEvents();

            // 6. 导入行会数据
            int importedGuilds = 0;
            int renamedGuilds = 0;

            foreach (var srcGuild in sourceGuilds.Binding)
            {
                string newName = srcGuild.GuildName;
                bool renamed = false;

                // 检查名称冲突
                if (existGuildNames.Contains(newName.ToLower()))
                {
                    newName = _sourcePrefix + newName;
                    renamed = true;
                    renamedGuilds++;
                    GuildRenameLog.Add($"\"{srcGuild.GuildName}\"\t\t\t\"{newName}\"");
                }
                existGuildNames.Add(newName.ToLower());

                // 创建新行会
                var newGuild = targetGuilds.CreateNewObject();
                CopyGuild(srcGuild, newGuild, newName);
                guildMap[srcGuild] = newGuild;
                importedGuilds++;

                if (renamed)
                {
                    LogTextBox.Text += $"行会重命名: {srcGuild.GuildName} -> {newName}\r\n";
                }
                else
                {
                    LogTextBox.Text += $"导入新行会: {srcGuild.GuildName}\r\n";
                }
                Application.DoEvents();
            }

            LogTextBox.Text += $"行会处理完成: 导入 {importedGuilds} 个行会，其中 {renamedGuilds} 个被重命名\r\n";
            Application.DoEvents();

            // 7. 导入宠物数据
            MigrateCompanions(sourceCompanions, targetCompanions, existCompanionNames, charMap, compMap, accountMap);

            // 8. 迁移其他关联数据
            MigrateAssociatedData(charMap, guildMap, compMap, accountMap);
        }

        private void MigrateCompanions(
            DBCollection<UserCompanion> sourceCompanions,
            DBCollection<UserCompanion> targetCompanions,
            HashSet<string> existCompanionNames,
            Dictionary<CharacterInfo, CharacterInfo> charMap,
            Dictionary<UserCompanion, UserCompanion> compMap,
            Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            try
            {
                LogTextBox.Text += "开始迁移宠物...\r\n";
                Application.DoEvents();
                int companionCount = 0, companionFail = 0, renamedCount = 0;
                var tempCompanionMap = new Dictionary<UserCompanion, UserCompanion>();
                var tempItems = new List<(UserCompanion tgt, UserCompanion src, IList<UserItem> srcItems)>();
                foreach (var src in sourceCompanions.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        // 检查账号关联
                        if (src.Account == null || !accountMap.ContainsKey(src.Account))
                        {
                            LogTextBox.Text += $"  跳过无效账号的宠物: {src.Name ?? "未命名"}\r\n";
                            continue;
                        }
                        var tgt = targetCompanions.CreateNewObject();
                        tgt.Account = accountMap[src.Account];
                        // 名称处理
                        string newName = src.Name;
                        bool renamed = false;
                        if (!string.IsNullOrEmpty(newName) && existCompanionNames.Contains(newName.ToLower()))
                        {
                            newName = _sourcePrefix + newName;
                            renamed = true;
                            renamedCount++;
                            CompanionRenameLog.Add($"\"{src.Name}\"\t\t\t\"{newName}\"");
                        }
                        existCompanionNames.Add(newName?.ToLower() ?? "");
                        tgt.Name = newName;
                        // 关联角色
                        if (src.Character != null && charMap.ContainsKey(src.Character))
                            tgt.Character = charMap[src.Character];
                        // 关联Info
                        if (src.Info != null)
                        {
                            var companionInfos = SEnvir.Session.GetCollection<CompanionInfo>();
                            if (companionInfos != null)
                            {
                                int infoIndex = src.Info.Index;
                                string infoName = src.Info.MonsterInfo?.MonsterName;
                                var targetInfo = companionInfos.Binding.FirstOrDefault(i =>
                                    i.Index == infoIndex ||
                                    (!string.IsNullOrEmpty(infoName) && i.MonsterInfo != null &&
                                     string.Equals(i.MonsterInfo.MonsterName, infoName, StringComparison.OrdinalIgnoreCase)));
                                if (targetInfo != null)
                                {
                                    tgt.Info = targetInfo;
                                }
                                else if (!string.IsNullOrEmpty(infoName))
                                {
                                    targetInfo = companionInfos.Binding.FirstOrDefault(i =>
                                        i.MonsterInfo != null &&
                                        !string.IsNullOrEmpty(i.MonsterInfo.MonsterName) &&
                                        i.MonsterInfo.MonsterName.IndexOf(infoName, StringComparison.OrdinalIgnoreCase) >= 0);
                                    if (targetInfo != null)
                                        tgt.Info = targetInfo;
                                }
                                if (targetInfo == null && companionInfos.Binding.Count > 0)
                                {
                                    tgt.Info = companionInfos.Binding.FirstOrDefault();
                                }
                            }
                        }
                        // 复制常规属性
                        string[] props = { "CompanionType", "Level", "Experience", "Hunger", "Index", "Slot", "Enabled", "Active", "ExtraInfo" };
                        BatchCopyProperties(src, tgt, props);

                        // 记录待迁移物品
                        if (src.Items != null && tgt.Items != null)
                        {
                            tempItems.Add((tgt, src, src.Items));
                        }
                        // 技能、装备、状态等集合属性可按需扩展
                        compMap[src] = tgt;
                        tempCompanionMap[src] = tgt;
                        companionCount++;
                        if (renamed)
                            LogTextBox.Text += $"  宠物重命名: {src.Name} -> {newName}\r\n";
                        else
                            LogTextBox.Text += $"  导入宠物: {newName}\r\n";
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        companionFail++;
                        LogTextBox.Text += $"宠物迁移失败: ({ex.Message})\r\n";
                    }
                }
                // 迁移物品并挂接到宠物
                foreach (var (tgt, src, srcItems) in tempItems)
                {
                    int matchedByName = 0, matchedByIndex = 0, noMatchCount = 0;
                    foreach (var srcItem in srcItems)
                    {
                        if (srcItem == null) continue;
                        var tgtItem = SEnvir.Session.GetCollection<UserItem>().CreateNewObject();
                        MigrateItemProperties(srcItem, tgtItem, SEnvir.ItemInfoList.Binding, ref matchedByName, ref matchedByIndex, ref noMatchCount);
                        tgtItem.Companion = tgt;
                        tgtItem.Character = null; // 确保不是角色物品
                        if (HasProperty(srcItem, "Slot") && HasProperty(tgtItem, "Slot"))
                            tgtItem.Slot = srcItem.Slot;
                        tgt.Items.Add(tgtItem);
                    }
                }
                // 最后整体保存一次
                SEnvir.Session.Save(false, SessionMode.Server);
                LogTextBox.Text += $"宠物迁移完成：共{companionCount}个，失败{companionFail}个，重命名{renamedCount}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"宠物迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateAssociatedData(
            Dictionary<CharacterInfo, CharacterInfo> charMap,
            Dictionary<GuildInfo, GuildInfo> guildMap,
            Dictionary<UserCompanion, UserCompanion> compMap,
            Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            try
            {
                LogTextBox.Text += "开始迁移关联数据...\r\n";
                Application.DoEvents();

                // 迁移角色物品
                MigrateUserItems(charMap, accountMap);

                // 迁移魔法技能
                MigrateUserMagics(charMap);

                // 迁移Buff
                MigrateBuffs(charMap);

                // 迁移药品快捷栏
                MigrateBeltLinks(charMap);

                // 迁移自动喝药栏
                MigrateAutoPotionLinks(charMap);

                // 迁移自动挂机设置
                MigrateAutoFightLinks(charMap);

                // 迁移任务
                MigrateQuests(charMap);

                // 迁移成就
                MigrateAchievements(charMap);

                // 迁移精炼信息
                MigrateRefines(charMap);

                // 迁移记忆传送点
                MigrateFixedPoints(charMap);

                // 迁移自定义变量
                MigrateUserValues(charMap);

                // 迁移商城购买记录
                MigrateStoreSales(accountMap);

                LogTextBox.Text += "关联数据迁移完成!\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"关联数据迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void CopyAccount(AccountInfo src, AccountInfo tgt, string newName)
        {
            // 基本属性
            tgt.EMailAddress = newName;

            tgt.Password = src.Password;

            tgt.CreationDate = src.CreationDate;

            tgt.CreationIP = src.CreationIP;

            tgt.LastLogin = src.LastLogin;

            tgt.LastIP = src.LastIP;
            tgt.HuntGold = src.HuntGold;
            tgt.AutoTime = src.AutoTime;

            // 高级属性
            if (HasProperty(src, "Activated") && HasProperty(tgt, "Activated"))
                SafeCopyProperty(src, tgt, "Activated");
            if (HasProperty(src, "Admin") && HasProperty(tgt, "Admin"))
                SafeCopyProperty(src, tgt, "Admin");
            if (HasProperty(src, "Banned") && HasProperty(tgt, "Banned"))
                SafeCopyProperty(src, tgt, "Banned");
            if (HasProperty(src, "BanReason") && HasProperty(tgt, "BanReason"))
                SafeCopyProperty(src, tgt, "BanReason");
            if (HasProperty(src, "ExpiryDate") && HasProperty(tgt, "ExpiryDate"))
                SafeCopyProperty(src, tgt, "ExpiryDate");
            if (HasProperty(src, "Gold") && HasProperty(tgt, "Gold"))
                SafeCopyProperty(src, tgt, "Gold");
            if (HasProperty(src, "GameGold") && HasProperty(tgt, "GameGold"))
                SafeCopyProperty(src, tgt, "GameGold");

            // 集合属性 (不需要复制，会由关联迁移自动创建)
        }

        private void CopyCharacter(CharacterInfo src, CharacterInfo tgt, string newName, Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            try
            {
                // 关联账号
                if (src.Account != null && accountMap.ContainsKey(src.Account))
                    tgt.Account = accountMap[src.Account];

                // 基本属性
                tgt.CharacterName = newName;
                string[] props = { "Class", "Gender", "Level", "CurrentMap", "Direction", "CreationIP", "CreationDate", "Dead", "Health", "Mana", "Experience", "CurrentHP", "CurrentMP", "FreeStats", "Intelligence", "Strength", "Vitality", "AC", "MAC", "DC", "MC", "SC", "Accuracy", "Agility", "Gold", "GameGold", "Credit", "PKPoints", "PKLevel", "Guild", "GuildRank", "GuildJoinDate", "FixedPointCount", "FixedPointTCount", "TodayOnlineMinutes", "TotalOnlineMinutes", "CraftLevel", "CraftExperience", "PatchGridSize", "CPUInfo", "MACInfo", "HDDInfo", "LastLogin", "LastIP", "LastLogout", "LastLocation", "Horse", "ExtraInfo" };
                BatchCopyProperties(src, tgt, props);

                // Hair/HairType特殊处理
                if (HasProperty(src, "Hair") && HasProperty(tgt, "HairType"))
                    tgt.HairType = Convert.ToInt32(src.GetType().GetProperty("Hair").GetValue(src));
                else if (HasProperty(src, "HairType") && HasProperty(tgt, "HairType"))
                    tgt.HairType = src.HairType;

                // 位置属性特殊处理
                if (HasProperty(src, "PositionX") && HasProperty(src, "PositionY") && HasProperty(tgt, "CurrentLocation"))
                {
                    try
                    {
                        int x = Convert.ToInt32(src.GetType().GetProperty("PositionX").GetValue(src));
                        int y = Convert.ToInt32(src.GetType().GetProperty("PositionY").GetValue(src));
                        tgt.CurrentLocation = new System.Drawing.Point(x, y);
                    }
                    catch (Exception ex)
                    {
                        LogTextBox.Text += $"复制角色位置时出错: {ex.Message}\r\n";
                    }
                }
                else if (HasProperty(src, "CurrentLocation") && HasProperty(tgt, "CurrentLocation"))
                    SafeCopyProperty(src, tgt, "CurrentLocation");

                // 绑定点信息
                if (HasProperty(src, "BindMap") && HasProperty(tgt, "BindMap"))
                    SafeCopyProperty(src, tgt, "BindMap");
                if (HasProperty(src, "BindLocation") && HasProperty(tgt, "BindLocation"))
                    SafeCopyProperty(src, tgt, "BindLocation");
                tgt.BindPoint = src.BindPoint;

                // 记忆传送点集合
                if (HasProperty(src, "FixedPoints") && HasProperty(tgt, "FixedPoints"))
                {
                    try
                    {
                        var srcPoints = src.GetType().GetProperty("FixedPoints").GetValue(src) as System.Collections.IList;
                        var tgtPoints = tgt.GetType().GetProperty("FixedPoints").GetValue(tgt) as System.Collections.IList;
                        if (srcPoints != null && tgtPoints != null)
                        {
                            foreach (var point in srcPoints)
                            {
                                var pointType = point.GetType();
                                var pointCopy = SEnvir.Session.GetCollection(pointType).GetType().GetMethod("CreateNewObject").Invoke(SEnvir.Session.GetCollection(pointType), null);
                                pointType.GetProperty("Character").SetValue(pointCopy, tgt);
                                string[] pointProps = { "Name", "Map", "Location", "Enabled" };
                                foreach (var p in pointProps)
                                {
                                    if (HasProperty(point, p) && HasProperty(pointCopy, p))
                                        SafeCopyProperty(point, pointCopy, p);
                                }
                                tgtPoints.Add(pointCopy);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogTextBox.Text += $"复制记忆传送点失败: {ex.Message}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"复制角色属性时出错: {ex.Message}\r\n";
            }
        }

        private void CopyGuild(GuildInfo src, GuildInfo tgt, string newName)
        {
            try
            {
                tgt.GuildName = newName;
                string[] props = { "Notice", "GuildFunds", "Level", "MemberLimit", "TotalContribution" };
                BatchCopyProperties(src, tgt, props);
                // 如有集合属性后续可在此扩展
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"复制行会属性时出错: {ex.Message}\r\n";
            }
        }

        #region 迁移关联数据方法

        private void MigrateUserItems(Dictionary<CharacterInfo, CharacterInfo> charMap, Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            try
            {
                var sourceItems = _sourceSession.GetCollection<UserItem>();
                var targetItems = SEnvir.Session.GetCollection<UserItem>();
                var targetItemInfos = SEnvir.ItemInfoList.Binding;
                int itemCount = 0, itemFail = 0, totalItems = sourceItems.Binding.Count;
                int matchedByName = 0, matchedByIndex = 0, noMatchCount = 0;
                Dictionary<UserItem, UserItem> itemMap = new Dictionary<UserItem, UserItem>();
                LogTextBox.Text += $"开始迁移物品，源数据库共有{totalItems}个物品...\r\n";
                Application.DoEvents();
                foreach (var src in sourceItems.Binding)
                {
                    try
                    {
                        var tgt = targetItems.CreateNewObject();
                        MigrateItemProperties(src, tgt, targetItemInfos, ref matchedByName, ref matchedByIndex, ref noMatchCount);
                        itemMap[src] = tgt;
                        itemCount++;
                        if (itemCount % 100 == 0)
                        {
                            LogTextBox.Text += $"已处理{itemCount}/{totalItems}个物品\r\n";
                            Application.DoEvents();
                        }
                    }
                    catch (Exception ex)
                    {
                        itemFail++;
                        LogTextBox.Text += $"物品迁移失败: ({ex.Message})\r\n";
                    }
                }
                // 角色物品挂接
                foreach (var charPair in charMap)
                {
                    var srcChar = charPair.Key;
                    var tgtChar = charPair.Value;
                    if (HasProperty(srcChar, "Items") && HasProperty(tgtChar, "Items"))
                    {
                        var srcItems = srcChar.GetType().GetProperty("Items").GetValue(srcChar) as IList<UserItem>;
                        var tgtItems = tgtChar.GetType().GetProperty("Items").GetValue(tgtChar) as IList<UserItem>;
                        if (srcItems != null && tgtItems != null)
                        {
                            foreach (var srcItem in srcItems)
                            {
                                if (itemMap.ContainsKey(srcItem))
                                {
                                    var tgtItem = itemMap[srcItem];
                                    tgtItem.Character = tgtChar;
                                    tgtItems.Add(tgtItem);
                                }
                            }
                        }
                    }
                }
                LogTextBox.Text += $"物品迁移完成: 共{itemCount}个物品，失败{itemFail}个\r\n";
                //LogTextBox.Text += $"物品Info按名称匹配:{matchedByName}，按索引匹配:{matchedByIndex}，未匹配:{noMatchCount}\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"物品迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateItemProperties(UserItem src, UserItem tgt, IList<ItemInfo> targetItemInfos, ref int matchedByName, ref int matchedByIndex, ref int noMatchCount)
        {
            try
            {
                ItemInfo targetInfo = null;
                string srcItemName = null;  // 将变量声明移到这里

                if (src.Info == null)
                {
                    LogTextBox.Text += $"警告: 物品实例ID={src.Index}的Info为空，无法匹配物品定义\r\n";
                }
                else
                {
                    if (HasProperty(src.Info, "ItemName"))
                    {
                        srcItemName = src.Info.GetType().GetProperty("ItemName").GetValue(src.Info) as string;
                    }

                    if (!string.IsNullOrEmpty(srcItemName))
                    {
                        targetInfo = targetItemInfos.FirstOrDefault(i =>
                        {
                            string targetName = null;
                            if (HasProperty(i, "ItemName"))
                            {
                                targetName = i.GetType().GetProperty("ItemName").GetValue(i) as string;
                            }
                            return !string.IsNullOrEmpty(targetName) &&
                                   targetName.Equals(srcItemName, StringComparison.OrdinalIgnoreCase);
                        });

                        if (targetInfo != null)
                        {
                            matchedByName++;
                            LogTextBox.Text += $"物品[{srcItemName}]按名称匹配成功\r\n";
                        }
                    }

                }

                tgt.Info = targetInfo;

                try
                {
                    // 基本属性
                    if (HasProperty(src, "Count") && HasProperty(tgt, "Count"))
                        tgt.Count = Convert.ToInt64(src.GetType().GetProperty("Count").GetValue(src));

                    // 修复CurrentDura/MaxDura改为CurrentDurability/MaxDurability问题
                    if (HasProperty(src, "CurrentDura") && HasProperty(tgt, "CurrentDurability"))
                    {
                        try
                        {
                            tgt.CurrentDurability = Convert.ToInt32(src.GetType().GetProperty("CurrentDura").GetValue(src));
                        }
                        catch
                        {
                            // 使用默认的最大耐久
                            if (HasProperty(tgt, "MaxDurability"))
                                tgt.CurrentDurability = tgt.MaxDurability;
                            else
                                tgt.CurrentDurability = 1000;
                        }
                    }
                    else if (HasProperty(src, "CurrentDurability") && HasProperty(tgt, "CurrentDurability"))
                    {
                        try
                        {
                            tgt.CurrentDurability = Convert.ToInt32(src.GetType().GetProperty("CurrentDurability").GetValue(src));
                        }
                        catch
                        {
                            // 使用默认的最大耐久
                            if (HasProperty(tgt, "MaxDurability"))
                                tgt.CurrentDurability = tgt.MaxDurability;
                            else
                                tgt.CurrentDurability = 1000;
                        }
                    }

                    if (HasProperty(src, "MaxDura") && HasProperty(tgt, "MaxDurability"))
                    {
                        try
                        {
                            tgt.MaxDurability = Convert.ToInt32(src.GetType().GetProperty("MaxDura").GetValue(src));
                        }
                        catch
                        {
                            // 设置默认最大耐久
                            tgt.MaxDurability = 1000;
                        }
                    }
                    else if (HasProperty(src, "MaxDurability") && HasProperty(tgt, "MaxDurability"))
                    {
                        try
                        {
                            tgt.MaxDurability = Convert.ToInt32(src.GetType().GetProperty("MaxDurability").GetValue(src));
                        }
                        catch
                        {
                            // 设置默认最大耐久
                            tgt.MaxDurability = 1000;
                        }
                    }

                    // 复制常见属性
                    if (HasProperty(src, "Level") && HasProperty(tgt, "Level"))
                        SafeCopyProperty(src, tgt, "Level");
                    if (HasProperty(src, "Colour") && HasProperty(tgt, "Colour"))
                        SafeCopyProperty(src, tgt, "Colour");
                    if (HasProperty(src, "CurrentRefine") && HasProperty(tgt, "CurrentRefine"))
                        SafeCopyProperty(src, tgt, "CurrentRefine");

                    // 属性类
                    if (HasProperty(src, "AddedStats") && HasProperty(tgt, "AddedStats"))
                    {
                        try
                        {
                            var srcStats = src.GetType().GetProperty("AddedStats").GetValue(src) as System.Collections.IEnumerable;
                            var tgtStats = tgt.GetType().GetProperty("AddedStats").GetValue(tgt) as System.Collections.IList;

                            if (srcStats != null && tgtStats != null)
                            {
                                foreach (var stat in srcStats)
                                {
                                    var statType = stat.GetType();
                                    var newStat = SEnvir.Session.GetCollection(statType).GetType().GetMethod("CreateNewObject").Invoke(SEnvir.Session.GetCollection(statType), null);

                                    // 复制属性
                                    if (HasProperty(stat, "Stat") && HasProperty(newStat, "Stat"))
                                        SafeCopyProperty(stat, newStat, "Stat");
                                    if (HasProperty(stat, "Amount") && HasProperty(newStat, "Amount"))
                                        SafeCopyProperty(stat, newStat, "Amount");
                                    if (HasProperty(stat, "StatSource") && HasProperty(newStat, "StatSource"))
                                        SafeCopyProperty(stat, newStat, "StatSource");
                                    if (HasProperty(stat, "SourceName") && HasProperty(newStat, "SourceName"))
                                        SafeCopyProperty(stat, newStat, "SourceName");
                                    // 归属
                                    if (HasProperty(newStat, "Item"))
                                        newStat.GetType().GetProperty("Item").SetValue(newStat, tgt);

                                    tgtStats.Add(newStat);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogTextBox.Text += $"复制物品AddedStats属性失败: {ex.Message}\r\n";
                        }
                    }

                    // 其他常见属性，以安全的方式复制
                    string[] commonProps = new[] {
                        "SoulBoundId", "WeddingRingId", "IsShopItem", "IsDefault",
                        "Experience", "Chance", "RefineType", "RefineResetCount"
                    };

                    BatchCopyProperties(src, tgt, commonProps);

                    // 尝试复制其他可能的属性，但排除已知的特殊属性
                    string[] excludedProps = new[] {
                        "Info", "Index", "ObserverCount", "Slots", "Count", "CurrentDurability",
                        "MaxDurability", "Level", "Colour", "AddedStats", "SoulBoundId", "WeddingRingId",
                        "IsShopItem", "IsDefault", "CurrentRefine", "Character", "Account", "Guild",
                        "Companion", "Refine", "Auction", "Mail", "Stats"
                    };

                    foreach (var prop in src.GetType().GetProperties())
                    {
                        if (excludedProps.Contains(prop.Name))
                            continue;

                        if (HasProperty(tgt, prop.Name))
                        {
                            try
                            {
                                SafeCopyProperty(src, tgt, prop.Name);
                            }
                            catch
                            {
                                // 忽略非关键属性的复制错误
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 捕获并记录所有属性设置过程中的错误
                    LogTextBox.Text += $"物品属性复制失败: {ex.Message}\r\n";
                }
            }
            catch (Exception ex)
            {
                // 记录整个处理过程中的主要错误
                LogTextBox.Text += $"物品迁移处理失败: {ex.Message}\r\n";
                throw; // 重新抛出异常以便上层函数处理
            }
        }

        private void MigrateUserMagics(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceMagicInfos = _sourceSession.GetCollection<MagicInfo>();
                var sourceMagics = _sourceSession.GetCollection<UserMagic>();
                var targetMagics = SEnvir.Session.GetCollection<UserMagic>();
                var magicInfos = SEnvir.MagicInfoList.Binding;
                int magicCount = 0, magicFail = 0, totalMagics = sourceMagics.Binding.Count;
                int matchedByName = 0, matchedByIndex = 0, noMatchCount = 0;
                Dictionary<UserMagic, UserMagic> magicMap = new Dictionary<UserMagic, UserMagic>();
                LogTextBox.Text += $"开始迁移魔法技能，源数据库共有{totalMagics}个魔法技能...\r\n";
                Application.DoEvents();
                foreach (var src in sourceMagics.Binding)
                {
                    try
                    {
                        var tgt = targetMagics.CreateNewObject();
                        // Info智能匹配
                        MagicInfo targetInfo = null;
                        string srcMagicName = null;
                        if (src.Info != null && HasProperty(src.Info, "Name"))
                            srcMagicName = src.Info.GetType().GetProperty("Name").GetValue(src.Info) as string;
                        if (!string.IsNullOrEmpty(srcMagicName))
                        {
                            targetInfo = magicInfos.FirstOrDefault(m =>
                            {
                                string targetName = null;
                                if (HasProperty(m, "Name"))
                                    targetName = m.GetType().GetProperty("Name").GetValue(m) as string;
                                return !string.IsNullOrEmpty(targetName) && targetName.Equals(srcMagicName, StringComparison.OrdinalIgnoreCase);
                            });
                            if (targetInfo != null) matchedByName++;
                        }
                        tgt.Info = targetInfo;
                        // 常规属性批量复制
                        string[] props = { "Level", "Experience", "Key", "Set1Key", "Set2Key", "Set3Key", "Set4Key" };
                        BatchCopyProperties(src, tgt, props);
                        // 关联角色
                        if (src.Character != null && charMap.ContainsKey(src.Character))
                        {
                            tgt.Character = charMap[src.Character];
                            if (HasProperty(tgt.Character, "Magics"))
                            {
                                var magics = tgt.Character.GetType().GetProperty("Magics").GetValue(tgt.Character) as IList<UserMagic>;
                                if (magics != null && !magics.Contains(tgt))
                                    magics.Add(tgt);
                            }
                        }
                        magicMap[src] = tgt;
                        magicCount++;
                        if (magicCount % 100 == 0)
                        {
                            LogTextBox.Text += $"已处理{magicCount}/{totalMagics}个魔法技能\r\n";
                            Application.DoEvents();
                        }
                    }
                    catch (Exception ex)
                    {
                        magicFail++;
                        LogTextBox.Text += $"魔法技能迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"魔法技能迁移完成: 共{magicCount}个，失败{magicFail}个\r\n";
                LogTextBox.Text += $"魔法技能Info按名称匹配:{matchedByName}，未匹配:{noMatchCount}\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"魔法技能迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateQuests(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceQuests = _sourceSession.GetCollection<UserQuest>();
                var targetQuests = SEnvir.Session.GetCollection<UserQuest>();
                var questInfos = SEnvir.Session.GetCollection<QuestInfo>();
                int questCount = 0, questFail = 0;
                LogTextBox.Text += "开始迁移任务...\r\n";
                Application.DoEvents();
                foreach (var src in sourceQuests.Binding)
                {
                    try
                    {
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetQuests.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // QuestInfo/QuestId智能匹配
                        if (HasProperty(src, "QuestInfo") && HasProperty(tgt, "QuestInfo"))
                            SafeCopyProperty(src, tgt, "QuestInfo");
                        else if (HasProperty(src, "QuestId") && HasProperty(tgt, "QuestInfo"))
                        {
                            try
                            {
                                int questId = Convert.ToInt32(src.GetType().GetProperty("QuestId").GetValue(src));
                                if (questInfos != null)
                                {
                                    var questInfo = questInfos.Binding.FirstOrDefault(q =>
                                        HasProperty(q, "Index") && Convert.ToInt32(q.GetType().GetProperty("Index").GetValue(q)) == questId);
                                    if (questInfo != null)
                                        tgt.GetType().GetProperty("QuestInfo").SetValue(tgt, questInfo);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogTextBox.Text += $"  转换QuestId到QuestInfo失败: {ex.Message}\r\n";
                            }
                        }
                        else if (HasProperty(src, "QuestInfo") && HasProperty(tgt, "QuestId"))
                        {
                            try
                            {
                                var questInfo = src.GetType().GetProperty("QuestInfo").GetValue(src);
                                if (questInfo != null && HasProperty(questInfo, "Index"))
                                {
                                    int questId = Convert.ToInt32(questInfo.GetType().GetProperty("Index").GetValue(questInfo));
                                    tgt.GetType().GetProperty("QuestId").SetValue(tgt, questId);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogTextBox.Text += $"  转换QuestInfo到QuestId失败: {ex.Message}\r\n";
                            }
                        }
                        // 常规属性批量复制
                        string[] props = { "Completed", "SelectedReward", "Track", "ExtraInfo" };
                        BatchCopyProperties(src, tgt, props);
                        // 任务阶段Tasks集合
                        if (HasProperty(src, "Tasks") && HasProperty(tgt, "Tasks"))
                        {
                            try
                            {
                                var srcTasks = src.GetType().GetProperty("Tasks").GetValue(src) as System.Collections.IList;
                                var tgtTasks = tgt.GetType().GetProperty("Tasks").GetValue(tgt) as System.Collections.IList;
                                if (srcTasks != null && tgtTasks != null)
                                {
                                    foreach (var task in srcTasks)
                                    {
                                        var taskType = task.GetType();
                                        var taskCopy = SEnvir.Session.GetCollection(taskType).GetType().GetMethod("CreateNewObject").Invoke(SEnvir.Session.GetCollection(taskType), null);
                                        taskType.GetProperty("Quest").SetValue(taskCopy, tgt);
                                        string[] taskProps = { "Task", "Amount" };
                                        foreach (var p in taskProps)
                                        {
                                            if (HasProperty(task, p) && HasProperty(taskCopy, p))
                                                SafeCopyProperty(task, taskCopy, p);
                                        }
                                        tgtTasks.Add(taskCopy);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                LogTextBox.Text += $"  复制任务内容失败: {ex.Message}\r\n";
                            }
                        }
                        // 添加到角色的Quests集合
                        if (HasProperty(tgt.Character, "Quests") && tgt.Character.GetType().GetProperty("Quests").GetValue(tgt.Character) is IList<UserQuest> quests)
                        {
                            if (!quests.Contains(tgt))
                                quests.Add(tgt);
                        }
                        questCount++;
                    }
                    catch (Exception ex)
                    {
                        questFail++;
                        LogTextBox.Text += $"任务迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"任务迁移完成：共{questCount}个，失败{questFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"任务迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateAchievements(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceAchievements = _sourceSession.GetCollection<UserAchievement>();
                var targetAchievements = SEnvir.Session.GetCollection<UserAchievement>();
                int achievementCount = 0, achievementFail = 0;
                LogTextBox.Text += "开始迁移成就...\r\n";
                Application.DoEvents();
                foreach (var src in sourceAchievements.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetAchievements.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Info", "Index", "Value", "CompletedDate", "Completed", "Tracked", "SelectedReward", "ExtraInfo" };
                        BatchCopyProperties(src, tgt, props);
                        // Tasks集合
                        if (HasProperty(src, "Tasks") && HasProperty(tgt, "Tasks"))
                        {
                            try
                            {
                                var srcTasks = src.GetType().GetProperty("Tasks").GetValue(src) as System.Collections.IList;
                                var tgtTasks = tgt.GetType().GetProperty("Tasks").GetValue(tgt) as System.Collections.IList;
                                if (srcTasks != null && tgtTasks != null)
                                {
                                    foreach (var task in srcTasks)
                                    {
                                        var taskType = task.GetType();
                                        var taskCopy = SEnvir.Session.GetCollection(taskType).GetType().GetMethod("CreateNewObject").Invoke(SEnvir.Session.GetCollection(taskType), null);
                                        taskType.GetProperty("Achievement").SetValue(taskCopy, tgt);
                                        string[] taskProps = { "Task", "Amount" };
                                        foreach (var p in taskProps)
                                        {
                                            if (HasProperty(task, p) && HasProperty(taskCopy, p))
                                                SafeCopyProperty(task, taskCopy, p);
                                        }
                                        tgtTasks.Add(taskCopy);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogTextBox.Text += $"  复制成就任务内容失败: {ex.Message}\r\n";
                            }
                        }
                        if (tgt.Character.Achievements != null && !tgt.Character.Achievements.Contains(tgt))
                        {
                            tgt.Character.Achievements.Add(tgt);
                        }
                        achievementCount++;
                    }
                    catch (Exception ex)
                    {
                        achievementFail++;
                        LogTextBox.Text += $"成就迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"成就迁移完成：共{achievementCount}个，失败{achievementFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"成就迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateRefines(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceRefines = _sourceSession.GetCollection<RefineInfo>();
                var targetRefines = SEnvir.Session.GetCollection<RefineInfo>();
                int refineCount = 0, refineFail = 0;
                LogTextBox.Text += "开始迁移精炼信息...\r\n";
                Application.DoEvents();
                foreach (var src in sourceRefines.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetRefines.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Type", "Info", "Progress", "ProgressCount", "Level", "Experience", "MaxExperience", "SuccessRate", "FailRate", "BreakRate", "MaxLevel", "ResetCount", "LastReset", "ExtraInfo" };
                        BatchCopyProperties(src, tgt, props);
                        // Materials集合
                        if (HasProperty(src, "Materials") && HasProperty(tgt, "Materials"))
                        {
                            try
                            {
                                var srcMaterials = src.GetType().GetProperty("Materials").GetValue(src) as System.Collections.IList;
                                var tgtMaterials = tgt.GetType().GetProperty("Materials").GetValue(tgt) as System.Collections.IList;
                                if (srcMaterials != null && tgtMaterials != null)
                                {
                                    foreach (var material in srcMaterials)
                                    {
                                        var materialType = material.GetType();
                                        var materialCopy = SEnvir.Session.GetCollection(materialType).GetType().GetMethod("CreateNewObject").Invoke(SEnvir.Session.GetCollection(materialType), null);
                                        materialType.GetProperty("Refine").SetValue(materialCopy, tgt);
                                        string[] matProps = { "ItemId", "Count" };
                                        foreach (var p in matProps)
                                        {
                                            if (HasProperty(material, p) && HasProperty(materialCopy, p))
                                                SafeCopyProperty(material, materialCopy, p);
                                        }
                                        tgtMaterials.Add(materialCopy);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogTextBox.Text += $"  复制精炼材料信息失败: {ex.Message}\r\n";
                            }
                        }
                        if (tgt.Character.Refines != null && !tgt.Character.Refines.Contains(tgt))
                        {
                            tgt.Character.Refines.Add(tgt);
                        }
                        refineCount++;
                    }
                    catch (Exception ex)
                    {
                        refineFail++;
                        LogTextBox.Text += $"精炼信息迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"精炼信息迁移完成：共{refineCount}个，失败{refineFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"精炼信息迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateFixedPoints(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourcePoints = _sourceSession.GetCollection<FixedPointInfo>();
                var targetPoints = SEnvir.Session.GetCollection<FixedPointInfo>();
                int pointCount = 0, pointFail = 0;
                LogTextBox.Text += "开始迁移记忆传送点...\r\n";
                Application.DoEvents();
                foreach (var src in sourcePoints.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetPoints.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Index", "Name", "MapFileName", "MapName", "PointName", "LocationX", "LocationY", "Enabled", "Created", "LastUsed", "UseCount", "ExtraInfo", "MapInfo", "Region", "PointType", "Cooldown", "UseLimit" };
                        BatchCopyProperties(src, tgt, props);
                        if (tgt.Character.FPointLinks != null && !tgt.Character.FPointLinks.Contains(tgt))
                        {
                            tgt.Character.FPointLinks.Add(tgt);
                        }
                        pointCount++;
                    }
                    catch (Exception ex)
                    {
                        pointFail++;
                        LogTextBox.Text += $"记忆传送点迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"记忆传送点迁移完成：共{pointCount}个，失败{pointFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"记忆传送点迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateUserValues(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceValues = _sourceSession.GetCollection<UserValue>();
                var targetValues = SEnvir.Session.GetCollection<UserValue>();
                int valueCount = 0, valueFail = 0;
                LogTextBox.Text += "开始迁移自定义变量...\r\n";
                Application.DoEvents();
                foreach (var src in sourceValues.Binding)
                {
                    try
                    {
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetValues.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Key", "Name", "Value", "ObjctValue" };
                        BatchCopyProperties(src, tgt, props);
                        // 添加到角色的UserValues集合
                        var collectionName = "Values";
                        if (HasProperty(tgt.Character, collectionName) && tgt.Character.GetType().GetProperty(collectionName).GetValue(tgt.Character) is IList<UserValue> values)
                        {
                            if (!values.Contains(tgt))
                                values.Add(tgt);
                        }
                        else if (HasProperty(tgt.Character, "UserValues") && tgt.Character.GetType().GetProperty("UserValues").GetValue(tgt.Character) is IList<UserValue> userValues)
                        {
                            if (!userValues.Contains(tgt))
                                userValues.Add(tgt);
                        }
                        valueCount++;
                    }
                    catch (Exception ex)
                    {
                        valueFail++;
                        LogTextBox.Text += $"自定义变量迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"自定义变量迁移完成：共{valueCount}个，失败{valueFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"自定义变量迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateStoreSales(Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            try
            {
                var sourceStoreSales = _sourceSession.GetCollection<GameStoreSale>();
                var targetStoreSales = SEnvir.Session.GetCollection<GameStoreSale>();
                var targetItemInfos = SEnvir.ItemInfoList.Binding;

                int salesCount = 0, salesFail = 0;
                int itemMatchCount = 0, itemNoMatchCount = 0;

                LogTextBox.Text += $"开始迁移商城购买记录...\r\n";
                LogTextBox.Text += $"源数据库共有{sourceStoreSales.Binding.Count}条商城记录\r\n";
                Application.DoEvents();

                // 检查源数据库的GameStoreSale结构
                //if (sourceStoreSales.Binding.Count > 0)
                //{
                //    var sampleSale = sourceStoreSales.Binding[0];
                //    LogTextBox.Text += "检查源数据库GameStoreSale结构:\r\n";
                //    foreach (var prop in sampleSale.GetType().GetProperties())
                //    {
                //        try
                //        {
                //            var value = prop.GetValue(sampleSale);
                //            LogTextBox.Text += $"  属性: {prop.Name}, 类型: {prop.PropertyType.Name}, 值: {(value == null ? "null" : value.ToString())}\r\n";
                //        }
                //        catch (Exception ex)
                //        {
                //            LogTextBox.Text += $"  无法读取属性 {prop.Name}: {ex.Message}\r\n";
                //        }
                //    }
                //}

                foreach (var src in sourceStoreSales.Binding)
                {
                    try
                    {
                        if (src == null) continue;

                        // 关联账号
                        if (src.Account == null || !accountMap.ContainsKey(src.Account)) continue;

                        var tgt = targetStoreSales.CreateNewObject();
                        tgt.Account = accountMap[src.Account];

                        // 复制属性
                        if (HasProperty(src, "Date") && HasProperty(tgt, "Date"))
                            SafeCopyProperty(src, tgt, "Date");
                        if (HasProperty(src, "Price") && HasProperty(tgt, "Price"))
                            SafeCopyProperty(src, tgt, "Price");
                        if (HasProperty(src, "Count") && HasProperty(tgt, "Count"))
                            SafeCopyProperty(src, tgt, "Count");
                        if (HasProperty(src, "HuntGold") && HasProperty(tgt, "HuntGold"))
                            SafeCopyProperty(src, tgt, "HuntGold");

                        // 尝试通过Index获取物品信息
                        int itemIndex = -1;
                        if (HasProperty(src, "Index"))
                        {
                            itemIndex = (int)src.GetType().GetProperty("Index").GetValue(src);
                            //LogTextBox.Text += $"  商城记录物品Index: {itemIndex}\r\n";
                        }

                        ItemInfo targetItemInfo = null;

                        // 1. 首先尝试从Item属性获取
                        if (HasProperty(src, "Item") && src.GetType().GetProperty("Item").GetValue(src) is ItemInfo srcItemInfo)
                        {
                            if (srcItemInfo != null)
                            {
                                int srcItemIndex = -1;
                                string srcItemName = null;

                                if (HasProperty(srcItemInfo, "Index"))
                                    srcItemIndex = (int)srcItemInfo.GetType().GetProperty("Index").GetValue(srcItemInfo);
                                if (HasProperty(srcItemInfo, "ItemName"))
                                    srcItemName = srcItemInfo.GetType().GetProperty("ItemName").GetValue(srcItemInfo) as string;

                                //LogTextBox.Text += $"  从Item属性获取: Index={srcItemIndex}, Name={srcItemName ?? "N/A"}\r\n";

                                // 尝试按Index匹配
                                if (srcItemIndex != -1)
                                {
                                    targetItemInfo = targetItemInfos.FirstOrDefault(i =>
                                        HasProperty(i, "Index") && (int)i.GetType().GetProperty("Index").GetValue(i) == srcItemIndex);
                                }

                                // 如果按Index未匹配到，尝试按Name匹配
                                if (targetItemInfo == null && !string.IsNullOrEmpty(srcItemName))
                                {
                                    targetItemInfo = targetItemInfos.FirstOrDefault(i =>
                                        HasProperty(i, "ItemName") && ((string)i.GetType().GetProperty("ItemName").GetValue(i)).Equals(srcItemName, StringComparison.OrdinalIgnoreCase));
                                }
                            }
                        }

                        // 2. 如果Item属性为空，尝试使用Index属性
                        if (targetItemInfo == null && itemIndex != -1)
                        {
                            targetItemInfo = targetItemInfos.FirstOrDefault(i =>
                                HasProperty(i, "Index") && (int)i.GetType().GetProperty("Index").GetValue(i) == itemIndex);

                            if (targetItemInfo != null)
                            {
                                string targetName = targetItemInfo.GetType().GetProperty("ItemName").GetValue(targetItemInfo) as string;
                                LogTextBox.Text += $"  通过Index属性匹配成功: {targetName}\r\n";
                                itemMatchCount++;
                            }
                        }

                        // 3. 设置目标Item属性
                        if (targetItemInfo != null)
                        {
                            if (HasProperty(tgt, "Item"))
                            {
                                tgt.GetType().GetProperty("Item").SetValue(tgt, targetItemInfo);
                                string finalName = targetItemInfo.GetType().GetProperty("ItemName").GetValue(targetItemInfo) as string;
                                LogTextBox.Text += $"  物品: {finalName}\r\n";
                            }
                        }
                        else
                        {
                            itemNoMatchCount++;
                            LogTextBox.Text += $"  警告：无法匹配物品 (Index:{itemIndex})\r\n";
                        }

                        // 确保添加到账号的StoreSales集合
                        if (tgt.Account.StoreSales != null && !tgt.Account.StoreSales.Contains(tgt))
                        {
                            tgt.Account.StoreSales.Add(tgt);
                        }

                        salesCount++;
                    }
                    catch (Exception ex)
                    {
                        salesFail++;
                        LogTextBox.Text += $"商城购买记录迁移失败: ({ex.Message})\r\n";
                    }
                }

                LogTextBox.Text += $"商城购买记录迁移完成：共{salesCount}个，失败{salesFail}个\r\n";
                LogTextBox.Text += $"物品匹配统计：成功{itemMatchCount}个，失败{itemNoMatchCount}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"商城购买记录迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        // 新增方法：迁移物品定义表


        #endregion



        private void MigrateBuffs(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceBuffs = _sourceSession.GetCollection<BuffInfo>();
                var targetBuffs = SEnvir.Session.GetCollection<BuffInfo>();
                int buffCount = 0, buffFail = 0;
                LogTextBox.Text += "开始迁移Buff...\r\n";
                Application.DoEvents();
                foreach (var src in sourceBuffs.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetBuffs.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Type", "Stats", "RemainingTime", "Pause", "Visible", "ItemIndex", "FromCustomBuff", "TickFrequency" };
                        BatchCopyProperties(src, tgt, props);
                        if (tgt.Character.Buffs != null && !tgt.Character.Buffs.Contains(tgt))
                        {
                            tgt.Character.Buffs.Add(tgt);
                        }
                        buffCount++;
                    }
                    catch (Exception ex)
                    {
                        buffFail++;
                        LogTextBox.Text += $"Buff迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"Buff迁移完成：共{buffCount}个，失败{buffFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"Buff迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateBeltLinks(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceLinks = _sourceSession.GetCollection<CharacterBeltLink>();
                var targetLinks = SEnvir.Session.GetCollection<CharacterBeltLink>();
                int linkCount = 0, linkFail = 0;
                LogTextBox.Text += "开始迁移药品快捷栏...\r\n";
                Application.DoEvents();
                foreach (var src in sourceLinks.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetLinks.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Slot", "LinkInfoIndex", "LinkItemIndex", "Enabled", "Cooldown", "LastUsed", "UseCount", "AutoUse", "AutoUseHealth", "AutoUseMana", "ExtraInfo", "Item", "Key", "UseCondition" };
                        BatchCopyProperties(src, tgt, props);
                        if (tgt.Character.BeltLinks != null && !tgt.Character.BeltLinks.Contains(tgt))
                        {
                            tgt.Character.BeltLinks.Add(tgt);
                        }
                        linkCount++;
                    }
                    catch (Exception ex)
                    {
                        linkFail++;
                        LogTextBox.Text += $"药品快捷栏迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"药品快捷栏迁移完成：共{linkCount}个，失败{linkFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"药品快捷栏迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateAutoPotionLinks(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceLinks = _sourceSession.GetCollection<AutoPotionLink>();
                var targetLinks = SEnvir.Session.GetCollection<AutoPotionLink>();
                int linkCount = 0, linkFail = 0;
                LogTextBox.Text += "开始迁移自动喝药栏...\r\n";
                Application.DoEvents();
                foreach (var src in sourceLinks.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetLinks.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Slot", "LinkInfoIndex", "Health", "Mana", "Enabled", "ExtraInfo", "Item", "UseCondition", "UseInterval", "UsePriority" };
                        BatchCopyProperties(src, tgt, props);
                        if (tgt.Character.AutoPotionLinks != null && !tgt.Character.AutoPotionLinks.Contains(tgt))
                        {
                            tgt.Character.AutoPotionLinks.Add(tgt);
                        }
                        linkCount++;
                    }
                    catch (Exception ex)
                    {
                        linkFail++;
                        LogTextBox.Text += $"自动喝药栏迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"自动喝药栏迁移完成：共{linkCount}个，失败{linkFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"自动喝药栏迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateAutoFightLinks(Dictionary<CharacterInfo, CharacterInfo> charMap)
        {
            try
            {
                var sourceLinks = _sourceSession.GetCollection<AutoFightConfig>();
                var targetLinks = SEnvir.Session.GetCollection<AutoFightConfig>();
                int linkCount = 0, linkFail = 0;
                LogTextBox.Text += "开始迁移自动挂机设置...\r\n";
                Application.DoEvents();
                foreach (var src in sourceLinks.Binding)
                {
                    try
                    {
                        if (src == null) continue;
                        if (src.Character == null || !charMap.ContainsKey(src.Character)) continue;
                        var tgt = targetLinks.CreateNewObject();
                        tgt.Character = charMap[src.Character];
                        // 常规属性批量复制
                        string[] props = { "Slot", "Info", "TimeCount", "Enabled", "Priority", "TargetType", "TargetRange", "UseInterval", "LastUsed", "UseCount", "MinHealth", "MinMana", "MaxHealth", "MaxMana", "ExtraInfo", "Magic", "UseCondition", "TargetCondition", "UsePriority", "TargetPriority" };
                        BatchCopyProperties(src, tgt, props);
                        if (tgt.Character.AutoFightLinks != null && !tgt.Character.AutoFightLinks.Contains(tgt))
                        {
                            tgt.Character.AutoFightLinks.Add(tgt);
                        }
                        linkCount++;
                    }
                    catch (Exception ex)
                    {
                        linkFail++;
                        LogTextBox.Text += $"自动挂机设置迁移失败: ({ex.Message})\r\n";
                    }
                }
                LogTextBox.Text += $"自动挂机设置迁移完成：共{linkCount}个，失败{linkFail}个\r\n";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                LogTextBox.Text += $"自动挂机设置迁移过程中发生错误: {ex.Message}\r\n";
            }
        }

        private void MigrateAccounts(
            DBCollection<AccountInfo> sourceAccounts,
            DBCollection<AccountInfo> targetAccounts,
            HashSet<string> existAccountNames,
            Dictionary<AccountInfo, AccountInfo> accountMap)
        {
            LogTextBox.Text += "开始迁移账号...\r\n";
            Application.DoEvents();

            int imported = 0, renamed = 0;
            foreach (var src in sourceAccounts.Binding)
            {
                if (src == null) continue;

                string newName = src.EMailAddress;
                bool isRenamed = false;

                // 检查重名
                if (existAccountNames.Contains(newName.ToLower()))
                {
                    newName = _sourcePrefix + newName;
                    isRenamed = true;
                    renamed++;
                    AccountRenameLog.Add($"\"{src.EMailAddress}\"\t\t\t\"{newName}\"");
                }
                existAccountNames.Add(newName.ToLower());

                // 创建新账号
                var tgt = targetAccounts.CreateNewObject();
                CopyAccount(src, tgt, newName);
                accountMap[src] = tgt;
                imported++;

                LogTextBox.Text += isRenamed
                    ? $"账号重命名: {src.EMailAddress} -> {newName}\r\n"
                    : $"导入新账号: {src.EMailAddress}\r\n";
                Application.DoEvents();
            }

            LogTextBox.Text += $"账号处理完成: 导入 {imported} 个账号，其中 {renamed} 个被重命名\r\n";
            Application.DoEvents();
        }

        // 批量复制属性
        private void BatchCopyProperties(object src, object tgt, string[] props)
        {
            foreach (var prop in props)
            {
                if (HasProperty(src, prop) && HasProperty(tgt, prop))
                    SafeCopyProperty(src, tgt, prop);
            }
        }
    }
}