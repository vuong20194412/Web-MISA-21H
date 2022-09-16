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
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `DepartmentId` char(36) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '' COMMENT 'Id phòng ban',
  `DepartmentCode` char(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Mã phòng ban',
  `DepartmentName` varchar(255) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Tên phòng ban',
  `CreatedDate` datetime NOT NULL COMMENT 'Thời điểm bản ghi được tạo',
  `CreatedBy` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Người tạo bản ghi',
  `ModifiedDate` datetime NOT NULL COMMENT 'Thời điểm gần nhất bản ghi được thay đổi',
  `ModifiedBy` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'Người gần nhất thay đổi bản ghi',
  PRIMARY KEY (`DepartmentId`),
  UNIQUE KEY `DepartmentCode` (`DepartmentCode`),
  UNIQUE KEY `departmentId_UNIQUE` (`DepartmentId`),
  UNIQUE KEY `UK_department` (`DepartmentId`,`DepartmentName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Thông tin phòng ban';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO `department` VALUES ('11452b0c-768e-5ff7-0d63-eeb1d8ed8cef','D007','Phòng nhân sự','2022-01-05 12:45:03','Phạm Ngọc Hiền','2022-02-19 07:19:31','Trần Ngọc Nhàn'),('142cb08f-7c31-21fa-8e90-67245e8b283e','D003','Phòng hành chính','2022-01-02 18:37:22','Nguyễn Văn Trung','2022-02-10 09:12:53','Lê Văn Lâm'),('17120d02-6ab5-3e43-18cb-66948daf6128','D006','Phòng sự kiện','2022-01-22 23:17:42','Nguyễn Văn Sơn','2022-02-01 00:00:08','Trần Đức Tuấn'),('4577565a-7e3e-493a-74dd-867949feb8b5','D005','Phòng sản xuất','2022-01-16 06:07:39','Phạm Thị Diễm','2022-02-17 03:26:16','Nguyễn Văn Lâm'),('469b3ece-744a-45d5-957d-e8c757976496','D004','Phòng phát triển','2022-01-08 03:29:16','Nguyễn Thu Diệp','2022-02-07 19:36:30','Nguyễn Văn Luân'),('4e272fc4-7875-78d6-7d32-6a1673ffca7c','D001','Phòng marketing','2022-01-01 00:07:13','Trương Huy Đạt','2022-02-01 00:00:04','Nguyễn Nhật Thắng'),('768f8e64-7d10-20c9-967d-e8c757976496','D002','Phòng tài chính','2022-01-12 21:18:55','Nguyễn Thị Vy','2022-02-11 11:49:26','Lê Thị Huyền');
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
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
