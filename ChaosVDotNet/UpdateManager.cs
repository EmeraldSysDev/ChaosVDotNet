/*
    ChaosVDotNet (UpdateManager.cs)
    Copyright (C) 2022 Ryan Omasta

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GTA;
using Newtonsoft.Json;

namespace ChaosVDotNet
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class UpdateManager : Script
    {
        private static readonly HttpClient client = new HttpClient();
        private const string BASE_URL = "https://api.github.com";
        private const string RELEASE_PATH = "repos/EmeraldSysDev/ChaosVDotNet/releases/latest";

        public UpdateManager()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls13;

            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Add("User-Agent", "ChaosVDotNet-v1.0.1.7/UpdateManager");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Version GetLoadedVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public ReleaseModel GetLatestInfo()
        {
            try
            {
                HttpResponseMessage ret = client.GetAsync(RELEASE_PATH).Result;
                if (!ret.IsSuccessStatusCode)
                {
                    GTA.UI.Notification.Show("[ChaosVDotNet] Could not get the latest version from source.");
                    return null;
                }

                string content = ret.Content.ReadAsStringAsync().Result;
                ReleaseModel latest = JsonConvert.DeserializeObject<ReleaseModel>(content);

                return latest;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
