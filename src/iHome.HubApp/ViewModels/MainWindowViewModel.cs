using iHome.HubApp.Services.UserService;
using ReactiveUI;
using System;
using System.Reactive;

namespace iHome.HubApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    public MainWindowViewModel(IUserService userService)
    {
        _userService = userService;
    }

    public ReactiveCommand<Unit, Unit> Login => ReactiveCommand.Create(_userService.Login);
    public ReactiveCommand<Unit, Unit> Logout => ReactiveCommand.Create(_userService.Logout);
    public ReactiveCommand<Unit, bool> IsAuthenticated => ReactiveCommand.Create(() => _userService.IsAuthenticated);
}
