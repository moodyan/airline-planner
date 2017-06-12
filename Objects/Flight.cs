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
    private int _airlineServiceId;

    public Flight(int Number, string DepartureCity, DateTime DepartureTime, string ArrivalCity, DateTime ArrivalTime, string Status, int AirlineServiceId, int Id = 0)
    {
      _id = Id;
      _number = Number;
      _departureCity = DepartureCity;
      _departureTime = DepartureTime;
      _arrivalCity = ArrivalCity;
      _arrivalTime = ArrivalTime;
      _status = Status;
      _airlineServiceId = AirlineServiceId;
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
        bool airlineServiceIdEquality = this.GetAirlineServiceId() == newFlight.GetAirlineServiceId();
        return (idEquality && numberEquality && departureCityEquality && departureTimeEquality && arrivalCityEquality && arrivalTimeEquality && statusEquality && airlineServiceIdEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public int GetAirlineServiceId()
    {
      return _airlineServiceId;
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
        int airlineServiceId = rdr.GetInt32(7);
        Flight newFlight = new Flight(flightNumber, flightDepartureCity, flightDepartureTime, flightArrivalCity, flightArrivalTime, flightStatus, airlineServiceId, flightId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (number, departure_city, departure_time, arrival_city, arrival_time, status, airline_services_id) OUTPUT INSERTED.id VALUES (@FlightNumber, @FlightDepartureCity, @FlightDepartureTime, @FlightArrivalCity, @FlightArrivalTime, @FlightStatus, @AirlineServiceId);", conn);

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

      SqlParameter airlineServiceIdParameter = new SqlParameter();
      airlineServiceIdParameter.ParameterName = "@AirlineServiceId";
      airlineServiceIdParameter.Value = this.GetAirlineServiceId();
      cmd.Parameters.Add(airlineServiceIdParameter);

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
