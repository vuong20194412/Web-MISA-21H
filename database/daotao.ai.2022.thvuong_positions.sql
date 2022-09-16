-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: daotao.ai.2022.thvuong
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `positions`
--

DROP TABLE IF EXISTS `positions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `positions` (
  `PositionId` char(36) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '' COMMENT 'Id vị trí',
  `PositionCode` char(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Mã vị trí',
  `PositionName` varchar(255) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Tên vị trí',
  `CreatedDate` datetime NOT NULL COMMENT 'Thời điểm bản ghi được tạo',
  `CreatedBy` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Người tạo bản ghi',
  `ModifiedDate` datetime NOT NULL COMMENT 'Thời điểm gần nhất bản ghi được thay đổi',
  `ModifiedBy` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Người gần nhất thay đổi bản ghi',
  PRIMARY KEY (`PositionId`),
  UNIQUE KEY `PositionCode` (`PositionCode`),
  UNIQUE KEY `PositionId_UNIQUE` (`PositionId`),
  UNIQUE KEY `UK_positions` (`PositionId`,`PositionName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Thông tin vị trí';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `positions`
--

LOCK TABLES `positions` WRITE;
/*!40000 ALTER TABLE `positions` DISABLE KEYS */;
INSERT INTO `positions` VALUES ('148ed882-32b8-218e-9c20-39c2f00615e8','P003','Chủ tịch','2022-03-28 11:06:08','Trần Thị Huệ','2022-04-01 00:01:08','Phan Vân Hiền'),('1b691e79-236d-5b5a-9d20-39c2f00615e8','P007','Tổng giám đốc','2022-03-05 02:37:42','Huỳnh Đức Quang','2022-04-16 12:54:16','Nguyễn Hải Duy'),('25c6c36e-1668-7d10-6e09-bf1378b8dc91','P004','Trưởng nhóm','2022-03-15 06:58:37','Vũ Hải Sơn','2022-04-23 07:58:08','Trương Vân Loan'),('354f1b13-17bf-1b52-87d5-ba100c6f7bce','P006','Phó phòng','2022-03-16 02:13:23','Phan Thị Tuyết','2022-04-01 01:08:43','Hồ Đức Thành'),('3700cc49-55b5-69ea-4929-a2925c0f334d','P001','Nhân viên','2022-03-14 08:32:44','Nguyễn Thị Tuyết','2022-04-05 22:31:25','Nguyễn Thị An'),('3b86d2ed-446c-5fce-56be-406293204378','P002','Trưởng phòng','2022-03-01 00:09:27','Nguyễn Thị Ngọc','2022-04-20 22:12:18','Trương Ngọc Tuyết'),('6b47b37f-3123-3ce7-14cf-9712082ff6cb','P005','Giám đốc','2022-03-08 23:10:10','Nguyễn Hồng Lâm','2022-04-24 14:19:34','Nguyễn Huy Thành');
/*!40000 ALTER TABLE `positions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-09-16 15:58:53
