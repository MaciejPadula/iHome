using iHome.Core.Models.Application;
using iHome.Core.Services.DatabaseService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace iHome.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly IDatabaseService _databaseService;
        private readonly ApplicationSettings _applicationSettings;
        int count = 0;

        public MainPage(IDatabaseService databaseService, IOptions<ApplicationSettings> options)
        {
            InitializeComponent();

            _applicationSettings = options.Value;
            _databaseService = databaseService;

            _databaseService.GetListOfRooms("");
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}