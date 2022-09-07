using iHome.Core.Models.Application;
using iHome.Core.Services.DatabaseService;
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

            var rooms = _databaseService.GetListOfRooms("google-oauth2|111005413535505222179");
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