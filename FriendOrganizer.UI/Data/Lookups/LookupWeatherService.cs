using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FriendOrganizer.UI.Data.Lookups
{
    public interface ILookupWeatherService
    {
        Task<string> LookupCurrentWeather();
        Task<string> LookupWeatherForDate(DateTime meetingDateFrom);
    }

    public class LookupWeatherService : ILookupWeatherService
    {
        private static HttpClient Client;

        private string Url =
            "http://api.openweathermap.org/data/2.5/forecast/?q=Goeteborg,SE&appid=a9a7c79f1d86e52c69292f86d198736f";

        public LookupWeatherService()
        {
            Client = new HttpClient();
        }

        public async Task<string> LookupCurrentWeather()
        {
            var response = await Client.GetAsync(Url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Http call returned none success response");

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<RootObject>(responseString);
                return weather.list.First().weather.First().description;
            }
            catch (Exception e)
            {
                return "Uknown weather";
            }
        }

        public async Task<string> LookupWeatherForDate(DateTime meetingDateFrom)
        {
            var response = await Client.GetAsync(Url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Http call returned none success response");

            if (meetingDateFrom - DateTime.Today > TimeSpan.FromDays(7))
                    return "Can't get weather for days more 7 days in the future";
            
            if (meetingDateFrom < DateTime.Today )
                return "Can't get weather for days in the past";
            

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<RootObject>(responseString);

                int bestMatch = 0;
                for (var index = 0; index < weather.list.Count; index++)
                {
                    var item = weather.list[index];
                    var dateTime = DateTime.Parse(item.dt_txt);
                    if ((dateTime - meetingDateFrom).TotalHours < 6)
                        bestMatch = index;
                }

                return weather.list[bestMatch].weather.First().description;
            }
            catch (Exception e)
            {
                return "Uknown weather";
            }
        }

        public class Main
        {
            public double temp { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double sea_level { get; set; }
            public double grnd_level { get; set; }
            public int humidity { get; set; }
            public double temp_kf { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public double deg { get; set; }
        }

        public class Snow
        {
            public double __invalid_name__3h { get; set; }
        }

        public class Sys
        {
            public string pod { get; set; }
        }

        public class Rain
        {
            public double __invalid_name__3h { get; set; }
        }

        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Snow snow { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
            public Rain rain { get; set; }
        }

        public class Coord
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
        }

        public class RootObject
        {
            public string cod { get; set; }
            public double message { get; set; }
            public int cnt { get; set; }
            public List<List> list { get; set; }
            public City city { get; set; }
        }
    }
}