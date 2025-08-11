using System.Runtime.InteropServices;

namespace WIWrapper.BackEnd.NativeInputs;

[StructLayout(LayoutKind.Sequential)]
public struct Input
{
    public uint Type;
    public InputUnion Data;
}
