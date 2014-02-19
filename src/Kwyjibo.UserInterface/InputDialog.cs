using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kwyjibo.UserInterface.Forms
{
    public partial class InputDialog : Form
    {
        public InputDialog(string msg, string caption, string defaultText)
        {
            InitializeComponent();
            messageLabel.Text = msg;
            this.Text = caption;
            inputText.Text = defaultText;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public static string Dialog(string msg, string caption, string defaultText)
        {
            InputDialog inp = new InputDialog(msg, caption, defaultText);
            inp.ShowDialog();
            return inp.inputText.Text;
        }
    }
}
