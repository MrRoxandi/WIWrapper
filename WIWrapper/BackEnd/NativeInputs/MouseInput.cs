using System.Runtime.InteropServices;

namespace WIWrapper.BackEnd.NativeInputs;

[StructLayout(LayoutKind.Sequential)]
public struct MouseInput
{
    public int dX, dY;
    public uint mouseData, dwFlags, time;
    public IntPtr dwExtraInfo;
}
