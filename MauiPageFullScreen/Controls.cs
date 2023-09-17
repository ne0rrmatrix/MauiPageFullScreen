

namespace MauiPageFullScreen;

public static class Controls
{
    private static bool FullScreen { get; set; } = false;
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
