USE [master]
GO
/****** Object:  Database [TAHCoffee]    Script Date: 2/7/2023 9:25:59 AM ******/
CREATE DATABASE [TAHCoffee]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TAHCoffee].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TAHCoffee] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TAHCoffee] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TAHCoffee] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TAHCoffee] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TAHCoffee] SET ARITHABORT OFF 
GO
ALTER DATABASE [TAHCoffee] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TAHCoffee] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TAHCoffee] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TAHCoffee] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TAHCoffee] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TAHCoffee] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TAHCoffee] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TAHCoffee] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TAHCoffee] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TAHCoffee] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TAHCoffee] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TAHCoffee] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TAHCoffee] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TAHCoffee] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TAHCoffee] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TAHCoffee] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TAHCoffee] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TAHCoffee] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TAHCoffee] SET  MULTI_USER 
GO
ALTER DATABASE [TAHCoffee] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TAHCoffee] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TAHCoffee] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TAHCoffee] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TAHCoffee] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TAHCoffee] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TAHCoffee] SET QUERY_STORE = ON
GO
ALTER DATABASE [TAHCoffee] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [TAHCoffee]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [char](5) NOT NULL,
	[DisplayName] [nvarchar](max) NOT NULL,
	[UnitId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[ImageURL] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[IdNumber] [int] IDENTITY(1,1) NOT NULL,
	[ExportTime] [datetime] NOT NULL,
	[CustomerId] [char](10) NULL,
	[PromoId] [varchar](20) NULL,
	[Total] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillInfor]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillInfor](
	[IdNumber] [int] NOT NULL,
	[ItemId] [char](5) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdNumber] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[FetchDataOfMonth]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   function [dbo].[FetchDataOfMonth](@Month int) 
returns table 
as return
select i.Id, i.DisplayName, SUM(bi.Quantity) Quantity, SUM(bi.Price) Price
from BillInfor bi, Bill b, Item i
where bi.IdNumber=b.IdNumber and i.Id=bi.ItemId and MONTH(b.ExportTime)=@Month and YEAR(b.ExportTime)= YEAR(GETDATE())
group by i.Id, i.DisplayName
GO
/****** Object:  UserDefinedFunction [dbo].[FetchCustomerOfMonth]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   function [dbo].[FetchCustomerOfMonth](@month int)
returns table 
as
return
select DAY(b.ExportTime) Day, sum(Quantity) Customer
from BillInfor bi, Bill b, Item i
where bi.IdNumber=b.IdNumber and month(b.ExportTime)=@month and bi.ItemId=i.Id and i.CategoryId=2 and YEAR(b.ExportTime) = YEAR(getdate())
group by DAY(b.ExportTime)
GO
/****** Object:  View [dbo].[MonthlyRevenue]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[MonthlyRevenue] as
SELECT
  ISNULL(Month,-999)  Month,
  Revenue
  FROM (
		select month(b.ExportTime) Month, sum(Price) Revenue
		from Bill b, BillInfor bi
		where b.IdNumber=bi.IdNumber and year(b.ExportTime)=year(GETDATE())
		group by month(b.ExportTime)) AS temp
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [char](10) NOT NULL,
	[DisplayName] [nvarchar](max) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[Password] [varchar](1000) NOT NULL,
	[AccountType] [varchar](10) NOT NULL,
	[ImageURL] [varchar](max) NOT NULL,
	[ManagedBy] [char](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promo]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promo](
	[Id] [varchar](20) NOT NULL,
	[DisplayName] [nvarchar](max) NOT NULL,
	[Script] [nvarchar](max) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 2/7/2023 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000000001', N'Nguyễn Văn A', N'nguyenvana@gmail.com', N'0822222222', N'5f4dcc3b5aa765d61d8327deb882cf99', N'staff', N'https://i.ibb.co/gD6SVPT/Cat.jpg', N'B000000001')
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000000002', N'Trần Văn Tt', N'tranvant@gmail.com', N'0754533256', N'696d29e0940a4957748fe3fc9efd22a3', N'staff', N'https://i.ibb.co/gD6SVPT/Cat.jpg', N'B000000001')
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000000005', N'Trần Văn T', N'tranvant@gmail.com', N'0754533256', N'5f4dcc3b5aa765d61d8327deb882cf99', N'staff', N'https://i.ibb.co/gD6SVPT/Cat.jpg', N'B000000001')
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000001   ', N'TEST', N'test@gmail.com', N'0888888888', N'5f4dcc3b5aa765d61d8327deb882cf99', N'admin', N'https://i.ibb.co/gD6SVPT/Cat.jpg', NULL)
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'B000000001', N'Nguyễn Thị B', N'nguyenthib@gmail.com', N'0123456789', N'5f4dcc3b5aa765d61d8327deb882cf99', N'admin', N'https://i.ibb.co/gD6SVPT/Cat.jpg', NULL)
GO
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (1, CAST(N'2022-10-20T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (2, CAST(N'2022-08-12T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (3, CAST(N'2021-05-26T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (4, CAST(N'2021-09-20T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (5, CAST(N'2021-01-31T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (6, CAST(N'2022-11-28T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (11, CAST(N'2022-12-12T15:01:16.813' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (12, CAST(N'2022-12-12T15:02:56.653' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (13, CAST(N'2022-12-12T15:07:03.320' AS DateTime), NULL, NULL, 111000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (14, CAST(N'2022-12-12T15:07:13.160' AS DateTime), NULL, NULL, 184000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (15, CAST(N'2022-12-12T15:08:16.857' AS DateTime), NULL, NULL, 296000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (16, CAST(N'2022-12-12T15:09:41.367' AS DateTime), NULL, NULL, 127000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (17, CAST(N'2022-12-12T15:15:26.160' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (18, CAST(N'2022-12-12T15:15:38.310' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (19, CAST(N'2022-12-12T15:17:32.860' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (20, CAST(N'2022-12-12T15:18:59.880' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (21, CAST(N'2022-12-12T15:19:28.643' AS DateTime), NULL, NULL, 227000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (22, CAST(N'2022-12-12T15:28:06.207' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (23, CAST(N'2022-12-12T15:31:20.893' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (24, CAST(N'2022-12-12T16:51:52.030' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (25, CAST(N'2022-12-12T16:52:27.323' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (26, CAST(N'2022-12-12T16:55:23.733' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (27, CAST(N'2022-12-12T17:09:14.070' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (28, CAST(N'2022-12-12T17:18:55.733' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (29, CAST(N'2022-12-12T17:19:12.790' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (30, CAST(N'2022-12-12T17:19:31.130' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (31, CAST(N'2022-12-12T17:19:40.387' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (32, CAST(N'2022-12-12T17:19:50.037' AS DateTime), NULL, NULL, 73000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (33, CAST(N'2022-12-12T17:24:07.773' AS DateTime), NULL, NULL, 120000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (34, CAST(N'2022-12-12T17:24:42.847' AS DateTime), NULL, NULL, 73000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (35, CAST(N'2022-12-12T17:28:42.700' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (36, CAST(N'2022-12-12T17:33:13.237' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (37, CAST(N'2022-12-12T17:34:06.127' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (38, CAST(N'2022-12-12T17:34:33.207' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (39, CAST(N'2022-12-12T18:00:42.083' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (40, CAST(N'2022-12-12T18:12:43.403' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (41, CAST(N'2022-12-12T18:18:07.420' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (42, CAST(N'2022-12-12T18:37:48.580' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (43, CAST(N'2022-12-12T18:38:29.353' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (44, CAST(N'2022-12-12T18:55:56.420' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (45, CAST(N'2022-12-12T18:56:04.713' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (46, CAST(N'2022-12-12T18:56:43.107' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (47, CAST(N'2022-12-12T18:58:16.353' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (48, CAST(N'2022-12-12T18:59:52.963' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (49, CAST(N'2022-12-12T19:01:38.130' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (50, CAST(N'2022-12-12T19:03:00.357' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (51, CAST(N'2022-12-12T19:03:07.977' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (52, CAST(N'2022-12-12T19:03:25.273' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (53, CAST(N'2022-12-12T19:05:19.490' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (54, CAST(N'2022-12-12T19:06:08.160' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (55, CAST(N'2022-12-12T19:06:54.553' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (56, CAST(N'2022-12-12T19:07:00.803' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (57, CAST(N'2022-12-12T19:07:15.930' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (58, CAST(N'2022-12-12T19:10:57.433' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (59, CAST(N'2022-12-12T19:12:14.147' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (60, CAST(N'2022-12-12T19:12:59.730' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (61, CAST(N'2022-12-12T19:13:15.930' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (62, CAST(N'2022-12-12T19:13:23.963' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (63, CAST(N'2022-12-12T19:13:53.027' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (64, CAST(N'2022-12-12T19:13:59.410' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (65, CAST(N'2022-12-12T19:14:48.203' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (66, CAST(N'2022-12-12T19:15:47.793' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (67, CAST(N'2022-12-12T19:16:27.180' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (68, CAST(N'2022-12-12T19:16:35.123' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (69, CAST(N'2022-12-12T19:17:16.210' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (70, CAST(N'2022-12-12T19:17:26.827' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (71, CAST(N'2022-12-12T19:19:41.403' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (72, CAST(N'2022-12-12T19:19:46.113' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (73, CAST(N'2022-12-12T19:22:49.450' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (74, CAST(N'2022-12-12T19:23:05.370' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (75, CAST(N'2022-12-12T19:23:14.873' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (76, CAST(N'2022-12-12T19:23:45.193' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (77, CAST(N'2022-12-12T19:24:15.363' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (78, CAST(N'2022-12-12T19:26:26.563' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (79, CAST(N'2022-12-12T19:27:44.760' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (80, CAST(N'2022-12-12T19:36:17.620' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (81, CAST(N'2022-12-12T19:36:36.083' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (82, CAST(N'2022-12-12T20:06:15.257' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (83, CAST(N'2022-12-12T20:06:42.563' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (84, CAST(N'2022-12-12T20:07:14.347' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (85, CAST(N'2022-12-12T20:07:24.570' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (86, CAST(N'2022-12-12T20:09:03.403' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (87, CAST(N'2022-12-12T20:09:40.027' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (88, CAST(N'2022-12-12T20:11:14.153' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (89, CAST(N'2022-12-12T20:11:24.073' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (90, CAST(N'2022-12-12T20:19:38.963' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (91, CAST(N'2022-12-12T20:20:58.753' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (92, CAST(N'2022-12-12T20:21:07.297' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (93, CAST(N'2022-12-12T20:21:38.403' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (94, CAST(N'2022-12-12T20:21:53.763' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (95, CAST(N'2022-12-12T20:22:05.250' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (96, CAST(N'2022-12-12T20:22:13.137' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (97, CAST(N'2022-12-12T20:22:20.703' AS DateTime), NULL, NULL, 16000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (98, CAST(N'2022-12-12T20:23:11.827' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (99, CAST(N'2022-12-12T20:23:50.090' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (100, CAST(N'2022-12-12T20:25:36.793' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (101, CAST(N'2022-12-12T20:25:44.307' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (102, CAST(N'2022-12-12T20:27:09.723' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (103, CAST(N'2022-12-12T20:27:17.840' AS DateTime), NULL, NULL, 24000)
GO
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (104, CAST(N'2022-12-12T20:27:24.663' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (105, CAST(N'2022-12-12T20:27:32.343' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (106, CAST(N'2022-12-12T20:28:06.263' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (107, CAST(N'2022-12-12T20:31:09.280' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (108, CAST(N'2022-12-12T20:31:26.737' AS DateTime), NULL, NULL, 48000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (109, CAST(N'2022-12-12T20:33:40.937' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (110, CAST(N'2022-12-12T20:37:18.247' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (111, CAST(N'2022-12-12T20:38:04.047' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (112, CAST(N'2022-12-12T20:43:50.603' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (113, CAST(N'2022-12-12T20:44:26.953' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (114, CAST(N'2022-12-12T21:27:13.547' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (115, CAST(N'2022-12-12T21:27:35.680' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (116, CAST(N'2022-12-12T21:28:57.297' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (117, CAST(N'2022-12-12T21:31:13.753' AS DateTime), NULL, NULL, 111000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (118, CAST(N'2022-12-12T21:31:22.010' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (119, CAST(N'2022-12-12T21:31:49.553' AS DateTime), NULL, NULL, 73000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (120, CAST(N'2022-12-12T21:32:40.257' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (121, CAST(N'2022-12-12T21:37:54.667' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (122, CAST(N'2022-12-12T21:38:30.530' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (123, CAST(N'2022-12-12T21:38:37.803' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (124, CAST(N'2022-12-12T21:38:50.887' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (125, CAST(N'2022-12-12T21:40:07.010' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (126, CAST(N'2022-12-12T21:41:19.880' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (127, CAST(N'2022-12-12T21:49:51.850' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (128, CAST(N'2022-12-12T21:49:55.623' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (129, CAST(N'2022-12-12T21:50:12.480' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (130, CAST(N'2022-12-12T21:51:58.743' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (131, CAST(N'2022-12-12T21:53:43.313' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (132, CAST(N'2022-12-12T21:53:48.257' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (133, CAST(N'2022-12-12T21:54:48.730' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (134, CAST(N'2022-12-12T21:54:57.880' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (135, CAST(N'2022-12-12T21:55:10.687' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (136, CAST(N'2022-12-12T21:55:46.880' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (137, CAST(N'2022-12-12T21:57:40.227' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (138, CAST(N'2022-12-12T22:01:08.023' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (139, CAST(N'2022-12-12T22:04:36.440' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (140, CAST(N'2022-12-12T22:13:38.810' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (141, CAST(N'2022-12-12T22:13:48.080' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (142, CAST(N'2022-12-12T22:15:41.657' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (143, CAST(N'2022-12-12T22:23:32.930' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (144, CAST(N'2022-12-12T22:24:23.383' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (145, CAST(N'2022-12-13T11:25:45.847' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (146, CAST(N'2022-12-13T11:25:52.767' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (147, CAST(N'2022-12-13T14:54:07.310' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (148, CAST(N'2022-12-13T14:54:18.633' AS DateTime), NULL, NULL, 318000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (149, CAST(N'2022-12-13T14:54:38.643' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (150, CAST(N'2022-12-13T23:16:24.457' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (151, CAST(N'2022-12-13T23:16:31.350' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (152, CAST(N'2022-12-13T23:16:38.917' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (153, CAST(N'2022-12-17T19:02:17.653' AS DateTime), NULL, NULL, 9000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (154, CAST(N'2022-12-17T19:23:00.823' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (155, CAST(N'2022-12-17T19:23:06.213' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (156, CAST(N'2022-12-18T01:17:30.607' AS DateTime), NULL, NULL, 73000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (157, CAST(N'2022-12-18T01:20:40.063' AS DateTime), NULL, NULL, 151000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (158, CAST(N'2022-12-18T01:21:51.973' AS DateTime), NULL, NULL, 82000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (159, CAST(N'2022-12-18T01:22:00.863' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (160, CAST(N'2022-12-18T01:22:10.020' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (161, CAST(N'2022-12-18T01:22:19.583' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (162, CAST(N'2022-12-18T01:27:59.607' AS DateTime), NULL, NULL, 103000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (163, CAST(N'2022-12-18T01:28:08.023' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (164, CAST(N'2022-12-18T01:28:20.350' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (165, CAST(N'2022-12-18T01:32:41.337' AS DateTime), NULL, NULL, 73000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (166, CAST(N'2022-12-19T22:41:35.077' AS DateTime), NULL, NULL, 9000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (167, CAST(N'2022-12-19T22:41:41.833' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (168, CAST(N'2022-12-19T22:43:11.137' AS DateTime), NULL, NULL, 258000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (169, CAST(N'2022-12-19T22:45:59.583' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (170, CAST(N'2022-12-19T22:47:53.337' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (171, CAST(N'2022-12-19T22:48:35.770' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (172, CAST(N'2022-12-19T22:49:00.040' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (173, CAST(N'2022-12-19T22:49:26.743' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (174, CAST(N'2022-12-19T22:50:18.100' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (175, CAST(N'2022-12-19T22:50:45.313' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (176, CAST(N'2022-12-19T22:51:11.423' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (177, CAST(N'2022-12-19T22:51:33.857' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (178, CAST(N'2022-12-19T22:51:58.657' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (179, CAST(N'2022-12-19T22:52:35.890' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (180, CAST(N'2022-12-19T22:52:48.287' AS DateTime), NULL, NULL, 49000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (181, CAST(N'2022-12-19T22:53:00.977' AS DateTime), NULL, NULL, 218000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (182, CAST(N'2022-12-19T23:28:16.930' AS DateTime), NULL, NULL, 345000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (183, CAST(N'2022-12-20T07:46:50.207' AS DateTime), NULL, NULL, 377000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (184, CAST(N'2022-12-20T09:32:52.547' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (185, CAST(N'2022-12-20T13:47:21.923' AS DateTime), NULL, NULL, 9000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (186, CAST(N'2022-12-20T13:48:02.990' AS DateTime), NULL, NULL, 152000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (187, CAST(N'2022-12-20T13:48:20.557' AS DateTime), NULL, NULL, 183000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (188, CAST(N'2022-12-20T13:49:42.423' AS DateTime), NULL, NULL, 377000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (189, CAST(N'2022-12-21T15:08:01.973' AS DateTime), NULL, NULL, 36000000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (191, CAST(N'2022-12-27T16:03:07.047' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (192, CAST(N'2022-12-27T16:05:50.407' AS DateTime), NULL, NULL, 82000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (193, CAST(N'2022-12-28T11:59:55.907' AS DateTime), NULL, NULL, 107000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (194, CAST(N'2022-12-28T12:00:02.170' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (195, CAST(N'2022-12-28T12:00:11.540' AS DateTime), NULL, NULL, 208000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (196, CAST(N'2023-01-21T15:17:49.280' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (197, CAST(N'2023-01-22T12:59:27.123' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (198, CAST(N'2023-01-22T14:35:59.580' AS DateTime), NULL, NULL, 47000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (199, CAST(N'2023-01-24T22:30:30.673' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (200, CAST(N'2023-01-30T12:32:41.853' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (201, CAST(N'2023-01-30T12:34:22.437' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (203, CAST(N'2023-01-30T12:36:56.033' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (204, CAST(N'2023-01-30T12:41:53.560' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (205, CAST(N'2023-01-30T12:42:02.240' AS DateTime), NULL, NULL, 15000)
GO
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (206, CAST(N'2023-01-30T12:42:37.163' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (207, CAST(N'2023-01-30T12:42:48.527' AS DateTime), NULL, NULL, 190000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (208, CAST(N'2023-01-30T12:43:03.760' AS DateTime), NULL, NULL, 117000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (209, CAST(N'2023-01-31T12:41:17.097' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (210, CAST(N'2023-01-31T12:41:34.443' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (211, CAST(N'2023-01-31T12:43:55.097' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (212, CAST(N'2023-01-31T12:44:01.947' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (213, CAST(N'2023-01-31T12:44:11.570' AS DateTime), NULL, NULL, 76000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (214, CAST(N'2023-01-31T12:44:25.980' AS DateTime), NULL, NULL, 15000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (215, CAST(N'2023-01-31T12:49:22.180' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (216, CAST(N'2023-01-31T12:49:28.940' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (217, CAST(N'2023-01-31T12:49:37.670' AS DateTime), NULL, NULL, 115000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (218, CAST(N'2023-01-31T12:50:04.547' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (219, CAST(N'2023-01-31T13:06:18.580' AS DateTime), NULL, NULL, 46000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (220, CAST(N'2023-01-31T13:07:24.010' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (221, CAST(N'2023-01-31T13:07:36.657' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (222, CAST(N'2023-01-31T13:07:51.383' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (223, CAST(N'2023-01-31T13:12:22.407' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (224, CAST(N'2023-01-31T13:12:53.427' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (225, CAST(N'2023-01-31T13:13:04.200' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (226, CAST(N'2023-01-31T15:35:08.953' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (227, CAST(N'2023-01-31T15:35:23.017' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (228, CAST(N'2023-01-31T16:04:10.597' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (229, CAST(N'2023-01-31T23:04:44.030' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (230, CAST(N'2023-01-31T23:54:08.247' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (231, CAST(N'2023-01-31T23:54:15.387' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (232, CAST(N'2023-02-01T00:27:37.100' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (233, CAST(N'2023-02-01T00:27:55.377' AS DateTime), NULL, NULL, 2900000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (234, CAST(N'2023-02-01T00:29:36.313' AS DateTime), NULL, NULL, 90000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (235, CAST(N'2023-02-01T00:44:48.373' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (236, CAST(N'2023-02-01T00:45:21.440' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (237, CAST(N'2023-02-01T00:45:36.173' AS DateTime), NULL, NULL, 35000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (238, CAST(N'2023-02-01T00:46:02.343' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (239, CAST(N'2023-02-01T00:47:26.377' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (240, CAST(N'2023-02-01T00:47:34.157' AS DateTime), NULL, NULL, 30000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (241, CAST(N'2023-02-01T00:47:43.917' AS DateTime), NULL, NULL, 30000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (242, CAST(N'2023-02-01T00:48:31.343' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (243, CAST(N'2023-02-01T00:49:09.327' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (244, CAST(N'2023-02-01T00:52:14.343' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (245, CAST(N'2023-02-01T15:45:08.760' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (246, CAST(N'2023-02-01T15:45:43.533' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (247, CAST(N'2023-02-01T15:51:01.817' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (248, CAST(N'2023-02-01T15:51:12.463' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (249, CAST(N'2023-02-01T15:52:12.507' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (250, CAST(N'2023-02-01T15:52:45.143' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (251, CAST(N'2023-02-01T15:57:30.413' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (252, CAST(N'2023-02-01T15:57:40.597' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (253, CAST(N'2023-02-01T15:57:48.830' AS DateTime), NULL, NULL, 56000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (254, CAST(N'2023-02-01T16:08:56.097' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (255, CAST(N'2023-02-01T16:12:17.933' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (256, CAST(N'2023-02-01T16:12:34.440' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (257, CAST(N'2023-02-01T16:12:40.733' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (258, CAST(N'2023-02-01T16:15:11.817' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (259, CAST(N'2023-02-01T16:15:26.390' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (260, CAST(N'2023-02-01T16:20:04.860' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (261, CAST(N'2023-02-01T16:31:27.150' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (262, CAST(N'2023-02-01T16:33:44.347' AS DateTime), NULL, NULL, 78000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (263, CAST(N'2023-02-01T16:34:37.627' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (264, CAST(N'2023-02-01T16:34:44.393' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (265, CAST(N'2023-02-01T16:36:04.560' AS DateTime), NULL, NULL, 53000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (266, CAST(N'2023-02-01T16:37:03.470' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (267, CAST(N'2023-02-01T16:37:07.977' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (268, CAST(N'2023-02-01T16:37:12.080' AS DateTime), NULL, NULL, 29000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (269, CAST(N'2023-02-01T16:37:31.467' AS DateTime), NULL, NULL, 25000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (270, CAST(N'2023-02-01T17:05:44.400' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (271, CAST(N'2023-02-02T11:33:13.397' AS DateTime), NULL, NULL, 24000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (272, CAST(N'2023-02-02T21:52:21.957' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (273, CAST(N'2023-02-02T21:52:26.613' AS DateTime), NULL, NULL, 8000)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (274, CAST(N'2023-02-03T11:52:47.603' AS DateTime), NULL, NULL, 29000)
SET IDENTITY_INSERT [dbo].[Bill] OFF
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (1, N'D0001', 4, 96000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (1, N'F0004', 4, 116000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (1, N'S0007', 5, 40000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (1, N'T0009', 3, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (1, N'T0013', 4, 80000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'D0002', 5, 145000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'S0006', 2, 18000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'S0008', 5, 40000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'T0009', 2, 16000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (2, N'T0013', 2, 40000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (3, N'D0001', 2, 48000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (3, N'D0002', 3, 87000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (3, N'S0007', 2, 16000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (3, N'T0012', 3, 240000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (4, N'S0006', 5, 45000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (4, N'S0007', 5, 40000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (4, N'S0008', 3, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (4, N'T0010', 4, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (4, N'T0011', 5, 40000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (5, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (5, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (12, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (12, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (16, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (17, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (17, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (18, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (20, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (20, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (20, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (21, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (21, N'D0002', 7, 203000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (22, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (23, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (23, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (23, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (24, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (25, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (26, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (26, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (26, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (27, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (28, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (28, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (28, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (29, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (29, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (30, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (30, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (31, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (31, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (31, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (32, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (32, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (32, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (33, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (34, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (34, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (34, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (35, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (36, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (37, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (38, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (38, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (39, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (40, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (41, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (42, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (43, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (44, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (45, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (46, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (47, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (48, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (49, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (50, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (51, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (52, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (53, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (54, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (55, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (56, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (57, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (58, N'D0002', 1, 29000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (59, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (60, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (61, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (62, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (63, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (64, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (65, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (66, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (67, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (68, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (69, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (70, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (71, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (72, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (73, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (74, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (75, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (76, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (77, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (78, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (79, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (80, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (81, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (82, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (83, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (83, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (83, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (84, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (85, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (86, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (87, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (88, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (89, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (90, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (91, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (92, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (93, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (94, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (95, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (96, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (97, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (97, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (98, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (98, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (98, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (99, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (100, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (101, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (102, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (103, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (104, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (105, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (106, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (107, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (108, N'D0001', 2, 48000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (109, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (110, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (111, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (112, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (113, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (114, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (115, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (116, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (116, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (116, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (117, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (118, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (119, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (119, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (119, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (120, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (120, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (120, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (121, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (122, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (123, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (124, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (124, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (124, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (125, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (125, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (125, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (126, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (126, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (126, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (127, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (127, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (127, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (128, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (129, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (129, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (130, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (131, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (132, N'D0001', 1, 24000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (133, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (133, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (134, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (135, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (136, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (136, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (136, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (137, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (137, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (138, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (138, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (138, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (139, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (140, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (141, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (142, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (143, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (143, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (143, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (144, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (145, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (145, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (145, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (146, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (147, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (147, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (147, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (148, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (148, N'D0001', 11, 264000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (148, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (149, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (150, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (150, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (151, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (152, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (152, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (152, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (153, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (154, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (154, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (154, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (155, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (155, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (156, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (156, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (156, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (157, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (158, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (159, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (159, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (159, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (160, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (160, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (160, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (161, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (162, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (163, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (163, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (163, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (164, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (164, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (164, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (165, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (165, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (165, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (166, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (167, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'F0003', 2, 58000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'F0004', 2, 58000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'F0005', 2, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (168, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'D0002', 1, 29000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (169, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (170, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (171, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (172, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (173, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (174, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (175, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'F0004', 1, 29000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (176, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (177, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (178, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (179, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (180, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (180, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (181, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0009', 1, 35000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0011', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'S0012', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (182, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0009', 1, 35000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0011', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'S0012', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'T0009', 1, 8000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (183, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (184, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (184, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (184, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (185, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0009', 1, 35000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0011', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (186, N'S0012', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (187, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0009', 1, 35000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0011', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'S0012', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'T0012', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (188, N'T0013', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (189, N'D0001', 1500, 36000000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (191, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (191, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (191, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (192, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (192, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (192, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (193, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (193, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (193, N'D0002', 2, 58000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (194, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (194, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (195, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (196, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (197, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (198, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (198, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (199, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (200, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (201, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (203, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (204, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (205, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (206, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (207, N'S0012', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (208, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (209, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (210, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (211, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (212, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (212, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (213, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (213, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (213, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (214, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (215, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (215, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (215, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (216, N'D0001', 1, 24000)
GO
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (216, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (217, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (217, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (217, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (217, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (217, N'S0008', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (218, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (219, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (219, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (219, N'S0007', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (220, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (221, N'F0004', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (222, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (223, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (224, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (225, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (226, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (227, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (228, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (229, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (230, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (231, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (232, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (233, N'D0002', 100, 2900000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (234, N'T0013', 10, 90000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (235, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (236, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (237, N'S0009', 1, 35000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (238, N'T0009', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (239, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (240, N'S0011', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (241, N'S0010', 1, 30000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (242, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (243, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (244, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (245, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (246, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (247, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (248, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (249, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (250, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (251, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (252, N'F0003', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (253, N'F0005', 1, 15000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (253, N'F0006', 1, 32000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (253, N'S0006', 1, 9000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (254, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (255, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (256, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (257, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (258, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (259, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (260, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (261, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (262, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (262, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (262, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (263, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (264, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (265, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (265, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (266, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (267, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (268, N'D0002', 1, 29000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (269, N'D0000', 1, 25000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (270, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (271, N'D0001', 1, 24000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (272, N'T0010', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (273, N'T0011', 1, 8000)
INSERT [dbo].[BillInfor] ([IdNumber], [ItemId], [Quantity], [Price]) VALUES (274, N'D0002', 1, 29000)
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [DisplayName]) VALUES (1, N'Food')
INSERT [dbo].[Category] ([Id], [DisplayName]) VALUES (2, N'Drink')
INSERT [dbo].[Category] ([Id], [DisplayName]) VALUES (3, N'Topping')
INSERT [dbo].[Category] ([Id], [DisplayName]) VALUES (4, N'Snack')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'D0000', N'Cafe đen', 2, 2, 25000, N'https://i.ibb.co/JK5xKF8/CaPheDen.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'D0001', N'Bạc xỉu', 2, 2, 24000, N'https://i.ibb.co/XpWWJ00/BacXiu.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'D0002', N'Cappuccino', 2, 2, 29000, N'https://i.ibb.co/ng1L10C/Cappuccino.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'F0003', N'Mỳ trộn hải sản', 1, 1, 29000, N'https://i.ibb.co/wwtZwG6/My-Tron-Hai-San.webp')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'F0004', N'Mỳ trứng', 1, 1, 29000, N'https://i.ibb.co/WsT9HMT/MyTrung.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'F0005', N'Bánh mì thịt', 1, 1, 15000, N'https://i.ibb.co/84TpKdH/Banh-Mi-Thit.jpgMi-Thit.webp')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'F0006', N'Bánh mì áp chảo', 1, 1, 32000, N'https://i.ibb.co/JyzFJ9b/314701762-450424847114218-8833011457128344711-n.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0006', N'Bánh Tiramisu', 3, 4, 9000, N'https://i.ibb.co/chLjKcR/Tiramisu.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0007', N'Bánh Cookies', 3, 4, 8000, N'https://i.ibb.co/Ybj0ZX3/Banh-Cookies.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0008', N'Bánh tráng nướng', 3, 4, 8000, N'https://i.ibb.co/yqJpBzK/Banh-Trang-Nuong.jpgTrang-Nuong.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0009', N'Bánh kem Chocolate', 3, 4, 35000, N'https://i.ibb.co/3r77NFT/319371299-838481044092659-5284167035331116987-n.png')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0010', N'Bánh Cupcake Chocolate Dâu', 3, 4, 30000, N'https://i.ibb.co/f0Lrn0h/319338293-1555501084920769-3265186710859057086-n.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0011', N'Bánh Cupcake Chocolate', 3, 4, 30000, N'https://i.ibb.co/82NXKfD/275231814-290852166512187-5030945336276554387-n.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0012', N'Bánh Cupcake Chocolate Đậu phông', 3, 4, 32000, N'https://i.ibb.co/LvNWv02/320110479-888185128873380-2992241394108306575-n-1.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'T0009', N'Thạch phô mai', 1, 3, 8000, N'https://i.ibb.co/4gNbv6Z/Thach-Pho-Mai.jpgPho-Mai.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'T0010', N'Thạch dừa', 1, 3, 8000, N'https://i.ibb.co/LZFWZD2/ThachDua.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'T0011', N'Thạch củ năng', 1, 3, 8000, N'https://i.ibb.co/F4s8JvF/Thach-Cu-Nang.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'T0012', N'Milk Foam', 1, 3, 9000, N'https://i.ibb.co/smMyHhY/MilkFoam.png')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'T0013', N'Bánh flan', 3, 3, 9000, N'https://i.ibb.co/YLbsL2P/BanhFlan.jpg')
GO
INSERT [dbo].[Promo] ([Id], [DisplayName], [Script], [StartTime], [EndTime]) VALUES (N'Buy1Get1', N'Tặng bánh snack', N'Mua đồ ăn nhẹ bất kì được tặng thêm 1 bánh tiramisu', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Promo] ([Id], [DisplayName], [Script], [StartTime], [EndTime]) VALUES (N'DrinkSale', N'Giảm giá Cappuccino', N'Giảm giá Cappuccino 50%', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2021-01-01T00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Unit] ON 

INSERT [dbo].[Unit] ([Id], [DisplayName]) VALUES (1, N'Phần')
INSERT [dbo].[Unit] ([Id], [DisplayName]) VALUES (2, N'Ly')
INSERT [dbo].[Unit] ([Id], [DisplayName]) VALUES (3, N'Cái')
SET IDENTITY_INSERT [dbo].[Unit] OFF
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([ManagedBy])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([PromoId])
REFERENCES [dbo].[Promo] ([Id])
GO
ALTER TABLE [dbo].[BillInfor]  WITH CHECK ADD FOREIGN KEY([IdNumber])
REFERENCES [dbo].[Bill] ([IdNumber])
GO
ALTER TABLE [dbo].[BillInfor]  WITH CHECK ADD FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([Id])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([UnitId])
REFERENCES [dbo].[Unit] ([Id])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [CHK_AccountType] CHECK  (([AccountType]='staff' OR [AccountType]='admin'))
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [CHK_AccountType]
GO
ALTER TABLE [dbo].[BillInfor]  WITH CHECK ADD  CONSTRAINT [CHK_Quantity] CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[BillInfor] CHECK CONSTRAINT [CHK_Quantity]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_Price] CHECK  (([Price]>(0)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_Price]
GO
USE [master]
GO
ALTER DATABASE [TAHCoffee] SET  READ_WRITE 
GO
