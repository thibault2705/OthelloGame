using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ProjetReversi
{
    public class Case : PictureBox
    {
        int i;
        int j;

        public int I
        {
            get { return i; }
        }

        public int J
        {
            get { return j; }
        }

        private const int caseWidth = 50;
        private Reversi reversi;
        private Pion pion;
        private Joueur joueur;
        private bool valid = false;

        internal Pion Pion
        {
            get { return pion; }
            set { pion = value; }
        }
        
        public Joueur Joueur
        {
            get { return joueur; }
            set { joueur = value; }
        }

        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public Case(Reversi r, int row, int col)
        {
            reversi = r;
            i = row;
            j = col;
            this.Height = caseWidth;
            this.Width = caseWidth;
        }

        protected override void OnPaint(PaintEventArgs pe) 
        {
            base.OnPaint(pe);

            if (reversi.Aide)
            {
                reversi.ChercheCasesValides();
                if (valid)
                {
                    if (reversi.LaMain.Couleur == Color.Black)
                        BackColor = Color.Gray;
                    if (reversi.LaMain.Couleur == Color.White)
                        BackColor = Color.OrangeRed;
                }
                else
                    BackColor = Color.SteelBlue;
            }
            else
                BackColor = Color.SteelBlue;

            if (pion != null) 
            {
                pion.Dessin(pe.Graphics, caseWidth, i, j);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!reversi.EndGame)
            {
                if (reversi.EstMouvementValid(i, j, true))
                {
                    joueur = reversi.LaMain;
                    joueur.Jouer(this);
                    reversi.PasseLaMain();
                    reversi.CompteScores();

                    if (!reversi.ExisteCaseValid())
                    {
                        reversi.AfficheResultat();
                    }
                }
                else
                {
                    reversi.PasseLaMain();
                    if (!reversi.ExisteCaseValid())
                    {
                        reversi.AfficheResultat();
                    }
                }
            }
            Refresh();
        }

        public string ToXML()
        {
            string text = "<PION>";
            text += " <COULEUR>";
            text += "  " + pion.Couleur.ToArgb().ToString();
            text += " </COULEUR>";
            text += " <X>";
            text += " " + i.ToString();
            text += " </X>";
            text += " <Y>";
            text += "  " + j.ToString();
            text += " </Y>";
            text += "</PION>";
            return text;
        }

        public Case(XmlNode xNN, Reversi r)
        {
            reversi = r;
            this.Height = caseWidth;
            this.Width = caseWidth;

            foreach (XmlNode xNNN in xNN.ChildNodes)
            {
                switch (xNNN.Name)
                {
                    case "COULEUR":
                        int c = int.Parse(xNNN.InnerText);
                        pion = new Pion(Color.FromArgb(c));
                        joueur = new Joueur(Color.FromArgb(c));
                        break;

                    case "X":
                        i = int.Parse(xNNN.InnerText);
                        break;

                    case "Y":
                        j = int.Parse(xNNN.InnerText);
                        break;
                }
            }
        }
    }
}
