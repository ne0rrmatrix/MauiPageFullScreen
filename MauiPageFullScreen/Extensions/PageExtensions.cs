#if MACCATALYST
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
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
		if (currentPage.NavigationProxy.ModalStack[^1] is Page modal)
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
}

#if MACCATALYST

/// <summary>
/// Helper class to toggle full-screen mode for the main window in Mac Catalyst.
/// </summary>
public static partial class FullScreenHelper
{
    // P/Invoke declarations for Objective-C runtime
    [LibraryImport("/usr/lib/libobjc.dylib", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr objc_getClass(string name);

    [LibraryImport("/usr/lib/libobjc.dylib", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr sel_registerName(string name);

    // objc_msgSend returning IntPtr
    [LibraryImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    private static partial IntPtr objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector);

    // objc_msgSend with IntPtr argument, returning void
    [LibraryImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    private static partial void objc_msgSend_Void(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [LibraryImport("/usr/lib/libobjc.dylib")]
    private static partial IntPtr object_getClass(IntPtr obj);

    [LibraryImport("/usr/lib/libobjc.dylib")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool class_respondsToSelector(IntPtr cls, IntPtr sel);

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
            var nsApp = objc_msgSend_IntPtr(nsApplicationClass, sharedApplicationSel);

            // Get the main window
            var mainWindowSel = sel_registerName("mainWindow");
            var nsWindow = objc_msgSend_IntPtr(nsApp, mainWindowSel);

            if (nsWindow != IntPtr.Zero)
            {
                // Check if window responds to toggleFullScreen:
                var toggleFullScreenSel = sel_registerName("toggleFullScreen:");
                var windowClass = object_getClass(nsWindow);

                if (class_respondsToSelector(windowClass, toggleFullScreenSel))
                {
                    // Toggle fullscreen on the main window
                    objc_msgSend_Void(nsWindow, toggleFullScreenSel, IntPtr.Zero);
                }
                else
                {
					System.Diagnostics.Trace.TraceError("Main window does not respond to toggleFullScreen:");
				}
            }
            else
            {
                System.Diagnostics.Trace.TraceError("Main window not found");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError($"Error toggling fullscreen: {ex.Message}");
        }
    }
}
#endif