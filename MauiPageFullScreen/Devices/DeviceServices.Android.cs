
using MauiPageFullScreen.Interface;
using Platform = Microsoft.Maui.ApplicationModel.Platform;

#if ANDROID
using Views = AndroidX.Core.View;
#endif

namespace MauiPageFullScreen.Devices;

internal class DeviceServices : IDeviceServices
{
    public void RestoreScreen()
    {
        PageExtensions.SetBarStatus(false);
        var activity = Platform.CurrentActivity;

        if (activity == null || activity.Window == null)
        {
            return;
        }

        Views.WindowCompat.SetDecorFitsSystemWindows(activity.Window, false);
        var windowInsetsControllerCompat = Views.WindowCompat.GetInsetsController(activity.Window, activity.Window.DecorView);
        var types = Views.WindowInsetsCompat.Type.StatusBars() |
                    Views.WindowInsetsCompat.Type.NavigationBars();
        windowInsetsControllerCompat.Show(types);
    }
    public void FullScreen()
    {
        PageExtensions.SetBarStatus(true);
        var activity = Platform.CurrentActivity;

        if (activity == null || activity.Window == null)
        {
            return;
        }

        Views.WindowCompat.SetDecorFitsSystemWindows(activity.Window, false);
        var windowInsetsControllerCompat = Views.WindowCompat.GetInsetsController(activity.Window, activity.Window.DecorView);
        var types = Views.WindowInsetsCompat.Type.StatusBars() |
                    Views.WindowInsetsCompat.Type.NavigationBars();

        windowInsetsControllerCompat.SystemBarsBehavior = Views.WindowInsetsControllerCompat.BehaviorShowBarsBySwipe;
        windowInsetsControllerCompat.Hide(types);
    }
}
