using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlinePlanner.Objects;

namespace AirlinePlanner
{
  [Collection("AirlinePlanner")]
  public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CitiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = City.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      City firstCity = new City("Portland");
      City secondCity = new City("Portland");

      //Assert
      Assert.Equal(firstCity, secondCity);
    }
    [Fact]
    public void Test_Save_SavesCityToDatabase()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCityObject()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCityInDatabase()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      City foundCity = City.Find(testCity.GetId());

      //Assert
      Assert.Equal(testCity, foundCity);
    }

    [Fact]
    public void Delete_DeletesCityFromDatabase_CityList()
    {
      //Arrange
      string name1 = "Portland";
      City testCity1 = new City(name1);
      testCity1.Save();

      string name2 = "Seattle";
      City testCity2 = new City(name2);
      testCity2.Save();

      //Act
      testCity1.Delete();
      List<City> resultCities = City.GetAll();
      List<City> testCityList = new List<City> {testCity2};

      //Assert
      Assert.Equal(testCityList, resultCities);
    }

    [Fact]
    public void Test_AddAirlineService_AddsAirlineServiceToCity()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      AirlineService testAirlineService = new AirlineService("United Airlines");
      testAirlineService.Save();

      AirlineService testAirlineService2 = new AirlineService("Alaska Airlines");
      testAirlineService2.Save();

      //Act
      testCity.AddAirlineService(testAirlineService);
      testCity.AddAirlineService(testAirlineService2);

      List<AirlineService> result = testCity.GetAirlineServices();
      List<AirlineService> testList = new List<AirlineService>{testAirlineService, testAirlineService2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetAirlineServices_ReturnsAllCityAirlineServices_AirlineServiceList()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      AirlineService testAirlineService1 = new AirlineService("United Airlines");
      testAirlineService1.Save();

      AirlineService testAirlineService2 = new AirlineService("Alaska Airlines");
      testAirlineService2.Save();

      //Act
      testCity.AddAirlineService(testAirlineService1);
      List<AirlineService> savedAirlineServices = testCity.GetAirlineServices();
      List<AirlineService> testList = new List<AirlineService> {testAirlineService1};

      //Assert
      Assert.Equal(testList, savedAirlineServices);
    }

    [Fact]
    public void Delete_DeletesCityAssociationsFromDatabase_CityList()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Alaska Airlines");
      testAirlineService.Save();

      string testName = "Seattle";
      City testCity = new City(testName);
      testCity.Save();

      //Act
      testCity.AddAirlineService(testAirlineService);
      testCity.Delete();

      List<City> resultAirlineServiceCities = testAirlineService.GetCities();
      List<City> testAirlineServiceCities = new List<City> {};

      //Assert
      Assert.Equal(testAirlineServiceCities, resultAirlineServiceCities);
    }

    public void Dispose()
    {
      City.DeleteAll();
      AirlineService.DeleteAll();
    }
  }
}
