/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26)
 Source Host           : localhost:3306
 Source Schema         : systemdb

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26)
 File Encoding         : 65001

 Date: 29/05/2025 09:51:42
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for accessoryexpinfo
-- ----------------------------
DROP TABLE IF EXISTS `accessoryexpinfo`;
CREATE TABLE `accessoryexpinfo`  (
  `Index` int(10) NOT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Exp` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for achievementinfo
-- ----------------------------
DROP TABLE IF EXISTS `achievementinfo`;
CREATE TABLE `achievementinfo`  (
  `Index` int(10) NOT NULL,
  `Category` int(11) NULL DEFAULT NULL,
  `Title` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `IsHidden` tinyint(1) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Grade` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for achievementrequirement
-- ----------------------------
DROP TABLE IF EXISTS `achievementrequirement`;
CREATE TABLE `achievementrequirement`  (
  `Index` int(10) NOT NULL,
  `Achievement` int(10) NULL DEFAULT NULL,
  `RequirementType` int(11) NULL DEFAULT NULL,
  `ItemParameter` int(10) NULL DEFAULT NULL,
  `MonsterParameter` int(10) NULL DEFAULT NULL,
  `MapParameter` int(10) NULL DEFAULT NULL,
  `MagicParameter` int(10) NULL DEFAULT NULL,
  `QuestParameter` int(10) NULL DEFAULT NULL,
  `AchievementParameter` int(10) NULL DEFAULT NULL,
  `NPCParameter` int(10) NULL DEFAULT NULL,
  `RequiredAmount` decimal(35, 5) NULL DEFAULT NULL,
  `Reverse` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for achievementreward
-- ----------------------------
DROP TABLE IF EXISTS `achievementreward`;
CREATE TABLE `achievementreward`  (
  `Index` int(10) NOT NULL,
  `Achievement` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `Bound` tinyint(1) NULL DEFAULT NULL,
  `Duration` int(11) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for basestat
-- ----------------------------
DROP TABLE IF EXISTS `basestat`;
CREATE TABLE `basestat`  (
  `Index` int(10) NOT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Health` int(11) NULL DEFAULT NULL,
  `Mana` int(11) NULL DEFAULT NULL,
  `BagWeight` int(11) NULL DEFAULT NULL,
  `WearWeight` int(11) NULL DEFAULT NULL,
  `HandWeight` int(11) NULL DEFAULT NULL,
  `Accuracy` int(11) NULL DEFAULT NULL,
  `Agility` int(11) NULL DEFAULT NULL,
  `MinAC` int(11) NULL DEFAULT NULL,
  `MaxAC` int(11) NULL DEFAULT NULL,
  `MinMR` int(11) NULL DEFAULT NULL,
  `MaxMR` int(11) NULL DEFAULT NULL,
  `MinDC` int(11) NULL DEFAULT NULL,
  `MaxDC` int(11) NULL DEFAULT NULL,
  `MinMC` int(11) NULL DEFAULT NULL,
  `MaxMC` int(11) NULL DEFAULT NULL,
  `MinSC` int(11) NULL DEFAULT NULL,
  `MaxSC` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for castleinfo
-- ----------------------------
DROP TABLE IF EXISTS `castleinfo`;
CREATE TABLE `castleinfo`  (
  `Index` int(10) NOT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Map` int(10) NULL DEFAULT NULL,
  `StartTime` bigint(20) NULL DEFAULT NULL,
  `Duration` bigint(20) NULL DEFAULT NULL,
  `FlagPoint` varchar(23) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CastleRegion` int(10) NULL DEFAULT NULL,
  `AttackSpawnRegion` int(10) NULL DEFAULT NULL,
  `AttackSpawnUnionRegion` int(10) NULL DEFAULT NULL,
  `DefenderSpawnRegion` int(10) NULL DEFAULT NULL,
  `DefenderSpawnUnionRegion` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  `InfoID` int(11) NULL DEFAULT NULL,
  `TaxRate` decimal(35, 5) NULL DEFAULT NULL,
  `Discount` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for companioninfo
-- ----------------------------
DROP TABLE IF EXISTS `companioninfo`;
CREATE TABLE `companioninfo`  (
  `Index` int(10) NOT NULL,
  `MonsterInfo` int(10) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `GameGoldPrice` int(11) NULL DEFAULT NULL,
  `Available` tinyint(1) NULL DEFAULT NULL,
  `Sorting` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for companionlevelinfo
-- ----------------------------
DROP TABLE IF EXISTS `companionlevelinfo`;
CREATE TABLE `companionlevelinfo`  (
  `Index` int(10) NOT NULL,
  `MonsterInfo` int(10) NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `MaxExperience` int(11) NULL DEFAULT NULL,
  `InventorySpace` int(11) NULL DEFAULT NULL,
  `InventoryWeight` int(11) NULL DEFAULT NULL,
  `MaxHunger` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for companionskillinfo
-- ----------------------------
DROP TABLE IF EXISTS `companionskillinfo`;
CREATE TABLE `companionskillinfo`  (
  `Index` int(10) NOT NULL,
  `MonsterInfo` int(10) NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `StatType` int(11) NULL DEFAULT NULL,
  `MaxAmount` int(11) NULL DEFAULT NULL,
  `Weight` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for craftexpinfo
-- ----------------------------
DROP TABLE IF EXISTS `craftexpinfo`;
CREATE TABLE `craftexpinfo`  (
  `Index` int(10) NOT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Exp` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for craftiteminfo
-- ----------------------------
DROP TABLE IF EXISTS `craftiteminfo`;
CREATE TABLE `craftiteminfo`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Blueprint` int(11) NULL DEFAULT NULL,
  `TargetItemType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `SortNumber` int(11) NULL DEFAULT NULL,
  `TargetAmount` int(11) NULL DEFAULT NULL,
  `SuccessRate` int(11) NULL DEFAULT NULL,
  `GainExp` int(11) NULL DEFAULT NULL,
  `TimeCost` int(11) NULL DEFAULT NULL,
  `GoldCost` int(11) NULL DEFAULT NULL,
  `LevelNeeded` int(11) NULL DEFAULT NULL,
  `Item1` int(10) NULL DEFAULT NULL,
  `Amount1` int(11) NULL DEFAULT NULL,
  `Item2` int(10) NULL DEFAULT NULL,
  `Amount2` int(11) NULL DEFAULT NULL,
  `Item3` int(10) NULL DEFAULT NULL,
  `Amount3` int(11) NULL DEFAULT NULL,
  `Item4` int(10) NULL DEFAULT NULL,
  `Amount4` int(11) NULL DEFAULT NULL,
  `Item5` int(10) NULL DEFAULT NULL,
  `Amount5` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for custombuffinfo
-- ----------------------------
DROP TABLE IF EXISTS `custombuffinfo`;
CREATE TABLE `custombuffinfo`  (
  `Index` int(10) NOT NULL,
  `BuffType` int(11) NULL DEFAULT NULL,
  `BuffLV` int(11) NULL DEFAULT NULL,
  `BuffName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `BuffGroup` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `SmallBuffIcon` int(11) NULL DEFAULT NULL,
  `BigBuffIcon` int(11) NULL DEFAULT NULL,
  `GetRate` int(11) NULL DEFAULT NULL,
  `GetRate2` int(11) NULL DEFAULT NULL,
  `Duration` bigint(20) NULL DEFAULT NULL,
  `PauseInSafeZone` tinyint(1) NULL DEFAULT NULL,
  `OfflineTicking` tinyint(1) NULL DEFAULT NULL,
  `DeleteAtMidnight` tinyint(1) NULL DEFAULT NULL,
  `OverheadTitle` int(11) NULL DEFAULT NULL,
  `frameCount` int(11) NULL DEFAULT NULL,
  `offSetX` int(11) NULL DEFAULT NULL,
  `offSetY` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for custombuffinfostat
-- ----------------------------
DROP TABLE IF EXISTS `custombuffinfostat`;
CREATE TABLE `custombuffinfostat`  (
  `Index` int(10) NOT NULL,
  `CustomBuff` int(10) NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for diymagiceffect
-- ----------------------------
DROP TABLE IF EXISTS `diymagiceffect`;
CREATE TABLE `diymagiceffect`  (
  `Index` int(10) NOT NULL,
  `MagicID` int(11) NULL DEFAULT NULL,
  `MagicType` int(11) NULL DEFAULT NULL,
  `startIndex` int(11) NULL DEFAULT NULL,
  `frameCount` int(11) NULL DEFAULT NULL,
  `frameDelay` int(11) NULL DEFAULT NULL,
  `magicDir` int(11) NULL DEFAULT NULL,
  `file` int(11) NULL DEFAULT NULL,
  `startLight` int(11) NULL DEFAULT NULL,
  `endLight` int(11) NULL DEFAULT NULL,
  `ExplosionMagicID` int(11) NULL DEFAULT NULL,
  `lightColour` int(11) NULL DEFAULT NULL,
  `MagicSoundIdx` int(11) NULL DEFAULT NULL,
  `MagicSound` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for dropinfo
-- ----------------------------
DROP TABLE IF EXISTS `dropinfo`;
CREATE TABLE `dropinfo`  (
  `Index` int(10) NOT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Chance` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `DropGroup` int(11) NULL DEFAULT NULL,
  `DropSet` int(11) NULL DEFAULT NULL,
  `PartOnly` tinyint(1) NULL DEFAULT NULL,
  `EasterEvent` tinyint(1) NULL DEFAULT NULL,
  `StrMonsterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `StrItemName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `AmountTemp` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for fishingareainfo
-- ----------------------------
DROP TABLE IF EXISTS `fishingareainfo`;
CREATE TABLE `fishingareainfo`  (
  `Index` int(10) NOT NULL,
  `Region` int(10) NULL DEFAULT NULL,
  `BindRegion` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for fullitemstat
-- ----------------------------
DROP TABLE IF EXISTS `fullitemstat`;
CREATE TABLE `fullitemstat`  (
  `Index` int(10) NOT NULL,
  `SourceName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `StatSource` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for guardinfo
-- ----------------------------
DROP TABLE IF EXISTS `guardinfo`;
CREATE TABLE `guardinfo`  (
  `Index` int(10) NOT NULL,
  `Map` int(10) NULL DEFAULT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `X` int(11) NULL DEFAULT NULL,
  `Y` int(11) NULL DEFAULT NULL,
  `Direction` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `StrMonsterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for guildlevelexp
-- ----------------------------
DROP TABLE IF EXISTS `guildlevelexp`;
CREATE TABLE `guildlevelexp`  (
  `Index` int(10) NOT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Exp` int(11) NULL DEFAULT NULL,
  `ActivCount` bigint(20) NULL DEFAULT NULL,
  `GuildFunds` bigint(20) NULL DEFAULT NULL,
  `TotalContribution` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for itemdisplayeffect
-- ----------------------------
DROP TABLE IF EXISTS `itemdisplayeffect`;
CREATE TABLE `itemdisplayeffect`  (
  `Index` int(10) NOT NULL,
  `Info` int(10) NULL DEFAULT NULL,
  `DrawInnerEffect` tinyint(1) NULL DEFAULT NULL,
  `InnerEffectLibrary` int(11) NULL DEFAULT NULL,
  `InnerImageStartIndex` int(11) NULL DEFAULT NULL,
  `InnerImageCount` int(11) NULL DEFAULT NULL,
  `EffectBehindImage` tinyint(1) NULL DEFAULT NULL,
  `DrawOuterEffect` tinyint(1) NULL DEFAULT NULL,
  `OuterEffectLibrary` int(11) NULL DEFAULT NULL,
  `IsUnisex` tinyint(1) NULL DEFAULT NULL,
  `OuterImageStartIndex` int(11) NULL DEFAULT NULL,
  `OuterImageCount` int(11) NULL DEFAULT NULL,
  `InnerX` int(11) NULL DEFAULT NULL,
  `InnerY` int(11) NULL DEFAULT NULL,
  `OuterX` int(11) NULL DEFAULT NULL,
  `OuterY` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for iteminfo
-- ----------------------------
DROP TABLE IF EXISTS `iteminfo`;
CREATE TABLE `iteminfo`  (
  `Index` int(10) NOT NULL,
  `ItemName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ItemType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `WeaponType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RequiredClass` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RequiredGender` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RequiredType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RequiredAmount` int(11) NULL DEFAULT NULL,
  `Shape` int(11) NULL DEFAULT NULL,
  `Effect` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Image` int(11) NULL DEFAULT NULL,
  `Durability` int(11) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `Weight` int(11) NULL DEFAULT NULL,
  `StackSize` int(11) NULL DEFAULT NULL,
  `StartItem` tinyint(1) NULL DEFAULT NULL,
  `SellRate` decimal(35, 5) NULL DEFAULT NULL,
  `CanRepair` tinyint(1) NULL DEFAULT NULL,
  `CanSell` tinyint(1) NULL DEFAULT NULL,
  `CanStore` tinyint(1) NULL DEFAULT NULL,
  `CanTreasure` tinyint(1) NULL DEFAULT NULL,
  `CanTrade` tinyint(1) NULL DEFAULT NULL,
  `NoMake` tinyint(1) NULL DEFAULT NULL,
  `CanDrop` tinyint(1) NULL DEFAULT NULL,
  `CanDeathDrop` tinyint(1) NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Rarity` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `CanAutoPot` tinyint(1) NULL DEFAULT NULL,
  `BuffIcon` int(11) NULL DEFAULT NULL,
  `PartCount` int(11) NULL DEFAULT NULL,
  `Duration` int(11) NULL DEFAULT NULL,
  `ItemTypeName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `RequiredClassName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `RequiredGenderName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `RequiredTypeName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `EffectName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for iteminfostat
-- ----------------------------
DROP TABLE IF EXISTS `iteminfostat`;
CREATE TABLE `iteminfostat`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `ShowHidden` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for langinfo
-- ----------------------------
DROP TABLE IF EXISTS `langinfo`;
CREATE TABLE `langinfo`  (
  `Index` int(10) NOT NULL,
  `Type` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `LangType` int(11) NULL DEFAULT NULL,
  `Key` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Value` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for magicinfo
-- ----------------------------
DROP TABLE IF EXISTS `magicinfo`;
CREATE TABLE `magicinfo`  (
  `Index` int(10) NOT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Magic` int(11) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `School` int(11) NULL DEFAULT NULL,
  `Action` int(11) NULL DEFAULT NULL,
  `Icon` int(11) NULL DEFAULT NULL,
  `MinBasePower` int(11) NULL DEFAULT NULL,
  `MaxBasePower` int(11) NULL DEFAULT NULL,
  `MinLevelPower` int(11) NULL DEFAULT NULL,
  `MaxLevelPower` int(11) NULL DEFAULT NULL,
  `BaseCost` int(11) NULL DEFAULT NULL,
  `LevelCost` int(11) NULL DEFAULT NULL,
  `NeedLevel1` int(11) NULL DEFAULT NULL,
  `NeedLevel2` int(11) NULL DEFAULT NULL,
  `NeedLevel3` int(11) NULL DEFAULT NULL,
  `Experience1` int(11) NULL DEFAULT NULL,
  `Experience2` int(11) NULL DEFAULT NULL,
  `Experience3` int(11) NULL DEFAULT NULL,
  `Delay` int(11) NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `X` int(11) NULL DEFAULT NULL,
  `Y` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for mapinfo
-- ----------------------------
DROP TABLE IF EXISTS `mapinfo`;
CREATE TABLE `mapinfo`  (
  `Index` int(10) NOT NULL,
  `FileName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MiniMap` int(11) NULL DEFAULT NULL,
  `Background` int(11) NULL DEFAULT NULL,
  `Light` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Weather` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Fight` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `AllowRT` tinyint(1) NULL DEFAULT NULL,
  `AllowFPM` tinyint(1) NULL DEFAULT NULL,
  `SkillDelay` int(11) NULL DEFAULT NULL,
  `CanHorse` tinyint(1) NULL DEFAULT NULL,
  `AllowTT` tinyint(1) NULL DEFAULT NULL,
  `DeathDrop` tinyint(1) NULL DEFAULT NULL,
  `CanMine` tinyint(1) NULL DEFAULT NULL,
  `CanMarriageRecall` tinyint(1) NULL DEFAULT NULL,
  `AllowRecall` tinyint(1) NULL DEFAULT NULL,
  `IsDynamic` tinyint(1) NULL DEFAULT NULL,
  `MinimumLevel` int(11) NULL DEFAULT NULL,
  `MaximumLevel` int(11) NULL DEFAULT NULL,
  `ReconnectMap` int(10) NULL DEFAULT NULL,
  `Music` int(11) NULL DEFAULT NULL,
  `MapSound` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MonsterHealth` int(11) NULL DEFAULT NULL,
  `MonsterDamage` int(11) NULL DEFAULT NULL,
  `MonsterBeware` int(11) NULL DEFAULT NULL,
  `DropRate` int(11) NULL DEFAULT NULL,
  `ExperienceRate` int(11) NULL DEFAULT NULL,
  `GoldRate` int(11) NULL DEFAULT NULL,
  `MaxMonsterHealth` int(11) NULL DEFAULT NULL,
  `MaxMonsterDamage` int(11) NULL DEFAULT NULL,
  `MaxMonsterBeware` int(11) NULL DEFAULT NULL,
  `MaxDropRate` int(11) NULL DEFAULT NULL,
  `MaxExperienceRate` int(11) NULL DEFAULT NULL,
  `MaxGoldRate` int(11) NULL DEFAULT NULL,
  `HealthRate` int(11) NULL DEFAULT NULL,
  `BlackScorching` tinyint(1) NULL DEFAULT NULL,
  `Script` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `NoUseItem` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `BanAndroidPlayer` tinyint(1) NULL DEFAULT NULL,
  `CanPlayName` tinyint(1) NULL DEFAULT NULL,
  `CanNoChat` tinyint(1) NULL DEFAULT NULL,
  `AttackMode` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `CanDeadTownRevive` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for mapregion
-- ----------------------------
DROP TABLE IF EXISTS `mapregion`;
CREATE TABLE `mapregion`  (
  `Index` int(10) NOT NULL,
  `Map` int(10) NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `BitRegion` mediumblob NULL,
  `PointRegion` mediumblob NULL,
  `Size` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for mineinfo
-- ----------------------------
DROP TABLE IF EXISTS `mineinfo`;
CREATE TABLE `mineinfo`  (
  `Index` int(10) NOT NULL,
  `Map` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Chance` int(11) NULL DEFAULT NULL,
  `StrItemName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monactcheck
-- ----------------------------
DROP TABLE IF EXISTS `monactcheck`;
CREATE TABLE `monactcheck`  (
  `Index` int(10) NOT NULL,
  `MonDAction` int(10) NULL DEFAULT NULL,
  `CheckType` int(11) NULL DEFAULT NULL,
  `Operators` int(11) NULL DEFAULT NULL,
  `IntParameter` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for monanimationeffect
-- ----------------------------
DROP TABLE IF EXISTS `monanimationeffect`;
CREATE TABLE `monanimationeffect`  (
  `Index` int(10) NOT NULL,
  `effectfile` int(11) NULL DEFAULT NULL,
  `effectfrom` int(11) NULL DEFAULT NULL,
  `effectframe` int(11) NULL DEFAULT NULL,
  `effectdelay` int(11) NULL DEFAULT NULL,
  `effectdir` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for monanimationframe
-- ----------------------------
DROP TABLE IF EXISTS `monanimationframe`;
CREATE TABLE `monanimationframe`  (
  `Index` int(10) NOT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `MonsterIdx` int(11) NULL DEFAULT NULL,
  `MonAnimation` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `startIndex` int(11) NULL DEFAULT NULL,
  `frameCount` int(11) NULL DEFAULT NULL,
  `offSet` int(11) NULL DEFAULT NULL,
  `frameDelay` int(11) NULL DEFAULT NULL,
  `Reversed` tinyint(1) NULL DEFAULT NULL,
  `StaticSpeed` tinyint(1) NULL DEFAULT NULL,
  `ActSound` int(11) NULL DEFAULT NULL,
  `ActSoundStr` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `effectfile` int(11) NULL DEFAULT NULL,
  `effectfrom` int(11) NULL DEFAULT NULL,
  `effectframe` int(11) NULL DEFAULT NULL,
  `effectdelay` int(11) NULL DEFAULT NULL,
  `effectdir` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monattackeffect
-- ----------------------------
DROP TABLE IF EXISTS `monattackeffect`;
CREATE TABLE `monattackeffect`  (
  `Index` int(10) NOT NULL,
  `MonDAction` int(10) NULL DEFAULT NULL,
  `AtkEffect` int(11) NULL DEFAULT NULL,
  `nParameter` int(11) NULL DEFAULT NULL,
  `sParameter` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for mondiyaiaction
-- ----------------------------
DROP TABLE IF EXISTS `mondiyaiaction`;
CREATE TABLE `mondiyaiaction`  (
  `Index` int(10) NOT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ActType` int(11) NULL DEFAULT NULL,
  `ActionID` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `SystemMagic` int(11) NULL DEFAULT NULL,
  `MagicID` int(11) NULL DEFAULT NULL,
  `PowerType` int(11) NULL DEFAULT NULL,
  `ElementType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Target` int(11) NULL DEFAULT NULL,
  `nDelay` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monstereventaction
-- ----------------------------
DROP TABLE IF EXISTS `monstereventaction`;
CREATE TABLE `monstereventaction`  (
  `Index` int(10) NOT NULL,
  `MonsterEvent` int(10) NULL DEFAULT NULL,
  `TriggerValue` int(11) NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  `StringParameter1` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MonsterParameter1` int(10) NULL DEFAULT NULL,
  `RespawnParameter1` int(10) NULL DEFAULT NULL,
  `RegionParameter1` int(10) NULL DEFAULT NULL,
  `MapParameter1` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monstereventinfo
-- ----------------------------
DROP TABLE IF EXISTS `monstereventinfo`;
CREATE TABLE `monstereventinfo`  (
  `Index` int(10) NOT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MaxValue` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monstereventtarget
-- ----------------------------
DROP TABLE IF EXISTS `monstereventtarget`;
CREATE TABLE `monstereventtarget`  (
  `Index` int(10) NOT NULL,
  `MonsterEvent` int(10) NULL DEFAULT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `DropSet` int(11) NULL DEFAULT NULL,
  `Value` int(11) NULL DEFAULT NULL,
  `StrEventDescription` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monsterinfo
-- ----------------------------
DROP TABLE IF EXISTS `monsterinfo`;
CREATE TABLE `monsterinfo`  (
  `Index` int(10) NOT NULL,
  `MonsterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Image` int(11) NULL DEFAULT NULL,
  `File` int(11) NULL DEFAULT NULL,
  `AttackSound` int(11) NULL DEFAULT NULL,
  `StruckSound` int(11) NULL DEFAULT NULL,
  `DieSound` int(11) NULL DEFAULT NULL,
  `BodyShape` int(11) NULL DEFAULT NULL,
  `Portrait` int(11) NULL DEFAULT NULL,
  `AI` int(11) NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `ViewRange` int(11) NULL DEFAULT NULL,
  `CoolEye` int(11) NULL DEFAULT NULL,
  `Experience` decimal(35, 5) NULL DEFAULT NULL,
  `Undead` tinyint(1) NULL DEFAULT NULL,
  `CanPush` tinyint(1) NULL DEFAULT NULL,
  `CanTame` tinyint(1) NULL DEFAULT NULL,
  `AttackDelay` int(11) NULL DEFAULT NULL,
  `MoveDelay` int(11) NULL DEFAULT NULL,
  `IsBoss` tinyint(1) NULL DEFAULT NULL,
  `Flag` int(11) NULL DEFAULT NULL,
  `CallFightPet` tinyint(1) NULL DEFAULT NULL,
  `CallMapSummoning` tinyint(1) NULL DEFAULT NULL,
  `DiyMoveMode` int(11) NULL DEFAULT NULL,
  `IsSlow` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for monsterinfostat
-- ----------------------------
DROP TABLE IF EXISTS `monsterinfostat`;
CREATE TABLE `monsterinfostat`  (
  `Index` int(10) NOT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for movementinfo
-- ----------------------------
DROP TABLE IF EXISTS `movementinfo`;
CREATE TABLE `movementinfo`  (
  `Index` int(10) NOT NULL,
  `SourceRegion` int(10) NULL DEFAULT NULL,
  `DestinationRegion` int(10) NULL DEFAULT NULL,
  `Icon` int(11) NULL DEFAULT NULL,
  `NeedItem` int(10) NULL DEFAULT NULL,
  `NeedSpawn` int(10) NULL DEFAULT NULL,
  `Effect` int(11) NULL DEFAULT NULL,
  `RequiredClass` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `ExtraInfo` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CanLinkTips` tinyint(1) NULL DEFAULT NULL,
  `SpanTime` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for npcaction
-- ----------------------------
DROP TABLE IF EXISTS `npcaction`;
CREATE TABLE `npcaction`  (
  `Index` int(10) NOT NULL,
  `ActionType` int(11) NULL DEFAULT NULL,
  `StringParameter1` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `IntParameter1` int(11) NULL DEFAULT NULL,
  `IntParameter2` int(11) NULL DEFAULT NULL,
  `ItemParameter1` int(10) NULL DEFAULT NULL,
  `MapParameter1` int(10) NULL DEFAULT NULL,
  `StatParameter1` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for npcbutton
-- ----------------------------
DROP TABLE IF EXISTS `npcbutton`;
CREATE TABLE `npcbutton`  (
  `Index` int(10) NOT NULL,
  `ButtonID` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for npccheck
-- ----------------------------
DROP TABLE IF EXISTS `npccheck`;
CREATE TABLE `npccheck`  (
  `Index` int(10) NOT NULL,
  `CheckType` int(11) NULL DEFAULT NULL,
  `Operator` int(11) NULL DEFAULT NULL,
  `StringParameter1` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `IntParameter1` int(11) NULL DEFAULT NULL,
  `IntParameter2` int(11) NULL DEFAULT NULL,
  `ItemParameter1` int(10) NULL DEFAULT NULL,
  `StatParameter1` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for npcgood
-- ----------------------------
DROP TABLE IF EXISTS `npcgood`;
CREATE TABLE `npcgood`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Rate` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for npcinfo
-- ----------------------------
DROP TABLE IF EXISTS `npcinfo`;
CREATE TABLE `npcinfo`  (
  `Index` int(10) NOT NULL,
  `NPCFile` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Region` int(10) NULL DEFAULT NULL,
  `NPCName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Image` int(11) NULL DEFAULT NULL,
  `Display` tinyint(1) NULL DEFAULT NULL,
  `WarDisplay` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for npctype
-- ----------------------------
DROP TABLE IF EXISTS `npctype`;
CREATE TABLE `npctype`  (
  `Index` int(10) NOT NULL,
  `ItemType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for playerexpinfo
-- ----------------------------
DROP TABLE IF EXISTS `playerexpinfo`;
CREATE TABLE `playerexpinfo`  (
  `Index` int(10) NOT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Exp` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for questinfo
-- ----------------------------
DROP TABLE IF EXISTS `questinfo`;
CREATE TABLE `questinfo`  (
  `Index` int(10) NOT NULL,
  `QuestName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `QuestType` int(11) NULL DEFAULT NULL,
  `AcceptText` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ProgressText` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CompletedText` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ArchiveText` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `StartNPC` int(10) NULL DEFAULT NULL,
  `FinishNPC` int(10) NULL DEFAULT NULL,
  `StartItem` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for questrequirement
-- ----------------------------
DROP TABLE IF EXISTS `questrequirement`;
CREATE TABLE `questrequirement`  (
  `Index` int(10) NOT NULL,
  `Quest` int(10) NULL DEFAULT NULL,
  `Requirement` int(11) NULL DEFAULT NULL,
  `IntParameter1` int(11) NULL DEFAULT NULL,
  `QuestParameter` int(10) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `QuertInfoIndex` int(11) NULL DEFAULT NULL,
  `RequirementName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ClassName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `QuestParameterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for questreward
-- ----------------------------
DROP TABLE IF EXISTS `questreward`;
CREATE TABLE `questreward`  (
  `Index` int(10) NOT NULL,
  `Quest` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `Choice` tinyint(1) NULL DEFAULT NULL,
  `Random` tinyint(1) NULL DEFAULT NULL,
  `Bound` tinyint(1) NULL DEFAULT NULL,
  `Duration` int(11) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `QuertInfoIndex` int(11) NULL DEFAULT NULL,
  `ItemName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ClassName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for questtask
-- ----------------------------
DROP TABLE IF EXISTS `questtask`;
CREATE TABLE `questtask`  (
  `Index` int(10) NOT NULL,
  `Quest` int(10) NULL DEFAULT NULL,
  `Task` int(11) NULL DEFAULT NULL,
  `ItemParameter` int(10) NULL DEFAULT NULL,
  `MobDescription` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `QuertInfoIndex` int(11) NULL DEFAULT NULL,
  `TaskName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ItemParameterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `IndexCopy` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for questtaskmonsterdetails
-- ----------------------------
DROP TABLE IF EXISTS `questtaskmonsterdetails`;
CREATE TABLE `questtaskmonsterdetails`  (
  `Index` int(10) NOT NULL,
  `Task` int(10) NULL DEFAULT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `Map` int(10) NULL DEFAULT NULL,
  `Chance` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `DropSet` int(11) NULL DEFAULT NULL,
  `QuerTaskIndex` int(11) NULL DEFAULT NULL,
  `MonsterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MapName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for respawninfo
-- ----------------------------
DROP TABLE IF EXISTS `respawninfo`;
CREATE TABLE `respawninfo`  (
  `Index` int(10) NOT NULL,
  `Monster` int(10) NULL DEFAULT NULL,
  `Region` int(10) NULL DEFAULT NULL,
  `MapID` int(11) NULL DEFAULT NULL,
  `MapX` int(11) NULL DEFAULT NULL,
  `MapY` int(11) NULL DEFAULT NULL,
  `Range` int(11) NULL DEFAULT NULL,
  `EventSpawn` tinyint(1) NULL DEFAULT NULL,
  `Delay` int(11) NULL DEFAULT NULL,
  `Count` int(11) NULL DEFAULT NULL,
  `RandomTime` int(11) NULL DEFAULT NULL,
  `DropSet` int(11) NULL DEFAULT NULL,
  `Announce` tinyint(1) NULL DEFAULT NULL,
  `Punctual` tinyint(1) NULL DEFAULT NULL,
  `EasterEventChance` int(11) NULL DEFAULT NULL,
  `StrMapFileName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `StrMapRegionDescition` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for safezoneinfo
-- ----------------------------
DROP TABLE IF EXISTS `safezoneinfo`;
CREATE TABLE `safezoneinfo`  (
  `Index` int(10) NOT NULL,
  `Region` int(10) NULL DEFAULT NULL,
  `BindRegion` int(10) NULL DEFAULT NULL,
  `StartPointX` int(11) NULL DEFAULT NULL,
  `StartPointY` int(11) NULL DEFAULT NULL,
  `StartClass` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RedZone` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for setgroup
-- ----------------------------
DROP TABLE IF EXISTS `setgroup`;
CREATE TABLE `setgroup`  (
  `Index` int(10) NOT NULL,
  `GroupName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `SetRequirement` int(11) NULL DEFAULT NULL,
  `RequiredNumItems` int(11) NULL DEFAULT NULL,
  `Set` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for setgroupitem
-- ----------------------------
DROP TABLE IF EXISTS `setgroupitem`;
CREATE TABLE `setgroupitem`  (
  `Index` int(10) NOT NULL,
  `SetGroupInfo` int(10) NULL DEFAULT NULL,
  `SetGroupItemInfo` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for setinfo
-- ----------------------------
DROP TABLE IF EXISTS `setinfo`;
CREATE TABLE `setinfo`  (
  `Index` int(10) NOT NULL,
  `SetName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `SetInfoShow` tinyint(1) NULL DEFAULT NULL,
  `SetDescription` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for setinfostat
-- ----------------------------
DROP TABLE IF EXISTS `setinfostat`;
CREATE TABLE `setinfostat`  (
  `Index` int(10) NOT NULL,
  `Group` int(10) NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for storeinfo
-- ----------------------------
DROP TABLE IF EXISTS `storeinfo`;
CREATE TABLE `storeinfo`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `HuntGoldPrice` int(11) NULL DEFAULT NULL,
  `Filter` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Available` tinyint(1) NULL DEFAULT NULL,
  `Duration` int(11) NULL DEFAULT NULL,
  `Recommend` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for triggercondition
-- ----------------------------
DROP TABLE IF EXISTS `triggercondition`;
CREATE TABLE `triggercondition`  (
  `Index` int(10) NOT NULL,
  `Trigger` int(10) NULL DEFAULT NULL,
  `ConditionType` int(11) NULL DEFAULT NULL,
  `TimeParameter` bigint(20) NULL DEFAULT NULL,
  `StatParameter` int(11) NULL DEFAULT NULL,
  `ItemParameter` int(10) NULL DEFAULT NULL,
  `MonsterParameter` int(10) NULL DEFAULT NULL,
  `MapParameter` int(10) NULL DEFAULT NULL,
  `MagicParameter` int(10) NULL DEFAULT NULL,
  `RequiredAmount` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for triggereffect
-- ----------------------------
DROP TABLE IF EXISTS `triggereffect`;
CREATE TABLE `triggereffect`  (
  `Index` int(10) NOT NULL,
  `Trigger` int(10) NULL DEFAULT NULL,
  `EffectType` int(11) NULL DEFAULT NULL,
  `Duration` bigint(20) NULL DEFAULT NULL,
  `StatParameter` int(11) NULL DEFAULT NULL,
  `ItemParameter` int(10) NULL DEFAULT NULL,
  `MonsterParameter` int(10) NULL DEFAULT NULL,
  `MapParameter` int(10) NULL DEFAULT NULL,
  `MagicParameter` int(10) NULL DEFAULT NULL,
  `GlobalNotification` tinyint(1) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for triggerinfo
-- ----------------------------
DROP TABLE IF EXISTS `triggerinfo`;
CREATE TABLE `triggerinfo`  (
  `Index` int(10) NOT NULL,
  `TriggerName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `TriggerDescription` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Probability` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for weaponcraftstatinfo
-- ----------------------------
DROP TABLE IF EXISTS `weaponcraftstatinfo`;
CREATE TABLE `weaponcraftstatinfo`  (
  `Index` int(10) NOT NULL,
  `RequiredClass` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `MinValue` int(11) NULL DEFAULT NULL,
  `MaxValue` int(11) NULL DEFAULT NULL,
  `Weight` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for weaponexpinfo
-- ----------------------------
DROP TABLE IF EXISTS `weaponexpinfo`;
CREATE TABLE `weaponexpinfo`  (
  `Index` int(10) NOT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Exp` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for weaponupgradenew
-- ----------------------------
DROP TABLE IF EXISTS `weaponupgradenew`;
CREATE TABLE `weaponupgradenew`  (
  `Index` int(10) NOT NULL,
  `Increment` int(11) NULL DEFAULT NULL,
  `BlackIronOre` int(11) NULL DEFAULT NULL,
  `SpendGold` int(11) NULL DEFAULT NULL,
  `BasicFragment` int(11) NULL DEFAULT NULL,
  `AdvanceFragment` int(11) NULL DEFAULT NULL,
  `SeniorFragment` int(11) NULL DEFAULT NULL,
  `RefinementStone` int(11) NULL DEFAULT NULL,
  `SuccessRate` int(11) NULL DEFAULT NULL,
  `FailureRate` int(11) NULL DEFAULT NULL,
  `TimeCost` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

SET FOREIGN_KEY_CHECKS = 1;
