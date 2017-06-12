using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlinePlanner.Objects;

namespace AirlinePlanner
{
  [Collection("AirlinePlanner")]
  public class AirlinePlannerTest : IDisposable
  {
    public AirlinePlannerTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = City.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    public void Dispose()
    {
      City.DeleteAll();
    }
  }
}
