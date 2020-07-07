using System;

namespace DotnetCoreWebApiRedoc.Models
{
    public class WeatherRecord
    {
        /// <summary>
        /// Date when the tempature was recorded
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// City where the tempature was recorded
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Celsius temperature
        /// </summary>
        public int TemperatureC { get; set; }
    }
}
