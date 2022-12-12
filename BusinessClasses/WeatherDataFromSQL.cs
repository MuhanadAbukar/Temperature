namespace SQLWeather
{
    public class WeatherFromSQL
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double Precipitation { get; set; }
        public double Humidity { get; set; }
        public double WindSpeedGust { get; set; }
        public double WindDirection { get; set; }
        public string Sky { get; set; }
    }
}