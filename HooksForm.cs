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

        private void buttonStartGlobal_Click(object sender, System.EventArgs e)
        {
            GlobalHook.SetHook(true, true);
        }

        private void buttonStopGlobal_Click(object sender, System.EventArgs e)
        {
            GlobalHook.UnHook();
        }

        private void buttonStartLocal_Click(object sender, System.EventArgs e)
        {
            LocalHook.SetHook(true, true);
        }

        private void buttonStopLocal_Click(object sender, System.EventArgs e)
        {
            LocalHook.UnHook();
        }

        public void GlobalMouseMoved(object sender, MouseEventArgs e)
        {
            labelPosGlobal.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0) GlobalLogWrite("MouseButton\t- " + e.Button);
        }

        public void GlobalMyKeyDown(object sender, KeyEventArgs e)
        {
            GlobalLogWrite("KeyDown\t\t- " + e.KeyData);
            e.Handled = true;
        }

        public void GlobalMyKeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalLogWrite("KeyPress\t\t- " + e.KeyChar);
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

        public void LocalMouseMoved(object sender, MouseEventArgs e)
        {
            labelPosLocal.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0) LocalLogWrite("MouseButton\t- " + e.Button);
        }

        public void LocalMyKeyDown(object sender, KeyEventArgs e)
        {
            LocalLogWrite("KeyDown\t\t- " + e.KeyData);
            e.Handled = true;
        }

        public void LocalMyKeyPress(object sender, KeyPressEventArgs e)
        {
            LocalLogWrite("KeyPress\t\t- " + e.KeyChar);
        }

        public void LocalMyKeyUp(object sender, KeyEventArgs e)
        {
            LocalLogWrite("KeyUp\t\t- " + e.KeyData);
        }

        private void LocalLogWrite(string txt)
        {
            textBoxLocalEvents.AppendText(txt + Environment.NewLine);
            textBoxLocalEvents.SelectionStart = textBoxLocalEvents.Text.Length;
        }

        private void HooksForm_Load(object sender, EventArgs e)
        {
            GlobalHook = new Hooks(true);
            LocalHook = new Hooks(false);

            GlobalHook.OnMouseActivity += GlobalMouseMoved;
            GlobalHook.KeyDown += GlobalMyKeyDown;
            GlobalHook.KeyPress += GlobalMyKeyPress;
            GlobalHook.KeyUp += GlobalMyKeyUp;

            LocalHook.OnMouseActivity += LocalMouseMoved;
            LocalHook.KeyDown += LocalMyKeyDown;
            LocalHook.KeyPress += LocalMyKeyPress;
            LocalHook.KeyUp += LocalMyKeyUp;
        }
    }
}
