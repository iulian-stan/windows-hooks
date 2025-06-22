using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsHooks
{
    /// <summary>
    /// This class allows you to tap keyboard and mouse and / or to detect their activity even when an
    /// application runes in background or does not have any user interface at all. This class raises
    /// common .NET events with KeyEventArgs and MouseEventArgs so you can easily retrive any information you need.
    /// </summary>
    partial class Hooks
    {
        #region Windows constants

        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure.
        /// </summary>
        private const int WH_KEYBOARD = 2;
        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.
        /// </summary>
        private const int WH_MOUSE = 7;
        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
        /// Cannot be set locally.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// Cannot be set locally.
        /// </summary>
        private const int WH_MOUSE_LL = 14;

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves.
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;
        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button.
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button.
        /// </summary>
        private const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button.
        /// </summary>
        private const int WM_LBUTTONDBLCLK = 0x203;
        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button.
        /// </summary>
        private const int WM_RBUTTONDOWN = 0x204;
        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button.
        /// </summary>
        private const int WM_RBUTTONUP = 0x205;
        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button.
        /// </summary>
        private const int WM_RBUTTONDBLCLK = 0x206;
        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button.
        /// </summary>
        private const int WM_MBUTTONDOWN = 0x207;
        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button.
        /// </summary>
        private const int WM_MBUTTONUP = 0x208;
        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button.
        /// </summary>
        private const int WM_MBUTTONDBLCLK = 0x209;
        /// <summary>
        /// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel.
        /// </summary>
        private const int WM_MOUSEWHEEL = 0x020A;
        /// <summary>
        /// The WM_XBUTTONDOWN message is posted when the user presses the middle mouse button.
        /// </summary>
        private const int WM_XBUTTONDOWN = 0x20B;
        /// <summary>
        /// The WM_XBUTTONUP message is posted when the user releases one of the X mouse buttons.
        /// </summary>
        private const int WM_XBUTTONUP = 0x20C;
        /// <summary>
        /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks of the X mouse buttons.
        /// </summary>
        private const int WM_XBUTTONDBLCLK = 0x020D;

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem
        /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        private const int WM_KEYDOWN = 0x100;
        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem
        /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed,
        /// or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        private const int WM_KEYUP = 0x101;
        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user
        /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then
        /// presses another key. It also occurs when no window currently has the keyboard focus;
        /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that
        /// receives the message can distinguish between these two contexts by checking the context
        /// code in the lParam parameter.
        /// </summary>
        private const int WM_SYSKEYDOWN = 0x104;
        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user
        /// releases a key that was pressed while the ALT key was held down. It also occurs when no
        /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent
        /// to the active window. The window that receives the message can distinguish between
        /// these two contexts by checking the context code in the lParam parameter.
        /// </summary>
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        #endregion

        #region Windows structure definitions

        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/windef/ns-windef-point
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            /// <summary>
            /// Specifies the x-coordinate of the point.
            /// </summary>
            public int x;
            /// <summary>
            /// Specifies the y-coordinate of the point.
            /// </summary>
            public int y;
        }

        /// <summary>
        /// Contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mousehookstructex
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class MouseHookStructEx
        {
            /// <summary>
            /// The x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;
            /// <summary>
            /// A handle to the window that will receive the mouse message corresponding to the mouse event.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// The hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message.
            /// </summary>
            public uint wHitTestCode;
            /// <summary>
            /// Additional information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
            public uint mouseData;
        }

        /// <summary>
        /// Contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class LowLevelKeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public uint vkCode;
            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// </summary>
            public uint scanCode;
            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public uint flags;
            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public uint time;
            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class MouseLLHookStruct
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;
            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta.
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward,
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
            /// One wheel click is defined as WHEEL_DELTA, which is 120.
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used.
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public uint mouseData;
            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose
            ///LLMHF_INJECTED Test the event-injected flag.
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
            /// </summary>
            public uint flags;
            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public uint time;
            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        #endregion

        #region Windows function imports

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events
        /// are associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">
        /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
        /// </param>
        /// <param name="lpfn">
        /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a
        /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link
        /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
        /// </param>
        /// <param name="hMod">
        /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter.
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by
        /// the current process and if the hook procedure is within the code associated with the current process.
        /// </param>
        /// <param name="dwThreadId">
        /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated.
        /// If this parameter is zero, the hook procedure is associated with all existing threads running in the
        /// same desktop as the calling thread.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="idHook">
        /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwindowshookex
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="idHook">Ignored.</param>
        /// <param name="nCode">
        /// [in] Specifies the hook code passed to the current hook procedure.
        /// The next hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies the wParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        /// [in] Specifies the lParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain.
        /// The current hook procedure must also return this value. The meaning of the return value depends on the hook type.
        /// For more information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-callnexthookex
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, UIntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The ToAscii function translates the specified virtual-key code and keyboard
        /// state to the corresponding character or characters. The function translates the code
        /// using the input language and physical keyboard layout identified by the keyboard layout handle.
        /// </summary>
        /// <param name="uVirtKey">
        /// [in] Specifies the virtual-key code to be translated.
        /// </param>
        /// <param name="uScanCode">
        /// [in] Specifies the hardware scan code of the key to be translated.
        /// The high-order bit of this value is set if the key is up (not pressed).
        /// </param>
        /// <param name="lpbKeyState">
        /// [in] Pointer to a 256-byte array that contains the current keyboard state.
        /// Each element (byte) in the array contains the state of one key.
        /// If the high-order bit of a byte is set, the key is down (pressed).
        /// The low bit, if set, indicates that the key is toggled on. In this function,
        /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state
        /// of the NUM LOCK and SCROLL LOCK keys is ignored.
        /// </param>
        /// <param name="lpwTransKey">
        /// [out] Pointer to the buffer that receives the translated character or characters.
        /// </param>
        /// <param name="fuState">
        /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.
        /// </param>
        /// <returns>
        /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values.
        /// Value Meaning
        /// 0 The specified virtual key has no translation for the current state of the keyboard.
        /// 1 One character was copied to the buffer.
        /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character
        /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified
        /// virtual key to form a single character.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-toascii
        /// </remarks>
        [DllImport("user32")]
        private static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, uint fuState);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="pbKeyState">
        /// [out] The 256-byte array that receives the status data for each virtual key.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeyboardstate
        /// </remarks>
        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, off—alternating each time the key is pressed).
        /// </summary>
        /// <param name="nVirtKey">
        /// [in] A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code.
        /// If a non-English keyboard layout is used, virtual keys with values in the range ASCII A through Z and 0 through 9 are used to specify most of the character keys. For example, for the German keyboard layout, the virtual key of value ASCII O (0x4F) refers to the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
        /// </param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows:
        ///   - If the high-order bit is 1, the key is down; otherwise, it is up.
        ///   - f the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int nVirtKey);

        /// <summary>
        /// Retrieves the thread identifier of the calling thread.
        /// </summary>
        /// <returns>
        /// TThe return value is the thread identifier of the calling thread.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthreadid
        /// </remarks>
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        #endregion

        #region Callback functions

        /// <summary>
        /// The CallWndProc hook procedure is an application-defined or library-defined callback
        /// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer
        /// to this callback function. CallWndProc is a placeholder for the application-defined
        /// or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message.
        /// If nCode is HC_ACTION, the hook procedure must process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the
        /// CallNextHookEx function without further processing and must return the
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread.
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero.
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message.
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook
        /// procedure does not call CallNextHookEx, the return value should be zero.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms644975(v=vs.85)
        /// </remarks>
        private delegate int HookProc(int nCode, UIntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Create an argument for mouse event handler.
        /// </summary>
        /// <param name="message">The identifier of the mouse message.</param>
        /// <param name="data">Additional 32 bit information about mouse event.</param>
        /// <param name="point">The x- and y-coordinates of the cursor, in screen coordinates.</param>
        /// <returns>Mouse event arguments</returns>
        private MouseEventArgs CreateMouseEventArgs(nuint message,uint data, POINT point)
        {
            MouseEvents mouseEvent = MouseEvents.Unknown;
            MouseButtons button = MouseButtons.None;
            ushort highWord = (ushort)(data >> 16);
            short mouseDelta = 0;
            switch (message)
            {
                case WM_MOUSEMOVE:
                    mouseEvent = MouseEvents.Move;
                    break;
                case WM_LBUTTONDOWN:
                    mouseEvent = MouseEvents.Down;
                    button = MouseButtons.Left;
                    break;
                case WM_LBUTTONUP:
                    mouseEvent = MouseEvents.Up;
                    button = MouseButtons.Left;
                    break;
                case WM_LBUTTONDBLCLK:
                    mouseEvent = MouseEvents.DoubleClick;
                    button = MouseButtons.Left;
                    break;
                case WM_RBUTTONDOWN:
                    mouseEvent = MouseEvents.Down;
                    button = MouseButtons.Right;
                    break;
                case WM_RBUTTONUP:
                    mouseEvent = MouseEvents.Up;
                    button = MouseButtons.Right;
                    break;
                case WM_RBUTTONDBLCLK:
                    mouseEvent = MouseEvents.DoubleClick;
                    button = MouseButtons.Right;
                    break;
                case WM_MBUTTONDOWN:
                    mouseEvent = MouseEvents.Down;
                    button = MouseButtons.Middle;
                    break;
                case WM_MBUTTONUP:
                    mouseEvent = MouseEvents.Up;
                    button = MouseButtons.Middle;
                    break;
                case WM_MBUTTONDBLCLK:
                    mouseEvent = MouseEvents.DoubleClick;
                    button = MouseButtons.Middle;
                    break;
                case WM_MOUSEWHEEL:
                    mouseEvent = MouseEvents.Wheel;
                    mouseDelta = (short)highWord;
                    break;
                case WM_XBUTTONDOWN:
                    mouseEvent = MouseEvents.Down;
                    switch (highWord)
                    {
                        case 0x0001:
                            button = MouseButtons.XButton1;
                            break;
                        case 0x0002:
                            button = MouseButtons.XButton1;
                            break;
                    }
                    break;
                case WM_XBUTTONUP:
                    mouseEvent = MouseEvents.Up;
                    switch (highWord)
                    {
                        case 0x0001:
                            button = MouseButtons.XButton1;
                            break;
                        case 0x0002:
                            button = MouseButtons.XButton1;
                            break;
                    }
                    break;
                case WM_XBUTTONDBLCLK:
                    mouseEvent = MouseEvents.DoubleClick;
                    switch (highWord)
                    {
                        case 0x0001:
                            button = MouseButtons.XButton1;
                            break;
                        case 0x0002:
                            button = MouseButtons.XButton1;
                            break;
                    }
                    break;
            }
            //generate event
            return new WindowsHooks.MouseEventArgs(mouseEvent, button, point.x, point.y, mouseDelta);
        }

        /// <summary>
        /// A callback function which will be called every time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] A code the hook procedure uses to determine how to process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the
        /// CallNextHookEx function without further processing and should return the value
        /// returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] The identifier of the mouse message.
        /// This parameter can be one of the following messages: WM_LBUTTONDOWN,
        /// WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_RBUTTONDOWN or WM_RBUTTONUP.
        /// </param>
        /// <param name="lParam">
        /// [in] A pointer to an MSLLHOOKSTRUCT structure.
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// If nCode is greater than or equal to zero, and the hook procedure did not process the message,
        /// it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise,
        /// other applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and
        /// may behave incorrectly as a result.
        /// If the hook procedure processed the message, it may return a nonzero value to prevent the system
        /// from passing the message to the rest of the hook chain or the target window procedure.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/mouseproc
        /// </remarks>
        private int MouseProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            //If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function
            //without further processing and should return the value returned by CallNextHookEx.
            if (nCode < 0)
            {
                return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }
               
            //Marshall the data from callback.
            MouseHookStructEx mouseHookStruct = Marshal.PtrToStructure<MouseHookStructEx>(lParam);

            MouseEventArgs eventArgs = CreateMouseEventArgs(wParam, mouseHookStruct.mouseData, mouseHookStruct.pt);

            //Raise mouse event
            OnMouseEvent?.Invoke(this, eventArgs);
            
            return 0;
        }

        /// <summary>
        /// A callback function which will be called every time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] A code the hook procedure uses to determine how to process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the
        /// CallNextHookEx function without further processing and should return the value
        /// returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] The identifier of the mouse message.
        /// This parameter can be one of the following messages: WM_LBUTTONDOWN,
        /// WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_RBUTTONDOWN or WM_RBUTTONUP.
        /// </param>
        /// <param name="lParam">
        /// [in] A pointer to an MSLLHOOKSTRUCT structure.
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// If nCode is greater than or equal to zero, and the hook procedure did not process the message,
        /// it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise,
        /// other applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and
        /// may behave incorrectly as a result.
        /// If the hook procedure processed the message, it may return a nonzero value to prevent the system
        /// from passing the message to the rest of the hook chain or the target window procedure.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc
        /// </remarks>
        private int LowLevelMouseProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            //If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function
            //without further processing and should return the value returned by CallNextHookEx.
            if (nCode < 0)
            {
                return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }

            //Marshall the data from callback.
            MouseHookStructEx mouseHookStruct = Marshal.PtrToStructure<MouseHookStructEx>(lParam);

            MouseEventArgs eventArgs = CreateMouseEventArgs(wParam, mouseHookStruct.mouseData, mouseHookStruct.pt);

            //Raise mouse event
            OnMouseEvent?.Invoke(this, eventArgs);

            return 0;
        }

        /// <summary>
        /// A callback function which will be called every time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] A code the hook procedure uses to determine how to process the message.
        /// If code is less than zero, the hook procedure must pass the message to the
        /// CallNextHookEx function without further processing and should return the value
        /// returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] The virtual-key code of the key that generated the keystroke message.
        /// </param>
        /// <param name="lParam">
        /// [in] The repeat count, scan code, extended-key flag, context code, previous key-state flag, and
        /// transition-state flag. For more information about The lParam parameter, see Keystroke Message Flags.
        /// </param>
        /// <returns>
        /// If code is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// If code is greater than or equal to zero, and the hook procedure did not process the message,
        /// it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise,
        /// other applications that have installed WH_KEYBOARD hooks will not receive hook notifications and
        /// may behave incorrectly as a result.
        /// If the hook procedure processed the message, it may return a nonzero value to prevent the system
        /// from passing the message to the rest of the hook chain or the target window procedure.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/keyboardproc
        /// </remarks>
        private int KeyboardHookProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            //If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function
            //without further processing and should return the value returned by CallNextHookEx.
            if (nCode < 0)
            {
                return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }

            UInt32 flags = (UInt32)lParam;
            //The value is 1 if the key is down before the message is sent; it is 0 if the key is up.
            byte prevState = (byte)(flags >> 30 & 1);
            //The value is 0 if the key is being pressed and 1 if it is being released.
            byte tranState = (byte)(flags >> 31);
            KeyEventArgs e = new KeyEventArgs((Keys)wParam);
            KeyboardEvents kbEvent = KeyboardEvents.Unknown;
            if (tranState == 0)
            {
                if (prevState == 1)
                    kbEvent = KeyboardEvents.KeyPress;
                else
                    kbEvent = KeyboardEvents.KeyDown;
            }
            else
            {
                kbEvent = KeyboardEvents.KeyDown;
            }

            //Raise keyboard event
            OnKeyboardEvent?.Invoke(this, new KeyboardEventArgs(kbEvent, (Keys)wParam));

            return 0;
        }

        /// <summary>
        /// A callback function which will be called every time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] A code the hook procedure uses to determine how to process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the
        /// CallNextHookEx function without further processing and should return the value
        /// returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] The identifier of the keyboard message.
        /// This parameter can be one of the following messages: WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP.
        /// </param>
        /// <param name="lParam">
        /// [in] A pointer to a KBDLLHOOKSTRUCT structure.
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// If nCode is greater than or equal to zero, and the hook procedure did not process the message,
        /// it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise,
        /// other applications that have installed WH_KEYBOARD_LL hooks will not receive hook notifications
        /// and may behave incorrectly as a result.
        /// If the hook procedure processed the message, it may return a nonzero value to prevent the system
        /// from passing the message to the rest of the hook chain or the target window procedure.
        /// </returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelkeyboardproc
        /// </remarks>
        private int LowLevelKeyboardHookProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            //If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function
            //without further processing and should return the value returned by CallNextHookEx.
            if (nCode < 0)
            {
                return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }

            //read structure KeyboardHookStruct at lParam
            LowLevelKeyboardHookStruct MyKeyboardHookStruct = Marshal.PtrToStructure<LowLevelKeyboardHookStruct>(lParam);
            //The value is 0 if the key is being pressed and 1 if it is being released.
            byte tranState = (byte)(MyKeyboardHookStruct.flags >> 7 & 1);
            byte repeatCount = (byte)(MyKeyboardHookStruct.flags & 0xffff);
            KeyboardEvents kbEvent = KeyboardEvents.Unknown;
            if (tranState == 0)
            {
                if (repeatCount > 0)
                    kbEvent = KeyboardEvents.KeyPress;
                else
                    kbEvent = KeyboardEvents.KeyDown;
            }
            else
            {
                kbEvent = KeyboardEvents.KeyUp;
            }

            //Raise keyboard event
            OnKeyboardEvent?.Invoke(this, new KeyboardEventArgs(kbEvent, (Keys)MyKeyboardHookStruct.vkCode));

            return 0;
        }

        #endregion
    }
}
