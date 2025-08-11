using System.Runtime.InteropServices;

namespace WIWrapper.BackEnd.NativeInputs;

[StructLayout(LayoutKind.Sequential)]
public struct KeyboardInput
{
    public ushort wVk, wScan;
    public uint dwFlags, time;
    public IntPtr dwExtraInfo;
}
