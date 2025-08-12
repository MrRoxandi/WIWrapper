# WIWrapper: A Fluent Windows Input Simulator

[![NuGet Version](https://img.shields.io/nuget/v/WIWrapper.svg?style=for-the-badge)](https://www.nuget.org/packages/WIWrapper/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
![Platform](https://img.shields.io/badge/platform-windows-blue.svg?style=for-the-badge)

**WIWrapper** (Windows Input Wrapper) is a simple, modern and intuitive .NET library for simulating keyboard and mouse input in Windows. It provides a convenient fluent wrapper over the low-level `SendInput` API, turning complex operations into elegant and readable call chains.

## ✨ Features

- **Fluent API**: Create sequences of actions using intuitive chains of methods (`.KeyDown().MouseMove().MouseClick()`).
- **Full keyboard simulation**: Supports keystrokes using virtual codes (`KeyCodes`), as well as entering Unicode characters for any language.
- **Precise mouse control**: Move the cursor in absolute or relative coordinates, simulate clicks of the left, middle, right and side (xButton) buttons.
- **Batch sending**: All input commands are accumulated and sent by a single system call `SendInputs()`, which ensures high performance and reliability.
- **Ease of use**: No complicated structures or P/Invoke in your code. Just create an instance of `InputWrapper` and start acting.
- **Automation**: Ideal for creating bots, UI testing automation tools, and any applications that require programmatic input management.

## Installation

The library is available as a NuGet package. You can install it using .NET CLI or Package Manager Console.

### .NET CLI

```shell
dotnet add package WIWrapper
```

### Package Manager Console

```shell
Install-Package WIWrapper
```

## 🎯 Quick Start

The main idea of the library is to build a chain of commands and execute them using the `SendInputs()` method.

### Example 1: Typing and pressing Enter

```cs
using WIWrapper;
using WIWrapper.BackEnd.NativeInputs.Keyboard;

// Create an instance, build a chain of commands and send them
new InputWrapper()
  .TypeString("Hello, automated world!")
  .KeyType(KeyCodes.Enter)
  .SendInputs();
```

### Example 2: Drawing a square with the mouse

This example demonstrates holding down a button, moving the mouse, and releasing it.

```cs
using WIWrapper;
using WIWrapper.BackEnd.NativeInputs.Mouse;

// Setting the cursor to the starting position
var wrapper = new InputWrapper();
wrapper.MouseSet(500, 500).SendInputs();

// Draw a 100x100px square

wrapper
  .MouseButton(MouseButton.Left, down: true) // Hold down the left button
  .MouseMove(100, 0) // Move to the right
  .MouseMove(0, 100) // Moving down
  .MouseMove(-100, 0) // Move to the left
  .MouseMove(0, -100) // Upward movement
  .MouseButton(MouseButton.Left, down: false) // Release the left button
  .SendInputs();
```

### Example 3: Keyboard shortcuts (Open Notepad and enter text)

```cs
using WIWrapper;
using WIWrapper.BackEnd.NativeInputs.Keyboard;

// 1. Press Win+R to open "Run"
var wrapper = new InputWrapper()

wrapper.KeyDown(KeyCodes.LWin)
  .KeyType(KeyCodes.R)
  .KeyUp(KeyCodes.LWin)
  .SendInputs();

// Giving the window time to appear
Thread.Sleep(500);

// 2. Enter "notepad" and press Enter

wrapper.TypeString("notepad")
  .KeyType(KeyCodes.Enter)
  .SendInputs();

// Giving the notebook time to open
Thread.Sleep(500);
// 3. Print text in notepad
wrapper.TypeString("WIWrapper is awesome!")
  .SendInputs();
```

## 📚 API Overview

| Method                    | Description                                                  |
| ------------------------- | ------------------------------------------------------------ |
| `KeyDown(key)`            | Simulates pressing (down) a key by its `KeyCode`.            |
| `KeyUp(key)`              | Simulates the release (up) of the key.                       |
| `KeyType(key)`            | Simulates full pressing (down, then up).                     |
| `TypeString(str)`         | Prints a string using Unicode characters.                    |
| `TypeChar(ch)`            | Prints a single Unicode character.                           |
| `MouseSet(x, y)`          | Sets the cursor to the absolute coordinates of the screen.   |
| `MouseMove(dx, dy)`       | Moves the cursor to the specified offset (relative to).      |
| `MouseButton(btn, down)`  | Presses or releases the left, right, or middle mouse button. |
| `MouseClick(btn)`         | Simulates a full click (down, then up).                      |
| `MouseXButton(num, down)` | Presses or releases the side mouse buttons (1 or 2).         |
| `MouseXClick(num)`        | Simulates a full click of the side button.                   |
| `SendInputs()`            | **Executes all commands in the queue.**                      |

## Assistance

We welcome any input! If you find a bug, have suggestions for improvement, or want to add a new feature.:

1. Open **Issue** to discuss your idea.
1. Make a fork of the repository.
1. Create a new branch for your changes.
1. Send **Pull Request**.

## 📜 License

This project is distributed under the MIT license. For more information, see the [LICENSE](LICENSE) file.
