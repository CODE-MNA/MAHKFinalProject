using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MAHKFinalProject.Payloads.Payloads;

namespace MAHKFinalProject.Authentication
{
    public class AuthHandler
    {
        RestClient client;
        string tokenPath = ".auth/token";

        string loggedInUserName = "";
        string accessToken = "";
       
        public Action AuthStateChanged;
        

        public AuthHandler()
        {
            string? apiURL = Environment.GetEnvironmentVariable("API_URL");
            if(apiURL == null){
                apiURL = "http://localhost:8000";
            }
            client = new RestClient(apiURL);
            client.Options.UseDefaultCredentials = true;


        }
        
     

        public string GetLoggedInUserStatus()
        {
            if (String.IsNullOrEmpty(loggedInUserName) && String.IsNullOrEmpty(GetLoggedInToken()))
            {
                return "Logged out!";
            }

            if (String.IsNullOrEmpty(loggedInUserName))
            {
                return "Logged in!";
            }

            return "Logged in as : " + loggedInUserName;
        }

        string ParseTokenFromPayload(string responseContent)
        {
            return responseContent;
        }


        //Break into two parts, RequestAccessToken(user,pass) returns token calls subfunction Authenticate(token)
        public void RequestLogin(string userName,string passWord)
        {



            string token = "";


            var req = new RestRequest("auth/login", Method.Post);
            
            Dictionary<string, string> jsonBody = new Dictionary<string, string>();

            jsonBody.Add("username",userName);
            jsonBody.Add("password",passWord);

            req.RequestFormat = DataFormat.Json;
            req.AddBody(jsonBody);

            try
            {
                var response = client.ExecutePost<Payload<dynamic>>(req);
                if (response.Data == null) return ;
                if (response.Data.Success == true)
                {
                    if (response.Data.Data.TryGetValue("token", out object parsedToken))
                    {
                        token = parsedToken.ToString();

                        
                    }


                    if(response.Data.Data.TryGetValue("username",out object username))
                    {
                        loggedInUserName = username.ToString();
                    }

                    StoreAccessTokenToStorage(token);

                }
                else
                {

                    ErrorPayload errorResponse = (ErrorPayload)response.Data;

                    token = errorResponse.Error;
                }
            }catch (Exception ex)
            {

            }
           




          
        }

        public void StoreAccessTokenToStorage(string token)
        {
            try
            {
                if (token == "" || token == "no token")
                {
                    loggedInUserName = "";
                    if (File.Exists(tokenPath))
                    {
                        File.Delete(tokenPath);
                    }

                    AuthStateChanged?.Invoke();

                    return;
                }
                // if there is no directory, create directory
                if (!Directory.Exists(tokenPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(tokenPath));
                }

                
                using (StreamWriter sw = new StreamWriter(tokenPath, false))
                {
                    sw.Write(token);

                }
                accessToken = token;

                AuthStateChanged?.Invoke();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


     

        public void Logout()
        {
            loggedInUserName = "";

            if (File.Exists(tokenPath))
            {
                File.Delete(tokenPath);

            }
            accessToken = "";

            AuthStateChanged?.Invoke();
        }

        public string GetLoggedInToken()
        {
           return accessToken;

        }

        public string LoadAccessTokenFromStorage()
        {
            string token = "";

            if (File.Exists(tokenPath))
            {
                using (StreamReader sr = new StreamReader(tokenPath))
                {
                    string content = sr.ReadToEnd();

                    token = content;


                }
            }
            accessToken = token.Trim();
            return token;


        }
    }
}
