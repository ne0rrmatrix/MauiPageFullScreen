#if WINDOWS
using MauiPageFullScreen;
using MauiPageFullScreen.Interface;
using Microsoft.UI.Windowing;
#endif

namespace MauiPageFullScreen.Interface;
interface IDeviceServices
{
#if WINDOWS
    AppWindow FullScreen();
    AppWindow RestoreScreen();
#else
    void FullScreen();
    void RestoreScreen();
#endif
}
