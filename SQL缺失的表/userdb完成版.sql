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

 Date: 29/05/2025 09:50:12
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
-- Records of accountinfo
-- ----------------------------
INSERT INTO `accountinfo` VALUES (1, 'admin@qq.com', 0x4BBBBEF6A4DF8BD245424F86A5483611E5152D2586BE43A363B279394CF7C049CC2DE0F6, '', '0001-01-01 00:00:00.000000', '', '', NULL, '192.168.1.82', '2025-04-07 20:08:36.604796', '192.168.1.194', '2025-05-03 20:45:13.807675', '', '0001-01-01 00:00:00.000000', 0, '', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '', '2025-05-03 00:00:01.587611', 28800, 6400000, 33332563, 0, 0, 0, 0, 0, 0, NULL, '0001-01-01 00:00:00.000000', 0, 2, 1, 100, 0, 'XOhCr5s8JVBdMgWiCSJU', 0, 0, '0001-01-01 00:00:00.000000', 0, 2, '', 0, '2025-04-07 20:08:36.614174');
INSERT INTO `accountinfo` VALUES (2, '2375219850@qq.com', 0xFCF69E3FD8F75850C182505F04DF62EDAB0F306F4A76D2F238488048A047353B19F951BD, '', '0001-01-01 00:00:00.000000', '', '', NULL, '223.153.5.91', '2025-04-30 15:14:22.181636', '127.0.0.1', '2025-05-29 02:21:44.009662', '', '0001-01-01 00:00:00.000000', 0, '', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '', '2025-05-29 02:21:55.913507', 28800, 400660169, 38604798, 0, 0, 0, 0, 0, 0, 1, '0001-01-01 00:00:00.000000', 0, 43, 0, 100, 0, 'Bb28202zZe9d1XPQDSFM', 0, 0, '0001-01-01 00:00:00.000000', 0, 3, '', 0, '2025-04-30 15:14:22.342881');

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
-- Records of auctionhistoryinfo
-- ----------------------------

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
-- Records of auctioninfo
-- ----------------------------

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
-- Records of autofightconfig
-- ----------------------------

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
-- Records of autopotionlink
-- ----------------------------

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
-- Records of blockinfo
-- ----------------------------

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
-- Records of buffinfo
-- ----------------------------
INSERT INTO `buffinfo` VALUES (1, NULL, 1, 2, 0x010000004F0000000F000000, 0, 60000, 31686, 0, 0, 0, 1, '', 0, '2025-04-07 20:08:36.614174');
INSERT INTO `buffinfo` VALUES (26, 1, NULL, 19, 0x010000002500000002000000, 922337203685477, 0, 0, 0, 0, 0, 0, '', 0, '2025-04-30 15:03:56.929515');
INSERT INTO `buffinfo` VALUES (31, NULL, 2, 2, 0x010000004F0000000F000000, 0, 60000, 50169, 0, 0, 0, 0, '', 0, '2025-04-30 15:14:22.342881');
INSERT INTO `buffinfo` VALUES (116, 2, NULL, 19, 0x01000000250000000A000000, 922337203685477, 0, 0, 0, 0, 0, 0, '', 0, '2025-05-03 20:45:25.949623');
INSERT INTO `buffinfo` VALUES (202, 3, NULL, 19, 0x01000000250000000A000000, 922337203685477, 0, 0, 0, 0, 0, 0, '', 0, '2025-05-11 12:33:30.910778');
INSERT INTO `buffinfo` VALUES (205, 3, NULL, 19, 0x01000000250000000A000000, 922337203685477, 0, 0, 0, 0, 0, 0, '', 0, '2025-05-29 02:21:56.019278');

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
-- Records of castlefundinfo
-- ----------------------------
INSERT INTO `castlefundinfo` VALUES (1, 1, 3150, 0, 0, '2024-08-26 10:20:19.903186');

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
-- Records of characterbeltlink
-- ----------------------------
INSERT INTO `characterbeltlink` VALUES (1, 3, 2, 135, 0, 0, '2025-04-30 15:15:23.031290');
INSERT INTO `characterbeltlink` VALUES (2, 3, 0, 181, -1, 0, '2025-04-30 21:23:11.178792');

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
-- Records of characterinfo
-- ----------------------------
INSERT INTO `characterinfo` VALUES (1, 1, '老十', 1, 0, 8, 24, 5, 1, -4288466, -4461397, '2025-04-30 15:05:58.098515', 0, 0, '2025-04-07 20:08:51.131058', '192.168.1.82', '<268,204>', 6, 0, 3, 3, 46, 87, 0, 0, 0, 0, 248.70000, 0, 0, 0, 0, 0, 0, 0x13000000000000002E0000000100000057000000020000002E0000000300000057000000040000000200000005000000020000000700000001000000080000000300000009000000050000000A000000010000000B000000020000000E000000050000000F0000000F000000110000000F00000025000000020000002700000001000000490000003F0000004A000000100000004B0000000D000000, 0x00000000, 0, 1, 2, 1, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, '2025-04-07 20:08:59.356550', NULL, NULL, '', '0001-01-01 00:00:00.000000', 0, 0.00000, 0.00000, 0.00000, 0, NULL, NULL, '', '', '', 3, 0, 0, '2025-04-07 20:08:51.140414');
INSERT INTO `characterinfo` VALUES (2, 1, '灵台', 1, 0, 30, 24, 5, 1, -2288883, -10235136, '2025-05-03 20:48:53.164893', 0, 0, '2025-04-30 15:06:18.861057', '223.153.5.91', '<459,399>', 1, 0, 5, 1, 179, 622, 0, 0, 0, 0, 0.00000, 0, 0, 0, 0, 0, 0, 0x1400000000000000B3000000010000006E02000002000000B3000000030000006E020000040000000200000005000000020000000700000001000000080000000200000009000000050000000A000000040000000B000000050000000E000000050000000F0000000F0000001000000002000000110000000F000000250000000A000000270000000100000049000000E60000004A000000180000004B00000016000000, 0x00000000, 0, 0, 0, 1, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, '2025-04-30 15:06:26.239895', NULL, NULL, '', '0001-01-01 00:00:00.000000', 0, 0.00000, 0.00000, 0.00000, 21, 1, NULL, '', '', '', 3, 0, 0, '2025-04-30 15:06:18.927406');
INSERT INTO `characterinfo` VALUES (3, 2, '灵台法', 1, 0, 50, 24, 5, 1, -14694913, -12023517, '2025-05-29 02:28:18.149463', 0, 0, '2025-04-30 15:14:50.330864', '223.153.5.91', '<404,351>', 1, 0, 5, 13, 379, 1533, 0, 0, 0, 0, 0.00000, 0, 0, 0, 0, 0, 0, 0x1D000000000000007B01000001000000FD050000020000007B01000003000000FD0500000400000002000000050000000200000007000000010000000800000004000000090000000D0000000A000000090000000B000000100000000C000000020000000D000000080000000E000000050000000F0000000F0000001000000003000000220000006E00000024000000CEFFFFFF25000000D8FFFFFF27000000010000003300000028000000490000007E0400004A000000280000004B000000280000004C000000CEFFFFFF4F0000000F0000009A000000640000009B00000064000000AB00000006000000, 0x00000000, 0, 0, 0, 1, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 0, 0, 0, 0, 4, 0, 1, 0, 0, 0, '2025-04-30 15:14:56.853586', NULL, NULL, '', '0001-01-01 00:00:00.000000', 0, 0.00000, 0.00000, 0.00000, 80, 2, NULL, 'BFEBFBFF000906A3', '00:50:56:C0:00:08', '6479_A77F_6A30_3697.', 3, 0, 0, '2025-04-30 15:14:50.384841');

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
-- Records of charactershop
-- ----------------------------

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
-- Records of fixedpointinfo
-- ----------------------------

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
-- Records of friendinfo
-- ----------------------------

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
-- Records of gamegoldpayment
-- ----------------------------

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
-- Records of gamegoldset
-- ----------------------------

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
-- Records of gamestoresale
-- ----------------------------
INSERT INTO `gamestoresale` VALUES (1, 80210, '2025-04-30 18:06:00.887625', 20000, 1, 2, 0, 0, '2025-04-30 18:06:01.018938');
INSERT INTO `gamestoresale` VALUES (2, 80211, '2025-04-30 18:06:17.874772', 60000, 1, 2, 0, 0, '2025-04-30 18:06:18.006314');
INSERT INTO `gamestoresale` VALUES (3, 80212, '2025-04-30 18:06:29.745813', 200000, 1, 2, 0, 0, '2025-04-30 18:06:29.877493');
INSERT INTO `gamestoresale` VALUES (4, 80013, '2025-05-03 20:32:35.485001', 500, 1, 1, 0, 0, '2025-05-03 20:32:35.513947');
INSERT INTO `gamestoresale` VALUES (5, 80008, '2025-05-03 20:32:37.506860', 200, 1, 1, 0, 0, '2025-05-03 20:32:37.521945');
INSERT INTO `gamestoresale` VALUES (6, 3205, '2025-05-03 20:33:26.684536', 10, 1, 1, 0, 0, '2025-05-03 20:33:26.700616');
INSERT INTO `gamestoresale` VALUES (7, 3205, '2025-05-03 20:33:28.622770', 10, 1, 1, 0, 0, '2025-05-03 20:33:28.638713');
INSERT INTO `gamestoresale` VALUES (8, 3205, '2025-05-03 20:33:30.254702', 10, 1, 1, 0, 0, '2025-05-03 20:33:30.270710');
INSERT INTO `gamestoresale` VALUES (9, 3205, '2025-05-03 20:33:51.772555', 10, 1, 1, 0, 0, '2025-05-03 20:33:51.788574');
INSERT INTO `gamestoresale` VALUES (10, 3205, '2025-05-03 20:33:53.692442', 10, 1, 1, 0, 0, '2025-05-03 20:33:53.707469');
INSERT INTO `gamestoresale` VALUES (11, 3205, '2025-05-03 20:33:55.318632', 10, 1, 1, 0, 0, '2025-05-03 20:33:55.333611');
INSERT INTO `gamestoresale` VALUES (12, 3205, '2025-05-03 20:33:56.984856', 10, 1, 1, 0, 0, '2025-05-03 20:33:56.999993');
INSERT INTO `gamestoresale` VALUES (13, 2972, '2025-05-05 20:48:23.574744', 500, 1, 2, 0, 0, '2025-05-05 20:48:23.635313');
INSERT INTO `gamestoresale` VALUES (14, 80008, '2025-05-09 10:26:50.359100', 200, 1, 2, 0, 0, '2025-05-09 10:26:50.424246');
INSERT INTO `gamestoresale` VALUES (15, 80404, '2025-05-09 10:44:00.837963', 1, 1, 2, 0, 0, '2025-05-09 10:44:00.844916');
INSERT INTO `gamestoresale` VALUES (16, 80404, '2025-05-09 10:44:43.563950', 1, 1, 2, 0, 0, '2025-05-09 10:44:43.566024');
INSERT INTO `gamestoresale` VALUES (17, 2080, '2025-05-09 10:46:04.085262', 200, 1, 2, 0, 0, '2025-05-09 10:46:04.086359');
INSERT INTO `gamestoresale` VALUES (18, 2080, '2025-05-09 10:46:08.071426', 200, 1, 2, 0, 0, '2025-05-09 10:46:08.072524');
INSERT INTO `gamestoresale` VALUES (19, 80404, '2025-05-09 10:46:36.991308', 1, 1, 2, 0, 0, '2025-05-09 10:46:36.994814');
INSERT INTO `gamestoresale` VALUES (20, 50013, '2025-05-09 11:28:23.081652', 200, 1, 2, 0, 0, '2025-05-09 11:28:23.083581');
INSERT INTO `gamestoresale` VALUES (21, 1948, '2025-05-29 02:23:45.700412', 1000, 1, 2, 0, 0, '2025-05-29 02:23:45.763893');

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
-- Records of golblevalue
-- ----------------------------
INSERT INTO `golblevalue` VALUES (1, 1001, 100, NULL, NULL);
INSERT INTO `golblevalue` VALUES (2, 551, 0, 0, '2025-05-29 02:16:43.477946');

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
-- Records of goldmarketinfo
-- ----------------------------

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
-- Records of guildallianceinfo
-- ----------------------------

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
-- Records of guildfundchangeinfo
-- ----------------------------

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
-- Records of guildinfo
-- ----------------------------
INSERT INTO `guildinfo` VALUES (1, '新手行会', 1, -65536, 0, 0, 0, 0, 0, '', 0.00000, 0, 0, 0, 0, 0, 0, '新成员', 0, 1, 0, '', NULL, NULL, 0, '2024-08-26 10:20:11.561871');

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
-- Records of guildmemberinfo
-- ----------------------------
INSERT INTO `guildmemberinfo` VALUES (1, 1, '新成员', 2, '2025-04-30 22:37:29.214508', 0, 0, 0, 0, 0, 0, '2025-04-30 22:37:29.472784');

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
-- Records of guildwarinfo
-- ----------------------------

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
-- Records of mailinfo
-- ----------------------------

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
-- Records of newautioninfo
-- ----------------------------

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
-- Records of redpacketclaiminfo
-- ----------------------------

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
-- Records of redpacketinfo
-- ----------------------------

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
-- Records of refineinfo
-- ----------------------------

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
-- Records of rewardpoolinfo
-- ----------------------------
INSERT INTO `rewardpoolinfo` VALUES (1, '彩币池', 2830.00, 1000000, 100, 200, 500, 1000, 5000, 0, '0001-01-01 00:00:00.000000');

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
-- Records of userachievement
-- ----------------------------

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
-- Records of userachievementrequirement
-- ----------------------------

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
-- Records of usercompanion
-- ----------------------------
INSERT INTO `usercompanion` VALUES (1, 1, 2, 11, '11', 1, 1318, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, '2025-05-03 20:31:31.376201');
INSERT INTO `usercompanion` VALUES (2, 2, 3, 19, '卡地亚', 1, 33, 67, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, '2025-05-09 10:26:03.658430');

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
-- Records of usercompanionunlock
-- ----------------------------

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
-- Records of userconquest
-- ----------------------------

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
-- Records of userconqueststats
-- ----------------------------

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
-- Records of userdrop
-- ----------------------------

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
-- Records of userfortuneinfo
-- ----------------------------

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
-- Records of useritem
-- ----------------------------
INSERT INTO `useritem` VALUES (1, 5, 4967, 5000, 1, 1001, 1, 0.00000, -4461397, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-07 20:08:59.298713', '新人出生地', 3, '新人上线赠送', '老十', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-07 20:08:59.408520');
INSERT INTO `useritem` VALUES (3, 18, 7910, 8000, 1, 1003, 1, 0.00000, -3030925, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-07 20:08:59.298713', '新人出生地', 3, '新人上线赠送', '老十', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-07 20:08:59.424511');
INSERT INTO `useritem` VALUES (18, 31, 5936, 6000, 1, 1000, 1, 6.19000, -13425978, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-08 12:34:53.475048', '银杏山谷', 3, '脚本系统', '老十', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-08 12:34:54.751212');
INSERT INTO `useritem` VALUES (20, 313, 0, 0, 1, 0, 1, 0.00000, -14750499, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 1, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-08 12:36:30.442112', '银杏山谷', 3, '脚本系统', '老十', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-08 12:36:30.444811');
INSERT INTO `useritem` VALUES (38, 5, 5000, 5000, 1, 1001, 1, 0.00000, -10235136, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 2, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:06:26.173486', '新人出生地', 3, '新人上线赠送', '灵台', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:06:26.286777');
INSERT INTO `useritem` VALUES (39, 7, 4000, 4000, 1, 1000, 1, 0.00000, -10103563, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 2, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:06:26.173486', '新人出生地', 3, '新人上线赠送', '灵台', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:06:26.302396');
INSERT INTO `useritem` VALUES (40, 18, 7840, 8000, 1, 1003, 1, 0.00000, -9578667, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 2, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:06:26.173486', '新人出生地', 3, '新人上线赠送', '灵台', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:06:26.302396');
INSERT INTO `useritem` VALUES (41, 5, 4528, 5000, 1, 1001, 1, 0.00000, -1295580, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:14:56.424552', '新人出生地', 3, '新人上线赠送', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:14:56.869214');
INSERT INTO `useritem` VALUES (42, 7, 3510, 4000, 1, 1000, 1, 8.15000, -12137441, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:14:56.424552', '新人出生地', 3, '新人上线赠送', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:14:56.884843');
INSERT INTO `useritem` VALUES (43, 18, 8000, 8000, 1, 2, 1, 0.00000, -6410087, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:14:56.424552', '新人出生地', 3, '新人上线赠送', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:14:56.884843');
INSERT INTO `useritem` VALUES (44, 135, 0, 0, 16, 1, 1, 0.00000, -6589069, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 15:15:20.695856', '银杏山谷', 3, '地摊老周', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 15:15:20.765678');
INSERT INTO `useritem` VALUES (105, 181, 2000, 2000, 190, 0, 1, 0.00000, -16732943, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-04-30 21:25:36.435082', '比奇城', 3, '恩英', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-04-30 21:25:36.653267');
INSERT INTO `useritem` VALUES (108, 80013, 0, 0, 1, 0, 1, 0.00000, -9794846, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 2, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-03 20:32:35.485001', '商城', 3, '商城购买', '灵台', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-03 20:32:35.508523');
INSERT INTO `useritem` VALUES (119, 80404, 199981, 200000, 1, 3, 1, 0.00000, -5484382, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 10:44:00.837963', '商城', 3, '商城购买', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 10:44:00.843940');
INSERT INTO `useritem` VALUES (120, 80404, 200000, 200000, 1, 4, 1, 0.00000, -4752541, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 10:44:43.563950', '商城', 3, '商城购买', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 10:44:43.565048');
INSERT INTO `useritem` VALUES (122, 2080, 0, 0, 1, 6, 1, 0.00000, -5369461, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 10:46:08.071426', '商城', 3, '商城购买', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 10:46:08.072524');
INSERT INTO `useritem` VALUES (123, 80404, 199978, 200000, 1, 5, 1, 0.00000, -6734601, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 10:46:36.991308', '商城', 3, '商城购买', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 10:46:36.994814');
INSERT INTO `useritem` VALUES (129, 50013, 999551, 1000000, 1, 1018, 1, 0.00000, -9339389, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 11:28:23.081652', '商城', 3, '商城购买', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 11:28:23.083581');
INSERT INTO `useritem` VALUES (130, 97, 2000, 2000, 3, 0, 1, 0.00000, -15212256, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 11:34:02.936326', '潘夜岛', 5, '浪子人鬼', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 11:34:02.937412');
INSERT INTO `useritem` VALUES (133, 775, 0, 0, 5, 19, 1, 0.00000, -4262745, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-09 11:34:19.704000', '潘夜岛', 5, '腐蚀人鬼', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-09 11:34:19.705089');
INSERT INTO `useritem` VALUES (140, 776, 0, 0, 1, 18, 1, 0.00000, -7788104, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:16.998590', '潘夜石窟1层', 5, '骷髅武士', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:17.035428');
INSERT INTO `useritem` VALUES (142, 102, 3936, 9000, 1, 9, 1, 0.00000, -2581967, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:21.519519', '潘夜石窟1层', 5, '骷髅士兵', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:21.531342');
INSERT INTO `useritem` VALUES (149, 99, 1905, 7000, 1, 6, 1, 0.00000, -15361780, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (150, 74, 7027, 8000, 1, 12, 1, 0.00000, -10856036, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (151, 104, 1976, 6000, 1, 11, 1, 0.00000, -11792789, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (153, 102, 5862, 9000, 1, 8, 1, 0.00000, -11934579, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (154, 24, 4880, 20000, 1, 13, 1, 0.00000, -10324053, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (156, 181, 2000, 2000, 5, 3, 1, 0.00000, -14133484, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (158, 196, 1500, 1500, 2, 4, 1, 0.00000, -371651, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (159, 37, 3307, 16000, 1, 15, 1, 0.00000, -9567831, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:41.823000', '潘夜石窟1层', 5, '骷髅士兵8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:41.834536');
INSERT INTO `useritem` VALUES (162, 180, 2000, 2000, 5, 1, 1, 0.00000, -9124479, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (164, 21, 15783, 20000, 1, 16, 1, 0.00000, -5374351, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (165, 100, 9000, 9000, 1, 7, 1, 0.00000, -11332184, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (166, 66, 2497, 8000, 1, 5, 1, 0.00000, -7618404, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (167, 35, 1134, 22000, 1, 17, 1, 0.00000, -9122512, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (168, 24, 9568, 20000, 1, 14, 1, 0.00000, -5240554, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (174, 123, 1335, 7000, 1, 10, 1, 0.00000, -15449212, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:43.994632', '潘夜石窟1层', 5, '骷髅武将8', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:44.006814');
INSERT INTO `useritem` VALUES (175, 98, 2000, 2000, 1, 2, 1, 0.00000, -3181252, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', NULL, NULL, NULL, 2, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-11 12:39:46.211434', '潘夜石窟1层', 5, '骷髅武士', '灵台法', '', 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-11 12:39:46.222860');
INSERT INTO `useritem` VALUES (176, 1948, 0, 0, 1, 7, 1, 0.00000, -3521430, '0001-01-01 00:00:00.000000', '0001-01-01 00:00:00.000000', 3, NULL, NULL, NULL, NULL, NULL, NULL, 4, 0, '0001-01-01 00:00:00.000000', 0, '0001-01-01 00:00:00.000000', '2025-05-29 02:23:45.700412', '商城', 3, '商城购买', '灵台法', NULL, 0, 0, 0, 0, 0, NULL, NULL, 0, '2025-05-29 02:23:45.763893');

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
-- Records of useritemstat
-- ----------------------------

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
-- Records of usermagic
-- ----------------------------
INSERT INTO `usermagic` VALUES (1, 1, 1, 1, 0, 0, 0, 0, 20, 0, '2025-04-08 12:40:34.209513');
INSERT INTO `usermagic` VALUES (2, 1, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.390159');
INSERT INTO `usermagic` VALUES (3, 5, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.390159');
INSERT INTO `usermagic` VALUES (4, 8, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.390159');
INSERT INTO `usermagic` VALUES (5, 9, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.390159');
INSERT INTO `usermagic` VALUES (6, 10, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (7, 11, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (8, 20, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (9, 21, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (10, 22, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (11, 23, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (12, 24, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (13, 31, 3, 12, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (14, 32, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (15, 33, 3, 4, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (16, 39, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (17, 40, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (18, 41, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (19, 53, 3, 10, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (20, 67, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (21, 72, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (22, 73, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (23, 74, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (24, 104, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (25, 110, 3, 11, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (26, 111, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (27, 112, 3, 0, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');
INSERT INTO `usermagic` VALUES (28, 113, 3, 6, 0, 0, 0, 3, 0, 0, '2025-04-30 18:03:21.405783');

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
-- Records of userquest
-- ----------------------------

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
-- Records of userquesttask
-- ----------------------------

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
-- Records of usersorting
-- ----------------------------

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
-- Records of usersortinglev
-- ----------------------------

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

-- ----------------------------
-- Records of uservalue
-- ----------------------------
INSERT INTO `uservalue` VALUES (1, 1, 573, 0, 0, '2025-04-07 20:09:59.305593');
INSERT INTO `uservalue` VALUES (2, 1, 314, 0, 0, '2025-04-07 20:11:49.153877');
INSERT INTO `uservalue` VALUES (3, 1, 41, 16, 0, '2025-04-08 12:30:42.145188');
INSERT INTO `uservalue` VALUES (4, 2, 573, 21, 0, '2025-04-30 15:07:26.227126');
INSERT INTO `uservalue` VALUES (5, 2, 314, 0, 0, '2025-04-30 15:08:09.820842');
INSERT INTO `uservalue` VALUES (6, 3, 573, 80, 0, '2025-04-30 15:15:56.484378');
INSERT INTO `uservalue` VALUES (7, 3, 314, 0, 0, '2025-04-30 15:16:05.710949');
INSERT INTO `uservalue` VALUES (8, 3, 43, 0, 0, '2025-04-30 21:15:56.442535');
INSERT INTO `uservalue` VALUES (9, 2, 0, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (10, 2, 43, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (11, 2, 44, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (12, 2, 46, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (13, 2, 50, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (14, 2, 51, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (15, 2, 60, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (16, 2, 300, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (17, 2, 303, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (18, 2, 21, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (19, 2, 327, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (20, 2, 333, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (21, 2, 334, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (22, 2, 367, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (23, 2, 368, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (24, 2, 369, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (25, 2, 393, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (26, 2, 372, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (27, 2, 561, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (28, 2, 560, 0, 0, '2025-05-02 23:59:59.625117');
INSERT INTO `uservalue` VALUES (29, 2, 572, 0, 0, '2025-05-02 23:59:59.625117');

SET FOREIGN_KEY_CHECKS = 1;
