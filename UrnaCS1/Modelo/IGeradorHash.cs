using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UrnaCS1.Modelo
{
    public interface IGeradorHash
    {
        string GerarHash(string arquivo);
    }
}
