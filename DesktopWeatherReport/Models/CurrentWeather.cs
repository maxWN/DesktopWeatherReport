namespace DesktopWeatherReport.Models
{
    public class CurrentWeather
    {
        public Coordinates coord { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

        // does not match actual retrieved data set property name
        public Weather[] primary { get; set; }
    }
}