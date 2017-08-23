using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;


namespace Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            sensor.Start();
            // sensor.ElevationAngle = -27;
            //subirCamara(sensor);
            //bajarCamara(sensor);
           // subeYBaja(sensor);
            centrarCamara(sensor);
            



            Console.ReadKey(true);
        }

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
            if(sensor.ElevationAngle!=27)
            sensor.ElevationAngle = 27;
        }

       static void bajarCamara(KinectSensor sensor)
        {
            if(sensor.ElevationAngle!=-27)
            sensor.ElevationAngle = -27;
        }

        static void centrarCamara(KinectSensor sensor)
        {
            if (sensor.ElevationAngle != 0)
                sensor.ElevationAngle = 0;
        }

    }
}
