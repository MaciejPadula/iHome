using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using iHome.HubApp.Exceptions;
using iHome.HubApp.Services.UserService;
using iHome.HubApp.ViewModels;
using iHome.HubApp.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace iHome.HubApp;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
        {
            if(ex is UIException)
            {
                Console.WriteLine($"UIException: {ex.Message}");
                return;
            }
            Console.WriteLine(ex.Message);
        });

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(
                    Bootstrapper.GetService<IUserService>()),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
