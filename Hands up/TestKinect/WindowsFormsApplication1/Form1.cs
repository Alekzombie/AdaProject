using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        KinectSensor sensor;
        public Form1()
        {
            InitializeComponent();
        }




        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {

            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            lbStatus.Text = sensor.Status.ToString();

        }

   

        //private Bitmap CreateBitmapFromSensor(ColorImageFrame frame)
        //{
        //    var pixelData = new byte[frame.PixelDataLength];
        //    frame.CopyPixelDataTo(pixelData);
        //    //return pixelData.Tobitmap
        //}
    }
}
