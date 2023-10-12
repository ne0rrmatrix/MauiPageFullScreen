
namespace MauiPageFullScreen.Extensions;

// Since MauiPageFullScreen can't access .NET MAUI internals we have to copy this code here
// https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Platform/PageExtensions.cs
static class PageExtensions
{
    private static bool? s_navBarIsVisible = null;
    private static bool? s_tabBarIsVisible = null;
    private static bool? s_backButton = null;
    private static string s_backButtonTitle = null;
    private static string s_pageTitle = null;
    static Page CurrentPage => GetCurrentPage(Application.Current?.MainPage ?? throw new InvalidOperationException($"{nameof(Application.Current.MainPage)} cannot be null."));
    static public void SetBarStatus(bool shouldBeFullScreen)
    {
#if IOS || MACCATALYST
#pragma warning disable CA1422 // Validate platform compatibility
        UIKit.UIApplication.SharedApplication.SetStatusBarHidden(shouldBeFullScreen, UIKit.UIStatusBarAnimation.Fade);
#pragma warning restore CA1422 // Validate platform compatibility
#endif
        // let's cache the CurrentPage here, since the user can navigate or background the app
        // while this method is running
        var currentPage = CurrentPage;

        if (shouldBeFullScreen)
        {
            s_navBarIsVisible = Shell.GetNavBarIsVisible(currentPage);
            s_tabBarIsVisible = Shell.GetTabBarIsVisible(currentPage);
            s_backButton = NavigationPage.GetHasBackButton(currentPage);
            s_backButtonTitle = NavigationPage.GetBackButtonTitle(currentPage);
            NavigationPage.SetBackButtonTitle(currentPage, string.Empty);
            NavigationPage.SetHasBackButton(currentPage, false);
            s_pageTitle = currentPage.Title;
            currentPage.Title = string.Empty;
            Shell.SetNavBarIsVisible(currentPage, false);
            Shell.SetTabBarIsVisible(currentPage, false);
            NavigationPage.SetHasNavigationBar(currentPage, false);
        }
        else
        {
            if (s_navBarIsVisible.HasValue)
            {
                NavigationPage.SetHasNavigationBar(currentPage, s_navBarIsVisible.Value);
                Shell.SetNavBarIsVisible(currentPage, s_navBarIsVisible.Value);
            }
            if (s_backButton.HasValue)
            {
                NavigationPage.SetHasBackButton(currentPage, s_backButton.Value);
                NavigationPage.SetBackButtonTitle(currentPage, s_backButtonTitle);
            }
            if (s_tabBarIsVisible.HasValue)
            {
                Shell.SetTabBarIsVisible(currentPage, s_tabBarIsVisible.Value);
            }
            if (s_pageTitle is not null)
            {
                currentPage.Title = s_pageTitle;
            }
        }
    }
    internal static Page GetCurrentPage(this Page currentPage)
    {
#pragma warning disable CA1826 // Do not use Enumerable methods on indexable collections
        if (currentPage.NavigationProxy.ModalStack.LastOrDefault() is Page modal)
        {
            return modal;
        }
#pragma warning restore CA1826 // Do not use Enumerable methods on indexable collections

        if (currentPage is FlyoutPage flyoutPage)
        {
            return GetCurrentPage(flyoutPage.Detail);
        }

        if (currentPage is Shell shell && shell.CurrentItem?.CurrentItem is IShellSectionController shellSectionController)
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