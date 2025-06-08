/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26)
 Source Host           : localhost:3306
 Source Schema         : userdb

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26)
 File Encoding         : 65001

 Date: 29/05/2025 09:51:21
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for accountinfo
-- ----------------------------
DROP TABLE IF EXISTS `accountinfo`;
CREATE TABLE `accountinfo`  (
  `Index` int(10) NOT NULL,
  `EMailAddress` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Password` blob NULL,
  `RealName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `BirthDate` datetime(6) NULL DEFAULT NULL,
  `Question` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Answer` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Referral` int(10) NULL DEFAULT NULL,
  `CreationIP` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CreationDate` datetime(6) NULL DEFAULT NULL,
  `LastIP` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `LastLogin` datetime(6) NULL DEFAULT NULL,
  `ActivationKey` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ActivationTime` datetime(6) NULL DEFAULT NULL,
  `Activated` tinyint(1) NULL DEFAULT NULL,
  `ResetKey` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ResetTime` datetime(6) NULL DEFAULT NULL,
  `PasswordTime` datetime(6) NULL DEFAULT NULL,
  `ChatBanExpiry` datetime(6) NULL DEFAULT NULL,
  `Banned` tinyint(1) NULL DEFAULT NULL,
  `ExpiryDate` datetime(6) NULL DEFAULT NULL,
  `BanReason` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `FlashTime` datetime(6) NULL DEFAULT NULL,
  `AutoTime` bigint(20) NULL DEFAULT NULL,
  `Gold` bigint(20) NULL DEFAULT NULL,
  `GameGold` int(11) NULL DEFAULT NULL,
  `AllowGroup` tinyint(1) NULL DEFAULT NULL,
  `AllowFriend` tinyint(1) NULL DEFAULT NULL,
  `AllowTrade` tinyint(1) NULL DEFAULT NULL,
  `AllowResurrectionOrder` tinyint(1) NULL DEFAULT NULL,
  `AllowGuild` tinyint(1) NULL DEFAULT NULL,
  `AllowGroupRecall` tinyint(1) NULL DEFAULT NULL,
  `GuildMember` int(10) NULL DEFAULT NULL,
  `GlobalTime` datetime(6) NULL DEFAULT NULL,
  `Horse` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `HuntGold` int(11) NULL DEFAULT NULL,
  `Admin` tinyint(1) NULL DEFAULT NULL,
  `StorageSize` int(11) NULL DEFAULT NULL,
  `PatchGridSize` int(11) NULL DEFAULT NULL,
  `LastSum` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `GoldBot` tinyint(1) NULL DEFAULT NULL,
  `ItemBot` tinyint(1) NULL DEFAULT NULL,
  `GuildTime` datetime(6) NULL DEFAULT NULL,
  `Observer` tinyint(1) NULL DEFAULT NULL,
  `LastCharacter` int(10) NULL DEFAULT NULL,
  `InviteCode` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auctionhistoryinfo
-- ----------------------------
DROP TABLE IF EXISTS `auctionhistoryinfo`;
CREATE TABLE `auctionhistoryinfo`  (
  `Index` int(10) NOT NULL,
  `Info` int(11) NULL DEFAULT NULL,
  `SaleCount` bigint(20) NULL DEFAULT NULL,
  `LastPrice` int(11) NULL DEFAULT NULL,
  `Average` blob NULL,
  `GameGoldAverage` blob NULL,
  `LastGameGoldPrice` int(11) NULL DEFAULT NULL,
  `PartIndex` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auctioninfo
-- ----------------------------
DROP TABLE IF EXISTS `auctioninfo`;
CREATE TABLE `auctioninfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `PriceType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Message` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for autofightconfig
-- ----------------------------
DROP TABLE IF EXISTS `autofightconfig`;
CREATE TABLE `autofightconfig`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Slot` int(11) NULL DEFAULT NULL,
  `MagicIndex` int(11) NULL DEFAULT NULL,
  `TimeCount` int(11) NULL DEFAULT NULL,
  `Enabled` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for autopotionlink
-- ----------------------------
DROP TABLE IF EXISTS `autopotionlink`;
CREATE TABLE `autopotionlink`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Slot` int(11) NULL DEFAULT NULL,
  `LinkInfoIndex` int(11) NULL DEFAULT NULL,
  `Health` int(11) NULL DEFAULT NULL,
  `Mana` int(11) NULL DEFAULT NULL,
  `Enabled` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for blockinfo
-- ----------------------------
DROP TABLE IF EXISTS `blockinfo`;
CREATE TABLE `blockinfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `BlockedAccount` int(10) NULL DEFAULT NULL,
  `BlockedName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for buffinfo
-- ----------------------------
DROP TABLE IF EXISTS `buffinfo`;
CREATE TABLE `buffinfo`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  `Stats` blob NULL,
  `RemainingTime` bigint(20) NULL DEFAULT NULL,
  `TickFrequency` bigint(20) NULL DEFAULT NULL,
  `TickTime` bigint(20) NULL DEFAULT NULL,
  `ItemIndex` int(11) NULL DEFAULT NULL,
  `FromCustomBuff` int(11) NULL DEFAULT NULL,
  `Visible` tinyint(1) NULL DEFAULT NULL,
  `Pause` tinyint(1) NULL DEFAULT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for castlefundinfo
-- ----------------------------
DROP TABLE IF EXISTS `castlefundinfo`;
CREATE TABLE `castlefundinfo`  (
  `Index` int(10) NOT NULL,
  `Castle` int(10) NULL DEFAULT NULL,
  `TotalTax` bigint(20) NULL DEFAULT NULL,
  `TotalDeposit` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for characterbeltlink
-- ----------------------------
DROP TABLE IF EXISTS `characterbeltlink`;
CREATE TABLE `characterbeltlink`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Slot` int(11) NULL DEFAULT NULL,
  `LinkInfoIndex` int(11) NULL DEFAULT NULL,
  `LinkItemIndex` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for characterinfo
-- ----------------------------
DROP TABLE IF EXISTS `characterinfo`;
CREATE TABLE `characterinfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `CharacterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Gender` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `PatchGridSize` int(11) NULL DEFAULT NULL,
  `FixedPointTCount` int(11) NULL DEFAULT NULL,
  `HairType` int(11) NULL DEFAULT NULL,
  `HairColour` int(11) NULL DEFAULT NULL,
  `ArmourColour` int(11) NULL DEFAULT NULL,
  `LastLogin` datetime(6) NULL DEFAULT NULL,
  `Deleted` tinyint(1) NULL DEFAULT NULL,
  `ProhibitListing` tinyint(1) NULL DEFAULT NULL,
  `CreationDate` datetime(6) NULL DEFAULT NULL,
  `CreationIP` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CurrentLocation` varchar(23) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CurrentMap` int(10) NULL DEFAULT NULL,
  `CharacterState` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Direction` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `BindPoint` int(10) NULL DEFAULT NULL,
  `CurrentHP` int(11) NULL DEFAULT NULL,
  `CurrentMP` int(11) NULL DEFAULT NULL,
  `DayExpAdd` bigint(20) NULL DEFAULT NULL,
  `DayDonations` bigint(20) NULL DEFAULT NULL,
  `DayActiveCount` bigint(20) NULL DEFAULT NULL,
  `TotalActiveCount` bigint(20) NULL DEFAULT NULL,
  `Experience` decimal(35, 5) NULL DEFAULT NULL,
  `Prestige` int(11) NULL DEFAULT NULL,
  `Contribute` int(11) NULL DEFAULT NULL,
  `CanThrusting` tinyint(1) NULL DEFAULT NULL,
  `CanHalfMoon` tinyint(1) NULL DEFAULT NULL,
  `CanDestructiveSurge` tinyint(1) NULL DEFAULT NULL,
  `CanFlameSplash` tinyint(1) NULL DEFAULT NULL,
  `LastStats` blob NULL,
  `HermitStats` blob NULL,
  `SpentPoints` int(11) NULL DEFAULT NULL,
  `AttackMode` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `PetMode` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Observable` tinyint(1) NULL DEFAULT NULL,
  `ItemReviveTime` datetime(6) NULL DEFAULT NULL,
  `ReincarnationPillTime` datetime(6) NULL DEFAULT NULL,
  `MarriageTeleportTime` datetime(6) NULL DEFAULT NULL,
  `GroupRecallTime` datetime(6) NULL DEFAULT NULL,
  `HideHelmet` tinyint(1) NULL DEFAULT NULL,
  `HideShield` tinyint(1) NULL DEFAULT NULL,
  `HideFashion` tinyint(1) NULL DEFAULT NULL,
  `CanDeathDrop` tinyint(1) NULL DEFAULT NULL,
  `Horse` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Rebirth` int(11) NULL DEFAULT NULL,
  `CraftLevel` int(11) NULL DEFAULT NULL,
  `CraftExp` int(11) NULL DEFAULT NULL,
  `RepeatableQuestCount` int(11) NULL DEFAULT NULL,
  `DailyQuestCount` int(11) NULL DEFAULT NULL,
  `CraftFinishTime` datetime(6) NULL DEFAULT NULL,
  `BookmarkedCraftItemInfo` int(10) NULL DEFAULT NULL,
  `CraftingItem` int(10) NULL DEFAULT NULL,
  `AchievementTitle` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `NextDeathDropChange` datetime(6) NULL DEFAULT NULL,
  `DailyFreeTossUsed` int(11) NULL DEFAULT NULL,
  `RewardPoolCoin` decimal(35, 5) NULL DEFAULT NULL,
  `TotalRewardPoolCoinEarned` decimal(35, 5) NULL DEFAULT NULL,
  `TotalRewardPoolCoinCashedOut` decimal(35, 5) NULL DEFAULT NULL,
  `TodayOnlineMinutes` int(11) NULL DEFAULT NULL,
  `Companion` int(10) NULL DEFAULT NULL,
  `Partner` int(10) NULL DEFAULT NULL,
  `CPUInfo` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MACInfo` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `HDDInfo` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `MaxFollower` int(11) NULL DEFAULT NULL,
  `AllowFollowing` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for charactershop
-- ----------------------------
DROP TABLE IF EXISTS `charactershop`;
CREATE TABLE `charactershop`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Date` datetime(6) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Price` bigint(20) NULL DEFAULT NULL,
  `PriceType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `IsSell` tinyint(1) NULL DEFAULT NULL,
  `BuyAccount` int(10) NULL DEFAULT NULL,
  `BuyDate` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for fixedpointinfo
-- ----------------------------
DROP TABLE IF EXISTS `fixedpointinfo`;
CREATE TABLE `fixedpointinfo`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `NameColour` int(11) NULL DEFAULT NULL,
  `QuestIndex` int(11) NULL DEFAULT NULL,
  `Pos` varchar(23) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for friendinfo
-- ----------------------------
DROP TABLE IF EXISTS `friendinfo`;
CREATE TABLE `friendinfo`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `AddDate` datetime(6) NULL DEFAULT NULL,
  `EMailAddress` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `LinkID` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gamegoldpayment
-- ----------------------------
DROP TABLE IF EXISTS `gamegoldpayment`;
CREATE TABLE `gamegoldpayment`  (
  `Index` int(10) NOT NULL,
  `RawMessage` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CharacterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `PaymentDate` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `TransactionID` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `TransactionType` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Status` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `GameGoldAmount` int(11) NULL DEFAULT NULL,
  `Receiver_EMail` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Payer_EMail` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Payer_ID` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Price` decimal(35, 5) NULL DEFAULT NULL,
  `Currency` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Fee` decimal(35, 5) NULL DEFAULT NULL,
  `Error` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gamegoldset
-- ----------------------------
DROP TABLE IF EXISTS `gamegoldset`;
CREATE TABLE `gamegoldset`  (
  `Index` int(10) NOT NULL,
  `Price` decimal(35, 5) NULL DEFAULT NULL,
  `GameGoldAmount` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for gamestoresale
-- ----------------------------
DROP TABLE IF EXISTS `gamestoresale`;
CREATE TABLE `gamestoresale`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Date` datetime(6) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `Count` bigint(20) NULL DEFAULT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `HuntGold` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for golblevalue
-- ----------------------------
DROP TABLE IF EXISTS `golblevalue`;
CREATE TABLE `golblevalue`  (
  `Index` int(10) NOT NULL,
  `Key` int(10) NOT NULL,
  `Value` int(10) NOT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for goldmarketinfo
-- ----------------------------
DROP TABLE IF EXISTS `goldmarketinfo`;
CREATE TABLE `goldmarketinfo`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Date` datetime(6) NULL DEFAULT NULL,
  `TradeState` int(11) NULL DEFAULT NULL,
  `TradeType` int(11) NULL DEFAULT NULL,
  `GoldCount` bigint(20) NULL DEFAULT NULL,
  `GameGoldPrice` bigint(20) NULL DEFAULT NULL,
  `CompletedCount` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for guildallianceinfo
-- ----------------------------
DROP TABLE IF EXISTS `guildallianceinfo`;
CREATE TABLE `guildallianceinfo`  (
  `Index` int(10) NOT NULL,
  `Guild1` int(10) NULL DEFAULT NULL,
  `Guild2` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for guildfundchangeinfo
-- ----------------------------
DROP TABLE IF EXISTS `guildfundchangeinfo`;
CREATE TABLE `guildfundchangeinfo`  (
  `Index` int(10) NOT NULL,
  `Guild` int(10) NULL DEFAULT NULL,
  `CharacterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Amount` bigint(20) NULL DEFAULT NULL,
  `OperationTime` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for guildinfo
-- ----------------------------
DROP TABLE IF EXISTS `guildinfo`;
CREATE TABLE `guildinfo`  (
  `Index` int(10) NOT NULL,
  `GuildName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `GuildFlag` int(11) NULL DEFAULT NULL,
  `FlagColor` int(11) NULL DEFAULT NULL,
  `MemberLimit` int(11) NULL DEFAULT NULL,
  `StorageSize` int(11) NULL DEFAULT NULL,
  `GuildFunds` bigint(20) NULL DEFAULT NULL,
  `GuildLevel` int(11) NULL DEFAULT NULL,
  `GuildExp` bigint(20) NULL DEFAULT NULL,
  `GuildNotice` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `GuildTax` decimal(35, 5) NULL DEFAULT NULL,
  `ActivCount` bigint(20) NULL DEFAULT NULL,
  `DailyActivCount` bigint(20) NULL DEFAULT NULL,
  `TotalContribution` bigint(20) NULL DEFAULT NULL,
  `GameGoldTotal` bigint(20) NULL DEFAULT NULL,
  `DailyContribution` bigint(20) NULL DEFAULT NULL,
  `DailyGrowth` bigint(20) NULL DEFAULT NULL,
  `DefaultRank` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `DefaultPermission` int(11) NULL DEFAULT NULL,
  `StarterGuild` tinyint(1) NULL DEFAULT NULL,
  `AllowApply` tinyint(1) NULL DEFAULT NULL,
  `GuildVaultNotice` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Conquest` int(10) NULL DEFAULT NULL,
  `Castle` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for guildmemberinfo
-- ----------------------------
DROP TABLE IF EXISTS `guildmemberinfo`;
CREATE TABLE `guildmemberinfo`  (
  `Index` int(10) NOT NULL,
  `Guild` int(10) NULL DEFAULT NULL,
  `Rank` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `JoinDate` datetime(6) NULL DEFAULT NULL,
  `TotalContribution` bigint(20) NULL DEFAULT NULL,
  `ActiveCount` bigint(20) NULL DEFAULT NULL,
  `DailyContribution` bigint(20) NULL DEFAULT NULL,
  `GameGoldTotal` bigint(20) NULL DEFAULT NULL,
  `Permission` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for guildwarinfo
-- ----------------------------
DROP TABLE IF EXISTS `guildwarinfo`;
CREATE TABLE `guildwarinfo`  (
  `Index` int(10) NOT NULL,
  `Guild1` int(10) NULL DEFAULT NULL,
  `Guild2` int(10) NULL DEFAULT NULL,
  `Duration` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for mailinfo
-- ----------------------------
DROP TABLE IF EXISTS `mailinfo`;
CREATE TABLE `mailinfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Sender` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Date` datetime(6) NULL DEFAULT NULL,
  `Subject` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Message` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Opened` tinyint(1) NULL DEFAULT NULL,
  `HasItem` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for newautioninfo
-- ----------------------------
DROP TABLE IF EXISTS `newautioninfo`;
CREATE TABLE `newautioninfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Date` datetime(6) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Price` bigint(20) NULL DEFAULT NULL,
  `LastPrice` bigint(20) NULL DEFAULT NULL,
  `LastCharacter` int(10) NULL DEFAULT NULL,
  `BuyItNowPrice` bigint(20) NULL DEFAULT NULL,
  `PriceAdd` bigint(20) NULL DEFAULT NULL,
  `PriceType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Closed` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for redpacketclaiminfo
-- ----------------------------
DROP TABLE IF EXISTS `redpacketclaiminfo`;
CREATE TABLE `redpacketclaiminfo`  (
  `Index` int(10) NOT NULL,
  `RedPacket` int(10) NULL DEFAULT NULL,
  `Claimer` int(10) NULL DEFAULT NULL,
  `Amount` decimal(35, 5) NULL DEFAULT NULL,
  `ClaimTime` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for redpacketinfo
-- ----------------------------
DROP TABLE IF EXISTS `redpacketinfo`;
CREATE TABLE `redpacketinfo`  (
  `Index` int(10) NOT NULL,
  `Currency` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `FaceValue` decimal(35, 5) NULL DEFAULT NULL,
  `RemainingValue` decimal(35, 5) NULL DEFAULT NULL,
  `TotalCount` int(11) NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  `Scope` int(11) NULL DEFAULT NULL,
  `Message` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Sender` int(10) NULL DEFAULT NULL,
  `SendTime` datetime(6) NULL DEFAULT NULL,
  `ExpireTime` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for refineinfo
-- ----------------------------
DROP TABLE IF EXISTS `refineinfo`;
CREATE TABLE `refineinfo`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Weapon` int(10) NULL DEFAULT NULL,
  `Quality` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Type` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `RetrieveTime` datetime(6) NULL DEFAULT NULL,
  `Chance` int(11) NULL DEFAULT NULL,
  `MaxChance` int(11) NULL DEFAULT NULL,
  `IsNewWeaponUpgrade` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for rewardpoolinfo
-- ----------------------------
DROP TABLE IF EXISTS `rewardpoolinfo`;
CREATE TABLE `rewardpoolinfo`  (
  `Index` int(10) NOT NULL,
  `CurrencyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `Balance` decimal(18, 2) NOT NULL,
  `MaxAmount` int(10) NOT NULL,
  `Tier1UpperLimit` int(10) NOT NULL,
  `Tier2UpperLimit` int(10) NOT NULL,
  `Tier3UpperLimit` int(10) NOT NULL,
  `Tier4UpperLimit` int(10) NOT NULL,
  `Tier5UpperLimit` int(10) NOT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for userachievement
-- ----------------------------
DROP TABLE IF EXISTS `userachievement`;
CREATE TABLE `userachievement`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `AchievementName` int(10) NULL DEFAULT NULL,
  `Completed` tinyint(1) NULL DEFAULT NULL,
  `CompleteDate` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for userachievementrequirement
-- ----------------------------
DROP TABLE IF EXISTS `userachievementrequirement`;
CREATE TABLE `userachievementrequirement`  (
  `Index` int(10) NOT NULL,
  `Achievement` int(10) NULL DEFAULT NULL,
  `Requirement` int(10) NULL DEFAULT NULL,
  `CurrentValue` decimal(35, 5) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for usercompanion
-- ----------------------------
DROP TABLE IF EXISTS `usercompanion`;
CREATE TABLE `usercompanion`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Info` int(10) NULL DEFAULT NULL,
  `Name` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Hunger` int(11) NULL DEFAULT NULL,
  `Experience` int(11) NULL DEFAULT NULL,
  `Level3` blob NULL,
  `Level5` blob NULL,
  `Level7` blob NULL,
  `Level10` blob NULL,
  `Level11` blob NULL,
  `Level13` blob NULL,
  `Level15` blob NULL,
  `AutoFeed` tinyint(1) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usercompanionunlock
-- ----------------------------
DROP TABLE IF EXISTS `usercompanionunlock`;
CREATE TABLE `usercompanionunlock`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `CompanionInfo` int(10) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for userconquest
-- ----------------------------
DROP TABLE IF EXISTS `userconquest`;
CREATE TABLE `userconquest`  (
  `Index` int(10) NOT NULL,
  `Guild` int(10) NULL DEFAULT NULL,
  `Castle` int(10) NULL DEFAULT NULL,
  `WarDate` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for userconqueststats
-- ----------------------------
DROP TABLE IF EXISTS `userconqueststats`;
CREATE TABLE `userconqueststats`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `WarStartDate` datetime(6) NULL DEFAULT NULL,
  `CastleName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CharacterName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `GuildName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Class` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `BossDamageTaken` int(11) NULL DEFAULT NULL,
  `BossDamageDealt` int(11) NULL DEFAULT NULL,
  `BossDeathCount` int(11) NULL DEFAULT NULL,
  `BossKillCount` int(11) NULL DEFAULT NULL,
  `PvPDamageTaken` int(11) NULL DEFAULT NULL,
  `PvPDamageDealt` int(11) NULL DEFAULT NULL,
  `PvPKillCount` int(11) NULL DEFAULT NULL,
  `PvPDeathCount` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userdrop
-- ----------------------------
DROP TABLE IF EXISTS `userdrop`;
CREATE TABLE `userdrop`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `Progress` decimal(35, 5) NULL DEFAULT NULL,
  `DropCount` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for userfortuneinfo
-- ----------------------------
DROP TABLE IF EXISTS `userfortuneinfo`;
CREATE TABLE `userfortuneinfo`  (
  `Index` int(10) NOT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `DropCount` bigint(20) NULL DEFAULT NULL,
  `DropProgress` decimal(35, 5) NULL DEFAULT NULL,
  `CheckTime` datetime(6) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for useritem
-- ----------------------------
DROP TABLE IF EXISTS `useritem`;
CREATE TABLE `useritem`  (
  `Index` int(10) NOT NULL,
  `Info` int(10) NULL DEFAULT NULL,
  `CurrentDurability` int(11) NULL DEFAULT NULL,
  `MaxDurability` int(11) NULL DEFAULT NULL,
  `Count` bigint(20) NULL DEFAULT NULL,
  `Slot` int(11) NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Experience` decimal(35, 5) NULL DEFAULT NULL,
  `Colour` int(11) NULL DEFAULT NULL,
  `SpecialRepairCoolDown` datetime(6) NULL DEFAULT NULL,
  `ResetCoolDown` datetime(6) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Account` int(10) NULL DEFAULT NULL,
  `Guild` int(10) NULL DEFAULT NULL,
  `Companion` int(10) NULL DEFAULT NULL,
  `Refine` int(10) NULL DEFAULT NULL,
  `Auction` int(10) NULL DEFAULT NULL,
  `Mail` int(10) NULL DEFAULT NULL,
  `Flags` int(11) NULL DEFAULT NULL,
  `IllusionExpireTime` bigint(20) NULL DEFAULT NULL,
  `IllusionExpireDateTime` datetime(6) NULL DEFAULT NULL,
  `ExpireTime` bigint(20) NULL DEFAULT NULL,
  `ExpireDateTime` datetime(6) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `SourceMap` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `SourceRace` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `SourceName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `OriginalOwner` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CustomPrefixText` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `CustomPrefixColor` int(11) NULL DEFAULT NULL,
  `IsRefine` tinyint(1) NULL DEFAULT NULL,
  `RefineResetCount` int(11) NULL DEFAULT NULL,
  `Chance` int(11) NULL DEFAULT NULL,
  `RefineType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `OwnerLessTime` datetime(6) NULL DEFAULT NULL,
  `OwnerlessType` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE,
  INDEX `Info`(`Info`) USING BTREE,
  INDEX `Character`(`Character`) USING BTREE,
  INDEX `Account`(`Account`) USING BTREE,
  INDEX `Guild`(`Guild`) USING BTREE,
  INDEX `Companion`(`Companion`) USING BTREE,
  INDEX `Refine`(`Refine`) USING BTREE,
  INDEX `Auction`(`Auction`) USING BTREE,
  INDEX `Mail`(`Mail`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for useritemstat
-- ----------------------------
DROP TABLE IF EXISTS `useritemstat`;
CREATE TABLE `useritemstat`  (
  `Index` int(10) NOT NULL,
  `Item` int(10) NULL DEFAULT NULL,
  `SourceName` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Stat` int(11) NULL DEFAULT NULL,
  `Amount` int(11) NULL DEFAULT NULL,
  `StatSource` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usermagic
-- ----------------------------
DROP TABLE IF EXISTS `usermagic`;
CREATE TABLE `usermagic`  (
  `Index` int(10) NOT NULL,
  `Info` int(10) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Set1Key` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Set2Key` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Set3Key` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Set4Key` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `Level` int(11) NULL DEFAULT NULL,
  `Experience` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for userquest
-- ----------------------------
DROP TABLE IF EXISTS `userquest`;
CREATE TABLE `userquest`  (
  `Index` int(10) NOT NULL,
  `QuestInfo` int(10) NULL DEFAULT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Completed` tinyint(1) NULL DEFAULT NULL,
  `SelectedReward` int(11) NULL DEFAULT NULL,
  `Track` tinyint(1) NULL DEFAULT NULL,
  `ExtraInfo` varchar(1000) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userquesttask
-- ----------------------------
DROP TABLE IF EXISTS `userquesttask`;
CREATE TABLE `userquesttask`  (
  `Index` int(10) NOT NULL,
  `Quest` int(10) NULL DEFAULT NULL,
  `Task` int(10) NULL DEFAULT NULL,
  `Amount` bigint(20) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for usersorting
-- ----------------------------
DROP TABLE IF EXISTS `usersorting`;
CREATE TABLE `usersorting`  (
  `Index` int(10) NOT NULL,
  `Companion` int(10) NULL DEFAULT NULL,
  `Enabled` tinyint(1) NULL DEFAULT NULL,
  `SetType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for usersortinglev
-- ----------------------------
DROP TABLE IF EXISTS `usersortinglev`;
CREATE TABLE `usersortinglev`  (
  `Index` int(10) NOT NULL,
  `Companion` int(10) NULL DEFAULT NULL,
  `Enabled` tinyint(1) NULL DEFAULT NULL,
  `SetType` tinyint(3) UNSIGNED NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for uservalue
-- ----------------------------
DROP TABLE IF EXISTS `uservalue`;
CREATE TABLE `uservalue`  (
  `Index` int(10) NOT NULL,
  `Character` int(10) NULL DEFAULT NULL,
  `Key` int(11) NULL DEFAULT NULL,
  `Value` int(11) NULL DEFAULT NULL,
  `ServerOnly` tinyint(1) NULL DEFAULT NULL,
  `CreatTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Index`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Fixed;

SET FOREIGN_KEY_CHECKS = 1;
