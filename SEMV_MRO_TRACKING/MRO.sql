USE [master]
GO
/****** Object:  Database [SEMV_MRO]    Script Date: 6/3/2021 4:53:30 PM ******/
CREATE DATABASE [SEMV_MRO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SEMV_MRO', FILENAME = N'D:\Data\SEMV_MRO.ndf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SEMV_MRO_log', FILENAME = N'D:\Logs\SEMV_MRO_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [SEMV_MRO] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SEMV_MRO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SEMV_MRO] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SEMV_MRO] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SEMV_MRO] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SEMV_MRO] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SEMV_MRO] SET ARITHABORT OFF 
GO
ALTER DATABASE [SEMV_MRO] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SEMV_MRO] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SEMV_MRO] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SEMV_MRO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SEMV_MRO] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SEMV_MRO] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SEMV_MRO] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SEMV_MRO] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SEMV_MRO] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SEMV_MRO] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SEMV_MRO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SEMV_MRO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SEMV_MRO] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SEMV_MRO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SEMV_MRO] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SEMV_MRO] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SEMV_MRO] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SEMV_MRO] SET RECOVERY FULL 
GO
ALTER DATABASE [SEMV_MRO] SET  MULTI_USER 
GO
ALTER DATABASE [SEMV_MRO] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SEMV_MRO] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SEMV_MRO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SEMV_MRO] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SEMV_MRO] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SEMV_MRO] SET QUERY_STORE = OFF
GO
USE [SEMV_MRO]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [SEMV_MRO]
GO
/****** Object:  User [stl_user1]    Script Date: 6/3/2021 4:53:30 PM ******/
CREATE USER [stl_user1] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [semv]    Script Date: 6/3/2021 4:53:30 PM ******/
CREATE USER [semv] FOR LOGIN [semv] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [semv]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [semv]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [semv]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [semv]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [semv]
GO
ALTER ROLE [db_datareader] ADD MEMBER [semv]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [semv]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [semv]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [semv]
GO
/****** Object:  Table [dbo].[mst_material]    Script Date: 6/3/2021 4:53:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mst_material](
	[Part_Number] [varchar](255) NULL,
	[Barcode] [varchar](255) NULL,
	[Unit] [nvarchar](255) NULL,
	[Material_Name] [nvarchar](255) NULL,
	[Material_Desc] [nvarchar](max) NULL,
	[Leadtime] [int] NULL,
	[Stock] [float] NULL,
	[Safety_Stock] [float] NULL,
	[Max_Stock] [float] NULL,
	[Total_Purchase] [float] NULL,
	[Price] [decimal](18, 0) NULL,
	[Supplier_Name] [varchar](255) NULL,
	[Supplier_Email] [varchar](255) NULL,
	[Timestamp] [datetime] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[V_MATERIAL_STOCK]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_MATERIAL_STOCK] AS SELECT Part_Number,Material_Name,Stock,Safety_Stock,Supplier_Name,Max_Stock,Total_Purchase,Unit,Price,Supplier_Email,
CASE
WHEN (Stock < Safety_Stock) AND (Total_Purchase = 0) THEN 'Need To Order'
WHEN (Stock + Total_Purchase < Safety_Stock) AND (Total_Purchase > 0) THEN 'Need To Order|Need To Confirm'
WHEN Total_Purchase > 0 THEN 'Need To Confirm'
WHEN Stock >= Safety_Stock AND (Total_Purchase = 0) THEN 'OK'
END as Stock_Status
FROM mst_material
GO
/****** Object:  Table [dbo].[mst_material_tmp]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mst_material_tmp](
	[Part_Number] [varchar](255) NULL,
	[Barcode] [varchar](255) NULL,
	[Unit] [nvarchar](255) NULL,
	[Material_Name] [nvarchar](255) NULL,
	[Material_Desc] [nvarchar](max) NULL,
	[Leadtime] [int] NULL,
	[Stock] [float] NULL,
	[Safety_Stock] [float] NULL,
	[Max_Stock] [float] NULL,
	[Total_Purchase] [float] NULL,
	[Price] [decimal](18, 0) NULL,
	[Supplier_Name] [varchar](255) NULL,
	[Supplier_Email] [varchar](255) NULL,
	[Timestamp] [datetime] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mst_users]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mst_users](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[sesa_id] [varchar](50) NULL,
	[name] [varchar](100) NULL,
	[password] [varchar](100) NULL,
	[level] [varchar](50) NULL,
	[record_date] [datetime] NULL,
	[last_login] [datetime] NULL,
 CONSTRAINT [PK__mst_user__D2D14637FF1E368A] PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_order_header]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_order_header](
	[Order_ID] [varchar](255) NULL,
	[Part_Number] [varchar](255) NULL,
	[Quantity_Req] [decimal](18, 0) NULL,
	[Request_By] [varchar](255) NULL,
	[Timestamp] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_scanin_header]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_scanin_header](
	[Order_ID] [varchar](255) NULL,
	[Part_Number] [varchar](255) NULL,
	[Quantity_Req] [decimal](18, 0) NULL,
	[Request_By] [varchar](255) NULL,
	[Timestamp] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_scanout_header]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_scanout_header](
	[Order_ID] [varchar](255) NULL,
	[Part_Number] [varchar](255) NULL,
	[Quantity_Req] [decimal](18, 0) NULL,
	[Request_By] [varchar](255) NULL,
	[Timestamp] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_transaction_history]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_transaction_history](
	[Part_Number] [varchar](255) NULL,
	[Purchase_Number] [varchar](255) NULL,
	[Sesa_user] [nvarchar](255) NULL,
	[Transaction_typ] [varchar](255) NULL,
	[Qty] [float] NULL,
	[Remark] [varchar](max) NULL,
	[Timestamp] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_mst_users]    Script Date: 6/3/2021 4:53:31 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_mst_users] ON [dbo].[mst_users]
(
	[sesa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[mst_material] ADD  CONSTRAINT [DF__mst_mater__Times__3A81B327]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[mst_material_tmp] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[mst_users] ADD  DEFAULT (getdate()) FOR [record_date]
GO
ALTER TABLE [dbo].[tbl_order_header] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[tbl_scanin_header] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[tbl_scanout_header] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[tbl_transaction_history] ADD  CONSTRAINT [DF__tbl_trans__Times__3C69FB99]  DEFAULT (getdate()) FOR [Timestamp]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_MST_MATERIAL]    Script Date: 6/3/2021 4:53:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[INSERT_MST_MATERIAL]
AS
BEGIN

		INSERT INTO [dbo].[mst_material]
           ([Part_Number]
           ,[Barcode]
           ,[Unit]
           ,[Material_Name]
           ,[Material_Desc]
           ,[Leadtime]
           ,[Stock]
           ,[Safety_Stock]
           ,[Max_Stock]
           ,[Total_Purchase]
           ,[Price]
           ,[Supplier_Name]
           ,[Supplier_Email])
     SELECT [Part_Number]
           ,[Barcode]
           ,[Unit]
           ,[Material_Name]
           ,[Material_Desc]
           ,[Leadtime]
           ,[Stock]
           ,[Safety_Stock]
           ,[Max_Stock]
           ,[Total_Purchase]
           ,[Price]
           ,[Supplier_Name]
           ,[Supplier_Email]
		   FROM [dbo].[mst_material_tmp]
END
GO
USE [master]
GO
ALTER DATABASE [SEMV_MRO] SET  READ_WRITE 
GO
