using System;

namespace DotnetCoreWebApiRedoc.Models
{
    public class WeatherForecast
    {
        /// <summary>
        /// Weather's date and time
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Celsius temperature
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Fahrenheit temperature
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Details about the weather
        /// </summary>
        public string Summary { get; set; }
    }
}
