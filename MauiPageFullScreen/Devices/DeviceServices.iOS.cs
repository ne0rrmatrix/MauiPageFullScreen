using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;
using MauiPageFullScreen.Interface;
using NavigationPage = Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.NavigationPage;
using Microsoft.Maui.Graphics;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
namespace MauiPageFullScreen.Devices;

internal class DeviceServices : IDeviceServices
{
    NavigationPageRenderer NavigationRenderer { get; set; } = new();
    public void FullScreen()
    {
        Extensions.PageExtensions.SetBarStatus(true);
        var item = UIViewControllers.GetTopViewController();
        NavigationRenderer.OverFullScreen(item);
    }

    public void RestoreScreen()
    {
        Extensions.PageExtensions.SetBarStatus(false);
        var item = UIViewControllers.GetTopViewController();
        NavigationRenderer.PageSheet(item);
    }
}
public class NavigationPageRenderer : Microsoft.Maui.Controls.Handlers.Compatibility.NavigationRenderer
{
    public void OverFullScreen(UIViewController ?parent)
    {
        try
        {
            if (parent != null && UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                // Change Navigation Style to Fullscreen (iOS 13 and above)
                parent.ModalPresentationStyle = UIKit.UIModalPresentationStyle.OverFullScreen;
                
            }

            base.WillMoveToParentViewController(parent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
    public void PageSheet(UIViewController? parent)
    {
        try
        {
            if (parent != null && UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                // Change Navigation Style to Fullscreen (iOS 13 and above)
                parent.ModalPresentationStyle = UIKit.UIModalPresentationStyle.PageSheet;

            }

            base.WillMoveToParentViewController(parent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}
public static class UIViewControllers
{
    public static UIViewController? GetTopViewController()
    {
        var window = UIApplication.SharedApplication.GetKeyWindow();
        var vc = window?.RootViewController;
        while (vc is { PresentedViewController: { } })
            vc = vc.PresentedViewController;

        if (vc is UINavigationController { ViewControllers: { } } navController)
            vc = navController.ViewControllers.Last();

        return vc;
    }

    public static UIWindow? GetKeyWindow(this UIApplication application)
    {
        if (!UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            return application.KeyWindow; // deprecated in iOS 13

        var window = application
            .ConnectedScenes
            .ToArray()
            .OfType<UIWindowScene>()
            .SelectMany(scene => scene.Windows)
            .FirstOrDefault(window => window.IsKeyWindow);

        return window;
    }
}