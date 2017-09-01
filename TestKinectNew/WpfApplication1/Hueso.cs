using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfApplication1
{
    public class Hueso
    {
       public Line linea;

        public Hueso(int grosor, SolidColorBrush color)
        {
            linea = new Line();
            linea.StrokeThickness = grosor;
            linea.Stroke = color;
        }

        public void setPosicionUno(int _x,int _y)
        {
            linea.X1 = _x;
            linea.Y1 = _y;
        }

        public void setPosicionDos(int _x,int _y)
        {
            linea.X2 = _x;
            linea.Y2 = _y;
        }
    }
}
