using System.Drawing;
using System.Runtime.InteropServices;
using WIWrapper.BackEnd.NativeInputs;

namespace WIWrapper.BackEnd;

public static partial class NativeMethods
{
    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial short GetKeyState(ushort virtualKeyCode);

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial uint SendInput(uint numberOfInputs, Input[] inputs, int sizeOfInputStructure);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

    [LibraryImport("user32.dll")]
    public static partial nint GetMessageExtraInfo();

    [LibraryImport("user32.dll", EntryPoint = "MapVirtualKeyW")]
    public static partial uint MapVirtualKey(uint uCode, uint uMapType);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetCursorPos(out POINT lpPoint);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetCursorPos(int X, int Y);

    [LibraryImport("user32.dll")]
    public static partial int GetSystemMetrics(int nIndex);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X, Y;
    }

    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;
    public const int SM_CXVIRTUALSCREEN = 78;
    public const int SM_CYVIRTUALSCREEN = 79;
    public const int SM_XVIRTUALSCREEN = 76;
    public const int SM_YVIRTUALSCREEN = 77;
}