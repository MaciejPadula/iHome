﻿using iHome.Models.Account;
using iHome.Models.Application;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Claims;

namespace iHome.Logic.UserInfo
{
    public class UserInfo: IUserInfo
    {
        IOptions<ApplicationSettings> _options;

        public UserInfo(IOptions<ApplicationSettings> options)
        {
            _options = options;
        }

        public async Task<string?> GetPublicIp(HttpContext httpContext)
        {
            string? ip = httpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == "127.0.0.1" || ip == "::1")
            {
                var httpClient = new HttpClient();
                ip = await httpClient.GetStringAsync("https://api.ipify.org");
            }
            Console.WriteLine(ip);
            return ip;
        }

        public string? GetUserEmail(string uuid)
        {
            return FetchData("user_id", uuid).email;
        }

        public List<string> GetEmails(string emailTest)
        {
            var emails = new List<string>();
            try
            {
                var content = Request("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=email:*" + emailTest + "*&search_engine=v3");
                var response = JsonConvert.DeserializeObject<List<User>>(content);
                
                response.ForEach(user => emails.Add(user.email));
            }
            catch { }
            return emails;
        }

        public string? GetUserUuid(ClaimsPrincipal user)
        {
            return user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        public string? GetUserUuid(string email)
        {
            return FetchData("email", email).user_id;
        }

        private string Request(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("authorization", _options.Value.Auth0ApiSecret);
            var content = client.Execute(request).Content;
            return content;
        }

        private User FetchData(string queryKey, string queryValue)
        {
            try
            {
                var content = Request("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=" + queryKey + ":" + queryValue + "&search_engine=v3");
                var response = JsonConvert.DeserializeObject<List<User>>(content);
                if(response.Count != 0)
                {
                    return response[0];
                }
            }
            catch { }
            return new User();
        }

    }
}
