using System;
using System.ComponentModel;
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
        /// <summary>
        /// Specifies if the hook is global (monitors the entire system)
        /// or local (only the events triggered on the current thread)
        /// </summary>
        private readonly bool isGlobalHook = true;

        /// The following delegates are explicitly declared
        /// to protect passed funvtions to unmanaged code from 
        /// garbage collecting
        /// <summary>
        /// Declare MouseHookProcedure as HookProc type.
        /// </summary>
        private static HookProc MouseHookProcedure;
        /// <summary>
        /// Declare KeyboardHookProcedure as HookProc type.
        /// </summary>
        private static HookProc KeyboardHookProcedure;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isGlobalHook">Specifies hook type (global/local)</param>
        public Hooks(bool isGlobalHook)
        {
            this.isGlobalHook = isGlobalHook;
        }

        /// <summary>
        /// Destruction.
        /// </summary>
        ~Hooks()
        {
            //uninstall hooks and do not throw exceptions
            Stop(true, true, false);
        }
        

        #region Event handlers

        /// <summary>
        /// Occurs when the user moves the mouse, presses any mouse button or scrolls the wheel
        /// </summary>
        public event MouseEventHandler OnMouseActivity;
        /// <summary>
        /// Occurs when the user presses a key
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when the user presses and releases 
        /// </summary>
        public event KeyPressEventHandler KeyPress;
        /// <summary>
        /// Occurs when the user releases a key
        /// </summary>
        public event KeyEventHandler KeyUp;

        #endregion

        #region Hook pointers

        /// <summary>
        /// Stores the handle to the mouse hook procedure.
        /// </summary>
        private int hMouseHook = 0;
        /// <summary>
        /// Stores the handle to the keyboard hook procedure.
        /// </summary>
        private int hKeyboardHook = 0;

        #endregion

        /// <summary>
        /// Installs both or one of mouse and/or keyboard hooks and starts rasing events
        /// </summary>
        /// <param name="InstallMouseHook"><b>true</b> if mouse events must be monitored</param>
        /// <param name="InstallKeyboardHook"><b>true</b> if keyboard events must be monitored</param>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void SetHook(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            // install Mouse hook only if it is not installed and must be installed
            if (hMouseHook == 0 && InstallMouseHook)
            {
                //install hook
                hMouseHook = SetWindowsHookEx(
                    isGlobalHook ? WH_MOUSE_LL : WH_MOUSE,
                    MouseHookProcedure = MouseHookProc,
                    isGlobalHook ? Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]) : IntPtr.Zero,
                    isGlobalHook ? 0 : AppDomain.GetCurrentThreadId());
                //If SetWindowsHookEx fails.
                if (hMouseHook == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup
                    Stop(true, false, false);
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }

            // install Keyboard hook only if it is not installed and must be installed
            if (hKeyboardHook == 0 && InstallKeyboardHook)
            {
                //install hook
                hKeyboardHook = SetWindowsHookEx(
                    isGlobalHook ? WH_KEYBOARD_LL : WH_KEYBOARD,
                    KeyboardHookProcedure = KeyboardHookProc,
                    isGlobalHook ? Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]) : IntPtr.Zero,
                    isGlobalHook ? 0 : AppDomain.GetCurrentThreadId());
                //If SetWindowsHookEx fails.
                if (hKeyboardHook == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup
                    Stop(false, true, false);
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        /// <summary>
        /// Stops monitoring both mouse and keyboard events and rasing events.
        /// </summary>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void UnHook()
        {
            Stop(true, true, true);
        }

        /// <summary>
        /// Stops monitoring both or one of mouse and/or keyboard events and rasing events.
        /// </summary>
        /// <param name="UninstallMouseHook"><b>true</b> if mouse hook must be uninstalled</param>
        /// <param name="UninstallKeyboardHook"><b>true</b> if keyboard hook must be uninstalled</param>
        /// <param name="ThrowExceptions"><b>true</b> if exceptions which occured during uninstalling must be thrown</param>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void Stop(bool UninstallMouseHook, bool UninstallKeyboardHook, bool ThrowExceptions)
        {
            //if mouse hook set and must be uninstalled
            if (hMouseHook != 0 && UninstallMouseHook)
            {
                //uninstall hook
                int retMouse = UnhookWindowsHookEx(hMouseHook);
                //reset invalid handle
                hMouseHook = 0;
                //if failed and exception must be thrown
                if (retMouse == 0 && ThrowExceptions)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }

            //if keyboard hook set and must be uninstalled
            if (hKeyboardHook != 0 && UninstallKeyboardHook)
            {
                //uninstall hook
                int retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                //reset invalid handle
                hKeyboardHook = 0;
                //if failed and exception must be thrown
                if (retKeyboard == 0 && ThrowExceptions)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }
    }
}
