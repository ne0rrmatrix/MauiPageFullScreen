namespace MauiPageFullScreen;
public static class Controls
{
    private static DeviceServices Control { get; set; } = new();
    private static bool FullScreen { get; set; } = false;
    /// <summary>
    /// Toggle Page Full Screen
    /// </summary>
    public static void SetFullScreenStatus()
    {
        if (FullScreen)
        {
            FullScreen = false;
            Control.RestoreScreen();
        }
        else
        {
            FullScreen = true;
            Control.FullScreen();
        }
    }
}
