using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Objects
{
  public class Flight
  {
    private int _id;
    private int _number;
    private string _departureCity;
    private DateTime _departureTime;
    private string _arrivalCity;
    private DateTime _arrivalTime;
    private string _status;

    public Flight(int Number, string DepartureCity, DateTime DepartureTime, string ArrivalCity, DateTime ArrivalTime, string Status, int Id = 0)
    {
      _id = Id;
      _number = Number;
      _departureCity = DepartureCity;
      _departureTime = DepartureTime;
      _arrivalCity = ArrivalCity;
      _arrivalTime = ArrivalTime;
      _status = Status;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = this.GetId() == newFlight.GetId();
        bool numberEquality = this.GetNumber() == newFlight.GetNumber();
        bool departureCityEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
        bool departureTimeEquality = this.GetDepartureTime() == newFlight.GetDepartureTime();
        bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
        bool arrivalTimeEquality = this.GetArrivalTime() == newFlight.GetArrivalTime();
        bool statusEquality = this.GetStatus() == newFlight.GetStatus();
        return (idEquality && numberEquality && departureCityEquality && departureTimeEquality && arrivalCityEquality && arrivalTimeEquality && statusEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public int GetNumber()
    {
      return _number;
    }
    public void SetNumber(int newNumber)
    {
      _number = newNumber;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public void SetDepartureCity(string newDepartureCity)
    {
      _departureCity = newDepartureCity;
    }
    public DateTime GetDepartureTime()
    {
      return _departureTime;
    }
    public void SetDepartureTime(DateTime newDepartureTime)
    {
      _departureTime = newDepartureTime;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public void SetArrivalCity(string newArrivalCity)
    {
      _arrivalCity = newArrivalCity;
    }
    public DateTime GetArrivalTime()
    {
      return _arrivalTime;
    }
    public void SetArrivalTime(DateTime newArrivalTime)
    {
      _arrivalTime = newArrivalTime;
    }
    public string GetStatus()
    {
      return _status;
    }
    public void SetStatus(string newStatus)
    {
      _status = newStatus;
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        int flightNumber = rdr.GetInt32(1);
        string flightDepartureCity = rdr.GetString(2);
        DateTime flightDepartureTime = rdr.GetDateTime(3);
        string flightArrivalCity = rdr.GetString(4);
        DateTime flightArrivalTime = rdr.GetDateTime(5);
        string flightStatus = rdr.GetString(6);
        Flight newFlight = new Flight(flightNumber, flightDepartureCity, flightDepartureTime, flightArrivalCity, flightArrivalTime, flightStatus, flightId);
        allFlights.Add(newFlight);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allFlights;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (number, departure_city, departure_time, arrival_city, arrival_time, status) OUTPUT INSERTED.id VALUES (@FlightNumber, @FlightDepartureCity, @FlightDepartureTime, @FlightArrivalCity, @FlightArrivalTime, @FlightStatus);", conn);

      SqlParameter numberParameter = new SqlParameter();
      numberParameter.ParameterName = "@FlightNumber";
      numberParameter.Value = this.GetNumber();
      cmd.Parameters.Add(numberParameter);

      SqlParameter departureCityParameter = new SqlParameter();
      departureCityParameter.ParameterName = "@FlightDepartureCity";
      departureCityParameter.Value = this.GetDepartureCity();
      cmd.Parameters.Add(departureCityParameter);

      SqlParameter departureTimeParameter = new SqlParameter();
      departureTimeParameter.ParameterName = "@FlightDepartureTime";
      departureTimeParameter.Value = this.GetDepartureTime();
      cmd.Parameters.Add(departureTimeParameter);

      SqlParameter arrivalCityParameter = new SqlParameter();
      arrivalCityParameter.ParameterName = "@FlightArrivalCity";
      arrivalCityParameter.Value = this.GetArrivalCity();
      cmd.Parameters.Add(arrivalCityParameter);

      SqlParameter arrivalTimeParameter = new SqlParameter();
      arrivalTimeParameter.ParameterName = "@FlightArrivalTime";
      arrivalTimeParameter.Value = this.GetArrivalTime();
      cmd.Parameters.Add(arrivalTimeParameter);

      SqlParameter statusParameter = new SqlParameter();
      statusParameter.ParameterName = "@FlightStatus";
      statusParameter.Value = this.GetStatus();
      cmd.Parameters.Add(statusParameter);

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

    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFlightId = 0;
      int foundFlightNumber = 0;
      string foundFlightDepartureCity = null;
      DateTime foundFlightDepartureTime = default(DateTime);
      string foundFlightArrivalCity = null;
      DateTime foundFlightArrivalTime = default(DateTime);
      string foundFlightStatus = null;

      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundFlightNumber = rdr.GetInt32(1);
        foundFlightDepartureCity = rdr.GetString(2);
        foundFlightDepartureTime = rdr.GetDateTime(3);
        foundFlightArrivalCity = rdr.GetString(4);
        foundFlightArrivalTime = rdr.GetDateTime(5);
        foundFlightStatus = rdr.GetString(6);
      }
      Flight foundFlight = new Flight(foundFlightNumber, foundFlightDepartureCity, foundFlightDepartureTime, foundFlightArrivalCity, foundFlightArrivalTime, foundFlightStatus, foundFlightId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundFlight;
    }

    public void AddAirlineService(AirlineService newAirlineService)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO airline_services_flights (airline_services_id, flights_id) VALUES (@AirlineServiceId, @FlightId);", conn);

      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = newAirlineService.GetId();
      cmd.Parameters.Add(airlineServiceIdParameter);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();
      cmd.Parameters.Add(flightIdParameter);

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

      SqlCommand cmd = new SqlCommand("SELECT airline_services_id FROM airline_services_flights WHERE flights_id = @FlightId;", conn);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();
      cmd.Parameters.Add(flightIdParameter);

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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
