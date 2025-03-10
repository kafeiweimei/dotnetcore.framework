/*
 Navicat Premium Data Transfer

 Source Server         : SqlServer数据库
 Source Server Type    : SQL Server
 Source Server Version : 12002269
 Source Host           : 127.0.0.1:1433
 Source Catalog        : WebApiTest
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 12002269
 File Encoding         : 65001

 Date: 11/03/2025 02:46:53
*/

CREATE DATABASE WebApiTest;

-- ----------------------------
-- Table structure for Advertisement
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Advertisement]') AND type IN ('U'))
	DROP TABLE [dbo].[Advertisement]
GO

CREATE TABLE [dbo].[Advertisement] (
  [ID] int  NOT NULL,
  [ImgUrl] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Title] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Url] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Remark] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Createdate] datetime  NULL
)
GO

ALTER TABLE [dbo].[Advertisement] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Advertisement
-- ----------------------------
INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'0', N'string', N'string', N'string', N'string', N'2025-03-07 05:53:41.847')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'1', N'测试', N'添加', N'内容', N'成功', N'2022-03-05 02:10:26.443')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'2', N'更新图片地址', N'更新标题内容', N'更新文档内容', N'更新备注', N'2022-07-18 16:16:59.617')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'3', N'添加图片链接', N'添加标题', N'添加内容链接', N'添加备注', N'2022-07-24 07:43:29.200')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'4', N'添加图片链接4', N'添加标题4', N'添加内容链接4', N'添加备注4', N'2022-07-24 07:57:15.100')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'12', N'测试', N'测试', N'测试', N'测试', N'2022-10-06 01:41:32.547')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'13', N'测试', N'测试', N'测试', N'测试', N'2022-10-06 01:41:32.547')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'22', N'string', N'trewtww', N'rewqre', N'rewqr', N'2025-03-07 10:15:36.887')
GO

INSERT INTO [dbo].[Advertisement] ([ID], [ImgUrl], [Title], [Url], [Remark], [Createdate]) VALUES (N'99', N'string', N'string', N'string', N'string', N'2025-03-07 15:21:10.240')
GO


-- ----------------------------
-- Primary Key structure for table Advertisement
-- ----------------------------
ALTER TABLE [dbo].[Advertisement] ADD CONSTRAINT [PK_Advertisement] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

