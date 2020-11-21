using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrnaCS1.Modelo;
using System.Security.Cryptography;
using System.IO;

namespace UrnaCS1
{
    public class HashGerador : IGeradorHash
    {
        public string GerarHash(string arquivo)
        {
            return ConvertHashToString(MakeHash(arquivo));
        }

        private byte[] MakeHash(string arquivo) {

            using (FileStream stream = File.OpenRead(arquivo)) {

                return MD5.Create().ComputeHash(stream);
                
            }
        }

        private string ConvertHashToString(byte[] data) {

            string result = "";
            foreach (byte b in data) {

                result += b.ToString("x2");
            
            }

            return result;
        
        }
    }
}
