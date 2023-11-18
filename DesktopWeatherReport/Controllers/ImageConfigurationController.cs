using Serilog;
using System.Windows.Forms;

namespace DesktopWeatherReport.Controllers
{
    public sealed class ImageConfigurationController : IImageConfigurationController
    {

        Form frm = null;

        public ImageConfigurationController() { }

        #region Public Methods

        /// <summary>
        /// Sets the weather image after matching the provided description
        /// </summary>
        /// <param name="_frm"></param>
        /// <param name="weatherState"></param>
        public void SetWeatherImage(DesktopWeatherReportForm _frm, string weatherState)
        {

            if (weatherState != null && _frm != null)
            {

                frm = _frm;

                switch (weatherState)
                {
                    // icons are all from the following source:
                    // http://www.iconarchive.com/show/oxygen-icons-by-oxygen-icons.org.18.html
                    case "Rain":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_showers_icon;
                        break;
                    case "Drizzle":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_showers_icon;
                        break;
                    case "Mist":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_many_clouds_icon;
                        break;
                    case "Sunny":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_clear_icon;
                        break;
                    case "Clear":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_clear_icon;
                        break;
                    case "Clouds":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_clouds_icon;
                        break;
                    case "Flooding":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_storm_day_icon;
                        break;
                    case "Snow":
                        _frm.pictureBox1.BackgroundImage = Properties.Resources.Status_weather_snow_icon;
                        break;
                    default:
                        Log.Error("Invalid parameter supplied!");
                        break;
                }

            }
        }

        #endregion Public Methods

    }
}
