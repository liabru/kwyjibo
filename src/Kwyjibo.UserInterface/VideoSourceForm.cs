using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using Kwyjibo.Modules;

namespace Kwyjibo.UserInterface.Forms
{
    public partial class VideoSourceForm : Form
    {
        public VideoInput Video = new VideoInput();
        private MainForm parentForm;

        public VideoSourceForm(MainForm parent)
        {
            InitializeComponent();
            refreshDeviceCombo();
            parentForm = parent;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            refreshDeviceCombo();
        }

        public void refreshDeviceCombo()
        {
            Video.RefreshDevices();
            deviceCombo.Items.Clear();
            foreach (FilterInfo fi in Video.Devices)
            {
                deviceCombo.Items.Add(fi.Name);
            }
            if (deviceCombo.Items.Count > 0)
                deviceCombo.SelectedIndex = 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Video.SetDevice(deviceCombo.SelectedIndex);
            parentForm.setVideo(Video);
            parentForm.setCalibrateUIState(CalibrateUIState.PlaceBoard);
            Close();
        }
    }
}
