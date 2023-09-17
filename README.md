# MauiPageFullScreen sets the Page to full screen or restore screen

Currently it works with Navigation Page, Tabbed page, and Shell page on Windows, Android, and IOS. Full screen is supported by Windows, android and IOS. It is not supported on Mac Catalyst.

## API Example:

```
private void FullScreen_Clicked(object sender, EventArgs e)
{
    Controls.SetFullScreen();
}
```

## Setup for usage:
The important part is using statement and builder statements
```
using MauiPageFullScreen;
```
Build arguements:
```
.UseFullScreen()
```
Example MauiProgram.cs
```
using MauiPageFullScreen;
using Microsoft.Extensions.Logging;

namespace MauiFullScreenSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseFullScreen()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
```

Example MainPage.xaml
```
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="MauiFullScreenSample.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <ScrollView>
        <VerticalStackLayout
            Padding="30,0"
            Spacing="25"
            VerticalOptions="Center">

            <Image
                HeightRequest="200"
                HorizontalOptions="Center"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                Source="dotnet_bot.png" />

            <Label
                FontSize="32"
                HorizontalOptions="Center"
                SemanticProperties.HeadingLevel="Level1"
                Text="Hello, World!" />

            <Label
                FontSize="18"
                HorizontalOptions="Center"
                SemanticProperties.Description="Welcome to dot net Multi platform App U I"
                SemanticProperties.HeadingLevel="Level2"
                Text="Welcome to .NET Multi-platform App UI" />

            <Button
                x:Name="CounterBtn"
                Clicked="OnCounterClicked"
                HorizontalOptions="Center"
                SemanticProperties.Hint="Counts the number of times you click"
                Text="Click me" />
            <Button
                x:Name="FullScreen"
                Clicked="FullScreen_Clicked"
                HorizontalOptions="Center"
                Text="Full Screen Test" />
        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
```

Example MainPage.xaml.cs
```
using MauiPageFullScreen;

namespace MauiFullScreenSample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
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

        private void FullScreen_Clicked(object sender, EventArgs e)
        {
            Controls.SetFullScreen();
        }
    }
}
```
