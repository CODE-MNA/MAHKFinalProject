using MAHKFinalProject.Payloads;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MAHKFinalProject.Payloads.Payloads;

namespace MAHKFinalProject.GameComponents
{
    public class ScoreAPIManager
    {
        const string API_URL = "http://localhost:8080";
        private readonly int HIGHSCORES_DISPLAY_LIMIT = 4;
        RestClient client;
        Game1 _g;

        RestResponse response;
        public ScoreAPIManager(Game1 g)
        {
            _g = g;
            client = new RestClient(API_URL);
            client.UseJson();

            string jwt = g.authHandler.GetLoggedInToken();

            if (!string.IsNullOrEmpty(jwt))
            {
                client.Authenticator = new JwtAuthenticator(jwt);

            }

            
            
        }

        

        public string GetScoreData(ICollection<string> levelNames)
        {
            string output = "No Data";
            var request = new RestRequest("scores/Rhythmatic");

          
           
            response = client.ExecuteGet(request);
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                try
                {

                  Payload<ScoreDetails[]> returnedBody =  PayloadJSONSerializor.DeserializeJSON<ScoreDetails[]>(response.Content);

                    foreach (var name in levelNames)
                    {
                        if (returnedBody.Data.TryGetValue(name, out ScoreDetails[] scores))
                        {
                            //Clear no data output if data is to be filled
                            if (output == "No Data") output = "";

                            output += $"{name} : \n\n";

                            for (int i = 0; i < scores.Length; i++)
                            {
                                if (i > HIGHSCORES_DISPLAY_LIMIT) break;

                                output += $"#{i + 1} : {scores[i].Highscore}\n";
                            }


                            output += "\n";
                        }


                    }


                }
                catch (Exception ex)
                {

                    output = ex.Message;

                   
                }
            }
            else
            {

                try
                {
                    Payload<dynamic> returnedBody = PayloadJSONSerializor.DeserializeJSON<dynamic>(response.Content);

                    if(returnedBody != null)
                    {
                        output = returnedBody.Error;

                    }

                }catch (Exception ex)
                {
                    output = "unexpected eror";
                }


            }





            return output;
            
        }

    }

   

    record ScoreDetails
    {
        [JsonPropertyName("highscore")]
        public int Highscore { get; set; }

        [JsonPropertyName("userID")]
        public int UserID { get; set; }

        [JsonPropertyName("levelID")]
        public int LevelID { get; set; }


    }
}
