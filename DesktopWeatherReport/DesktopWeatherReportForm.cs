using DesktopWeatherReport.Controllers;
using DesktopWeatherReport.Models;
using Serilog;
using System;
using System.Windows.Forms;

namespace DesktopWeatherReport
{
    public partial class DesktopWeatherReportForm : Form
    {
        private readonly IOpenWeatherMapController openWeatherMapController;
        private readonly IImageConfigurationController imageConfigurationController;
        private CurrentWeather formData;

        public DesktopWeatherReportForm(IOpenWeatherMapController openWeatherMapController, IImageConfigurationController imageConfigurationController)
        {
            InitializeComponent();
            SetDefaultAppView();
            this.openWeatherMapController = openWeatherMapController;
            this.imageConfigurationController = imageConfigurationController;
            pictureBox1.BackgroundImage = Properties.Resources.Todays_Weather;
        }

        #region View Config Methods 

        /// <summary>
        /// Loads basic app UI
        /// </summary>
        public void SetDefaultAppView()
        {
            //define list attributes
            WeatherTable.View = View.Details;
            WeatherTable.LabelEdit = true;
            WeatherTable.AllowColumnReorder = true;
            WeatherTable.GridLines = true;
            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            WeatherTable.Columns.Add("Region", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Perspiration", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Temperature", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Wind", -2, HorizontalAlignment.Center);
            WeatherTable.Columns[0].Width = 200;
            //WeatherTable.Columns[0].Text.
        }

        /// <summary>
        /// Configure list with API data
        /// </summary>
        /// <param name="weatherMap"></param>
        public void ConfigureListView(CurrentWeather weatherMap)
        {
            if (weatherMap != null)
            {
                label1.Text = weatherMap.primary[0].main;               
                label1.Visible = true;
                imageConfigurationController.SetWeatherImage(this, label1.Text);

                // Create three items and three sets of subitems for each item.
                ListViewItem item1 = new ListViewItem(weatherMap.name, 0);
                item1.SubItems.Add(weatherMap.clouds.all.ToString() + " %");
                item1.SubItems.Add(weatherMap.main.temp.ToString() + " °C");
                item1.SubItems.Add(weatherMap.wind.speed.ToString() + " MPH");

                // Add the items to the ListView.
                WeatherTable.Items.AddRange(new ListViewItem[] { item1 });
            }
            else
            {
                // if no data was found, or error occurred, populate list with dummy data
                DefaultListViewPopulation();
            }

        }

        /// <summary>
        /// Populate list with default values if connection is broken
        /// </summary>
        public void DefaultListViewPopulation()
        {

            ListViewItem item1 = new ListViewItem("city name 1", 0);
            item1.SubItems.Add("severity of rainfall");
            item1.SubItems.Add("temp");
            item1.SubItems.Add("wind speed");
            //ListViewItem first argument in constructor represents the name of a new column
            ListViewItem item2 = new ListViewItem("city name 2", 1);
            item2.SubItems.Add("z");
            item2.SubItems.Add("x");
            item2.SubItems.Add("y");
            ListViewItem item3 = new ListViewItem("city name 3", 0);

            item3.Checked = true;
            item3.SubItems.Add("z");
            item3.SubItems.Add("x");
            item3.SubItems.Add("y");

            // Create columns for the items and subitems.
            // Width of -2 represents auto-size.
            WeatherTable.Columns.Add("Location", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Perspiration", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Temperature", -2, HorizontalAlignment.Left);
            WeatherTable.Columns.Add("Wind", -2, HorizontalAlignment.Center);
            WeatherTable.Items.AddRange(new ListViewItem[] { item1, item2, item3 });
            // Prevent user interaction
            SubmitBtn.Enabled = false;
        }

        #endregion View Config Methods

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            Log.Information($"Entered {base.ToString()}.{nameof(SubmitBtn_Click)}");

            try
            {
                formData = openWeatherMapController.GetCurrentWeather(textBox1.Text);
                if (formData != null)
                {
                    ConfigureListView(formData);
                }
            }
            catch (Exception ex)
            {
                Log.Error("The following error, " + ex.Message + ", occurred in SubmitBtn_Click()");
                ErrorMessage.Text = "Error occurred: Please restart application.";
                ErrorMessage.Visible = true;
            }
        }
    }
}
