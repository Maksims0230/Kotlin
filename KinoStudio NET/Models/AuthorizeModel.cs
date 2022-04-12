using KinoStudio_NET.ViewModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KinoStudio_NET.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace KinoStudio_NET.Models
{
    internal static class AuthorizeModel
    {
        public static async Task<bool> Exists(this AuthorizeViewModel authorize)
        {
            var md5Helper = new MD5Helper(authorize.Password);
            authorize.Password = md5Helper.GetEncryptedLine();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:44373/api/Users/exists"),
                Content = new StringContent(JsonConvert.SerializeObject(authorize), Encoding.UTF8, "application/json"),
            };
            authorize.Password = md5Helper.GetNotEncryptedLine();
            var response = await client.SendAsync(request).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        public static async Task<User> GetByLogin(this AuthorizeViewModel authorize)
        {
            var md5Helper = new MD5Helper(authorize.Password);
            authorize.Password = md5Helper.GetEncryptedLine();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:44373/api/Users/"),
                Content = new StringContent(JsonConvert.SerializeObject(authorize), Encoding.UTF8, "application/json"),
            };
            authorize.Password = md5Helper.GetNotEncryptedLine();
            var response = await client.SendAsync(request).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result)
                       ?.FirstOrDefault(usr => usr.Login == authorize.Login) ??
                   new User();
        }
    }
}