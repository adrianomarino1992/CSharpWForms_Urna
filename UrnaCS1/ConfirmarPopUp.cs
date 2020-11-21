using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UrnaCS1.Modelo;

namespace UrnaCS1
{
    public partial class ConfirmarPopUp : Form
    {        

        public delegate void Gravar();

        private Gravar _confirmar;

        public ConfirmarPopUp(string numeroCandidato, string nomeCandidato, Gravar confirmar)
        {
            InitializeComponent();
           
            _confirmar = confirmar;

            this.label3.Text = numeroCandidato; 

            this.label4.Text = nomeCandidato;

            this.label3.Location = new Point(
                    Convert.ToInt32((this.Width / 2) - (this.label3.Width / 2)), this.label3.Location.Y
                );
            this.label4.Location = new Point(
                   Convert.ToInt32((this.Width / 2) - (this.label4.Width / 2)), this.label4.Location.Y
               );
        }

        private void btnCorrige_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirma_Click(object sender, EventArgs e)
        {
            _confirmar();
            this.Close();
        }
    }
}
