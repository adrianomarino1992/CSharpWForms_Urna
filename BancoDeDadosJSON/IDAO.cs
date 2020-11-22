using System;
using System.Collections.Generic;
using System.Text;

namespace BancoDeDadosJSON
{
    public interface IDAO<T>
    {        
        object Buscar(object id);
        
        List<T> Buscar();
        
    }
}
