using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrnaCS1.Modelo;
using System.CodeDom.Compiler;
using BancoDeDadosJSON;

namespace UrnaCS1
{
    public class Voto : IVoto
    {
        private string _diretorio;
        public string Diretorio
        {
            get
            {
                if (_diretorio == null)
                    _diretorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Votos\\");
                return _diretorio;
            }
            set
            {
                _diretorio = value;
            }
        }

        public bool Gravar(string numero, ISerializador serializador, IGeradorHash hashgenerator)
        {
            if (!Directory.Exists(Diretorio))
                Directory.CreateDirectory(Diretorio);

            long totalVotos =
                (
                from string v in Directory.GetFiles(Diretorio)
                where (!v.Contains("vld"))
                select v
                ).Count();

            var voto = new { Numero = numero, ValidacaoFile = $"vt_{totalVotos + 1}.vld" };

            try
            {
                string votoSerializado = serializador.Serialize(voto);

                string votoPath = Path.Combine(Diretorio, $"vt_{totalVotos + 1}.vt");

                using (StreamWriter sw = new StreamWriter(votoPath))
                {

                    sw.Write(votoSerializado);
                }

                string hash = hashgenerator.GerarHash(votoPath);

                string validacaoPath = Path.Combine(Diretorio, $"vt_{totalVotos + 1}.vld");

                using (StreamWriter sw = new StreamWriter(validacaoPath))
                {

                    sw.Write(hash);
                }

                return true;
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show("Erro", ex.Message);

                return false;
            }



        }


    }
}
