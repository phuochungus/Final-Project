USE [TAHCoffee]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[Bill]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[BillInfor]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[Category]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[Item]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[Promo]    Script Date: 12/12/2022 1:52:09 AM ******/
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
/****** Object:  Table [dbo].[Unit]    Script Date: 12/12/2022 1:52:09 AM ******/
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
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000000001', N'Nguyễn Văn A', N'nguyenvana@gmail.com', N'0822222222', N'5f4dcc3b5aa765d61d8327deb882cf99', N'staff', N'https://drive.google.com/uc?export=view&id=1bTU3K8rDXMLbdxQj1TM9wGaeNlS3OfxK', N'B000000001')
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'A000000002', N'Trần Văn T', N'tranvant@gmail.com', N'0754533256', N'5f4dcc3b5aa765d61d8327deb882cf99', N'staff', N'https://drive.google.com/uc?export=view&id=1bTU3K8rDXMLbdxQj1TM9wGaeNlS3OfxK', N'B000000001')
INSERT [dbo].[Account] ([Id], [DisplayName], [Email], [PhoneNumber], [Password], [AccountType], [ImageURL], [ManagedBy]) VALUES (N'B000000001', N'Nguyễn Thị B', N'nguyenthib@gmail.com', N'0123456789', N'5f4dcc3b5aa765d61d8327deb882cf99', N'admin', N'https://drive.google.com/uc?export=view&id=1bTU3K8rDXMLbdxQj1TM9wGaeNlS3OfxK', NULL)
GO
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (1, CAST(N'2022-10-20T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (2, CAST(N'2022-08-12T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (3, CAST(N'2021-05-26T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (4, CAST(N'2021-09-20T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (5, CAST(N'2021-01-31T00:00:00.000' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Bill] ([IdNumber], [ExportTime], [CustomerId], [PromoId], [Total]) VALUES (6, CAST(N'2022-11-28T00:00:00.000' AS DateTime), NULL, NULL, 0)
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
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0006', N'Bánh Tiramisu', 3, 4, 9000, N'https://i.ibb.co/chLjKcR/Tiramisu.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0007', N'Bánh Cookies', 3, 4, 8000, N'https://i.ibb.co/Ybj0ZX3/Banh-Cookies.jpg')
INSERT [dbo].[Item] ([Id], [DisplayName], [UnitId], [CategoryId], [Price], [ImageURL]) VALUES (N'S0008', N'Bánh tráng nướng', 3, 4, 8000, N'https://i.ibb.co/yqJpBzK/Banh-Trang-Nuong.jpgTrang-Nuong.jpg')
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
