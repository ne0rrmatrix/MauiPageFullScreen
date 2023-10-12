using MauiPageFullScreen;

namespace MauiFullScreenSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void FullScreen_Toggled(object sender, EventArgs e)
        {
            Controls.ToggleFullScreenStatus();
        }

        private void FullScreen_Clicked(object sender, EventArgs e)
        {
            Controls.FullScreen();
        }

        private void RestoreScreen_Clicked(object sender, EventArgs e)
        {
            Controls.RestoreScreen();
        }
    }
}