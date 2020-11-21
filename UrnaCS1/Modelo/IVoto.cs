using BancoDeDadosJSON;

namespace UrnaCS1.Modelo
{
    public interface IVoto
    {

        bool Gravar(string numero, ISerializador serializador, IGeradorHash geradorHash);       

        string Diretorio { get; set; }

    }
}
