using DesktopWeatherReport.Models;
using System.Threading.Tasks;

namespace DesktopWeatherReport.Controllers
{
    public interface IOpenWeatherMapController
    {
        Task<CurrentWeather> GetCurrentWeather(string location);
    }
}