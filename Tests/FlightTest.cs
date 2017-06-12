using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlinePlanner.Objects;

namespace AirlinePlanner
{
  [Collection("AirlinePlanner")]
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_FlightEmptyAtFirst()
    {
      //Arrange, Act
      int result = Flight.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameNumber()
    {
      //Arrange, Act
      Flight firstFlight = new Flight(1234, "Portland", new DateTime (2016, 01, 01), "Los Angeles", new DateTime (2016, 01, 01), "ON TIME", 1);
      Flight secondFlight = new Flight(1234, "Portland", new DateTime (2016, 01, 01), "Los Angeles", new DateTime (2016, 01, 01), "ON TIME", 1);

      //Assert
      Assert.Equal(firstFlight, secondFlight);
    }
    [Fact]
    public void Test_Save_SavesFlightToDatabase()
    {
      //Arrange
      Flight testFlight = new Flight(1234, "Portland", new DateTime (2016, 01, 01, 07, 05, 00), "Los Angeles", new DateTime (2017, 01, 01, 12, 10, 00), "ON TIME", 1);
      testFlight.Save();

      //Act
      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};

      //Assert
      Assert.Equal(testList, result);
    }
    public void Dispose()
    {
      City.DeleteAll();
      AirlineService.DeleteAll();
      Flight.DeleteAll();
    }
  }
}
