using DesktopWeatherReport.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesktopWeatherReport.Controllers
{
    public class OpenWeatherMapController : IOpenWeatherMapController
    {
        private readonly IHttpClientFactory httpClientFactory;

        public OpenWeatherMapController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        #region Private Methods

        /// <summary>
        /// Gets the asynchronous response after making the call to the API.
        /// </summary>
        private async Task<T> GetAsync<T>(string requestUrl) where T : class
        {
            T retVal = null;

            #region Actions

            try
            {
                HttpClient client = httpClientFactory.CreateClient();

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                HttpContent content = response.Content;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ToString());
                }

                retVal = await ParseContent<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return retVal;

            #endregion Actions
        }

        /// <summary>
        /// Parses the returned HTTP content into .NET types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<T> ParseContent<T>(HttpContent content) where T : class
        {
            dynamic retVal = null;

            string result = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(result) && !string.IsNullOrWhiteSpace(result))
            {
                JObject jobject = JObject.Parse(result);
                JToken memberName = (JArray)jobject["weather"];
                retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                retVal.primary = memberName.ToObject<List<Weather>>().ToArray();
            }

            return retVal;
        }

        /// <summary>
        /// Constructs an API URL from parameters
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string BuildParameters(string arg)
        {
            // TODO: Create service that first requires user to sign up to site
            // then enter in their API key?
            return $"{Properties.Settings.Default.PARTIAL_URL}q={arg}{Properties.Settings.Default.SECRET_KEY}";
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Passes the location to the API call
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public CurrentWeather GetCurrentWeather(string location)
        {
            CurrentWeather retVal = null;
            string uriPath;

            if (!string.IsNullOrEmpty(location) && !string.IsNullOrWhiteSpace(location))
            {
                uriPath = BuildParameters(location);

                Task.Run(async () =>
                {
                    retVal = await GetAsync<CurrentWeather>(uriPath);
                })
                .Wait();
            }
            else
            {
                throw new ArgumentNullException("GetCurrentWeather() Error: Invalid argument was given.");
            }

            return retVal;
        }

        #endregion Public Methods
    }
}