using MauiPageFullScreen;

namespace MauiFullScreenSample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    void FullScreen_Toggled(object sender, EventArgs e)
    {
        Controls.ToggleFullScreenStatus();
    }

    void FullScreen_Clicked(object sender, EventArgs e)
    {
        Controls.FullScreen();
    }

    void RestoreScreen_Clicked(object sender, EventArgs e)
    {
        Controls.RestoreScreen();
    }
}