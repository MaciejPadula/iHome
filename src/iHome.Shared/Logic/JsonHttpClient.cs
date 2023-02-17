using Newtonsoft.Json;
using System.Text;

namespace iHome.Shared.Logic;

public class JsonHttpClient : HttpClient
{
    public void PostSync(string url, object body)
    {
        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = PostAsync(url, content).Result;
    }

    public T? PostSync<T>(string url)
    {
        return PostSync<T>(url, new { });
    }

    public T? PostSync<T>(string url, object body)
    {
        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = PostAsync(url, content)
            .Result
            .Content
            .ReadAsStringAsync()
            .Result;

        return JsonConvert.DeserializeObject<T>(response);
    }

    public T? GetSync<T>(string url)
    {
        var response = GetAsync(url)
            .Result
            .Content
            .ReadAsStringAsync()
            .Result;

        return JsonConvert.DeserializeObject<T>(response);
    }
}
