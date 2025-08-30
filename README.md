# MauiPageFullScreen sets the Page to full screen or restore screen

Currently it works with Navigation Page, Tabbed page, and Shell page. Full screen is supported by Windows, android and IOS. It is not supported on Mac Catalyst.

|[FullScreenStatus.Maui](https://www.nuget.org/packages/FullScreenStatus.Maui/)|.NET 9|
|:---:|:---:|
|Stable|[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ne0rrmatrix_MauiPageFullScreen&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ne0rrmatrix_MauiPageFullScreen)|

## API Example:

```
      Controls.ToggleFullScreenStatus();
      Controls.FullScreen();
      Controls.RestoreScreen();
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

            <Button
                x:Name="FullScreenToggle"
                Clicked="FullScreen_Toggled"
                HorizontalOptions="Center"
                Text="Toggle Full Screen" />
            <Button
                x:Name="FullScreen"
                Clicked="FullScreen_Clicked"
                Text="Full Screen" />
            <Button
                x:Name="RestoreScreen"
                Clicked="RestoreScreen_Clicked"
                Text="Restore Screen" />
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
```
On IOS devices plist must be adjusted with included code below:

```
<key>UIViewControllerBasedStatusBarAppearance</key>
	<false/>
```
