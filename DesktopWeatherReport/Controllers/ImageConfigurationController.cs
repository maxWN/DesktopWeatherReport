using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopWeatherReport.Controllers
{
    public class ImageConfigurationController : IImageConfigurationController
    {

        Form frm = null;

        public ImageConfigurationController() { }

        #region public methods

        /// <summary>
        /// Sets the weather image, given the provided description
        /// </summary>
        /// <param name="weatherState"></param>
        /// <returns></returns>
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
                        Console.WriteLine("Default case");
                        break;
                }

            }
        }

        #endregion public methods

    }
}
