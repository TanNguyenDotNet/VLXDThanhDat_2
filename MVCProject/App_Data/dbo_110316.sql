/*
Navicat SQL Server Data Transfer

Source Server         : localhost_SQLSERVER
Source Server Version : 110000
Source Host           : localhost:1433
Source Database       : aspnet
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2016-03-11 22:39:21
*/


-- ----------------------------
-- Table structure for __MigrationHistory
-- ----------------------------
DROP TABLE [dbo].[__MigrationHistory]
GO
CREATE TABLE [dbo].[__MigrationHistory] (
[MigrationId] nvarchar(150) NOT NULL ,
[ContextKey] nvarchar(300) NOT NULL ,
[Model] varbinary(MAX) NOT NULL ,
[ProductVersion] nvarchar(32) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AppNetUserType
-- ----------------------------
DROP TABLE [dbo].[AppNetUserType]
GO
CREATE TABLE [dbo].[AppNetUserType] (
[Username] varchar(150) NOT NULL ,
[Email] varchar(150) NOT NULL ,
[Fax] varchar(20) NULL ,
[Address] nvarchar(255) NOT NULL ,
[Phone] varchar(20) NOT NULL ,
[UserType] varchar(255) NOT NULL ,
[DateCreate] varchar(10) NOT NULL ,
[Expire] varchar(12) NOT NULL ,
[LocationID] int NOT NULL ,
[District] nvarchar(255) NULL ,
[State] varchar(2) NOT NULL ,
[TaxID] varchar(15) NULL ,
[DisplayName] nvarchar(MAX) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetRoles
-- ----------------------------
DROP TABLE [dbo].[AspNetRoles]
GO
CREATE TABLE [dbo].[AspNetRoles] (
[Id] nvarchar(128) NOT NULL ,
[Name] nvarchar(MAX) NOT NULL ,
[Order] int NOT NULL ,
[GroupName] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserClaims
-- ----------------------------
DROP TABLE [dbo].[AspNetUserClaims]
GO
CREATE TABLE [dbo].[AspNetUserClaims] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClaimType] nvarchar(MAX) NULL ,
[ClaimValue] nvarchar(MAX) NULL ,
[User_Id] nvarchar(128) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserLogins
-- ----------------------------
DROP TABLE [dbo].[AspNetUserLogins]
GO
CREATE TABLE [dbo].[AspNetUserLogins] (
[UserId] nvarchar(128) NOT NULL ,
[LoginProvider] nvarchar(128) NOT NULL ,
[ProviderKey] nvarchar(128) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserRoles
-- ----------------------------
DROP TABLE [dbo].[AspNetUserRoles]
GO
CREATE TABLE [dbo].[AspNetUserRoles] (
[UserId] nvarchar(128) NOT NULL ,
[RoleId] nvarchar(128) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUsers
-- ----------------------------
DROP TABLE [dbo].[AspNetUsers]
GO
CREATE TABLE [dbo].[AspNetUsers] (
[Id] nvarchar(128) NOT NULL ,
[UserName] nvarchar(MAX) NULL ,
[PasswordHash] nvarchar(MAX) NULL ,
[SecurityStamp] nvarchar(MAX) NULL ,
[Discriminator] nvarchar(128) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for Catalog
-- ----------------------------
DROP TABLE [dbo].[Catalog]
GO
CREATE TABLE [dbo].[Catalog] (
[ID] int NOT NULL IDENTITY(1,1) ,
[PID] int NULL ,
[Title] nvarchar(255) NULL ,
[Description] nvarchar(MAX) NULL ,
[LocationID] int NULL DEFAULT ((0)) ,
[Code] varchar(5) NULL ,
[UserID] nvarchar(128) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Catalog]', RESEED, 1011)
GO

-- ----------------------------
-- Table structure for Location
-- ----------------------------
DROP TABLE [dbo].[Location]
GO
CREATE TABLE [dbo].[Location] (
[ID] int NOT NULL IDENTITY(1,1) ,
[LocationName] nvarchar(255) NULL ,
[LocationCode] varchar(5) NULL ,
[Order] int NULL DEFAULT ((0)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[Location]', RESEED, 126)
GO

-- ----------------------------
-- Table structure for LocationSub
-- ----------------------------
DROP TABLE [dbo].[LocationSub]
GO
CREATE TABLE [dbo].[LocationSub] (
[ID] int NOT NULL IDENTITY(1,1) ,
[IDLocation] int NOT NULL ,
[Name] nvarchar(255) NULL ,
[Description] nvarchar(255) NULL ,
[IsDel] tinyint NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[LocationSub]', RESEED, 2)
GO

-- ----------------------------
-- Table structure for Product
-- ----------------------------
DROP TABLE [dbo].[Product]
GO
CREATE TABLE [dbo].[Product] (
[ID] bigint NOT NULL IDENTITY(1,1) ,
[ItemCode] varchar(150) NOT NULL ,
[Barcode] varchar(150) NULL ,
[CatID] bigint NOT NULL ,
[SKU] nvarchar(150) NULL ,
[SupplierID] int NULL ,
[ImageLink] ntext NULL ,
[Adwords] ntext NULL ,
[Show] bit NOT NULL ,
[DateCreate] datetime NULL ,
[Color] nvarchar(150) NULL ,
[Dimension] varchar(50) NULL ,
[Unit] nvarchar(255) NULL ,
[Warranty] varchar(5) NULL ,
[IsDel] bit NOT NULL DEFAULT ((0)) ,
[IsState] tinyint NULL DEFAULT ((0)) ,
[UserID] nvarchar(128) NULL ,
[ProductName] nvarchar(255) NOT NULL ,
[TaxID] tinyint NULL DEFAULT ((0)) ,
[Price] decimal(14,2) NULL DEFAULT ((0)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[Product]', RESEED, 11833)
GO

-- ----------------------------
-- Table structure for ProductCode
-- ----------------------------
DROP TABLE [dbo].[ProductCode]
GO
CREATE TABLE [dbo].[ProductCode] (
[Group1] varchar(5) NULL ,
[Group2] varchar(5) NULL ,
[CatID] bit NOT NULL ,
[ScrollNumber] int NOT NULL ,
[ID] int NOT NULL IDENTITY(1,1) ,
[Active] bit NULL ,
[CatCode] varchar(5) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[ProductCode]', RESEED, 18)
GO

-- ----------------------------
-- Table structure for ProductImage
-- ----------------------------
DROP TABLE [dbo].[ProductImage]
GO
CREATE TABLE [dbo].[ProductImage] (
[ID] bigint NOT NULL IDENTITY(1,1) ,
[ProductCode] varchar(50) NULL ,
[Image] varchar(255) NULL ,
[ImageLink] varchar(MAX) NULL ,
[Note] nvarchar(MAX) NULL ,
[ImageAddIn] varchar(MAX) NULL ,
[Component] varchar(50) NULL DEFAULT ('Product') 
)


GO
DBCC CHECKIDENT(N'[dbo].[ProductImage]', RESEED, 5)
GO

-- ----------------------------
-- Table structure for ProductName
-- ----------------------------
DROP TABLE [dbo].[ProductName]
GO
CREATE TABLE [dbo].[ProductName] (
[ID] bigint NOT NULL IDENTITY(1,1) ,
[ProductID] bigint NOT NULL ,
[Name] nvarchar(MAX) NULL ,
[LocationID] int NULL DEFAULT ((0)) ,
[UserId] nvarchar(128) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[ProductName]', RESEED, 2)
GO

-- ----------------------------
-- Table structure for ProductPrice
-- ----------------------------
DROP TABLE [dbo].[ProductPrice]
GO
CREATE TABLE [dbo].[ProductPrice] (
[ID] bigint NOT NULL IDENTITY(1,1) ,
[ProductID] bigint NOT NULL ,
[Price] decimal(14,2) NOT NULL ,
[Created] date NULL ,
[Description] nvarchar(MAX) NULL ,
[UserID] nvarchar(128) NULL ,
[LocationID] int NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[ProductPrice]', RESEED, 22)
GO

-- ----------------------------
-- Table structure for Promotion
-- ----------------------------
DROP TABLE [dbo].[Promotion]
GO
CREATE TABLE [dbo].[Promotion] (
[Active] bit NOT NULL DEFAULT ((0)) ,
[PromotionCode] varchar(50) NOT NULL ,
[StartDate] datetime NULL ,
[EndDate] datetime NULL ,
[UserID] nvarchar(128) NULL ,
[Created] date NULL ,
[PromotionTypeID] int NOT NULL ,
[PromotionValue] decimal(14,2) NOT NULL ,
[ID] bigint NOT NULL IDENTITY(1,1) ,
[LocationID] int NULL ,
[Title] nvarchar(MAX) NOT NULL ,
[ProductID] bigint NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Promotion]', RESEED, 10007)
GO

-- ----------------------------
-- Table structure for PromotionType
-- ----------------------------
DROP TABLE [dbo].[PromotionType]
GO
CREATE TABLE [dbo].[PromotionType] (
[ProTypeName] nvarchar(255) NOT NULL ,
[AddType] varchar(5) NOT NULL ,
[ExRate] decimal(5,2) NULL ,
[ID] int NOT NULL IDENTITY(1,1) 
)


GO
DBCC CHECKIDENT(N'[dbo].[PromotionType]', RESEED, 4)
GO

-- ----------------------------
-- Table structure for Supplier
-- ----------------------------
DROP TABLE [dbo].[Supplier]
GO
CREATE TABLE [dbo].[Supplier] (
[ID] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(50) NULL ,
[Address] ntext NULL ,
[Phone] varchar(20) NULL ,
[TaxCode] varchar(20) NULL ,
[AccountBank] varchar(50) NULL ,
[NameBank] nvarchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for Tax
-- ----------------------------
DROP TABLE [dbo].[Tax]
GO
CREATE TABLE [dbo].[Tax] (
[ID] tinyint NOT NULL IDENTITY(1,1) ,
[Desc] nvarchar(255) NULL ,
[TaxRate] float(53) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Tax]', RESEED, 12)
GO

-- ----------------------------
-- Table structure for UserRoles
-- ----------------------------
DROP TABLE [dbo].[UserRoles]
GO
CREATE TABLE [dbo].[UserRoles] (
[UserName] varchar(150) NOT NULL ,
[RoleId] varchar(10) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for Warranty
-- ----------------------------
DROP TABLE [dbo].[Warranty]
GO
CREATE TABLE [dbo].[Warranty] (
[Title] nvarchar(255) NULL ,
[DVT] varchar(1) NULL ,
[Value] tinyint NULL ,
[ID] tinyint NOT NULL IDENTITY(1,1) 
)


GO
DBCC CHECKIDENT(N'[dbo].[Warranty]', RESEED, 10)
GO

-- ----------------------------
-- Indexes structure for table __MigrationHistory
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table __MigrationHistory
-- ----------------------------
ALTER TABLE [dbo].[__MigrationHistory] ADD PRIMARY KEY ([MigrationId], [ContextKey])
GO

-- ----------------------------
-- Indexes structure for table AppNetUserType
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table AppNetUserType
-- ----------------------------
ALTER TABLE [dbo].[AppNetUserType] ADD PRIMARY KEY ([Username])
GO

-- ----------------------------
-- Indexes structure for table AspNetRoles
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table AspNetRoles
-- ----------------------------
ALTER TABLE [dbo].[AspNetRoles] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table AspNetUserClaims
-- ----------------------------
CREATE INDEX [IX_User_Id] ON [dbo].[AspNetUserClaims]
([User_Id] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table AspNetUserClaims
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserClaims] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table AspNetUserLogins
-- ----------------------------
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
([UserId] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table AspNetUserLogins
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserLogins] ADD PRIMARY KEY ([UserId], [LoginProvider], [ProviderKey])
GO

-- ----------------------------
-- Indexes structure for table AspNetUserRoles
-- ----------------------------
CREATE INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
([RoleId] ASC) 
GO
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
([UserId] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table AspNetUserRoles
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserRoles] ADD PRIMARY KEY ([UserId], [RoleId])
GO

-- ----------------------------
-- Indexes structure for table AspNetUsers
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table AspNetUsers
-- ----------------------------
ALTER TABLE [dbo].[AspNetUsers] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table Catalog
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Catalog
-- ----------------------------
ALTER TABLE [dbo].[Catalog] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table Location
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Location
-- ----------------------------
ALTER TABLE [dbo].[Location] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table LocationSub
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table LocationSub
-- ----------------------------
ALTER TABLE [dbo].[LocationSub] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table Product
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Product
-- ----------------------------
ALTER TABLE [dbo].[Product] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table ProductCode
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ProductCode
-- ----------------------------
ALTER TABLE [dbo].[ProductCode] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table ProductImage
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ProductImage
-- ----------------------------
ALTER TABLE [dbo].[ProductImage] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table ProductName
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ProductName
-- ----------------------------
ALTER TABLE [dbo].[ProductName] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Uniques structure for table ProductName
-- ----------------------------
ALTER TABLE [dbo].[ProductName] ADD UNIQUE ([ProductID] ASC, [LocationID] ASC)
GO

-- ----------------------------
-- Indexes structure for table ProductPrice
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ProductPrice
-- ----------------------------
ALTER TABLE [dbo].[ProductPrice] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Uniques structure for table ProductPrice
-- ----------------------------
ALTER TABLE [dbo].[ProductPrice] ADD UNIQUE ([ProductID] ASC, [LocationID] ASC)
GO

-- ----------------------------
-- Indexes structure for table Promotion
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Promotion
-- ----------------------------
ALTER TABLE [dbo].[Promotion] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table PromotionType
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table PromotionType
-- ----------------------------
ALTER TABLE [dbo].[PromotionType] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table Supplier
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier
-- ----------------------------
ALTER TABLE [dbo].[Supplier] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table Tax
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Tax
-- ----------------------------
ALTER TABLE [dbo].[Tax] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table UserRoles
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD PRIMARY KEY ([UserName], [RoleId])
GO

-- ----------------------------
-- Indexes structure for table Warranty
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Warranty
-- ----------------------------
ALTER TABLE [dbo].[Warranty] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[AspNetUserClaims]
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserClaims] ADD FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[AspNetUserLogins]
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserLogins] ADD FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[AspNetUserRoles]
-- ----------------------------
ALTER TABLE [dbo].[AspNetUserRoles] ADD FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
