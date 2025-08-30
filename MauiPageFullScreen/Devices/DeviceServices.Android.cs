
using Android.Views;
using AndroidX.Core.View;
using MauiPageFullScreen.Interface;
using Platform = Microsoft.Maui.ApplicationModel.Platform;

#if ANDROID
using Views = AndroidX.Core.View;
#endif

namespace MauiPageFullScreen.Devices;

class DeviceServices : IDeviceServices
{
    static Android.Views.Window window => Platform.CurrentActivity?.Window ?? throw new InvalidOperationException("Current activity is null");
    static Android.Views.View decorView => window.DecorView ?? throw new InvalidOperationException("DecorView is null");
    static AndroidX.Core.View.WindowInsetsControllerCompat insetsController => WindowCompat.GetInsetsController(window, decorView) ?? throw new InvalidOperationException("InsetsController is null");
    public void RestoreScreen()
    {
        window.ClearFlags(WindowManagerFlags.Fullscreen);
        window.SetFlags(WindowManagerFlags.DrawsSystemBarBackgrounds, WindowManagerFlags.DrawsSystemBarBackgrounds);
        insetsController.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorDefault;
        if (OperatingSystem.IsAndroidVersionAtLeast(34))
        {
            insetsController.Show(WindowInsets.Type.SystemBars());
        }
    }
    public void FullScreen()
    {
        window.ClearFlags(WindowManagerFlags.LayoutNoLimits);
        window.AddFlags(WindowManagerFlags.Fullscreen);
        window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
        insetsController.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
        if (OperatingSystem.IsAndroidVersionAtLeast(34))
        {
            insetsController.Hide(WindowInsets.Type.SystemBars());
        }
    }
}
