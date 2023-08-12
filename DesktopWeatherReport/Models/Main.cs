namespace DesktopWeatherReport.Models
{
    ///<summary>
    /// Represents numeric data associated with weather conditions
    /// </summary>
    public class Main
    {
        public double temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }
}