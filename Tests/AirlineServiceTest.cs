using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlinePlanner.Objects;

namespace AirlinePlanner
{
  [Collection("AirlinePlanner")]
  public class AirlineServiceTest : IDisposable
  {
    public AirlineServiceTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_AirlineServicesEmptyAtFirst()
    {
      //Arrange, Act
      int result = AirlineService.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      AirlineService firstAirlineService = new AirlineService("Alaska Airlines");
      AirlineService secondAirlineService = new AirlineService("Alaska Airlines");

      //Assert
      Assert.Equal(firstAirlineService, secondAirlineService);
    }
    [Fact]
    public void Test_Save_SavesAirlineServiceToDatabase()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Alaska Airlines");
      testAirlineService.Save();

      //Act
      List<AirlineService> result = AirlineService.GetAll();
      List<AirlineService> testList = new List<AirlineService>{testAirlineService};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToAirlineServiceObject()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Alaska Airlines");
      testAirlineService.Save();

      //Act
      AirlineService savedAirlineService = AirlineService.GetAll()[0];

      int result = savedAirlineService.GetId();
      int testId = testAirlineService.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsAirlineServiceInDatabase()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Alaska Airlines");
      testAirlineService.Save();

      //Act
      AirlineService foundAirlineService = AirlineService.Find(testAirlineService.GetId());

      //Assert
      Assert.Equal(testAirlineService, foundAirlineService);
    }

    [Fact]
    public void Delete_DeletesAirlineServiceFromDatabase_AirlineServiceList()
    {
      //Arrange
      string name1 = "Alaska Airlines";
      AirlineService testAirlineService1 = new AirlineService(name1);
      testAirlineService1.Save();

      string name2 = "United Airlines";
      AirlineService testAirlineService2 = new AirlineService(name2);
      testAirlineService2.Save();

      //Act
      testAirlineService1.Delete();
      List<AirlineService> resultAirlineServices = AirlineService.GetAll();
      List<AirlineService> testAirlineServiceList = new List<AirlineService> {testAirlineService2};

      //Assert
      Assert.Equal(testAirlineServiceList, resultAirlineServices);
    }

    public void Dispose()
    {
      AirlineService.DeleteAll();
      City.DeleteAll();
    }
  }
}
