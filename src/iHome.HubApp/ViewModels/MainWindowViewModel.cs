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
    private readonly IRoomProvider _roomProvider;
    private readonly ApiClientSettings _clientSettings;

    public MainWindowViewModel(IUserService userService, IRoomProvider roomProvider, ApiClientSettings clientSettings)
    {
        _userService = userService;
        _roomProvider = roomProvider;
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
                var rooms = _roomProvider.GetRoomsForHub();
            }
        }
        catch(Exception)
        {
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
