using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BancoDeDadosJSON
{
    public class CandidatoDAO : IDAO<Candidato>
    {
        List<Candidato> _todos;

        private ISerializador _serializador;

        string _pathDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB\\Candidatos\\");
        string _pathData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB\\Candidatos\\candidatos.db");
        public List<Candidato> Todos
        {
            get
            {
                if (_todos == null)
                    _todos = this.Buscar();

                return _todos;
            }
            set
            {
                _todos = value;
            }
        }
        public object Buscar(object id)
        {
            try
            {
                Candidato result =
                    (
                    from Candidato c in this.Todos
                    where c.Numero == id.ToString()
                    select c
                    ).First();

                if (result == null)
                {
                    return false;
                }
                else
                {
                    return result;
                }
            }
            catch(Exception ex) {
                string r = ex.Message;
                return false;
            }

        }

        public List<Candidato> Buscar()
        {
           
            if ((!Directory.Exists(_pathDB)) || (!File.Exists(_pathData)))
                return new List<Candidato>();


            using (StreamReader sr = new StreamReader(_pathData))
            {
               
               return _serializador.Deserialize<List<Candidato>>(sr.ReadToEnd());
            
            }            
            
        }

     

        public CandidatoDAO(ISerializador serializador)
        {
            this._serializador = serializador;
        }
    }
}
