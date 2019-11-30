using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AdventOfCodeApi.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AdventOfCodeApi.Services
{


    public interface IAdventOfCodeService
    {
        Task<List<AocMember>> GetAsync();
    }

    public class AdventOfCodeService : IAdventOfCodeService
    {
        private List<AocMember> _aocMembers;
        private DateTime? _lastLoad;
        //private TimeSpan _pollingIntervale = TimeSpan.FromMinutes(1);

        //private TimeSpan _firstTimeToLoad = new TimeSpan(08, 00, 00);
        //private TimeSpan _lastTimeToLoad = new TimeSpan(17, 00, 00);

        private readonly IOptionsMonitor<AppOption> _appOption;

        //private List<int> _days = new List<int>
        //{
        //    1,2,3,4,5
        //};

        public AdventOfCodeService(IOptionsMonitor<AppOption> appOption)
        {
            _appOption = appOption;
        }

        public async Task<List<AocMember>> GetAsync()
        {
            AppOption appOption = _appOption?.CurrentValue;

            if (_appOption.CurrentValue == null)
            {
                throw new ApplicationException("Can not get. Need to define app section in appsettings.json");
            }

            if(string.IsNullOrWhiteSpace(appOption.AocApiAddress))
            {
                throw new ApplicationException("Can not get. App:AocApiAddress is missing in appsettings.json");
            }
            if (string.IsNullOrWhiteSpace(appOption.SessionId))
            {
                throw new ApplicationException("Can not get. App:SessionId is missing in appsettings.json");
            }

            //Always load if cache is empty
            if (_aocMembers == null)
            {
                _aocMembers = await GetDataAsync(appOption.AocApiAddress, appOption.SessionId);
                _lastLoad = DateTime.Now;
            }
            //Just return the cached value
            else if (false && DateTime.Now - _lastLoad < appOption.PollingIntervale)
            {
                return _aocMembers;
            }
            else
            {
                //Check if this is a valid load date
                int dayOfTheWeek = (int)DateTime.Now.DayOfWeek;
                if (appOption.DaysToLoad == null || appOption.DaysToLoad.Count == 0 || appOption.DaysToLoad.Contains(dayOfTheWeek))
                {
                    //Check if is within time span
                    if(DateTime.Now.TimeOfDay >= appOption.FirstTimeOfDayToLoad && DateTime.Now.TimeOfDay <= appOption.LastTimeOfDayToLoad)
                    {
                        _aocMembers = await GetDataAsync(appOption.AocApiAddress, appOption.SessionId);
                        _lastLoad = DateTime.Now;
                    }
                }

            }
            
            return _aocMembers;
        }

        private async Task<List<AocMember>> GetDataAsync(string url, string sessionId)
        {
            List<AocMember> aocMembers = new List<AocMember>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("cookie", $"session={sessionId}");
                var result = await client.GetStringAsync(url);
                var jsonResult = JObject.Parse(result);

                var @event = jsonResult["event"].Value<string>();
                var ovnerId = jsonResult["owner_id"].Value<string>();

                var jsonMembers = jsonResult["members"].Values();

                foreach (var jsonMember in jsonMembers)
                {
                    AocMember aocMember = new AocMember();
                    aocMembers.Add(aocMember);

                    aocMember.Stars = jsonMember["stars"].Value<int>();
                    aocMember.LocalScore = jsonMember["local_score"].Value<int>();
                    aocMember.LastStarTs = jsonMember["last_star_ts"].Value<int>();
                    aocMember.Name = jsonMember["name"].Value<string>();
                    aocMember.Id = jsonMember["id"].Value<string>();
                    aocMember.GlobalScore = jsonMember["global_score"].Value<int>();

                    var jsonCompletionDayLevels = jsonMember["completion_day_level"].Values();

                    int day = 1;
                    foreach (var jsonCompletionDayLevel in jsonCompletionDayLevels)
                    {
                        int starId = 1;
                        foreach (var jsonCompletionDayLevelStar in jsonCompletionDayLevel.Values())
                        {
                            AocCompletionDayLevel aocCompletionDayLevel = new AocCompletionDayLevel();
                            aocCompletionDayLevel.Day = day;
                            aocCompletionDayLevel.StarNumber = starId;
                            aocCompletionDayLevel.GetStarTs = jsonCompletionDayLevelStar["get_star_ts"].Value<long>();

                            aocMember.CompletionDayLevels.Add(aocCompletionDayLevel);

                            starId++;
                        }

                        day++;
                    }

                }

            }

            return aocMembers;
        }

    }
}
