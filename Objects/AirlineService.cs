using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Objects
{
  public class AirlineService
  {
    private int _id;
    private string _name;

    public AirlineService(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherAirlineService)
    {
      if (!(otherAirlineService is AirlineService))
      {
        return false;
      }
      else
      {
        AirlineService newAirlineService = (AirlineService) otherAirlineService;
        bool idEquality = this.GetId() == newAirlineService.GetId();
        bool nameEquality = this.GetName() == newAirlineService.GetName();
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
    public static List<AirlineService> GetAll()
    {
      List<AirlineService> allAirlineServices = new List<AirlineService>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM airline_services;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int airlineServiceId = rdr.GetInt32(0);
        string airlineServiceName = rdr.GetString(1);
        AirlineService newAirlineService = new AirlineService(airlineServiceName, airlineServiceId);
        allAirlineServices.Add(newAirlineService);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allAirlineServices;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO airline_services (name) OUTPUT INSERTED.id VALUES (@AirlineServiceName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AirlineServiceName";
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

    public static AirlineService Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM airline_services WHERE id = @AirlineServiceId;", conn);
      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = id.ToString();
      cmd.Parameters.Add(airlineServiceIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAirlineServiceId = 0;
      string foundAirlineServiceName = null;

      while(rdr.Read())
      {
        foundAirlineServiceId = rdr.GetInt32(0);
        foundAirlineServiceName = rdr.GetString(1);
      }
      AirlineService foundAirlineService = new AirlineService(foundAirlineServiceName, foundAirlineServiceId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAirlineService;
    }

    public void AddCity(City newCity)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities_airline_services (cities_id, airline_services_id) VALUES (@CityId, @AirlineServiceId);", conn);

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = newCity.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = this.GetId();
      cmd.Parameters.Add(airlineServiceIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<City> GetCities()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT cities_id FROM cities_airline_services WHERE airline_services_id = @AirlineServiceId;", conn);

      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = this.GetId();
      cmd.Parameters.Add(airlineServiceIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> cityIds = new List<int> {};

      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        cityIds.Add(cityId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<City> cities = new List<City> {};

      foreach (int cityId in cityIds)
      {
        SqlCommand cityQuery = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);

        SqlParameter cityIdParameter = new SqlParameter();
        cityIdParameter.ParameterName = "@CityId";
        cityIdParameter.Value = cityId;
        cityQuery.Parameters.Add(cityIdParameter);

        SqlDataReader queryReader = cityQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisCityId = queryReader.GetInt32(0);
          string cityName = queryReader.GetString(1);
          City foundCity = new City(cityName, thisCityId);
          cities.Add(foundCity);
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
      return cities;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM airline_services WHERE id = @AirlineServiceId; DELETE FROM cities_airline_services WHERE airline_services_id = @AirlineServiceId;", conn);
      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = this.GetId();

      cmd.Parameters.Add(airlineServiceIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM airline_services;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
