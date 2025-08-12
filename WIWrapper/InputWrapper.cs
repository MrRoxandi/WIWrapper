using System.Runtime.InteropServices;
using WIWrapper.BackEnd;
using WIWrapper.BackEnd.NativeInputs;
using WIWrapper.BackEnd.NativeInputs.Keyboard;
using WIWrapper.BackEnd.NativeInputs.Mouse;

namespace WIWrapper;

public class InputWrapper
{
    private readonly List<Input> _inputs = [];

    public static bool IsExtendedKey(KeyCodes key) => key switch
    {
        KeyCodes.RControl or KeyCodes.RAlt => true,

        KeyCodes.Divide or KeyCodes.NumPadEnter => true,

        KeyCodes.Insert or KeyCodes.Delete or KeyCodes.Home or KeyCodes.End
            or KeyCodes.PageUp or KeyCodes.PageDown => true,

        KeyCodes.Up or KeyCodes.Down or KeyCodes.Left or KeyCodes.Right => true,

        KeyCodes.LWin or KeyCodes.RWin or KeyCodes.Apps => true,

        KeyCodes.PrintScreen or KeyCodes.NumLock or KeyCodes.Sleep => true,

        _ => false
    };
    public static MouseFlag ToMouseFlag(MouseButton button, bool down) => button switch
    {
        BackEnd.NativeInputs.Mouse.MouseButton.Left => down ? MouseFlag.LeftDown : MouseFlag.LeftUp,
        BackEnd.NativeInputs.Mouse.MouseButton.Right => down ? MouseFlag.RightDown : MouseFlag.RightUp,
        BackEnd.NativeInputs.Mouse.MouseButton.Middle => down ? MouseFlag.MiddleDown : MouseFlag.MiddleUp,
        _ => throw new NotImplementedException("[ERROR]: This button is not implemented")
    };

    private static (int X, int Y) NormalizeCoordinates(int x, int y)
    {
        var width = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
        var height = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);

        return ((int)(x * 65535.0 / (width - 1)), (int)(y * 65535.0 / (height - 1)));
    }

    public InputWrapper KeyDown(KeyCodes key)
    {
        var input = new Input
        {
            Type = (uint)InputType.Keyboard,
            Data =
            {
                KeyboardInput = new KeyboardInput
                {
                    wVk = (ushort)key,
                    wScan = (ushort)NativeMethods.MapVirtualKey((uint)key, 0), // MAPVK_VK_TO_VSC = 0
                    dwFlags = (uint)(KeyboardFlags.ScanCode | (IsExtendedKey(key) ? KeyboardFlags.ExtendedKey : 0)),
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper KeyUp(KeyCodes key)
    {
        var input = new Input
        {
            Type = (uint)InputType.Keyboard,
            Data =
            {
                KeyboardInput = new KeyboardInput
                {
                    wVk = (ushort)key,
                    wScan = (ushort)NativeMethods.MapVirtualKey((uint)key, 0), // MAPVK_VK_TO_VSC = 0
                    dwFlags = (uint)(KeyboardFlags.ScanCode | KeyboardFlags.KeyUp | (IsExtendedKey(key) ? KeyboardFlags.ExtendedKey : 0)),
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper KeyType(KeyCodes key) => this.KeyDown(key).KeyUp(key);

    public InputWrapper CharDown(char ch)
    {
        var input = new Input
        {
            Type = (uint)InputType.Keyboard,
            Data =
            {
                KeyboardInput = new()
                {
                    wVk = 0,
                    wScan = ch,
                    dwFlags = (uint)KeyboardFlags.Unicode,
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };

        _inputs.Add(input);
        return this;
    }

    public InputWrapper CharUp(char ch)
    {
        var input = new Input
        {
            Type = (uint)InputType.Keyboard,
            Data =
            {
                KeyboardInput = new()
                {
                    wVk = 0,
                    wScan = ch,
                    dwFlags = (uint)(KeyboardFlags.Unicode | KeyboardFlags.KeyUp),
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };

        _inputs.Add(input);
        return this;
    }

    public InputWrapper TypeChar(char ch) => this.CharDown(ch).CharUp(ch);

    public InputWrapper TypeString(string str) 
    {
        foreach (var ch in str)
            this.TypeChar(ch);
        return this;
    }

    public InputWrapper MouseButton(MouseButton button, bool down = true)
    {
        var input = new Input
        {
            Type = (uint)InputType.Mouse,
            Data =
            {
                MouseInput = 
                {
                    dX = 0, dY = 0,
                    mouseData = 0,
                    dwFlags = (uint)ToMouseFlag(button, down),
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper MouseXButton(int xButton, bool down = true)
    {
        if (xButton != 1 &&  xButton != 2) 
            throw new ArgumentOutOfRangeException(nameof(xButton), "XButton value must be 1 or 2.");

        var input = new Input
        {
            Type = (uint)InputType.Mouse,
            Data =
            {
                MouseInput =
                {
                    dX = 0, dY = 0,
                    mouseData = xButton == 1 ? 0x0001u : 0x0002u,
                    dwFlags = (uint)(down? MouseFlag.XDown : MouseFlag.XUp),
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper MouseClick(MouseButton button) => this.MouseButton(button).MouseButton(button, false);
    
    public InputWrapper MouseXClick(int button) => this.MouseXButton(button).MouseXButton(button, false);

    public static (int X, int Y) MousePosition()
    {
        if (NativeMethods.GetCursorPos(out var point))
        {
            return (point.X, point.Y);
        }
        else
        {
            throw new InvalidOperationException("[ERROR]: Unable to get cursor position");
        }
    }

    public InputWrapper MouseMove(int dx, int dy, bool normalized = true) 
    {
        var (X, Y) = normalized ? NormalizeCoordinates(dx, dy) : (dx, dy); 
        var input = new Input
        {
            Type = (uint)InputType.Mouse,
            Data =
            {
                MouseInput = new()
                {
                    dX = X, dY = Y,
                    dwFlags = (uint) MouseFlag.Move,
                    dwExtraInfo= NativeMethods.GetMessageExtraInfo(),
                    time = 0,
                    mouseData = 0,
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper MouseSet(int x, int y, bool normalized = true) 
    {
        var (X, Y) = normalized ? NormalizeCoordinates(x, y) : (x, y);
        var input = new Input
        {
            Type = (uint)InputType.Mouse,
            Data =
            {
                MouseInput = new()
                {
                    dX = X, dY = Y,
                    dwFlags = (uint) (MouseFlag.Move | MouseFlag.Absolute),
                    dwExtraInfo= NativeMethods.GetMessageExtraInfo(),
                    time = 0,
                    mouseData = 0,
                }
            }
        };
        _inputs.Add(input);
        return this;
    }

    public InputWrapper WheelScroll(uint distance, bool vertical = false)
    {
        var input = new Input
        {
            Type = (uint)InputType.Mouse,
            Data =
            {
                MouseInput = new()
                {
                    dX = 0, dY = 0, time = 0,
                    mouseData = distance,
                    dwFlags = (uint) (vertical ? MouseFlag.VerticalWheel  : MouseFlag.HorizontalWheel),
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                }
            }
        };
        _inputs.Add(input);
        return this;
    }
    public InputWrapper SendInputs()
    {
        if (_inputs.Count == 0) return this;
        var result = NativeMethods.SendInput((uint)_inputs.Count, [.. _inputs], Marshal.SizeOf<Input>());
        if (result != _inputs.Count)
        {
            throw new Exception($"[ERROR]: not all inputs were correctly handled by system\nError code: {Marshal.GetLastWin32Error()}");
        }

        _inputs.Clear();
        return this;
    }
}
