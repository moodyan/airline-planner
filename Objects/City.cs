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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCityId = 0;
      string foundCityName = null;

      while(rdr.Read())
      {
        foundCityId = rdr.GetInt32(0);
        foundCityName = rdr.GetString(1);
      }
      City foundCity = new City(foundCityName, foundCityId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCity;
    }

    public void AddAirlineService(AirlineService newAirlineService)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities_airline_services (cities_id, airline_services_id) VALUES (@CityId, @AirlineServiceId);", conn);
      SqlParameter CityIdParameter = new SqlParameter();
      CityIdParameter.ParameterName = "@CityId";
      CityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(CityIdParameter);

      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = newAirlineService.GetId();
      cmd.Parameters.Add(airlineServiceIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<AirlineService> GetAirlineServices()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT airline_services_id FROM cities_airline_services WHERE cities_id = @CityId;", conn);

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> airlineServiceIds = new List<int> {};

      while (rdr.Read())
      {
        int airlineServiceId = rdr.GetInt32(0);
        airlineServiceIds.Add(airlineServiceId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<AirlineService> airlineServices = new List<AirlineService> {};

      foreach (int airlineServiceId in airlineServiceIds)
      {
        SqlCommand airlineServiceQuery = new SqlCommand("SELECT * FROM airline_services WHERE id = @AirlineServiceId;", conn);

        SqlParameter airlineServiceIdParameter = new SqlParameter();
        airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
        airlineServiceIdParameter.Value = airlineServiceId;
        airlineServiceQuery.Parameters.Add(airlineServiceIdParameter);

        SqlDataReader queryReader = airlineServiceQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisAirlineServiceId = queryReader.GetInt32(0);
          string airlineServiceName = queryReader.GetString(1);
          AirlineService foundAirlineService = new AirlineService(airlineServiceName, thisAirlineServiceId);
          airlineServices.Add(foundAirlineService);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      if (conn != null)
      {
        conn.Close();
      }
      return airlineServices;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_airline_services WHERE cities_id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
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
