using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Globals;
using KinoStudio_NET.Models;
using Newtonsoft.Json;

namespace KinoStudio_NET.ViewModel;

internal class AuthorizeViewModel : INotifyPropertyChanged
{
    private DelegateCommand? _authorize;

    private Action? _closeAction;
    private int _id;
    private string? _login;
    private string? _password;
    private int _roleId;

    public AuthorizeViewModel(int id, string login, string password, int roleId)
    {
        _id = id;
        _login = login;
        _password = password;
        _roleId = roleId;
    }

    public AuthorizeViewModel()
    {
    }

    [JsonProperty("UserId")]
    public int Id
    {
        get => _id;
        set
        {
            if (_id != default) return;
            _id = value;
            OnPropertyChanged();
        }
    }

    [JsonProperty("Login")]
    public string Login
    {
        get => _login!;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }

    [JsonProperty("Password")]
    public string Password
    {
        get => _password!;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    [JsonProperty("RoleId")]
    public int RoleId
    {
        get => _roleId;
        set
        {
            _roleId = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public Action? CloseAction
    {
        get => _closeAction;
        set => _closeAction ??= value;
    }

    [JsonIgnore]
    public DelegateCommand Authorize
    {
        get
        {
            return _authorize ??= new DelegateCommand(async _ =>
            {
                if ((Login?.Length ?? 0) > 0 && (Password?.Length ?? 0) > 0 && await this.Exists())
                {
                    if (!string.IsNullOrEmpty(_login) && _password!.Length >= 1)
                    {
                        var usr = await this.GetByLogin();
                        (UserGlobal.Login, UserGlobal.RoleId) = (usr.Login, usr.RoleId);
                        UserGlobal.RunStartWindow();
                        CloseAction?.Invoke();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}