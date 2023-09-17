#if WINDOWS
using MauiPageFullScreen;
using MauiPageFullScreen.Interface;
using Microsoft.UI.Windowing;
#endif

namespace MauiPageFullScreen.Interface;
interface IDeviceServices
{
#if IOS || ANDROID || MACCATALYST
    public void FullScreen();
    public void RestoreScreen();
#elif WINDOWS
    public AppWindow FullScreen();
    public AppWindow RestoreScreen();
#else
    public void FullScreen();
    public void RestoreScreen();
#endif
}
