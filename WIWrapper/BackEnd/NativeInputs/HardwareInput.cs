using System.Runtime.InteropServices;

namespace WIWrapper.BackEnd.NativeInputs;

[StructLayout(LayoutKind.Sequential)]
public struct HardwareInput
{
    public uint uMsg;
    public ushort wParamL, wParamH;
}
