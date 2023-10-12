namespace MauiPageFullScreen;
public static class Controls
{
    private static DeviceServices Control { get; set; } = new();
    private static bool IsFullScreen { get; set; } = false;
    /// <summary>
    /// Toggle Page Full Screen
    /// </summary>
    public static void ToggleFullScreenStatus()
    {
        if (IsFullScreen)
        {
            IsFullScreen = false;
            Control.RestoreScreen();
        }
        else
        {
            IsFullScreen = true;
            Control.FullScreen();
        }
    }
    public static void FullScreen()
    {
        Control.FullScreen();
    }
    public static void RestoreScreen()
    {
        Control.RestoreScreen();
    }
}
