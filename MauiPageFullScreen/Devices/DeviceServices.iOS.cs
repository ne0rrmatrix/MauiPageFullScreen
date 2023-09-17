
using MauiPageFullScreen.Interface;

namespace MauiPageFullScreen.Devices;

internal class DeviceServices : IDeviceServices
{
    public void FullScreen()
    {
        PageExtensions.SetBarStatus(true);
    }

    public void RestoreScreen()
    {
        PageExtensions.SetBarStatus(false);
    }
}
