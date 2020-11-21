using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BancoDeDadosJSON
{
    public class SerializadorJSON : ISerializador
    {
        private JavaScriptSerializer _js = new JavaScriptSerializer();

        public string Serialize(object content)
        { 
            return _js.Serialize(content);
        }

        public T Deserialize<T>(object content)
        {           

            return _js.Deserialize<T>(content.ToString());
        }
    }
}
