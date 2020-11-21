using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BancoDeDadosJSON;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

namespace UrnaCS1
{
    public partial class Urna : Form
    {
        private IDAO<Candidato> _candidatoDAO;

        private bool _valid = false;

        public const int WM_NCLBUTTONDOWN = 0xA1;

        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MoveForm(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.Size == Screen.PrimaryScreen.WorkingArea.Size)
            {

                this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3, Screen.PrimaryScreen.WorkingArea.Height / 2);
            }


            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();

                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Gradient(object sender, PaintEventArgs e)
        {
            if (sender.GetType() == typeof(Panel))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(((Panel)sender).ClientRectangle,
                                                                   Color.White,
                                                                   Color.Gainsboro,
                                                                   150F))
                {
                    e.Graphics.FillRectangle(brush, ((Panel)sender).ClientRectangle);
                }
            }
        }

        private void FormResize(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Panel))
            {
                ((Panel)sender).Invalidate();
            }

        }

        private void CloseForm(Object sender, EventArgs e)
        {

            this.Close();
        }

        public void AddEvents()
        {

            foreach (Control c in this.pnlTeclado.Controls)
            {
                if (!c.Name.Contains("btn"))
                {
                    c.Click += (s, e) =>
                    {
                        string digit = ((Button)s).Text.Trim();

                        SetDigit(digit);

                    };
                }
            }

        }

        public void SetDigit(string digit)
        {

            if (lblN1.Text.Trim() != "" && lblN2.Text.Trim() != "")
            {
                return;
            }
            else
            {

                if (lblN1.Text.Trim() == "")
                {
                    lblN1.Text = digit;
                }
                else
                {
                    lblN2.Text = digit;

                    DataBase($"{lblN1.Text}{lblN2.Text}");
                }

            }

        }
        private void btnConfirma_Click(object sender, EventArgs e)
        {
            if (lblN1.Text.Trim() != "" && lblN2.Text.Trim() != "" && _valid)
            {
                string numero = $"{lblN1.Text}{lblN2.Text}";

                string nome = lblNome.Text;

                ConfirmarPopUp popup = new ConfirmarPopUp(
                    numeroCandidato: numero,
                    nomeCandidato: nome,
                    confirmar: () =>
                    {

                        Voto vt = new Voto();

                        if (vt.Gravar(numero, new SerializadorJSON(), new HashGerador()))
                        {
                            MessageBox.Show("Voto Finalizado");

                            Reset(new object(), new EventArgs());
                        }
                        else
                        {
                            MessageBox.Show("Erro ao atribuir voto");
                        }

                    });

                popup.ShowDialog();
            }
            else
            {

                MessageBox.Show("Informe um candidato");
            }
        }
        private void btnBranco_Click(object sender, EventArgs e)
        {
            if (lblN1.Text != "")
                return;
                      

            Reset(new object(), new EventArgs());

            _valid = true;

            lblN1.Text = "0";
            lblN2.Text = "0";
            lblNome.Text = "VOTO EM BRANCO";
            this.lblNome.Location = new Point(
                  Convert.ToInt32((this.pnlFotoContainer.Width / 2) - (this.lblNome.Width / 2)), this.lblNome.Location.Y
              );

        }
        public void Reset(object sender, EventArgs e)
        {
            _valid = false;           

            lblN1.Text = String.Empty;
            lblN2.Text = String.Empty;
            string pathImageDefault = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens\\userInit.png");
            if (File.Exists(pathImageDefault))
            {
                ptbFoto.Load(pathImageDefault);
            }
            else
            {

                ptbFoto.InitialImage = null;
            }

            lblNome.Text = "";

        }

        /*
         * Dispara a consulta ao banco em um Thread separado para não travar a tela do usuario enquanto busca as informações
         */
        public void DataBase(string numero = null)
        {
            _valid = false;

           

            _ = Task.Run(() =>
            {

                if (_candidatoDAO == null)
                    _candidatoDAO = new CandidatoDAO(new SerializadorJSON());

                if (numero != null && numero != "00")
                {

                    object result = _candidatoDAO.Buscar(numero);

                    if (result.GetType() == typeof(Candidato))
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            Candidato r = (Candidato)result;

                            lblN1.Text = r.Numero.Substring(0, 1);

                            lblN2.Text = r.Numero.Substring(1, 1);

                            lblNome.Text = r.Nome.ToUpper();

                            this.lblNome.Location = new Point(
                                 Convert.ToInt32((this.pnlFotoContainer.Width / 2) - (this.lblNome.Width / 2)), this.lblNome.Location.Y
                             );

                            string pathImageDefault = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userInit.png");

                            string fotoCandidato = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"DB\\Candidatos\\Fotos\\{r.Foto}");

                            if (File.Exists(fotoCandidato))
                            {
                                ptbFoto.Load(fotoCandidato);
                            }
                            else
                            {
                                Reset(new object(), new EventArgs());

                                MessageBox.Show("Não foi possivel carregar os dados do candidato");

                                return;
                            }

                            _valid = true;

                        }));
                    }
                    else
                    {
                        MessageBox.Show("Candidato não existe");
                    }

                    

                }
            });



        }

        public Urna()
        {
            InitializeComponent();

            AddEvents();

            Reset(new object(), new EventArgs());


        }


    }
}
