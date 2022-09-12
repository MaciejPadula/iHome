using iHome.Core.Logic.Account;
using iHome.Core.Models.Application;
using iHome.Core.Models.Errors;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Claims;

namespace iHome.Core.Logic.UserInfo
{
    public class UserInfo : IUserInfo
    {
        IOptions<ApplicationSettings> _options;

        public UserInfo(IOptions<ApplicationSettings> options)
        {
            _options = options;
        }

        public string GetUserEmail(string uuid)
        {
            return FetchData("user_id", uuid).Email;
        }

        public List<string> GetEmails(string emailTest)
        {
            var emails = new List<string>();
            try
            {
                var content = Request("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=email:*" + emailTest + "*&search_engine=v3");
                var response = JsonConvert.DeserializeObject<List<User>>(content);
                if (response != null)
                {
                    response.ForEach(user => emails.Add(user.Email));
                }
            }
            catch { }
            return emails;
        }

        public string GetUserUuid(ClaimsPrincipal user)
        {
            var uuid = user?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (uuid == null)
                throw new UserNotFoundException();

            return uuid;
        }
        public string GetUserUuid(string email)
        {
            var uuid = FetchData("email", email).UserId;
            if (uuid == null)
                throw new UserNotFoundException();
            return uuid;
        }

        private string Request(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("authorization", "Bearer " + _options.Value.Auth0ApiSecret);
            var content = client.Execute(request).Content;
            if (content == null)
            {
                return "";
            }
            return content;
        }

        private User FetchData(string queryKey, string queryValue)
        {
            try
            {
                var content = Request("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=" + queryKey + ":" + queryValue + "&search_engine=v3");
                var response = JsonConvert.DeserializeObject<List<User>>(content);
                if (response != null && response.Count != 0)
                {
                    return response[0];
                }
            }
            catch { }
            return new User();
        }

    }
}
