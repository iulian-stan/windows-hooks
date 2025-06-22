using System;
using System.Windows.Forms;

namespace WindowsHooks
{
    /// <summary>
    /// Mouse event type.
    /// </summary>
    public enum MouseEvents
    {
        Unknown,
        Click,
        DoubleClick,
        Down,
        Move,
        Up,
        Wheel
    }

    /// <summary>
    /// Delegate tha handles mouse events <see cref="MouseEventArgs"/>.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Mouse event argument
    /// </summary>
    public class MouseEventArgs(MouseEvents type, MouseButtons button, int x, int y, int delta) : EventArgs
    {
        /// <summary>
        ///  Gets event type.
        /// </summary>
        public MouseEvents Type { get; } = type;

        /// <summary>
        ///  Gets which mouse button was pressed.
        /// </summary>
        public MouseButtons Button { get; } = button;

        /// <summary>
        ///  Gets the x-coordinate of a mouse click.
        /// </summary>
        public int X { get; } = x;

        /// <summary>
        ///  Gets the y-coordinate of a mouse click.
        /// </summary>
        public int Y { get; } = y;

        /// <summary>
        ///  Gets a signed count of the number of detents the mouse wheel has rotated.
        /// </summary>
        public int Delta { get; } = delta;
    }
}
