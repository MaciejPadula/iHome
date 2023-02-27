using iHome.Devices.ApiClient;
using iHome.Devices.Contract.Interfaces;
using iHome.HubApp.Services.UserService;
using ReactiveUI;
using System;
using System.Reactive;

namespace iHome.HubApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly IDeviceProvider _deviceProvider;
    private readonly ApiClientSettings _clientSettings;

    public MainWindowViewModel(IUserService userService, IDeviceProvider deviceProvider, ApiClientSettings clientSettings)
    {
        _userService = userService;
        _deviceProvider = deviceProvider;
        _clientSettings = clientSettings;
    }

    public ReactiveCommand<Unit, Unit> Login => ReactiveCommand.CreateFromTask(async () =>
    {
        try
        {
            await _userService.Login();

            if (_userService.IsAuthenticated)
            {
                _clientSettings.Authorization = _userService.AccessToken;
            }
            var d = _deviceProvider.GetDeviceData(new()
            {
                DeviceId = Guid.Parse("5da7d086-fa89-43b8-97b4-3f95f8092e0b")
            });
        }
        catch(Exception e)
        {
            var i = 2;
        }
    });

    public ReactiveCommand<Unit, Unit> Logout => ReactiveCommand.Create(() =>
    {
        try
        {
            _userService.Logout();
        }
        catch
        {

        }
    });
    
    public ReactiveCommand<Unit, bool> IsAuthenticated => ReactiveCommand.Create(() => _userService.IsAuthenticated);
}
