

namespace MauiPageFullScreen;

public static class Controls
{
    private static bool FullScreen { get; set; } = false;
    /// <summary>
    /// Toggle Page Full Screen
    /// </summary>
    public static void SetFullScreenStatus()
    {
        if (FullScreen)
        {
            FullScreen = false;
            DeviceService.RestoreScreen();
        }
        else
        {
            FullScreen = true;
            DeviceService.FullScreen();
        }
    }
}
