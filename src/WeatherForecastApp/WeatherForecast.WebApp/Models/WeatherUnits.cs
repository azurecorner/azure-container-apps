﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WeatherForecast.Infrastructure.Models;

public partial class WeatherUnits
{
    public int Id { get; set; }

    public int? LocationId { get; set; }

    public string Time { get; set; }

    public string Interval { get; set; }

    public string Temperature { get; set; }

    public string Windspeed { get; set; }

    public string Winddirection { get; set; }

    public string IsDay { get; set; }

    public string Weathercode { get; set; }

    public virtual LocationViewModel Location { get; set; }
}