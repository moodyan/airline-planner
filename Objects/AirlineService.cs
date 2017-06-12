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

    // public void AddFlight(Flight newFlight)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO airline_services_flights (airline_services_id, flights_id) VALUES (@AirlineServiceId, @FlightId);", conn);
    //
    //   SqlParameter airlineServiceIdParameter = new SqlParameter();
    //   airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
    //   airlineServiceIdParameter.Value = newFlight.GetId();
    //   cmd.Parameters.Add(airlineServiceIdParameter);
    //
    //   SqlParameter flightIdParameter = new SqlParameter();
    //   flightIdParameter.ParameterName = "@FlightId";
    //   flightIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(flightIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    // //
    // public List<Flight> GetFlights()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT flights_id FROM airline_services_flights WHERE airline_services_id = @AirlineServiceId;", conn);
    //
    //   SqlParameter airlineServiceIdParameter = new SqlParameter();
    //   airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
    //   airlineServiceIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(airlineServiceIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   List<int> flightIds = new List<int> {};
    //
    //   while (rdr.Read())
    //   {
    //     int flightId = rdr.GetInt32(0);
    //     flightIds.Add(flightId);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //
    //   List<Flight> flights = new List<Flight> {};
    //
    //   foreach (int flightId in flightIds)
    //   {
    //     SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);
    //
    //     SqlParameter flightIdParameter = new SqlParameter();
    //     flightIdParameter.ParameterName = "@FlightId";
    //     flightIdParameter.Value = flightId;
    //     flightQuery.Parameters.Add(flightIdParameter);
    //
    //     SqlDataReader queryReader = flightQuery.ExecuteReader();
    //     while (queryReader.Read())
    //     {
    //       int thisFlightId = queryReader.GetInt32(0);
    //       int flightNumber = queryReader.GetInt32(1);
    //       string flightDepartureCity = queryReader.GetString(2);
    //       DateTime flightDepartureTime = queryReader.GetDateTime(3);
    //       string flightArrivalCity = queryReader.GetString(4);
    //       DateTime flightArrivalTime = queryReader.GetDateTime(5);
    //       string flightStatus = queryReader.GetString(6);
    //       Flight foundFlight = new Flight(flightNumber, flightDepartureCity, flightDepartureTime, flightArrivalCity, flightArrivalTime, flightStatus, thisFlightId);
    //       flights.Add(foundFlight);
    //     }
    //     if (queryReader != null)
    //     {
    //       queryReader.Close();
    //     }
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return flights;
    // }

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
