using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDeDadosJSON
{
    public interface ISerializador
    {
        string Serialize(object content);

        T Deserialize<T>(object content);
    }
}
