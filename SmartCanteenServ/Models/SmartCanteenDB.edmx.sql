
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/11/2016 14:53:00
-- Generated from EDMX file: D:\Backup -MVC\MVC\SmartCanteenServ\SmartCanteenServ\Models\SmartCanteenDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SmartCanteenDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CouponMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CouponMaster];
GO
IF OBJECT_ID(N'[dbo].[CouponNotification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CouponNotification];
GO
IF OBJECT_ID(N'[dbo].[DailyMenuMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DailyMenuMaster];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[Floor_Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Floor_Master];
GO
IF OBJECT_ID(N'[dbo].[FoodCategoryMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FoodCategoryMaster];
GO
IF OBJECT_ID(N'[dbo].[FoodOrderMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FoodOrderMaster];
GO
IF OBJECT_ID(N'[dbo].[MenuItem_Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MenuItem_Master];
GO
IF OBJECT_ID(N'[dbo].[Other_Expense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Other_Expense];
GO
IF OBJECT_ID(N'[dbo].[UserMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserMaster];
GO
IF OBJECT_ID(N'[dbo].[Weekday_Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Weekday_Master];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CouponMasters'
CREATE TABLE [dbo].[CouponMasters] (
    [CM_ID] bigint IDENTITY(1,1) NOT NULL,
    [CM_Empid] varchar(20)  NOT NULL,
    [CM_Date] datetime  NOT NULL,
    [CM_FLR_ID] int  NOT NULL,
    [CM_Requested_bln] bit  NOT NULL,
    [CM_Accepted_bln] bit  NOT NULL
);
GO

-- Creating table 'CouponNotifications'
CREATE TABLE [dbo].[CouponNotifications] (
    [CN_ID] bigint IDENTITY(1,1) NOT NULL,
    [CN_CM_ID] bigint  NOT NULL,
    [CN_CM_Empid] varchar(20)  NULL,
    [CN_CM_Date] datetime  NOT NULL,
    [CN_CM_FLR_ID] int  NOT NULL,
    [CN_CM_Requested_bln] bit  NOT NULL,
    [CN_CM_Accepted_bln] bit  NOT NULL
);
GO

-- Creating table 'DailyMenuMasters'
CREATE TABLE [dbo].[DailyMenuMasters] (
    [DMM_ID] int IDENTITY(1,1) NOT NULL,
    [DMM_WDM_ID] int  NULL,
    [DMM_MI_ID] int  NULL,
    [DMM_MI_FCM_ID] int  NULL,
    [DMM_FM_ID] int  NULL,
    [DMM_From_Date] datetime  NULL,
    [DMM_To_Date] datetime  NULL,
    [Todays_Date] datetime  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Empid] int IDENTITY(1,1) NOT NULL,
    [EmpFirstName] varchar(50)  NULL,
    [EmpLastName] varchar(50)  NULL,
    [EmpMobNo] bigint  NULL,
    [EmpLoginId] varchar(20)  NOT NULL,
    [EmpPassword] varchar(20)  NULL,
    [EmpType] int  NULL,
    [Emp_FM_ID] int  NULL
);
GO

-- Creating table 'Floor_Master'
CREATE TABLE [dbo].[Floor_Master] (
    [FM_Id] int IDENTITY(1,1) NOT NULL,
    [FM_Description] varchar(100)  NOT NULL
);
GO

-- Creating table 'FoodCategoryMasters'
CREATE TABLE [dbo].[FoodCategoryMasters] (
    [FCM_ID] int IDENTITY(1,1) NOT NULL,
    [FCM_Type] varchar(100)  NOT NULL
);
GO

-- Creating table 'FoodOrderMasters'
CREATE TABLE [dbo].[FoodOrderMasters] (
    [FOM_ID] bigint IDENTITY(1,1) NOT NULL,
    [FOM_EmpLoginId] varchar(20)  NOT NULL,
    [FOM_FloorId] int  NOT NULL,
    [FOM_CategoryId] int  NOT NULL,
    [FOM_MenuItemId] int  NOT NULL,
    [FOM_Qty] int  NOT NULL,
    [FOM_OrderNumber] bigint  NULL,
    [FOM_OrderCost] float  NOT NULL,
    [FOM_Date] datetime  NOT NULL,
    [FOM_ToCart] bit  NOT NULL,
    [FOM_RequestedBln] bit  NOT NULL,
    [FOM_ConfirmedBln] bit  NOT NULL,
    [FOM_CancelledBln] bit  NOT NULL
);
GO

-- Creating table 'MenuItem_Master'
CREATE TABLE [dbo].[MenuItem_Master] (
    [MI_ID] int IDENTITY(1,1) NOT NULL,
    [MI_Description] varchar(100)  NULL,
    [MI_Price] float  NULL,
    [MI_FCM_ID] int  NULL,
    [MI_FoodType] int  NOT NULL
);
GO

-- Creating table 'UserMasters'
CREATE TABLE [dbo].[UserMasters] (
    [UserType] int IDENTITY(1,1) NOT NULL,
    [UserTypedescription] varchar(20)  NULL
);
GO

-- Creating table 'Weekday_Master'
CREATE TABLE [dbo].[Weekday_Master] (
    [WDM_ID] int IDENTITY(1,1) NOT NULL,
    [WDM_Description] varchar(10)  NOT NULL
);
GO

-- Creating table 'Other_Expense'
CREATE TABLE [dbo].[Other_Expense] (
    [Expense_Id] int IDENTITY(1,1) NOT NULL,
    [EmpLoginId] varchar(20)  NOT NULL,
    [Expense_Amount] float  NOT NULL,
    [Expense_Date] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CM_ID] in table 'CouponMasters'
ALTER TABLE [dbo].[CouponMasters]
ADD CONSTRAINT [PK_CouponMasters]
    PRIMARY KEY CLUSTERED ([CM_ID] ASC);
GO

-- Creating primary key on [CN_ID] in table 'CouponNotifications'
ALTER TABLE [dbo].[CouponNotifications]
ADD CONSTRAINT [PK_CouponNotifications]
    PRIMARY KEY CLUSTERED ([CN_ID] ASC);
GO

-- Creating primary key on [DMM_ID] in table 'DailyMenuMasters'
ALTER TABLE [dbo].[DailyMenuMasters]
ADD CONSTRAINT [PK_DailyMenuMasters]
    PRIMARY KEY CLUSTERED ([DMM_ID] ASC);
GO

-- Creating primary key on [EmpLoginId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([EmpLoginId] ASC);
GO

-- Creating primary key on [FM_Description] in table 'Floor_Master'
ALTER TABLE [dbo].[Floor_Master]
ADD CONSTRAINT [PK_Floor_Master]
    PRIMARY KEY CLUSTERED ([FM_Description] ASC);
GO

-- Creating primary key on [FCM_Type] in table 'FoodCategoryMasters'
ALTER TABLE [dbo].[FoodCategoryMasters]
ADD CONSTRAINT [PK_FoodCategoryMasters]
    PRIMARY KEY CLUSTERED ([FCM_Type] ASC);
GO

-- Creating primary key on [FOM_ID] in table 'FoodOrderMasters'
ALTER TABLE [dbo].[FoodOrderMasters]
ADD CONSTRAINT [PK_FoodOrderMasters]
    PRIMARY KEY CLUSTERED ([FOM_ID] ASC);
GO

-- Creating primary key on [MI_ID] in table 'MenuItem_Master'
ALTER TABLE [dbo].[MenuItem_Master]
ADD CONSTRAINT [PK_MenuItem_Master]
    PRIMARY KEY CLUSTERED ([MI_ID] ASC);
GO

-- Creating primary key on [UserType] in table 'UserMasters'
ALTER TABLE [dbo].[UserMasters]
ADD CONSTRAINT [PK_UserMasters]
    PRIMARY KEY CLUSTERED ([UserType] ASC);
GO

-- Creating primary key on [WDM_Description] in table 'Weekday_Master'
ALTER TABLE [dbo].[Weekday_Master]
ADD CONSTRAINT [PK_Weekday_Master]
    PRIMARY KEY CLUSTERED ([WDM_Description] ASC);
GO

-- Creating primary key on [Expense_Id] in table 'Other_Expense'
ALTER TABLE [dbo].[Other_Expense]
ADD CONSTRAINT [PK_Other_Expense]
    PRIMARY KEY CLUSTERED ([Expense_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------