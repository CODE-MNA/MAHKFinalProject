using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAHKFinalProject.Payloads
{
    public static class Payloads
    {
        public record Payload
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public virtual Dictionary<string, object>? Data { get; set; }
         

        }

        public record Payload<T> 
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public Dictionary<string, T>? Data { get; set; }

            [JsonPropertyName("error")]
            public string Error { get; set; }
        }
        public record ErrorPayload : Payload<object>
        {
          

            [JsonPropertyName("error")]
            public string Error { get; set; }
          

        }
    }
}
