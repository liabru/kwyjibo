using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;

namespace Kwyjibo
{
    /// <summary>
    /// Video input wrapper class, allows easier control of video sources. This module is stateful.
    /// </summary>
    /// <remarks></remarks>
    public class VideoInput
    {
        /// <summary>
        /// List of devices.
        /// </summary>
        public FilterInfoCollection Devices = null;

        /// <summary>
        /// The video source.
        /// </summary>
        public IVideoSource Source = null;

        /// <summary>
        /// The sources identifier string.
        /// </summary>
        public string MonkierString = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public VideoInput()
        {
            RefreshDevices();
        }

        /// <summary>
        /// Refreshes the devices.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshDevices()
        {
            Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="videoDeviceNum">The video device number.</param>
        /// <remarks></remarks>
        public void SetDevice(int videoDeviceNum)
        {
            Stop();

            if (videoDeviceNum >= 0 && Devices.Count > videoDeviceNum)
            {
                MonkierString = Devices[videoDeviceNum].MonikerString;
                Source = new VideoCaptureDevice(MonkierString);
                VideoCaptureDevice s = (VideoCaptureDevice)Source;
                s.DesiredFrameRate = 30;
                s.DesiredFrameSize = new Size(640, 480);
            }
        }

        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="monikerString">The moniker string.</param>
        /// <remarks></remarks>
        public void SetDevice(string monikerString)
        {
            if (monikerString == "") 
                return;

            Stop();
            MonkierString = monikerString;
            Source = new VideoCaptureDevice(MonkierString);
            VideoCaptureDevice s = (VideoCaptureDevice)Source;
            s.DesiredFrameRate = 30;
            s.DesiredFrameSize = new Size(640, 480);
        }

        /// <summary>
        /// Displays the property page.
        /// </summary>
        /// <remarks></remarks>
        public void DisplayPropertyPage()
        {
            var source = ((VideoCaptureDevice)Source);
            if (source != null)
                source.DisplayPropertyPage(IntPtr.Zero);
        }

        /// <summary>
        /// Loads a video file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <remarks></remarks>
        public void LoadVideo(string path)
        {
            Stop();
            Source = new FileVideoSource(path);
        }

        /// <summary>
        /// Starts the video stream.
        /// </summary>
        /// <remarks></remarks>
        public void Start()
        {
            if (Source != null && !Source.IsRunning)
            {
                Source.Start();
            }
        }

        /// <summary>
        /// Stops the video stream.
        /// </summary>
        /// <remarks></remarks>
        public void Stop()
        {
            if (Source != null && Source.IsRunning)
            {
                Source.SignalToStop();
            }
        }
    }
}
