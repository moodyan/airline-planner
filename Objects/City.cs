using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Objects
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = this.GetId() == newCity.GetId();
        bool nameEquality = this.GetName() == newCity.GetName();
        return (idEquality && nameEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCities;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
