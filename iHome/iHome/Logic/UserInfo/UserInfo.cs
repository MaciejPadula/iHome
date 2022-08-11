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
        public string? GetUserUuid(ClaimsPrincipal user)
        {
            return user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        public string? GetUserUuid(string? email)
        {
            try
            {
                var client = new RestClient("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=email:" + email + "&search_engine=v3");
                var request = new RestRequest();
                request.AddHeader("authorization", _options.Value.Auth0ApiSecret);
                var content = client.Execute(request).Content;
                var response = JsonConvert.DeserializeObject<List<Dictionary<dynamic, dynamic>>>(content);
                return response[0]["user_id"];
            }
            catch { }
            return null;
        }
    }
}
