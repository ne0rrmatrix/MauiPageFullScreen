// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MauiPageFullScreen.Services;
internal static partial class DeviceService
{
    public static partial void FullScreen()
    {
        PageExtensions.SetBarStatus(true);
    }
    public static partial void RestoreScreen()
    {
        PageExtensions.SetBarStatus(false);
    }
}
