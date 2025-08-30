#if MACCATALYST
using System.Runtime.InteropServices;
using System.Diagnostics;
#endif

namespace MauiPageFullScreen.Extensions;

// Since MauiPageFullScreen can't access .NET MAUI internals we have to copy this code here
// https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Platform/PageExtensions.cs

static class PageExtensions
{
	static bool? navBarIsVisible;
	static bool? tabBarIsVisible;
	static bool? backButton;
	static string? backButtonTitle;
	static string? pageTitle;
	static Page CurrentPage => GetCurrentPage(Application.Current?.Windows[0].Page ?? throw new InvalidOperationException($"{nameof(Application.Current.MainPage)} cannot be null."));
	public static void SetBarStatus(bool setFullScreen)
	{
#if IOS
#pragma warning disable CA1422 // Validate platform compatibility
		UIKit.UIApplication.SharedApplication.SetStatusBarHidden(setFullScreen, UIKit.UIStatusBarAnimation.Fade);
#pragma warning restore CA1422 // Validate platform compatibility
#endif
#if MACCATALYST
        FullScreenHelper.ToggleFullScreen();
#endif
		// let's cache the CurrentPage here, since the user can navigate or background the app
		// while this method is running
		var currentPage = CurrentPage;

		if (setFullScreen)
		{
			navBarIsVisible = Shell.GetNavBarIsVisible(currentPage);
			tabBarIsVisible = Shell.GetTabBarIsVisible(currentPage);
			backButton = NavigationPage.GetHasBackButton(currentPage);
			backButtonTitle = NavigationPage.GetBackButtonTitle(currentPage);
			NavigationPage.SetBackButtonTitle(currentPage, string.Empty);
			NavigationPage.SetHasBackButton(currentPage, false);
			pageTitle = currentPage.Title;
			currentPage.Title = string.Empty;
			Shell.SetNavBarIsVisible(currentPage, false);
			Shell.SetTabBarIsVisible(currentPage, false);
			NavigationPage.SetHasNavigationBar(currentPage, false);
		}
		else
		{
			if (navBarIsVisible.HasValue)
			{
				NavigationPage.SetHasNavigationBar(currentPage, navBarIsVisible.Value);
				Shell.SetNavBarIsVisible(currentPage, navBarIsVisible.Value);
			}
			if (backButton.HasValue)
			{
				NavigationPage.SetHasBackButton(currentPage, backButton.Value);
			}
			if (tabBarIsVisible.HasValue)
			{
				Shell.SetTabBarIsVisible(currentPage, tabBarIsVisible.Value);
			}
			if (pageTitle is not null)
			{
				currentPage.Title = pageTitle;
			}
			if (backButtonTitle is not null)
			{
				NavigationPage.SetBackButtonTitle(currentPage, backButtonTitle);
			}
		}
	}
	internal static Page GetCurrentPage(this Page currentPage)
	{
		if (currentPage.NavigationProxy.ModalStack.LastOrDefault() is Page modal)
		{
			return modal;
		}

		if (currentPage is FlyoutPage flyoutPage)
		{
			return GetCurrentPage(flyoutPage.Detail);
		}

		if (currentPage is Shell { CurrentItem.CurrentItem: IShellSectionController shellSectionController })
		{
			return shellSectionController.PresentedPage;
		}

		if (currentPage is IPageContainer<Page> paigeContainer)
		{
			return GetCurrentPage(paigeContainer.CurrentPage);
		}

		return currentPage;
	}

	internal record struct ParentWindow
	{
		/// <summary>
		/// Checks if the parent window is null.
		/// </summary>
		public static bool Exists
		{
			get
			{
				if (CurrentPage.GetParentWindow() is null)
				{
					return false;
				}
				if (CurrentPage.GetParentWindow().Handler is null)
				{
					return false;
				}

				return CurrentPage.GetParentWindow().Handler?.PlatformView is not null;
			}
		}
	}
}

#if MACCATALYST


/// <summary>
/// Helper class to toggle full-screen mode for the main window in Mac Catalyst.
/// </summary>
public static class FullScreenHelper
{
    // P/Invoke declarations for Objective-C runtime
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr objc_getClass(string name);
    
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr sel_registerName(string name);
    
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);
    
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1);
    
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, int arg1);
    
    [DllImport("/usr/lib/libobjc.dylib")]
    static extern bool class_respondsToSelector(IntPtr cls, IntPtr sel);
    
    /// <summary>
    /// Toggles the full-screen mode for the main window.
    /// </summary>
    public static void ToggleFullScreen()
    {
        try
        {
            // Get the NSApplication shared instance
            var nsApplicationClass = objc_getClass("NSApplication");
            var sharedApplicationSel = sel_registerName("sharedApplication");
            var nsApp = objc_msgSend(nsApplicationClass, sharedApplicationSel);

            // Get the main window
            var mainWindowSel = sel_registerName("mainWindow");
            var nsWindow = objc_msgSend(nsApp, mainWindowSel);

            if (nsWindow != IntPtr.Zero)
            {
                // Check if window responds to toggleFullScreen:
                var toggleFullScreenSel = sel_registerName("toggleFullScreen:");
                var windowClass = objc_msgSend(nsWindow, sel_registerName("class"));

                if (class_respondsToSelector(windowClass, toggleFullScreenSel))
                {
                    // Toggle fullscreen on the main window
                    objc_msgSend(nsWindow, toggleFullScreenSel, nsWindow);
                    Debug.WriteLine("Successfully toggled fullscreen");
                }
                else
                {
                    Debug.WriteLine("Window does not respond to toggleFullScreen:");
                }
            }
            else
            {
                Debug.WriteLine("Main window not found");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error toggling fullscreen: {ex.Message}");
        }
    }
}
#endif