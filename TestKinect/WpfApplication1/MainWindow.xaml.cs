using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.Drawing;
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

        public MainWindow()
        {

            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/up.png", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/sentado.png", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/IJustCantWithMySwag.jpg", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/k.jpg", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/kthxbye.jpg", AppDomain.CurrentDomain.BaseDirectory))));
            images.Add(new BitmapImage(new Uri(string.Format("{0}/Images/reallyBro.jpg", AppDomain.CurrentDomain.BaseDirectory))));


            InitializeComponent();

            //KinectSensor sensor = KinectSensor.KinectSensors[0];
            // sensor.Start();
            //mensaje(sensor.IsRunning.ToString());
            //sensor.ElevationAngle = 27;
            // sensor.ColorStream.Enable();

            CanvasBorderWalala.BorderThickness = new Thickness(1);

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
                // sensor.ColorStream.Enable(ColorImageFormat.RawBayerResolution640x480Fps30);
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
            canvas.Children.Clear();
            Skeleton[] esqueletos = null;
            using (SkeletonFrame framesEsqueleto = e.OpenSkeletonFrame())
            {
                if (framesEsqueleto != null)
                {
                    esqueletos = new Skeleton[framesEsqueleto.SkeletonArrayLength];
                    framesEsqueleto.CopySkeletonDataTo(esqueletos);
                }
            }

            if (esqueletos == null) return;
            foreach (Skeleton esqueleto in esqueletos)
            {

                if (esqueleto.TrackingState == SkeletonTrackingState.Tracked)
                {
                    List<SkeletonPoint> spookyScarySkeleton = new List<SkeletonPoint>();


                    Joint jointCabeza = esqueleto.Joints[JointType.Head];
                    Joint jointMuñecaIzq = esqueleto.Joints[JointType.WristLeft];
                    Joint jointMuñecaDer = esqueleto.Joints[JointType.WristRight];
                    Joint jointManoIzq = esqueleto.Joints[JointType.HandLeft];
                    Joint jointManoDer = esqueleto.Joints[JointType.HandRight];
                    Joint jointHombroIzq = esqueleto.Joints[JointType.ShoulderLeft];
                    Joint jointHombroDer = esqueleto.Joints[JointType.ShoulderRight];
                    Joint jointHombroCentro = esqueleto.Joints[JointType.ShoulderCenter];
                    Joint jointCodoIzq = esqueleto.Joints[JointType.ElbowLeft];
                    Joint jointCodoDer = esqueleto.Joints[JointType.ElbowRight];
                    Joint jointSpine = esqueleto.Joints[JointType.Spine];
                    Joint jointCaderaCent = esqueleto.Joints[JointType.HipCenter];
                    Joint jointCaderaIzq = esqueleto.Joints[JointType.HipLeft];
                    Joint jointCaderaDer = esqueleto.Joints[JointType.HipRight];
                    Joint jointRodillaIzq = esqueleto.Joints[JointType.KneeLeft];
                    Joint jointRodillaDer = esqueleto.Joints[JointType.KneeRight];
                    Joint jointTobilloIzq = esqueleto.Joints[JointType.AnkleLeft];
                    Joint jointTobilloDer = esqueleto.Joints[JointType.AnkleRight];
                    Joint jointPieIzq = esqueleto.Joints[JointType.FootLeft];
                    Joint jointPieDer = esqueleto.Joints[JointType.FootRight];

                    SkeletonPoint posicionCabeza = jointCabeza.Position;                    
                    SkeletonPoint posicionMuñecaIzq = jointMuñecaIzq.Position;
                    SkeletonPoint posicionMuñecaDer = jointMuñecaDer.Position;
                    SkeletonPoint posicionManoIzq = jointManoIzq.Position;
                    SkeletonPoint posicionManoDer = jointManoDer.Position;
                    SkeletonPoint posicionHombroIzq = jointHombroIzq.Position;
                    SkeletonPoint posicionHombroDer = jointHombroDer.Position;
                    SkeletonPoint posicionHombroCentro = jointHombroCentro.Position;
                    SkeletonPoint posicionCodoIzq = jointCodoIzq.Position;
                    SkeletonPoint posicionCodoDer = jointCodoDer.Position;
                    SkeletonPoint posicionSpine = jointSpine.Position;
                    SkeletonPoint posicionCaderaCent = jointCaderaCent.Position;
                    SkeletonPoint posicionCaderaIzq = jointCaderaIzq.Position;
                    SkeletonPoint posicionCaderaDer = jointCaderaDer.Position;
                    SkeletonPoint posicionRodillaIzq = jointRodillaIzq.Position;
                    SkeletonPoint posicionRodillaDer = jointRodillaDer.Position;
                    SkeletonPoint posicionTobilloIzq = jointTobilloIzq.Position;
                    SkeletonPoint posicionTobilloDer = jointTobilloDer.Position;
                    SkeletonPoint posicionPieIzq = jointPieIzq.Position;
                    SkeletonPoint posicionPieDer = jointPieDer.Position;



                    spookyScarySkeleton.Add(posicionCabeza);
                    spookyScarySkeleton.Add(posicionMuñecaIzq);
                    spookyScarySkeleton.Add(posicionMuñecaDer);
                    spookyScarySkeleton.Add(posicionManoIzq);
                    spookyScarySkeleton.Add(posicionManoDer);
                    spookyScarySkeleton.Add(posicionHombroIzq);
                    spookyScarySkeleton.Add(posicionHombroDer);
                    spookyScarySkeleton.Add(posicionHombroCentro);
                    spookyScarySkeleton.Add(posicionCodoIzq);
                    spookyScarySkeleton.Add(posicionCodoDer);
                    spookyScarySkeleton.Add(posicionSpine);
                    spookyScarySkeleton.Add(posicionCaderaCent);
                    spookyScarySkeleton.Add(posicionCaderaIzq);
                    spookyScarySkeleton.Add(posicionCaderaDer);
                    spookyScarySkeleton.Add(posicionRodillaIzq);
                    spookyScarySkeleton.Add(posicionRodillaDer);
                    spookyScarySkeleton.Add(posicionTobilloIzq);
                    spookyScarySkeleton.Add(posicionTobilloDer);
                    spookyScarySkeleton.Add(posicionPieIzq);
                    spookyScarySkeleton.Add(posicionPieDer);

                    SolidColorBrush colorRojo = new SolidColorBrush(Colors.Red);

                    Hueso huesoMuñecaIzq = new Hueso(2, colorRojo);
                    Hueso huesoMuñecaDer = new Hueso(2, colorRojo);
                    Hueso huesoAntebrazoIzq = new Hueso(2, colorRojo);
                    Hueso huesoBrazoIzq = new Hueso(2, colorRojo);
                    Hueso huesoHombroIzq = new Hueso(2, colorRojo);
                    Hueso huesoCuello = new Hueso(2, colorRojo);
                    Hueso huesoHombroDer = new Hueso(2, colorRojo);
                    Hueso huesoBrazoDer = new Hueso(2, colorRojo);
                    Hueso huesoAntebrazoDer = new Hueso(2, colorRojo);
                    Hueso huesoSpine = new Hueso(2, colorRojo);
                    Hueso huesoCaderaCent = new Hueso(2, colorRojo);
                    Hueso huesoCaderaIzq = new Hueso(2, colorRojo);
                    Hueso huesoCaderaDer = new Hueso(2, colorRojo);
                    Hueso huesoMusloIzq = new Hueso(2, colorRojo);
                    Hueso huesoMusloDer = new Hueso(2, colorRojo);
                    Hueso huesoPantoIzq = new Hueso(2, colorRojo);
                    Hueso huesoPantoDer = new Hueso(2, colorRojo);
                    Hueso huesoPieIzq = new Hueso(2, colorRojo);
                    Hueso huesoPieDer = new Hueso(2, colorRojo);



                    ElipseSeteada eliCab = new ElipseSeteada(20, colorRojo);
                    ElipseSeteada eliManoIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliManoDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliMuñecaIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliMuñecaDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliHomIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliHomDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliHomCent = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliCodoIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliCodoDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliSpine = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliCaderaCent = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliCaderaIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliCaderaDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliRodillaIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliRodillaDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliTobilloIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliTobilloDer = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliPieIzq = new ElipseSeteada(10, colorRojo);
                    ElipseSeteada eliPieDer = new ElipseSeteada(10, colorRojo);


                    ColorImagePoint puntoCabeza = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCabeza.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoManoIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointManoIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoManoDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointManoDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoMuñecaIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointMuñecaIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoMuñecaDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointMuñecaDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoHomIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointHombroIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoHomDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointHombroDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoHomCent = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointHombroCentro.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoCodoIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCodoIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoCodoDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCodoDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoSpine = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointSpine.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoCaderaCent = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCaderaCent.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoCaderaIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCaderaIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoCaderaDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointCaderaDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoRodillaIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointRodillaIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoRodillaDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointRodillaDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoTobilloIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointTobilloIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoTobilloDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointTobilloDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoPieIzq = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointPieIzq.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
                    ColorImagePoint puntoPieDer = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointPieDer.Position, ColorImageFormat.RawBayerResolution640x480Fps30);


                    eliCab.setPosicion(puntoCabeza.X, puntoCabeza.Y);
                    eliManoIzq.setPosicion(puntoManoIzq.X, puntoManoIzq.Y);
                    eliManoDer.setPosicion(puntoManoDer.X, puntoManoDer.Y);
                    eliMuñecaIzq.setPosicion(puntoMuñecaIzq.X, puntoMuñecaIzq.Y);
                    eliMuñecaDer.setPosicion(puntoMuñecaDer.X, puntoMuñecaDer.Y);
                    eliHomIzq.setPosicion(puntoHomIzq.X, puntoHomIzq.Y);
                    eliHomDer.setPosicion(puntoHomDer.X, puntoHomDer.Y);
                    eliHomCent.setPosicion(puntoHomCent.X, puntoHomCent.Y);
                    eliCodoIzq.setPosicion(puntoCodoIzq.X, puntoCodoIzq.Y);
                    eliCodoDer.setPosicion(puntoCodoDer.X, puntoCodoDer.Y);
                    eliSpine.setPosicion(puntoSpine.X, puntoSpine.Y);
                    eliCaderaCent.setPosicion(puntoCaderaCent.X, puntoCaderaCent.Y);
                    eliCaderaIzq.setPosicion(puntoCaderaIzq.X, puntoCaderaIzq.Y);
                    eliCaderaDer.setPosicion(puntoCaderaDer.X, puntoCaderaDer.Y);
                    eliRodillaIzq.setPosicion(puntoRodillaIzq.X, puntoRodillaIzq.Y);
                    eliRodillaDer.setPosicion(puntoRodillaDer.X, puntoRodillaDer.Y);
                    eliTobilloIzq.setPosicion(puntoTobilloIzq.X, puntoTobilloIzq.Y);
                    eliTobilloDer.setPosicion(puntoTobilloDer.X, puntoTobilloDer.Y);
                    eliPieIzq.setPosicion(puntoPieIzq.X, puntoPieIzq.Y);
                    eliPieDer.setPosicion(puntoPieDer.X, puntoPieDer.Y);
                    huesoMuñecaIzq.setPosicionUno(puntoManoIzq.X, puntoManoIzq.Y);
                    huesoMuñecaIzq.setPosicionDos(puntoMuñecaIzq.X, puntoMuñecaIzq.Y);
                    huesoMuñecaDer.setPosicionUno(puntoManoDer.X, puntoManoDer.Y);
                    huesoMuñecaDer.setPosicionDos(puntoMuñecaDer.X, puntoMuñecaDer.Y);
                    huesoAntebrazoIzq.setPosicionUno(puntoMuñecaIzq.X, puntoMuñecaIzq.Y);
                    huesoAntebrazoIzq.setPosicionDos(puntoCodoIzq.X, puntoCodoIzq.Y);
                    huesoBrazoIzq.setPosicionUno(puntoCodoIzq.X, puntoCodoIzq.Y);
                    huesoBrazoIzq.setPosicionDos(puntoHomIzq.X, puntoHomIzq.Y);
                    huesoHombroIzq.setPosicionUno(puntoHomIzq.X, puntoHomIzq.Y);
                    huesoHombroIzq.setPosicionDos(puntoHomCent.X, puntoHomCent.Y);
                    huesoCuello.setPosicionUno(puntoHomCent.X, puntoHomCent.Y);
                    huesoCuello.setPosicionDos(puntoCabeza.X, puntoCabeza.Y);
                    huesoHombroDer.setPosicionUno(puntoHomCent.X, puntoHomCent.Y);
                    huesoHombroDer.setPosicionDos(puntoHomDer.X, puntoHomDer.Y);
                    huesoBrazoDer.setPosicionUno(puntoHomDer.X, puntoHomDer.Y);
                    huesoBrazoDer.setPosicionDos(puntoCodoDer.X, puntoCodoDer.Y);
                    huesoAntebrazoDer.setPosicionUno(puntoCodoDer.X, puntoCodoDer.Y);
                    huesoAntebrazoDer.setPosicionDos(puntoMuñecaDer.X, puntoMuñecaDer.Y);
                    huesoSpine.setPosicionUno(puntoHomCent.X, puntoHomCent.Y);
                    huesoSpine.setPosicionDos(puntoSpine.X, puntoSpine.Y);
                    huesoCaderaCent.setPosicionUno(puntoSpine.X, puntoSpine.Y);
                    huesoCaderaCent.setPosicionDos(puntoCaderaCent.X, puntoCaderaCent.Y);
                    huesoCaderaIzq.setPosicionUno(puntoCaderaCent.X, puntoCaderaCent.Y);
                    huesoCaderaIzq.setPosicionDos(puntoCaderaIzq.X, puntoCaderaIzq.Y);
                    huesoCaderaDer.setPosicionUno(puntoCaderaCent.X, puntoCaderaCent.Y);
                    huesoCaderaDer.setPosicionDos(puntoCaderaDer.X, puntoCaderaDer.Y);
                    huesoMusloIzq.setPosicionUno(puntoCaderaIzq.X, puntoCaderaIzq.Y);
                    huesoMusloIzq.setPosicionDos(puntoRodillaIzq.X, puntoRodillaIzq.Y);
                    huesoMusloDer.setPosicionUno(puntoCaderaDer.X, puntoCaderaDer.Y);
                    huesoMusloDer.setPosicionDos(puntoRodillaDer.X, puntoRodillaDer.Y);
                    huesoPantoIzq.setPosicionUno(puntoRodillaIzq.X, puntoRodillaIzq.Y);
                    huesoPantoIzq.setPosicionDos(puntoTobilloIzq.X, puntoTobilloIzq.Y);
                    huesoPantoDer.setPosicionUno(puntoRodillaDer.X, puntoRodillaDer.Y);
                    huesoPantoDer.setPosicionDos(puntoTobilloDer.X, puntoTobilloDer.Y);
                    huesoPieIzq.setPosicionUno(puntoTobilloIzq.X, puntoTobilloIzq.Y);
                    huesoPieIzq.setPosicionDos(puntoPieIzq.X, puntoPieIzq.Y);
                    huesoPieDer.setPosicionUno(puntoTobilloDer.X, puntoTobilloDer.Y);
                    huesoPieDer.setPosicionDos(puntoPieDer.X, puntoPieDer.Y);

                    canvas.Children.Add(eliCab.miElipse);
                    canvas.Children.Add(eliManoIzq.miElipse);
                    canvas.Children.Add(eliManoDer.miElipse);
                    canvas.Children.Add(eliMuñecaIzq.miElipse);
                    canvas.Children.Add(eliMuñecaDer.miElipse);
                    canvas.Children.Add(eliHomIzq.miElipse);
                    canvas.Children.Add(eliHomDer.miElipse);
                    canvas.Children.Add(eliHomCent.miElipse);
                    canvas.Children.Add(eliCodoIzq.miElipse);
                    canvas.Children.Add(eliCodoDer.miElipse);
                    canvas.Children.Add(eliSpine.miElipse);
                    canvas.Children.Add(eliCaderaCent.miElipse);
                    canvas.Children.Add(eliCaderaIzq.miElipse);
                    canvas.Children.Add(eliCaderaDer.miElipse);
                    canvas.Children.Add(eliRodillaIzq.miElipse);
                    canvas.Children.Add(eliRodillaDer.miElipse);
                    canvas.Children.Add(eliTobilloIzq.miElipse);
                    canvas.Children.Add(eliTobilloDer.miElipse);
                    canvas.Children.Add(eliPieIzq.miElipse);
                    canvas.Children.Add(eliPieDer.miElipse);

                    canvas.Children.Add(huesoMuñecaIzq.linea);
                    canvas.Children.Add(huesoMuñecaDer.linea);
                    canvas.Children.Add(huesoAntebrazoIzq.linea);
                    canvas.Children.Add(huesoBrazoIzq.linea);
                    canvas.Children.Add(huesoHombroIzq.linea);
                    canvas.Children.Add(huesoCuello.linea);
                    canvas.Children.Add(huesoHombroDer.linea);
                    canvas.Children.Add(huesoAntebrazoDer.linea);
                    canvas.Children.Add(huesoBrazoDer.linea);
                    canvas.Children.Add(huesoSpine.linea);
                    canvas.Children.Add(huesoCaderaCent.linea);
                    canvas.Children.Add(huesoCaderaIzq.linea);
                    canvas.Children.Add(huesoCaderaDer.linea);
                    canvas.Children.Add(huesoMusloIzq.linea);
                    canvas.Children.Add(huesoMusloDer.linea);
                    canvas.Children.Add(huesoPantoIzq.linea);
                    canvas.Children.Add(huesoPantoDer.linea);
                    canvas.Children.Add(huesoPieIzq.linea);
                    canvas.Children.Add(huesoPieDer.linea);

                    Console.WriteLine(((posicionManoDer.Y + posicionManoIzq.Y)/2));
                    if (((posicionManoIzq.Y + posicionManoDer.Y) / 2) > 0.8)
                        posicion.Source = images[0];

                    if (Math.Round((posicionCabeza.Y)) < 0.1)
                        posicion.Source = images[1];
                    

                    lbSkel.Content = string.Format(": X:{0} , Y:{1} , Z:{2}", posicionSpine.X, posicionSpine.Y, posicionSpine.Z);

                    //bool leftyEdge;
                    //bool rightyEdge;
                    //bool isClose;
                    //bool isFar;

                    //if (posicionSpine.Z < 1.5 )
                    //{
                    //    isClose = true;
                    //    isFar = false;
                    //    posicion.Source = images[5];
                    //}
                    //else
                    //{
                    //    isClose = false;
                    //    isFar = true;
                    //    posicion.Source = images[3];
                    //}
                    //if (posicionSpine.X < -0.4)
                    //{
                    //    leftyEdge = true;
                    //    posicion.Source = images[2];
                    //}
                    //else
                    //    rightyEdge = false;

                    //if (posicionSpine.X > 0.4 )
                    //{
                    //    rightyEdge = true;
                    //    posicion.Source = images[4];
                    //}
                        
                    //else
                    //    leftyEdge = false;



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
