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
using Coding4Fun.Kinect.WinForm;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        KinectSensor sensor;
        public Form1()
        {
            InitializeComponent();
        }

        void mensaje(string msg)
        {
            MessageBox.Show(msg);
        }

        private void btnstream_Click(object sender, EventArgs e)
        {
            if (btnstream.Text.Equals("Empezar"))
            {
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    btnstream.Text = "Parar";
                    sensor = KinectSensor.KinectSensors[0];
                    KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
                }
                sensor.Start();
               // lbConexionID.Content = sensor.DeviceConnectionId;
                sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                sensor.ColorFrameReady += Sensor_ColorFrameReady;
                //sensor.SkeletonStream.Enable();
                //sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;
            }
            else
            {
                if (sensor != null && sensor.IsRunning)
                {
                    sensor.Stop();
                    this.btnstream.Text = "Empezar";
                    this.imagen.Image = null;
                    this.imagen.Refresh();
                }
            }
        }

        //private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        //{
        //    using (var frame = e.OpenSkeletonFrame())
        //    {
        //        imagen.Image = CreateskeletonFromSensor(frame);
        //    }
        //}

        //private Image CreateskeletonFromSensor(SkeletonFrame frame)
        //{
        //    Bitmap bs = null;
        //    try
        //    {
        //        var pixelData = new Skeleton[frame.SkeletonArrayLength];
        //        frame.CopySkeletonDataTo(pixelData);
        //        bs = pixelData;
        //    }
        //    catch (Exception ex)
        //    {
        //        mensaje(ex.Message);
        //    }
        //    return bs;
        //}

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                imagen.Image = CreateBitmapFromSensor(frame);
            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            //lbStatus.Content = sensor.Status.ToString();

        }

        private Bitmap CreateBitmapFromSensor(ColorImageFrame frame)
        {

            Bitmap bs = null;
            //var pixelData = new byte[frame.PixelDataLength];
            //frame.CopyPixelDataTo(pixelData);
            //bs = pixelData.ToBitmap(frame.Width, frame.Height);
            try
            {
                var pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                bs = pixelData.ToBitmap(frame.Width, frame.Height);
            }
            catch (Exception ex)
            {
                mensaje(ex.Message);
            }
            return bs;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            if (sensor != null && sensor.IsRunning)
            {
                sensor.Stop();
            }
        }
    }
}
