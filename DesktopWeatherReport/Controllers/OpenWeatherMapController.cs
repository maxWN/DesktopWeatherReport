using DesktopWeatherReport.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace DesktopWeatherReport.Controllers
{
    public sealed class OpenWeatherMapController : IOpenWeatherMapController
    {
        private readonly IHttpClientFactory httpClientFactory;

        public OpenWeatherMapController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #region Private Methods

        /// <summary>
        /// Gets the asynchronous response after making the call to the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string requestUrl) where T : class
        {
            T weatherReport = null;

            try
            {
                HttpClient client = httpClientFactory.CreateClient();

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                HttpContent content = response.Content;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ToString());
                }

                weatherReport = await ParseResponseContent<T>(content);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }

            return weatherReport;

        }

        /// <summary>
        /// Parses the returned HTTP content into .NET types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<T> ParseResponseContent<T>(HttpContent content) where T : class
        {
            dynamic parsedContent = null;
            try
            {

                string result = await content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(result))
                    throw new ArgumentNullException($"{base.ToString()}.{nameof(ParseResponseContent)} Error: response content cannot be null.");

                JObject jobject = JObject.Parse(result);
                JToken memberName = (JArray)jobject["weather"];
                parsedContent = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                parsedContent.primary = memberName.ToObject<List<Weather>>().ToArray();

            }
            catch(Exception ex) 
            {
                Log.Error(ex.Message);
                throw;
            }

            return parsedContent;
        }

        /// <summary>
        /// Constructs an API URL from parameters
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string BuildRequestUri(string arg)
        {
            // TODO: Create controller that first requires user to sign 
            // up to site then enter in their API key?
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.PARTIAL_URL) || string.IsNullOrWhiteSpace(Properties.Settings.Default.SECRET_KEY))
                throw new ArgumentNullException($"{base.ToString()}.{nameof(BuildRequestUri)} Error: null or empty argument was given.");

            return $"{Properties.Settings.Default.PARTIAL_URL}q={arg}{Properties.Settings.Default.SECRET_KEY}";
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Passes the location to the API call
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<CurrentWeather> GetCurrentWeather(string location)
        {
            CurrentWeather currentWeather = null;
            string uriPath;

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentNullException($"{base.ToString()}.{nameof(GetCurrentWeather)} Error: null or empty location was given.");

            try
            {
                uriPath = BuildRequestUri(location);

                currentWeather = await GetAsync<CurrentWeather>(uriPath);
            }
            catch (Exception ex) {
                Log.Error(ex.Message);
                throw;
            }

            return currentWeather;
        }

        #endregion Public Methods
    }
}