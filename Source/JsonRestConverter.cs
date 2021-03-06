﻿using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yansoft.Rest
{
    public class JsonRestConverter : IConverter
    {
        public JsonSerializer JsonSerializer { get; set; } 
            = new JsonSerializer();

        public string MediaType => "application/json";

        public Encoding Encoding => Encoding.UTF8;
        
        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            using (var stream = await content.ReadAsStreamAsync())
            using (var sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                return JsonSerializer.Deserialize<T>(reader);
            }
        }
        
        public virtual Task<HttpContent> SerializeAsync(object o)
        {
            var stringContent = JToken.FromObject(o, JsonSerializer).ToString();
            var content = new StringContent(stringContent, Encoding, MediaType);
            return Task.FromResult(content as HttpContent);
        }
    }
}
