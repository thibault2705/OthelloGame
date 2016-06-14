using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetReversi
{
    public class Joueur
    {
        private Color couleur;

        public Color Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        public Joueur(Color c)
        {
            couleur = c;
        }

        public void Jouer(Case c)
        {
            c.Joueur = this;
            c.Pion = new Pion(couleur);
        }

        public override bool Equals(object obj)
        {
            Joueur that = obj as Joueur;
            if (that == null)
            {
                return false;
            }

            return this.couleur.ToArgb() == that.Couleur.ToArgb();
        }
    }
}
