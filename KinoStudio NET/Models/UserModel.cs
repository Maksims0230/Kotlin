using KinoStudio_NET.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KinoStudio_NET.Models;

internal static class UserModel
{
    public static async Task<User> Get(this User user)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://localhost:44373/api/Users/{user.Id}")
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result) ??
               new User();
    }

    public static async Task<User> Update(this User user)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"https://localhost:44373/api/Users/{user.Id}"),
            Content =
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"),
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result) ??
               default!;
    }

    public static async Task<User> Add(this User user)
    {
        if (user.Id != 0) user.Id = 0;
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:49034/api/Users/"),
            Content =
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"),
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result) ??
               default!;
    }

    public static async Task<bool> Delete(this User user)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"https://localhost:44373/api/Users/{user.Id}")
        };
        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public static List<Role> GetRoles()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://localhost:44373/api/Roles/")
        };
        var response = client.Send(request);
        return JsonConvert.DeserializeObject<List<Role>>(response.Content.ReadAsStringAsync().Result) ??
               default!;
    }

    public static async Task<bool> Exists(this User user)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://localhost:44373/api/Users/exists"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request).ConfigureAwait(false);
        return response.IsSuccessStatusCode;
    }

    public static async Task<bool> ExistsById(this User user)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://localhost:44373/api/Users/exists_id"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request).ConfigureAwait(false);
        return response.IsSuccessStatusCode;
    }

    public static bool IsNullOrDefault(this User user)
    {
        var (login, password, roleId) = user!;
        return user.Equals(null) || string.IsNullOrWhiteSpace(login) ||
               string.IsNullOrEmpty(password) || roleId == default;
    }
}