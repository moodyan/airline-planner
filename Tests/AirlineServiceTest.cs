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
    public void AddCity_AddsCityToAirlineService_CityList()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Virgin Airlines");
      testAirlineService.Save();

      City testCity = new City("San Diego");
      testCity.Save();

      //Act
      testAirlineService.AddCity(testCity);

      List<City> result = testAirlineService.GetCities();
      List<City> testList = new List<City>{testCity};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetCities_ReturnsAllAirlineServiceCities_CityList()
    {
      //Arrange
      AirlineService testAirlineService = new AirlineService("Virgin Airlines");
      testAirlineService.Save();

      City testCity1 = new City("New York City");
      testCity1.Save();

      City testCity2 = new City("San Diego");
      testCity2.Save();

      //Act
      testAirlineService.AddCity(testCity1);
      List<City> result = testAirlineService.GetCities();
      List<City> testList = new List<City> {testCity1};

      //Assert
      Assert.Equal(testList, result);
    }

    // [Fact]
    // public void Test_AddFlight_AddsFlightToAirlineService()
    // {
    //   //Arrange
    //   AirlineService testAirlineService = new AirlineService("Alaska");
    //   testAirlineService.Save();
    //
    //   Flight testFlight = new Flight(1234, "Portland", new DateTime (2016, 01, 01, 07, 05, 00), "Los Angeles", new DateTime (2017, 01, 01, 12, 10, 00), "ON TIME");
    //   testFlight.Save();
    //
    //   Flight testFlight2 = new Flight(4321, "LAX", new DateTime (2016, 01, 01, 07, 05, 00), "PDX", new DateTime (2017, 01, 01, 12, 10, 00), "ON TIME");
    //   testFlight2.Save();
    //
    //   //Act
    //   testAirlineService.AddFlight(testFlight);
    //   testAirlineService.AddFlight(testFlight2);
    //
    //   List<Flight> result = testAirlineService.GetFlights();
    //   List<Flight> testList = new List<Flight>{testFlight, testFlight2};
    //   Console.WriteLine(result);
    //   Console.WriteLine("testList id = {0}, {1}", testList[0].GetId(), testList[1].GetId());
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void GetFlights_ReturnsAllAirlineServiceFlights_FlightList()
    // {
    //   //Arrange
    //   AirlineService testAirlineService = new AirlineService("Alaska");
    //   testAirlineService.Save();
    //
    //   Flight testFlight1 = new Flight(1234, "Portland", new DateTime (2016, 01, 01, 07, 05, 00), "Los Angeles", new DateTime (2017, 01, 01, 12, 10, 00), "ON TIME");
    //   testFlight1.Save();
    //
    //   Flight testFlight2 = new Flight(4321, "LAX", new DateTime (2016, 01, 01, 07, 05, 00), "PDX", new DateTime (2017, 01, 01, 12, 10, 00), "ON TIME");
    //   testFlight2.Save();
    //
    //   //Act
    //   testAirlineService.AddFlight(testFlight1);
    //   List<Flight> savedFlights = testAirlineService.GetFlights();
    //   List<Flight> testList = new List<Flight> {testFlight1};
    //
    //
    //   //Assert
    //   Assert.Equal(testList, savedFlights);
    // }

    public void Dispose()
    {
      AirlineService.DeleteAll();
      City.DeleteAll();
      Flight.DeleteAll();
    }
  }
}
