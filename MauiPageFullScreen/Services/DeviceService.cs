// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINDOWS
using Microsoft.UI.Windowing;
#endif

namespace MauiPageFullScreen.Services;
internal static partial class DeviceService
{

#if ANDROID || IOS || MACCATALYST
    public static partial void FullScreen();
    public static partial void RestoreScreen();
#endif
#if WINDOWS
    public static partial AppWindow FullScreen();
    public static partial AppWindow RestoreScreen();
#endif
}
