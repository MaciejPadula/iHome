using DynamicData;
using iHome.Devices.ApiClient;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.HubApp.Services.UserService;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace iHome.HubApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly IRoomProvider _roomProvider;
    private readonly ApiClientSettings _clientSettings;

    private IEnumerable<GetRoomRequestRoom>? _rooms;

    public IEnumerable<GetRoomRequestRoom> Rooms
    {
        get
        {
            return _rooms ?? Enumerable.Empty<GetRoomRequestRoom>();
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _rooms, value);
        }
    }

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
                Rooms = _roomProvider.GetRoomsForHub();
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
