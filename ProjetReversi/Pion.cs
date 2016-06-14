using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetReversi
{
    class Pion
    {
        private Color couleur;

        public Color Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        public Pion(Color c)
        {
            couleur = c;
        }

        public void Dessin(Graphics g, int width, int i, int j)
        {
            SolidBrush pionBrush = new SolidBrush(couleur);
            g.FillEllipse(pionBrush, 5, 5, width - 10, width - 10);
        }
    }
}
