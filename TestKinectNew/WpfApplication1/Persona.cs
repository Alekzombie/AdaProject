using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace WpfApplication1
{

    public enum TipoPersona
    {
        Niño = 0,
        Adulto = 1
    }
    class Persona
    {
        public int id;
        Skeleton esqueleto;
        public TipoPersona tipoPersona;

        public Persona(int posicion,Skeleton skeleton, TipoPersona type)
        {
            id = posicion;
            esqueleto = skeleton;
            tipoPersona = type;
        }


    }


}
