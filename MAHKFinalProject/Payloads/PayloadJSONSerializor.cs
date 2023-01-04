using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static MAHKFinalProject.Payloads.Payloads;

namespace MAHKFinalProject.Payloads
{
    public static class PayloadJSONSerializor
    {

       public static Payload<T> DeserializeJSON<T>(string json)
        {

            Payload<T> oob;

            oob = JsonConvert.DeserializeObject<Payload<T>>(json);

            return oob;

        }

    }
}
