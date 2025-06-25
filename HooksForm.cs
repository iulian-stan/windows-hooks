using System;
using System.Windows.Forms;

namespace WindowsHooks
{
    public partial class HooksForm : Form
    {
        private Hooks GlobalHook;
        private Hooks LocalHook;

        public HooksForm()
        {
            InitializeComponent();
        }

        private void ButtonStartGlobal_Click(object sender, EventArgs e)
        {
            GlobalHook.SetHook(true, true);
        }

        private void ButtonStopGlobal_Click(object sender, EventArgs e)
        {
            GlobalHook.UnHook();
        }

        private void ButtonStartLocal_Click(object sender, EventArgs e)
        {
            LocalHook.SetHook(true, true);
        }

        private void ButtonStopLocal_Click(object sender, EventArgs e)
        {
            LocalHook.UnHook();
        }

        public void GlobalMouseEventHandler(object sender, MouseEventArgs e)
        {
            switch (e.Type)
            {
                case MouseEvents.Unknown:
                    //TODO: Event handler not implemented
                    break;
                case MouseEvents.Click:
                    GlobalLogWrite("Click\t\t- " + e.Button);
                    break;
                case MouseEvents.DoubleClick:
                    GlobalLogWrite("DoubleClick\t- " + e.Button);
                    break;
                case MouseEvents.Down:
                    GlobalLogWrite("ButtonDown\t- " + e.Button);
                    break;
                case MouseEvents.Up:
                    GlobalLogWrite("ButtonUp\t\t- " + e.Button);
                    break;
                case MouseEvents.Move:
                case MouseEvents.Wheel:
                    labelPosGlobal.Text = string.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
                    break;
            }
        }

        public void GlobalKeyboardEventHandler(object sender, KeyboardEventArgs e)
        {
            switch (e.Type)
            {
                case KeyboardEvents.Unknown:
                    //TODO: Event handler not implemented
                    break;
                case KeyboardEvents.KeyDown:
                    GlobalLogWrite("KeyDown\t\t- " + e.Key);
                    break;
                case KeyboardEvents.KeyPress:
                    GlobalLogWrite("KeyPress\t\t- " + e.Key);
                    break;
                case KeyboardEvents.KeyUp:
                    GlobalLogWrite("KeyUp\t\t- " + e.Key);
                    break;
            }
        }

        public void GlobalMyKeyDown(object sender, KeyEventArgs e)
        {
            GlobalLogWrite("KeyDown\t\t- " + e.KeyData);
            e.Handled = true;
        }

        public void GlobalMyKeyPress(object sender, KeyEventArgs e)
        {
            GlobalLogWrite("KeyPress\t\t- " + e.KeyData);
        }

        public void GlobalMyKeyUp(object sender, KeyEventArgs e)
        {
            GlobalLogWrite("KeyUp\t\t- " + e.KeyData);
        }

        private void GlobalLogWrite(string txt)
        {
            textBoxGlobalEvents.AppendText(txt + Environment.NewLine);
            textBoxGlobalEvents.SelectionStart = textBoxGlobalEvents.Text.Length;
        }

        public void LocalMouseEventHandler(object sender, MouseEventArgs e)
        {
            switch (e.Type)
            {
                case MouseEvents.Unknown:
                    //TODO: Event handler not implemented
                    break;
                case MouseEvents.Click:
                    LocalLogWrite("Click\t\t- " + e.Button);
                    break;
                case MouseEvents.DoubleClick:
                    LocalLogWrite("DoubleClick\t- " + e.Button);
                    break;
                case MouseEvents.Down:
                    LocalLogWrite("ButtonDown\t- " + e.Button);
                    break;
                case MouseEvents.Up:
                    LocalLogWrite("ButtonUp\t\t- " + e.Button);
                    break;
                case MouseEvents.Move:
                case MouseEvents.Wheel:
                    labelPosLocal.Text = string.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
                    break;
            }
        }

        public void LocalKeyboardEventHandler(object sender, KeyboardEventArgs e)
        {
            switch (e.Type)
            {
                case KeyboardEvents.Unknown:
                    //TODO: Event handler not implemented
                    break;
                case KeyboardEvents.KeyDown:
                    LocalLogWrite("KeyDown\t\t- " + e.Key);
                    break;
                case KeyboardEvents.KeyPress:
                    LocalLogWrite("KeyPress\t\t- " + e.Key);
                    break;
                case KeyboardEvents.KeyUp:
                    LocalLogWrite("KeyUp\t\t- " + e.Key);
                    break;
            }
        }

        private void LocalLogWrite(string txt)
        {
            textBoxLocalEvents.AppendText(txt + System.Environment.NewLine);
            textBoxLocalEvents.SelectionStart = textBoxLocalEvents.Text.Length;
        }

        private void HooksForm_Load(object sender, System.EventArgs e)
        {
            GlobalHook = new Hooks(true);
            LocalHook = new Hooks(false);

            GlobalHook.OnMouseEvent += GlobalMouseEventHandler;
            GlobalHook.OnKeyboardEvent += GlobalKeyboardEventHandler;

            LocalHook.OnMouseEvent += LocalMouseEventHandler;
            LocalHook.OnKeyboardEvent += LocalKeyboardEventHandler;
        }
    }
}
