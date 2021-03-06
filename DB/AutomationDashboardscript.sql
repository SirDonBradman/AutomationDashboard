USE [master]
GO
/****** Object:  Database [AutomationDashboard]    Script Date: 01/27/2018 1:53:01 AM ******/
CREATE DATABASE [AutomationDashboard]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AutomationDashboard1', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AutomationDashboard1.mdf' , SIZE = 77824KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AutomationDashboard1_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AutomationDashboard1_log.ldf' , SIZE = 1219712KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AutomationDashboard] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AutomationDashboard].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AutomationDashboard] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AutomationDashboard] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AutomationDashboard] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AutomationDashboard] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AutomationDashboard] SET ARITHABORT OFF 
GO
ALTER DATABASE [AutomationDashboard] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AutomationDashboard] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AutomationDashboard] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AutomationDashboard] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AutomationDashboard] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AutomationDashboard] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AutomationDashboard] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AutomationDashboard] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AutomationDashboard] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AutomationDashboard] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AutomationDashboard] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AutomationDashboard] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AutomationDashboard] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AutomationDashboard] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AutomationDashboard] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AutomationDashboard] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AutomationDashboard] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AutomationDashboard] SET RECOVERY FULL 
GO
ALTER DATABASE [AutomationDashboard] SET  MULTI_USER 
GO
ALTER DATABASE [AutomationDashboard] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AutomationDashboard] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AutomationDashboard] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AutomationDashboard] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [AutomationDashboard] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'AutomationDashboard', N'ON'
GO
USE [AutomationDashboard]
GO
/****** Object:  User [NT AUTHORITY\SYSTEM]    Script Date: 01/27/2018 1:53:01 AM ******/
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [Automation]    Script Date: 01/27/2018 1:53:01 AM ******/
CREATE USER [Automation] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [AutoDash]    Script Date: 01/27/2018 1:53:01 AM ******/
CREATE USER [AutoDash] FOR LOGIN [AutoDash] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Automation]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Automation]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Automation]
GO
/****** Object:  Table [dbo].[AutomateData]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AutomateData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductArea] [varchar](max) NOT NULL,
	[NoOfTFSTestcase] [int] NULL,
	[NoOfAutomatedTestcases] [int] NULL,
	[TeamId] [int] NULL,
	[date] [date] NOT NULL,
	[ExpectedTestCases] [int] NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CoreModSmokes]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CoreModSmokes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[detail_results] [varbinary](max) NULL,
	[test_run_stats] [varchar](max) NULL,
	[TeamId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DailySmokes]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DailySmokes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[detail_results] [varbinary](max) NULL,
	[test_run_stats] [varchar](max) NULL,
	[TeamId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExpectedTestsForSuites]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpectedTestsForSuites](
	[SuiteId] [int] NOT NULL,
	[ExpectedTests] [int] NOT NULL,
 CONSTRAINT [PK_ExpectedTestsForSuites] PRIMARY KEY CLUSTERED 
(
	[SuiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FullPass]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FullPass](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[detail_results] [varbinary](max) NULL,
	[test_run_stats] [varchar](max) NULL,
	[TeamId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductTeams]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductTeams](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [varchar](max) NOT NULL,
	[TeamProject] [varchar](max) NULL,
	[TestPlanName] [varchar](max) NULL,
	[TotalTFSTests] [int] NULL,
	[TotalAutomatedTests] [int] NULL,
	[TotalExpectedTests] [int] NULL,
 CONSTRAINT [PK_ProductTeams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestPlans]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestPlans](
	[PlanId] [int] NOT NULL,
	[TestPlanName] [varchar](max) NOT NULL,
	[TeamId] [int] NOT NULL,
	[TestCaseCount] [int] NOT NULL,
 CONSTRAINT [PK_TestPlans] PRIMARY KEY CLUSTERED 
(
	[PlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestSuites]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestSuites](
	[SuiteId] [int] NOT NULL,
	[SuiteName] [varchar](max) NOT NULL,
	[TestPlanId] [int] NOT NULL,
	[ParentSuite] [int] NOT NULL,
	[TotalTests] [int] NOT NULL,
	[TotalAutomatedTests] [int] NOT NULL,
 CONSTRAINT [PK_TestSuites] PRIMARY KEY CLUSTERED 
(
	[SuiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WeeklyTriaging]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeeklyTriaging](
	[EntryNumber] [int] IDENTITY(1,1) NOT NULL,
	[TeamId] [int] NULL,
	[ProductArea] [varchar](max) NULL,
	[NoOfTestCasesDebugged] [int] NULL DEFAULT ((0)),
	[Date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EntryNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[AutomateData]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
ALTER TABLE [dbo].[CoreModSmokes]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
ALTER TABLE [dbo].[DailySmokes]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
ALTER TABLE [dbo].[FullPass]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
ALTER TABLE [dbo].[TestPlans]  WITH CHECK ADD  CONSTRAINT [FK_TestPlans_ProductTeams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
ALTER TABLE [dbo].[TestPlans] CHECK CONSTRAINT [FK_TestPlans_ProductTeams]
GO
ALTER TABLE [dbo].[TestSuites]  WITH CHECK ADD  CONSTRAINT [FK_TestSuites_TestPlans] FOREIGN KEY([TestPlanId])
REFERENCES [dbo].[TestPlans] ([PlanId])
GO
ALTER TABLE [dbo].[TestSuites] CHECK CONSTRAINT [FK_TestSuites_TestPlans]
GO
ALTER TABLE [dbo].[WeeklyTriaging]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[ProductTeams] ([TeamId])
GO
/****** Object:  StoredProcedure [dbo].[spDeleteTestPlansForTeam]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  create procedure [dbo].[spDeleteTestPlansForTeam]
  @TeamId int
  as
  begin
  delete from TestPlans
  where TeamId = @TeamId
  end

GO
/****** Object:  StoredProcedure [dbo].[spDeleteTestSuitesForTeam]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  create procedure [dbo].[spDeleteTestSuitesForTeam]
  @TeamId int
  as
  begin
  delete from TestSuites
  where TestPlanId in (select PlanId from [AutomationDashboard].[dbo].[TestPlans] where TeamId = @TeamId)
  end

GO
/****** Object:  StoredProcedure [dbo].[spGetAggregateWeeklyStatus]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  create procedure [dbo].[spGetAggregateWeeklyStatus]
  @TeamId int
  as
  begin
  select SUM(NoOfTFSTestcase) as NoOfTFSTestcase,SUM(NoOfAutomatedTestcases) as NoOfAutomatedTescases, [date] from [AutomateData]
  where TeamId = @TeamId
  group by [date]
  end
GO
/****** Object:  StoredProcedure [dbo].[spGetCurrentStatusOfAutomationForATeam]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spGetCurrentStatusOfAutomationForATeam] --
@TeamId int
as
begin
select * from AutomateData a
where NoOfTFSTestcase = (select MAX(NoOfTFSTestcase) from AutomateData where TeamId = a.TeamId and ProductArea = a.ProductArea) and
NoOfAutomatedTestcases = (select MAX(NoOfAutomatedTestcases) from AutomateData where TeamId = a.TeamId  and ProductArea = a.ProductArea)
and a.TeamId = @TeamId 
order by ProductArea asc
end

GO
/****** Object:  StoredProcedure [dbo].[spGetPreviousAndCurrentWeekAutomationAggregateData]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spGetPreviousAndCurrentWeekAutomationAggregateData]
@TeamId int
as
begin
Select SUM(TFS) as TotalTFS, Sum(Automated) as TotalAutomated
from
(
select ProductArea, MAX(NoOfTFSTestcase) as TFS, MAX(NoOfAutomatedTestcases) as Automated
from AutomateData 
where TeamId=@TeamId 
group by ProductArea
)tbl
union
Select SUM(TFS) as TotalTFS, Sum(Automated) as TotalAutomated
from
(
select ProductArea, MAX(NoOfTFSTestcase) as TFS, MAX(NoOfAutomatedTestcases) as Automated
from AutomateData 
where TeamId=@TeamId  and date < (select MAX(date) from AutomateData where TeamId=@TeamId)
group by ProductArea
)tbl
end
GO
/****** Object:  StoredProcedure [dbo].[spGetStatusOfAutomationForATeamWithSpecifiedDate]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spGetStatusOfAutomationForATeamWithSpecifiedDate] --8, '2016-10-07'
@TeamId int,
@date Date
as
begin
select * from AutomateData a
where NoOfTFSTestcase = (select MAX(NoOfTFSTestcase) from AutomateData where TeamId = a.TeamId and ProductArea = a.ProductArea and date<=@date) and
NoOfAutomatedTestcases = (select MAX(NoOfAutomatedTestcases) from AutomateData where TeamId = a.TeamId  and ProductArea = a.ProductArea and date<=@date)
and TeamId = @TeamId
end
GO
/****** Object:  StoredProcedure [dbo].[spGetSuiteDetails]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  create procedure [dbo].[spGetSuiteDetails]
   @suiteId int
  as
  begin
  select t.SuiteId, t.SuiteName, t.TestPlanId, t.ParentSuite, t.TotalTests, t.TotalAutomatedTests, e.ExpectedTests 
  from TestSuites t, ExpectedTestsForSuites e
  where t.SuiteId = e.SuiteId and t.SuiteId = @suiteId
  order by ParentSuite
  end
  
GO
/****** Object:  StoredProcedure [dbo].[spGetTestSuitesForTeam]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  CREATE procedure [dbo].[spGetTestSuitesForTeam]
  @TeamId int
  as
  begin
  select t.SuiteId, t.SuiteName, t.TestPlanId, t.ParentSuite, t.TotalTests, t.TotalAutomatedTests, e.ExpectedTests 
  from TestSuites t, ExpectedTestsForSuites e
  where TestPlanId in (select PlanId from [AutomationDashboard].[dbo].[TestPlans] where TeamId = @TeamId)
  and (TotalTests>0 or TotalAutomatedTests>0) and t.SuiteId = e.SuiteId
  order by ParentSuite
  end
GO
/****** Object:  StoredProcedure [dbo].[spGetWeeklyProgressForProductArea]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  CREATE procedure [dbo].[spGetWeeklyProgressForProductArea] --'Fund Transactions', 1
  @productArea varchar(max),
  @teamId int,
  @from date,
  @to date
  as
  begin
  with a
  as
  (
  select top 2 * from [dbo].[AutomateData] a
  where ProductArea = @productArea and TeamId = @teamId and date between @from and @to
  order by date desc
  )
  select top 1 a.ProductArea, (Max(a.NoOfAutomatedTestcases) over (partition by a.ProductArea) - MIN(a.NoOfAutomatedTestcases) over (partition by a.ProductArea)) as AutomatedThisWeek, 
  (Max(a.NoOfTFSTestcase) over (partition by a.ProductArea) - MIN(a.NoOfTFSTestcase) over (partition by a.ProductArea)) as TFSTestCasesThisWeek
  ,a.TeamId 
  from a
  end
GO
/****** Object:  StoredProcedure [dbo].[spGetWeeklyProgressForProductAreaBetweenDates]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE procedure [dbo].[spGetWeeklyProgressForProductAreaBetweenDates] --'User Management', 1, '2016-09-21','2016-09-14'
  @productArea varchar(max),
  @teamId int,
  @Date1 date,
  @Date2 date
  as
  begin
  with a
  as
  (
  select top 2 * from [dbo].[AutomateData] a
  where ProductArea = @productArea and TeamId = @teamId and a.date >= @Date1 and a.date <= @Date2
  order by date desc
  )
  select top 1 a.ProductArea, (Max(a.NoOfAutomatedTestcases) over (partition by a.ProductArea) - MIN(a.NoOfAutomatedTestcases) over (partition by a.ProductArea)) as AutomatedThisWeek, 
  (Max(a.NoOfTFSTestcase) over (partition by a.ProductArea) - MIN(a.NoOfTFSTestcase) over (partition by a.ProductArea)) as TFSTestCasesThisWeek
  ,a.TeamId 
  from a
  end
GO
/****** Object:  StoredProcedure [dbo].[spGetWeeklyTriagingForWeekForGivenTeam]    Script Date: 01/27/2018 1:53:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spGetWeeklyTriagingForWeekForGivenTeam]
@CurrentFriday date,
@PreviousFriday date,
@TeamId int
as
begin
select * from WeeklyTriaging
where TeamId = @TeamId and date<=@CurrentFriday and date>@PreviousFriday
end

GO
USE [master]
GO
ALTER DATABASE [AutomationDashboard] SET  READ_WRITE 
GO
