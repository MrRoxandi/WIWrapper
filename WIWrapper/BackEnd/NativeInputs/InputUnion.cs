using System.Runtime.InteropServices;

namespace WIWrapper.BackEnd.NativeInputs;

[StructLayout(LayoutKind.Explicit)]
public struct InputUnion
{
    [FieldOffset(0)] public MouseInput MouseInput;
    [FieldOffset(0)] public KeyboardInput KeyboardInput;
    [FieldOffset(0)] public HardwareInput HardwareInput;
}
