USE [master]
GO
/****** Object:  Database [AutomationDB]    Script Date: 01/27/2018 1:54:30 AM ******/
CREATE DATABASE [AutomationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AutomationDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AutomationDB.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AutomationDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AutomationDB_log.ldf' , SIZE = 9216KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AutomationDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AutomationDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AutomationDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AutomationDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AutomationDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AutomationDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AutomationDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AutomationDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AutomationDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AutomationDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AutomationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AutomationDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AutomationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AutomationDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AutomationDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AutomationDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AutomationDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AutomationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AutomationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AutomationDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AutomationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AutomationDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AutomationDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AutomationDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AutomationDB] SET RECOVERY FULL 
GO
ALTER DATABASE [AutomationDB] SET  MULTI_USER 
GO
ALTER DATABASE [AutomationDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AutomationDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AutomationDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AutomationDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [AutomationDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'AutomationDB', N'ON'
GO
USE [AutomationDB]
GO
/****** Object:  User [NT AUTHORITY\SYSTEM]    Script Date: 01/27/2018 1:54:30 AM ******/
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [Automation]    Script Date: 01/27/2018 1:54:30 AM ******/
CREATE USER [Automation] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [AutoDash]    Script Date: 01/27/2018 1:54:30 AM ******/
CREATE USER [AutoDash] FOR LOGIN [AutoDash] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Automation]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Automation]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Automation]
GO
/****** Object:  Table [dbo].[ActiveAutomationBuilds]    Script Date: 01/27/2018 1:54:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveAutomationBuilds](
	[Build] [nvarchar](max) NOT NULL,
	[JobId] [int] NOT NULL,
 CONSTRAINT [PK_ActiveAutomationBuilds] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AutomationTasks]    Script Date: 01/27/2018 1:54:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AutomationTasks](
	[Id] [int] NOT NULL,
	[Config] [varbinary](max) NOT NULL,
	[AutomationReport] [varbinary](max) NULL,
	[IsComplete] [bit] NULL,
	[SlaveId] [nvarchar](100) NULL,
	[JobId] [int] NULL,
	[Suite] [varchar](max) NOT NULL,
	[SuiteName] [nvarchar](max) NULL,
	[TimeForCompletion] [nvarchar](max) NULL,
	[DBExceptionsLog] [varbinary](max) NULL,
	[ConfigurationsUsedForExecution] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ManagerJobs]    Script Date: 01/27/2018 1:54:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ManagerJobs](
	[Id] [int] NOT NULL,
	[AutomationRunType] [nvarchar](max) NOT NULL,
	[Status] [bit] NULL,
	[Config] [varbinary](max) NOT NULL,
	[TimeForCompletion] [nvarchar](max) NULL,
 CONSTRAINT [PK_ManagerJobs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SlaveMachines]    Script Date: 01/27/2018 1:54:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlaveMachines](
	[SlaveId] [nvarchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[UpdateOptimus] [bit] NULL,
 CONSTRAINT [PK_SlaveMachines] PRIMARY KEY CLUSTERED 
(
	[SlaveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ActiveAutomationBuilds]  WITH CHECK ADD  CONSTRAINT [FK_ActiveAutomationBuilds_ManagerJobs] FOREIGN KEY([JobId])
REFERENCES [dbo].[ManagerJobs] ([Id])
GO
ALTER TABLE [dbo].[ActiveAutomationBuilds] CHECK CONSTRAINT [FK_ActiveAutomationBuilds_ManagerJobs]
GO
ALTER TABLE [dbo].[AutomationTasks]  WITH CHECK ADD FOREIGN KEY([JobId])
REFERENCES [dbo].[ManagerJobs] ([Id])
GO
ALTER TABLE [dbo].[AutomationTasks]  WITH CHECK ADD FOREIGN KEY([SlaveId])
REFERENCES [dbo].[SlaveMachines] ([SlaveId])
GO
USE [master]
GO
ALTER DATABASE [AutomationDB] SET  READ_WRITE 
GO
