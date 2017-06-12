USE [master]
GO
/****** Object:  Database [airline_planner]    Script Date: 6/12/2017 4:02:37 PM ******/
CREATE DATABASE [airline_planner]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'airline_planner', FILENAME = N'C:\Users\epicodus\airline_planner.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'airline_planner_log', FILENAME = N'C:\Users\epicodus\airline_planner_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [airline_planner] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [airline_planner].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [airline_planner] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [airline_planner] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [airline_planner] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [airline_planner] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [airline_planner] SET ARITHABORT OFF 
GO
ALTER DATABASE [airline_planner] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [airline_planner] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [airline_planner] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [airline_planner] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [airline_planner] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [airline_planner] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [airline_planner] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [airline_planner] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [airline_planner] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [airline_planner] SET  ENABLE_BROKER 
GO
ALTER DATABASE [airline_planner] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [airline_planner] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [airline_planner] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [airline_planner] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [airline_planner] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [airline_planner] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [airline_planner] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [airline_planner] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [airline_planner] SET  MULTI_USER 
GO
ALTER DATABASE [airline_planner] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [airline_planner] SET DB_CHAINING OFF 
GO
ALTER DATABASE [airline_planner] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [airline_planner] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [airline_planner] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [airline_planner] SET QUERY_STORE = OFF
GO
USE [airline_planner]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [airline_planner]
GO
/****** Object:  Table [dbo].[airline_services]    Script Date: 6/12/2017 4:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[airline_services](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[airline_services_flights]    Script Date: 6/12/2017 4:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[airline_services_flights](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[airline_services_id] [int] NULL,
	[flights_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cities]    Script Date: 6/12/2017 4:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cities_airline_services]    Script Date: 6/12/2017 4:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cities_airline_services](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cities_id] [int] NULL,
	[airline_services_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[flights]    Script Date: 6/12/2017 4:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[flights](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [int] NULL,
	[departure_city] [varchar](100) NULL,
	[departure_time] [datetime] NULL,
	[arrival_city] [varchar](100) NULL,
	[arrival_time] [datetime] NULL,
	[status] [varchar](50) NULL
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [airline_planner] SET  READ_WRITE 
GO
