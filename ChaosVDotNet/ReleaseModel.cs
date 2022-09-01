using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ChaosVDotNet
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReleaseModel
    {
        [JsonProperty(PropertyName = "tag_name")]
        public Version Tag { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "draft")]
        public bool Draft { get; set; }
        [JsonProperty(PropertyName = "prerelease")]
        public bool PreRel { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(PropertyName = "published_at")]
        public DateTime PublishedAt { get; set; }
    }
}
