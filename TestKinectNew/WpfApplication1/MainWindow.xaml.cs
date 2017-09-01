using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Coding4Fun.Kinect.Wpf;

namespace WpfApplication1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor sensor;
        List<BitmapImage> images = new List<BitmapImage>();
        DrawingGroup drawingGroup;
        DrawingImage imageSource;
        Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        Pen trackedBonePen = new Pen(Brushes.Red, 6);
        Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        Brush inferredJointBrush = Brushes.Yellow;
        double JointThickness = 3;



        public MainWindow()
        {

            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/up.png", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/sentado.png", AppDomain.CurrentDomain.BaseDirectory))));

            InitializeComponent();

        }

        void mensaje(string msg)
        {
            MessageBox.Show(msg);
        }

        #region metodosDeCamara
        static void subeYBaja(KinectSensor sensor)
        {
            while (true)
            {
                subirCamara(sensor);
                bajarCamara(sensor);
            }
        }

        static void subirCamara(KinectSensor sensor)
        {
            if (sensor.ElevationAngle != 27)
                sensor.ElevationAngle = 27;
        }

        static void bajarCamara(KinectSensor sensor)
        {
            if (sensor.ElevationAngle != -27)
                sensor.ElevationAngle = -27;
        }

        static void centrarCamara(KinectSensor sensor)
        {
            if (sensor.ElevationAngle != 0)
                sensor.ElevationAngle = 0;
        }
        #endregion metodosDeCamara

        void iniciarSensor(KinectSensor sensor)
        {

        }

        void pararSensor(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.AudioSource.Stop();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.drawingGroup = new DrawingGroup();
            this.imageSource = new DrawingImage(this.drawingGroup);

            image.Source = this.imageSource;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btnstream_Click(object sender, RoutedEventArgs e)
        {
            if (btnstream.Content.Equals("Empezar"))
            {
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    btnstream.Content = "Parar";
                    sensor = KinectSensor.KinectSensors[0];
                    KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
                }
                sensor.Start();
                lbConexionID.Content = sensor.DeviceConnectionId;
                sensor.ColorStream.Enable();
                sensor.ColorFrameReady += Sensor_ColorFrameReady;
                sensor.SkeletonStream.Enable();
                sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

            }
            else
            {
                if (sensor != null && sensor.IsRunning)
                {
                    sensor.Stop();
                    this.btnstream.Content = "Empezar";
                    this.imagen.Source = null;

                }
            }
        }

        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] esqueletos = new Skeleton[0];
            using (SkeletonFrame framesEsqueleto = e.OpenSkeletonFrame())
            {
                if (framesEsqueleto != null)
                {
                    esqueletos = new Skeleton[framesEsqueleto.SkeletonArrayLength];
                    framesEsqueleto.CopySkeletonDataTo(esqueletos);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0, 320, 240));
                if (esqueletos == null) return;
                foreach (Skeleton esqueleto in esqueletos)
                {
                    if (esqueleto.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        DrawBonesAndJoints(esqueleto, dc);
                    }
                    drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, 320, 240));
                }
            }
        }

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                //imagen.Source = CreateBitmapFromSensor(frame);
                //  BitmapSource bms = CreateBitmapFromSensor(frame);
                // imagen.Source=BitmapSource.Create(bms.Width,bms.Height,96,96,PixelFormats.Bgr32,null,)
                if (frame == null)
                    return;

                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);

                imagen.Source = BitmapSource.Create(frame.Width, frame.Height, 96, 96, PixelFormats.Bgr32, null, pixelData, frame.Width * frame.BytesPerPixel);

            }
        }

        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }

        }

        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint jointZero = skeleton.Joints[jointType0];
            Joint jointOne = skeleton.Joints[jointType1];

            //verifica si los Joint si encuentran o no
            if (jointZero.TrackingState == JointTrackingState.NotTracked ||
                jointOne.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            Pen drawPen = this.inferredBonePen;
            if (jointZero.TrackingState == JointTrackingState.Tracked &&
                jointOne.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, SkeletonPointToScreen(jointZero.Position), SkeletonPointToScreen(jointOne.Position));

        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution320x240Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            lbStatus.Content = sensor.Status.ToString();

        }

        private BitmapSource CreateBitmapFromSensor(ColorImageFrame frame)
        {

            BitmapSource bs = null;
            try
            {
                var pixelData = new byte[frame.PixelDataLength];

                frame.CopyPixelDataTo(pixelData);
                bs = pixelData.ToBitmapSource(frame.Width, frame.Height);
            }
            catch (Exception ex)
            {
                mensaje(ex.Message);
            }
            return bs;
        }
    }
}
