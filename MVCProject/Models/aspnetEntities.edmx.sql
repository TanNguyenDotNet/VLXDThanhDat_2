
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/22/2016 18:24:24
-- Generated from EDMX file: F:\C#\VLXDThanhDat\MVCProject\Models\aspnetEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__AspNetUse__RoleI__5CD6CB2B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK__AspNetUse__RoleI__5CD6CB2B];
GO
IF OBJECT_ID(N'[dbo].[FK__AspNetUse__User___5AEE82B9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK__AspNetUse__User___5AEE82B9];
GO
IF OBJECT_ID(N'[dbo].[FK__AspNetUse__UserI__5BE2A6F2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK__AspNetUse__UserI__5BE2A6F2];
GO
IF OBJECT_ID(N'[dbo].[FK__AspNetUse__UserI__5DCAEF64]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK__AspNetUse__UserI__5DCAEF64];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[AppNetUserType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppNetUserType];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[Catalog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Catalog];
GO
IF OBJECT_ID(N'[dbo].[CatalogUnit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CatalogUnit];
GO
IF OBJECT_ID(N'[dbo].[Location]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Location];
GO
IF OBJECT_ID(N'[dbo].[LocationSub]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocationSub];
GO
IF OBJECT_ID(N'[dbo].[LogSystem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogSystem];
GO
IF OBJECT_ID(N'[dbo].[PaymentDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentDetail];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[ProductCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCode];
GO
IF OBJECT_ID(N'[dbo].[ProductImage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductImage];
GO
IF OBJECT_ID(N'[dbo].[ProductName]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductName];
GO
IF OBJECT_ID(N'[dbo].[ProductPrice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductPrice];
GO
IF OBJECT_ID(N'[dbo].[Promotion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Promotion];
GO
IF OBJECT_ID(N'[dbo].[PromotionType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PromotionType];
GO
IF OBJECT_ID(N'[dbo].[Supplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supplier];
GO
IF OBJECT_ID(N'[dbo].[Tax]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tax];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[Warranty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Warranty];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Order] int  NOT NULL,
    [GroupName] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL,
    [User_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [UserId] nvarchar(128)  NOT NULL,
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [Discriminator] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'Catalogs'
CREATE TABLE [dbo].[Catalogs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PID] int  NULL,
    [Title] nvarchar(255)  NULL,
    [Description] nvarchar(max)  NULL,
    [LocationID] int  NULL,
    [Code] varchar(5)  NULL,
    [UserID] nvarchar(128)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LocationName] nvarchar(255)  NULL,
    [LocationCode] varchar(5)  NULL,
    [Order] int  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ItemCode] varchar(150)  NOT NULL,
    [Barcode] varchar(150)  NULL,
    [CatID] bigint  NOT NULL,
    [SKU] nvarchar(150)  NULL,
    [SupplierID] int  NULL,
    [ImageLink] nvarchar(max)  NULL,
    [Adwords] nvarchar(max)  NULL,
    [Show] bit  NOT NULL,
    [DateCreate] datetime  NULL,
    [Color] nvarchar(150)  NULL,
    [Dimension] varchar(50)  NULL,
    [Unit] nvarchar(255)  NULL,
    [Warranty] varchar(5)  NULL,
    [IsDel] bit  NOT NULL,
    [IsState] tinyint  NULL,
    [UserID] nvarchar(128)  NULL,
    [ProductName] nvarchar(255)  NOT NULL,
    [TaxID] tinyint  NULL,
    [Price] decimal(14,2)  NULL
);
GO

-- Creating table 'ProductNames'
CREATE TABLE [dbo].[ProductNames] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ProductID] bigint  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [LocationID] int  NULL,
    [UserId] nvarchar(128)  NULL
);
GO

-- Creating table 'ProductPrices'
CREATE TABLE [dbo].[ProductPrices] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ProductID] bigint  NOT NULL,
    [Price] decimal(14,2)  NOT NULL,
    [Created] datetime  NULL,
    [Description] nvarchar(max)  NULL,
    [UserID] nvarchar(128)  NULL,
    [LocationID] int  NULL
);
GO

-- Creating table 'Promotions'
CREATE TABLE [dbo].[Promotions] (
    [ProductID] bigint  NULL,
    [Active] bit  NOT NULL,
    [PromotionCode] varchar(50)  NOT NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [UserID] nvarchar(128)  NULL,
    [Created] datetime  NULL,
    [PromotionTypeID] int  NOT NULL,
    [PromotionValue] decimal(14,2)  NOT NULL,
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [LocationID] int  NULL,
    [Title] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PromotionTypes'
CREATE TABLE [dbo].[PromotionTypes] (
    [ProTypeName] nvarchar(255)  NOT NULL,
    [AddType] varchar(5)  NOT NULL,
    [ExRate] decimal(5,2)  NULL,
    [ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Address] nvarchar(max)  NULL,
    [Phone] varchar(20)  NULL,
    [TaxCode] varchar(20)  NULL,
    [AccountBank] varchar(50)  NULL,
    [NameBank] nvarchar(255)  NULL
);
GO

-- Creating table 'ProductCodes'
CREATE TABLE [dbo].[ProductCodes] (
    [Group1] varchar(5)  NULL,
    [Group2] varchar(5)  NULL,
    [CatID] bit  NOT NULL,
    [ScrollNumber] int  NOT NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [Active] bit  NULL,
    [CatCode] varchar(5)  NULL
);
GO

-- Creating table 'Warranties'
CREATE TABLE [dbo].[Warranties] (
    [Title] nvarchar(255)  NULL,
    [DVT] varchar(1)  NULL,
    [Value] tinyint  NULL,
    [ID] tinyint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'ProductImages'
CREATE TABLE [dbo].[ProductImages] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ProductCode] varchar(50)  NULL,
    [Image] varchar(255)  NULL,
    [ImageLink] varchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [ImageAddIn] varchar(max)  NULL,
    [Component] varchar(50)  NULL
);
GO

-- Creating table 'Taxes'
CREATE TABLE [dbo].[Taxes] (
    [ID] tinyint IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(255)  NULL,
    [TaxRate] float  NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [UserName] varchar(150)  NOT NULL,
    [RoleId] varchar(10)  NOT NULL
);
GO

-- Creating table 'AppNetUserTypes'
CREATE TABLE [dbo].[AppNetUserTypes] (
    [Username] varchar(150)  NOT NULL,
    [Email] varchar(150)  NOT NULL,
    [Fax] varchar(20)  NULL,
    [Address] nvarchar(255)  NOT NULL,
    [Phone] varchar(20)  NOT NULL,
    [UserType] varchar(255)  NOT NULL,
    [DateCreate] varchar(10)  NOT NULL,
    [Expire] varchar(12)  NOT NULL,
    [LocationID] int  NOT NULL,
    [LocationSubID] int  NULL,
    [District] nvarchar(255)  NULL,
    [State] varchar(2)  NOT NULL,
    [TaxID] varchar(15)  NULL,
    [DisplayName] nvarchar(max)  NOT NULL,
    [UserOfName] nvarchar(128)  NULL,
    [UserOfType] nvarchar(50)  NULL,
    [IsDel] bit  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'CatalogUnits'
CREATE TABLE [dbo].[CatalogUnits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Show] bit  NULL,
    [Description] nvarchar(50)  NULL
);
GO

-- Creating table 'LocationSubs'
CREATE TABLE [dbo].[LocationSubs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [IDLocation] int  NOT NULL,
    [Name] nvarchar(255)  NULL,
    [LocationPrice] nvarchar(3)  NULL,
    [Description] nvarchar(255)  NULL,
    [IsDel] tinyint  NULL,
    [Show] bit  NULL
);
GO

-- Creating table 'LogSystems'
CREATE TABLE [dbo].[LogSystems] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Date] nvarchar(20)  NULL,
    [Code] nvarchar(50)  NULL,
    [F1] nvarchar(50)  NULL,
    [F2] nvarchar(50)  NULL,
    [F3] nvarchar(50)  NULL,
    [F4] nvarchar(50)  NULL,
    [F5] nvarchar(50)  NULL,
    [F6] nvarchar(50)  NULL,
    [F7] nvarchar(50)  NULL,
    [F8] nvarchar(50)  NULL,
    [F9] nvarchar(50)  NULL,
    [F10] nvarchar(50)  NULL,
    [F11] nvarchar(50)  NULL,
    [F12] nvarchar(50)  NULL,
    [F13] nvarchar(50)  NULL,
    [F14] nvarchar(50)  NULL,
    [F15] nvarchar(50)  NULL,
    [F16] nvarchar(50)  NULL,
    [F17] nvarchar(50)  NULL,
    [F18] nvarchar(50)  NULL,
    [F19] nvarchar(50)  NULL,
    [F20] nvarchar(max)  NULL
);
GO

-- Creating table 'PaymentDetails'
CREATE TABLE [dbo].[PaymentDetails] (
    [ID] uniqueidentifier  NOT NULL,
    [IDAccount] nvarchar(36)  NULL,
    [IDAccountInput] nvarchar(36)  NULL,
    [IDLocationSub] int  NULL,
    [Pay] decimal(18,2)  NULL,
    [PayDate] nvarchar(20)  NULL,
    [PayDateSystem] nvarchar(20)  NULL,
    [DebtBefore] decimal(18,2)  NULL,
    [DebtAfter] decimal(18,2)  NULL,
    [Description] nvarchar(255)  NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [LoginProvider], [ProviderKey] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [ProviderKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Catalogs'
ALTER TABLE [dbo].[Catalogs]
ADD CONSTRAINT [PK_Catalogs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductNames'
ALTER TABLE [dbo].[ProductNames]
ADD CONSTRAINT [PK_ProductNames]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductPrices'
ALTER TABLE [dbo].[ProductPrices]
ADD CONSTRAINT [PK_ProductPrices]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Promotions'
ALTER TABLE [dbo].[Promotions]
ADD CONSTRAINT [PK_Promotions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PromotionTypes'
ALTER TABLE [dbo].[PromotionTypes]
ADD CONSTRAINT [PK_PromotionTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductCodes'
ALTER TABLE [dbo].[ProductCodes]
ADD CONSTRAINT [PK_ProductCodes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Warranties'
ALTER TABLE [dbo].[Warranties]
ADD CONSTRAINT [PK_Warranties]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductImages'
ALTER TABLE [dbo].[ProductImages]
ADD CONSTRAINT [PK_ProductImages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Taxes'
ALTER TABLE [dbo].[Taxes]
ADD CONSTRAINT [PK_Taxes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserName], [RoleId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([UserName], [RoleId] ASC);
GO

-- Creating primary key on [Username] in table 'AppNetUserTypes'
ALTER TABLE [dbo].[AppNetUserTypes]
ADD CONSTRAINT [PK_AppNetUserTypes]
    PRIMARY KEY CLUSTERED ([Username] ASC);
GO

-- Creating primary key on [ID] in table 'CatalogUnits'
ALTER TABLE [dbo].[CatalogUnits]
ADD CONSTRAINT [PK_CatalogUnits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'LocationSubs'
ALTER TABLE [dbo].[LocationSubs]
ADD CONSTRAINT [PK_LocationSubs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'LogSystems'
ALTER TABLE [dbo].[LogSystems]
ADD CONSTRAINT [PK_LogSystems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PaymentDetails'
ALTER TABLE [dbo].[PaymentDetails]
ADD CONSTRAINT [PK_PaymentDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_User_Id]
ON [dbo].[AspNetUserClaims]
    ([User_Id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------