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
        //Personas
        Persona niño;
        Persona adulto;
        // Fin Personas

        private KinectSensor sensor;

        DrawingGroup drawingGroup;
        DrawingImage imageSource;
        Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        Pen trackedBonePen = new Pen(Brushes.Red, 6);
        Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        Brush inferredJointBrush = Brushes.Yellow;
        double JointThickness = 3;

        Skeleton[] esqueletos;



        public MainWindow()
        {
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

        //BORRRARRRR
        float hmax = 0;
        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
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
                for (int i = 0; i < esqueletos.Length; i++)
                {
                    if (esqueletos[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (calcularAltura(esqueletos[i]) > 1.5)
                        {
                            if (adulto == null)
                            {
                                Console.WriteLine("se genera adulto");
                                adulto = new Persona(i, esqueletos[i], TipoPersona.Adulto);
                            }

                        }
                        else
                        {
                            if (niño == null)
                            {
                                Console.WriteLine("se genera niño");
                                niño = new Persona(i, esqueletos[i], TipoPersona.Niño);
                            }
                        }
                    }
                    DrawBonesAndJoints(esqueletos[i], dc);
                    #region foreach esqueletos (hasta el momento no es necesario)
                    //foreach (Skeleton esqueleto in esqueletos)
                    //{
                    //    if (esqueleto.TrackingState == SkeletonTrackingState.Tracked)
                    //    {
                    //        DrawBonesAndJoints(esqueleto, dc);
                    //        if (calcularAltura(esqueleto) > hmax)
                    //        {
                    //            hmax = calcularAltura(esqueleto);
                    //            Console.WriteLine(hmax);
                    //            if (calcularAltura(esqueleto) < 1)
                    //            {
                    //                p2 = new Persona(esqueleto, TipoPersona.Niño);
                    //                Console.WriteLine("se ha generado niño");
                    //            }
                    //            else
                    //            {
                    //                p1 = new Persona(esqueleto, TipoPersona.Adulto);
                    //                Console.WriteLine("se ha generado adulto");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Console.WriteLine(hmax);
                    //        }
                    // }
                    #endregion
                    drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, 320, 240));
                }
            }
        }

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
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
                    drawBrush = this.trackedJointBrush;
                else if (joint.TrackingState == JointTrackingState.Inferred)
                    drawBrush = this.inferredJointBrush;

                if (drawBrush != null)
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
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
            posicionCerca();
            posicionSentado();
            posicionManosArriba();
            posicionLaterales();

        }

        private float calcularAltura(Skeleton esqueleto)
        {
            Joint tobillo = esqueleto.Joints[JointType.AnkleLeft];
            Joint pecho = esqueleto.Joints[JointType.Head];

            float hx = pecho.Position.Y - tobillo.Position.Y;
            return hx;
        }
        private void posicionSentado()
        {
            if (niño != null)
            {
                if (esqueletos[niño.id].Joints[JointType.Head].Position.Y < 0)
                    lblStatusNiño.Content = "Sentado";
            }
            if (adulto != null)
            {
                if (esqueletos[adulto.id].Joints[JointType.Head].Position.Y < 0)
                    lblStatusAdulto.Content = "Sentado";
            }
        }

        float posjmi = 0;
        float posjmd = 0;
        private void posicionManosArriba()
        {
            if (niño != null)
            {
                posjmd = esqueletos[niño.id].Joints[JointType.HandLeft].Position.Y;
                posjmd = esqueletos[niño.id].Joints[JointType.HandRight].Position.Y;
                if (posjmd > 0.7 || posjmi > 0.7)
                    lblStatusNiño.Content ="Manos arriba";
            }
            if (adulto != null)
            {
                posjmd = esqueletos[adulto.id].Joints[JointType.HandLeft].Position.Y;
                posjmd = esqueletos[adulto.id].Joints[JointType.HandRight].Position.Y;
                if (posjmd > 0.7 || posjmi > 0.7)
                    lblStatusAdulto.Content = "Manos arriba";
            }

        }

        private void posicionCerca()
        {
            if (niño != null)
            {
               if(esqueletos[niño.id].Joints[JointType.Spine].Position.Z < 1.5)
                    lblStatusNiño.Content = "Cerca";
                else
                    lblStatusNiño.Content = "Lejos";
                
            }

            if(adulto != null)
            {
                if(esqueletos[adulto.id].Joints[JointType.Spine].Position.Z < 1.5)
                    lblStatusNiño.Content = "Cerca";
                else
                    lblStatusAdulto.Content = "Lejos";
            }
        }

        public void posicionLaterales()
        {
            if(niño != null)
            {
                if(esqueletos[niño.id].Joints[JointType.Spine].Position.X < -0.4)
                    lblStatusNiño.Content = "Por la izquierda";

                if (esqueletos[niño.id].Joints[JointType.Spine].Position.X > 0.4)
                    lblStatusNiño.Content = "Por la derecha";
            }

            if(adulto != null)
            {
                if (esqueletos[adulto.id].Joints[JointType.Spine].Position.X < -0.4)
                    lblStatusNiño.Content = "Por la izquierda";

                if (esqueletos[adulto.id].Joints[JointType.Spine].Position.X > 0.4)
                    lblStatusNiño.Content = "Por la derecha";
            }
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // el segundo parametro de MapSkeletonPointToDepthPoint, osea El DepthImageFormat es la resolucion que captura y la que es dibujada en el drawingContext
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
