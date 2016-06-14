using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ProjetReversi
{
    public partial class Reversi : Form
    {
        // attributs
        private const int grilleWidth = 8;
        private Case[,] grille = new Case[grilleWidth, grilleWidth];

        private Joueur joueurNoir = new Joueur(Color.Black);
        private Joueur joueurBlanc = new Joueur(Color.White);
        private Joueur laMain;

        public Joueur LaMain
        {
            get { return laMain; }
        }

        private int scoresNoir = 0;
        private int scoresBlanc = 0;

        private bool endGame = false;

        public bool EndGame
        {
            get { return endGame; }
            set { endGame = value; }
        }

        private bool aide=true;

        public bool Aide
        {
            get { return aide; }
        }

        public Reversi()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    Case c = new Case(this, i, j);
                    grille[i, j] = c;
                    this.tableLayoutPanelGrille.Controls.Add(c, i, j);
                }
            }

            laMain = joueurNoir; // debut par joueurNoir
            ChangeStatut();
        }

        public void CreeGrille()
        {
            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    Case c = new Case(this, i, j);
                    grille[i, j] = c;
                    this.tableLayoutPanelGrille.Controls.Add(c, i, j);
                }
            }
        }

        public void PasseLaMain()
        {
            if (laMain.Equals(joueurNoir))
            {
                laMain = joueurBlanc;
            }
            else if (laMain.Equals(joueurBlanc))
            {
                laMain = joueurNoir;
            }
            ChangeStatut();
            Refresh();
        }

        public void ChangeStatut()
        {
            string stt;

            if (laMain.Equals(joueurNoir))
            {
                stt = "Au NOIR de jouer!";
                labelStatut.Text = stt;
            }
            else if (laMain.Equals(joueurBlanc))
            { 
                stt = "Au BLANC de jouer!";
                labelStatut.Text = stt;
            }
        }

        public bool EstMouvementValid(int row, int col, bool poseCase = false)
        {
            if (grille[row, col].Joueur != null)
                return false;

            #region verifier 4 premiers mouvements
            int totalPion = 0;
            totalPion = scoresBlanc + scoresNoir;

            if (totalPion < 4)
            {
                if ((row == 3 || row == 4) && (col == 3 || col == 4))
                {
                    return true;
                }
            }
            #endregion

            else
            {
                bool valid = false;

                #region verifier a droite
                int count = 0;
                for (int i = col + 1; i < grilleWidth; i++)
                {
                    if (grille[row, i].Joueur == null) break;
                    else if (!laMain.Equals(grille[row, i].Joueur)) count++;
                    else if (laMain.Equals(grille[row, i].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = col; j < i; j++)
                                laMain.Jouer(grille[row, j]);
                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier a gauche
                count = 0;
                for (int i = col - 1; i > -1; i--)
                {
                    if (grille[row, i].Joueur == null) break;
                    else if (!laMain.Equals(grille[row, i].Joueur)) count++;
                    else if (laMain.Equals(grille[row, i].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = col; j > i; j--)
                                laMain.Jouer(grille[row, j]);

                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier en bas
                count = 0;
                for (int i = row + 1; i < grilleWidth; i++)
                {
                    if (grille[i, col].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, col].Joueur)) count++;
                    else if (laMain.Equals(grille[i, col].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = row; j < i; j++)
                                laMain.Jouer(grille[j, col]);

                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier a haut
                count = 0;
                for (int i = row - 1; i > -1; i--)
                {
                    if (grille[i, col].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, col].Joueur)) count++;
                    else if (laMain.Equals(grille[i, col].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = row; j > i; j--)
                                laMain.Jouer(grille[j, col]);
                            
                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier en bas a droite
                count = 0;
                int iTemp = col + 1;
                for (int i = row + 1; i < grilleWidth && iTemp < grilleWidth; i++, iTemp++)
                {
                    if (grille[i, iTemp].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, iTemp].Joueur)) count++;
                    else if (laMain.Equals(grille[i, iTemp].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = row; j < i; j++)
                                for (int k = col; k < iTemp; k++)
                                    laMain.Jouer(grille[j, k]);
                            
                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier en haut a gauche
                count = 0;
                iTemp = col - 1;
                for (int i = row - 1; i > -1 && iTemp > -1; i--, iTemp--)
                {
                    if (grille[i, iTemp].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, iTemp].Joueur)) count++;
                    else if (laMain.Equals(grille[i, iTemp].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;
                            valid = true;

                            for (int j = row; j > i; j--)
                                for (int k = col; k > iTemp; k--)
                                    laMain.Jouer(grille[j, k]);

                            break;
                        }
                        else break;
                    }
                }
                #endregion

                #region verifier en bas a gauche
                count = 0;
                iTemp = col - 1;
                for (int i = row + 1; i < grilleWidth && iTemp > -1; i++, iTemp--)
                {
                    if (grille[i, iTemp].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, iTemp].Joueur)) count++;
                    else if (laMain.Equals(grille[i, iTemp].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = row; j < i; j++)
                                for (int k = col; k > iTemp; k--)
                                    laMain.Jouer(grille[j, k]);
                            
                            break;
                        }
                        else
                            break;
                    }
                }
                #endregion

                #region verifier en haut a droite
                count = 0;
                iTemp = col + 1;
                for (int i = row - 1; i > -1 && iTemp < grilleWidth; i--, iTemp++)
                {
                    if (grille[i, iTemp].Joueur == null) break;
                    else if (!laMain.Equals(grille[i, iTemp].Joueur)) count++;
                    else if (laMain.Equals(grille[i, iTemp].Joueur))
                    {
                        if (count > 0)
                        {
                            if (!poseCase) return true;

                            valid = true;

                            for (int j = row; j > i; j--)
                                for (int k = col; k < iTemp; k++)
                                    laMain.Jouer(grille[j, k]);
                            
                            break;
                        }
                        else
                            break;
                    }
                }
                #endregion
                return valid;
            }
            return false;
        }

        public void CompteScores()
        {
            scoresNoir = 0;
            scoresBlanc = 0;

            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    if (grille[i, j].Joueur!=null)
                    {
                        if (joueurNoir.Equals(grille[i, j].Joueur))
                            scoresNoir++;
                        else if (joueurBlanc.Equals(grille[i, j].Joueur))
                            scoresBlanc++;
                    }
                }
            }

            labelScoresNoir.Text = scoresNoir.ToString();
            labelScoresBlanc.Text = scoresBlanc.ToString();

            Refresh();
        }

        public void ChercheCasesValides()
        {
            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    if (EstMouvementValid(i, j))
                        grille[i, j].Valid = true;
                    else
                        grille[i, j].Valid = false;
                }
            }
        }

        public bool ExisteCaseValid()
        {
            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    if (EstMouvementValid(i, j))
                        return true;
                }
            }
            return false;
        }

        public void AfficheResultat()
        {
            string stt;

            if (scoresNoir > scoresBlanc)
            {
                stt = "Joueur NOIR gagne le jeux!";
            }
            else if (scoresNoir < scoresBlanc)
            {
                stt = "Joueur BLANC gagne le jeux!";
            }
            else
            {
                stt = "On n'a pas de champion!";
            }
            labelStatut.Text = stt;

            endGame = true;
        }

        private void buttonQuitter_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Confirmer?", "Quitter", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonRejouer_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Confirmer?", "Rejouer", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ResetGrille();   
                CompteScores();
                laMain = joueurNoir;
                ChangeStatut();
            }
            Refresh();
        }

        private void ResetGrille()
        {
            for (int i = 0; i < grilleWidth; i++)
            {
                for (int j = 0; j < grilleWidth; j++)
                {
                    grille[i, j].Pion = null;
                    grille[i, j].Joueur = null;
                    grille[i, j].Valid = false;
                }
            }
        }

        private void buttonPasser_Click(object sender, EventArgs e)
        {
            if(ExisteCaseValid())
                PasseLaMain();
        }

        private void checkBoxAide_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAide.Checked)
            {
                aide = true;
                ChercheCasesValides();
            }
            else
                aide = false;

            Refresh();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string nomFichier = saveDialog.FileName;
                StreamWriter sw = new StreamWriter(nomFichier);
                sw.WriteLine("<!--?xml version=\"1.0\" encoding=\"UTF-8\" ?--> ");
                sw.WriteLine("<REVERSI>");

                for (int i = 0; i < grilleWidth; i++)
                {
                    for (int j = 0; j < grilleWidth; j++)
                    {
                        if (grille[i, j].Pion != null)
                            sw.WriteLine(grille[i, j].ToXML());
                    }
                }

                sw.WriteLine(" <LAMAIN>");
                sw.WriteLine(laMain.Couleur.ToArgb().ToString());
                sw.WriteLine(" </LAMAIN>");

                sw.WriteLine("</REVERSI>");
                sw.Close();
            }
        }

        private void buttonLoadGame_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Confirmer?", "Ouvrir fichier XML", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                #region choisir file a ouvrir
                OpenFileDialog opfd = new OpenFileDialog();
                opfd.Filter = "Fichier xml|*.xml";
                opfd.Title = "Choisir le fichier";
                opfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (opfd.ShowDialog() == DialogResult.OK)
                {
                    // code de relecture
                    Charger(opfd.FileName);
                }
                #endregion
            }
        }

        
        private void Charger(string fileName)
        {
            ResetGrille();
            #region ouvrir file .xml
            try
            {
                List<Case> cases = new List<Case>();

                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                foreach (XmlNode xN in doc.ChildNodes)
                {
                    if (xN.Name == "REVERSI")
                    {
                        foreach (XmlNode xNN in xN.ChildNodes)
                        {
                            if (xNN.Name == "PION")
                            {
                                Case c = new Case(xNN,this);
                                cases.Add(c);
                            }

                            else if(xNN.Name == "LAMAIN")
                            {
                                int c = int.Parse(xNN.InnerText);
                                Color coul = Color.FromArgb(c);
                                if (coul == Color.Black)
                                    laMain = joueurNoir;
                                else if (coul == Color.White)
                                    laMain = joueurBlanc;
                            }
                        }
                    }
                }
                Refresh(); 
                foreach (Case c in cases)
                {
                    c.Joueur.Jouer(grille[c.I, c.J]);
                }

                // ko lam dc tu buoc nay
                CompteScores();
                ChercheCasesValides();
                
                ChangeStatut();

                Refresh();
            }
            #endregion

            #region erreur d'ouvrir file .xml
            catch (FormatException ex)
            {
                MessageBox.Show("Le fichier " + fileName + " n'est pas utilisable! Valeurs incorrects dans le fichier!", "Erreur");
            }
            catch (Exception autre)
            {
                MessageBox.Show("Le fichier " + fileName + " n'est pas utilisable!", "Autre erreur");
            }
            #endregion
        }
    }
}
