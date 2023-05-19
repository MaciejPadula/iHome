using System.Text;

namespace iHome.Microservices.UsersApi.Infrastructure.Helpers
{
    public class Auth0QueryBuilder
    {
        private readonly StringBuilder _stringBuilder;
        private readonly Dictionary<string, string> _queryParameters = new();

        public Auth0QueryBuilder(string baseUrl)
        {
            _stringBuilder = new StringBuilder(baseUrl);
            _stringBuilder.Append("/api/v2/users?q=");
        }

        public Auth0QueryBuilder WithUserId(string? userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _queryParameters.Add("user_id", userId);
            }

            return this;
        }

        public Auth0QueryBuilder WithEmail(string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                _queryParameters.Add("email", email);
            }

            return this;
        }

        public Auth0QueryBuilder WithUsername(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _queryParameters.Add("name", name);
            }

            return this;
        }

        public string Build()
        {
            var query = _queryParameters.Select(param => $"{param.Key}:{param.Value}");
            _stringBuilder.Append(string.Join(" OR ", query));
            return _stringBuilder.ToString();
        }
    }
}
