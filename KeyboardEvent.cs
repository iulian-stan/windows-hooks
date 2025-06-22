using System;
using System.Windows.Forms;

namespace WindowsHooks
{
    public enum KeyboardEvents
    {
        Unknown,
        KeyDown,
        KeyPress,
        KeyUp
    }

    /// <summary>
    /// Delegate tha handles keyboard events <see cref="KeyboardEventArgs"/>.
    /// </summary>
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    /// <summary>
    /// Keybpord event argument
    /// </summary>
    public class KeyboardEventArgs(KeyboardEvents type, Keys key) : EventArgs
    {
        /// <summary>
        ///  Gets event type.
        /// </summary>
        public KeyboardEvents Type { get; } = type;

        /// <summary>
        ///  Gets which key was pressed.
        /// </summary>
        public Keys Key { get; } = key;
    }

}
