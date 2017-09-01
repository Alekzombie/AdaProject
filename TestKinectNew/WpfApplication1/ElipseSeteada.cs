using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfApplication1
{
    public class ElipseSeteada
    {
       public Ellipse miElipse;

        public ElipseSeteada(int diametro,SolidColorBrush color)
        {
            miElipse = new Ellipse();
            miElipse.Height = diametro;
            miElipse.Width = diametro;
            miElipse.Fill = new SolidColorBrush(Colors.Red);
        }

        public void setPosicion(int x,int y)
        {
            miElipse.Margin = new System.Windows.Thickness(x-miElipse.Width/2, y-miElipse.Height/2, 0, 0);
        }
    }
}
