namespace DesktopWeatherReport.Models
{
    /// <summary>
    /// Represents data associated with geography, and solar position
    /// </summary>
    public class Sys
    {
        public string type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }
}