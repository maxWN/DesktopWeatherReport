using DesktopWeatherReport.Models;

namespace DesktopWeatherReport.Controllers
{
    public interface IOpenWeatherMapController
    {
        CurrentWeather GetCurrentWeather(string location);
    }
}