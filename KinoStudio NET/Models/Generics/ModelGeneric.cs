using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;

namespace KinoStudio_NET.Models.Generics;

public static class ModelGeneric
{
    public static string Path = "https://localhost:44373/api/";

    public static async Task<ObservableCollection<TObj>> GetAll<TObj>(this ObservableCollection<TObj> obj)
        where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{Path}{new TObj().Path}/")
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<ObservableCollection<TObj>>(
                   response.Content.ReadAsStringAsync().Result) ??
               new ObservableCollection<TObj>();
    }

    public static async Task<TObj> Get<TObj>(this TObj obj) where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{Path}{obj.Path}/{obj.Id}")
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<TObj>(response.Content.ReadAsStringAsync().Result) ??
               new TObj();
    }

    public static async Task<TObj> Update<TObj>(this TObj obj) where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"{Path}{obj.Path}/{obj.Id}"),
            Content =
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<TObj>(response.Content.ReadAsStringAsync().Result) ??
               default!;
    }

    public static async Task<TObj> Add<TObj>(this TObj obj) where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{Path}{obj.Path}/"),
            Content =
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request);
        return JsonConvert.DeserializeObject<TObj>(response.Content.ReadAsStringAsync().Result) ??
               default!;
    }

    public static async Task<bool> Delete<TObj>(this TObj obj) where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{Path}{obj.Path}/{obj.Id}")
        };
        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public static async Task<bool> Exist<TObj>(this TObj obj) where TObj : ViewModelAbstract, new()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{Path}{obj.Path}/{obj.Id}")
        };
        var response = await client.SendAsync(request);
        return response.StatusCode != HttpStatusCode.NotFound;
    }
}