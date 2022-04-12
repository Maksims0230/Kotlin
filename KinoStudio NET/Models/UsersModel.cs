using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KinoStudio_NET.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace KinoStudio_NET.Models
{
    internal static class UsersModel
    {
        public static List<User> GetUsers()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:44373/api/Users/"),
            };
            var response = client.Send(request);
            return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result) ??
                   new List<User>();
        }

        public static async Task<List<User>> GetUsersAsync()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:44373/api/Users/"),
            };
            var response = await client.SendAsync(request);
            return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result) ??
                   new List<User>();
        }

        public static async Task<ObservableCollection<User>> ImportCSV(this UsersViewModel usersViewModel)
        {
            var users = new ObservableCollection<User>();
            var OPF = new OpenFileDialog
            {
                Filter = "Файлы csv|*.csv",
                Multiselect = false
            };

            if (OPF.ShowDialog() ?? false)
            {
                var usersLine = "";
                using (var reader = new StreamReader(OPF.FileName))
                {
                    usersLine = await reader.ReadToEndAsync();
                }

                var updateDB = MessageBox.Show("Выполнить обновление в бд в соответствии с данным файлом?",
                    "Выбрать способ", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

                foreach (var userLine in usersLine.Split('\r', '\n').Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    User user;
                    var userValues = userLine.Split(',');

                    if (updateDB)
                    {
                        user = new User()
                        {
                            Id = Convert.ToInt32(userValues[0]),
                            Login = userValues[1],
                            Password = userValues[2],
                            RoleId = Convert.ToInt32(userValues[3]),
                        };

                        user = await user.Exists()
                            ? await user.Update()
                            : await new User(0, userValues[1], userValues[2], Convert.ToInt32(userValues[3])).Add();
                    }
                    else
                    {
                        user = new User();
                        user.SetValues(Convert.ToInt32(userValues[0]), userValues[1], userValues[2],
                            Convert.ToInt32(userValues[3]));
                    }

                    users.Add(user);
                }
            }

            return users.Count > 0 ? users : usersViewModel.Users;
        }

        public static async void ExportCSV(this UsersViewModel usersViewModel)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Куда сохранить?"
            };
            var res = folderBrowserDialog.ShowDialog();
            if (res != System.Windows.Forms.DialogResult.None && res != System.Windows.Forms.DialogResult.Cancel)
            {
                using (var file = new FileStream(
                           $@"{folderBrowserDialog.SelectedPath}\{DateTime.Now:dd-MM-yyyy hh_mm_ss}.csv",
                           FileMode.Create))
                await using (var streamWriter = new StreamWriter(file))
                {
                    usersViewModel.Users.Select(x => x.ToString()).ToList().ForEach(async userLine =>
                        await streamWriter.WriteAsync($"{userLine}{streamWriter.NewLine}"));
                }
            }
        }

        public static bool ExistsIsNullOrEmptyUser(this UsersViewModel usersViewModel) =>
            usersViewModel.Users.Any(UserModel.IsNullOrDefault) || usersViewModel.Users.Any(user => user.Id == 0);
    }
}