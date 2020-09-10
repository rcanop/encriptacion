using RafaCano.Util.Encriptacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Encriptacion.wpf
{
/// <summary>
/// Lee y escribe en un fichero
/// </summary>
    public class LeerEscribir
    {
        private string fichero;
        private bool existe;
        public LeerEscribir()
        {
                
        }

        public string LeerFicheroUTF8(string fich, bool protegido)
        {
            string contenido = "";
            if (fich?.Length > 0)
            {
                fichero = Path.GetFullPath(fich);
                existe = File.Exists(fichero);
            }
            if (!existe)
                throw new IOException($"No se encuentra el fichero: {fichero}");

            if (protegido)
            {
                Crypto crypto = new Crypto();
                crypto.DecryptFile(new FileInfo(fichero), "utf-8", false);
                contenido = crypto.Result;

            }
            else
            {
                using (FileStream fs = new FileStream(fichero, FileMode.Open, FileAccess.Read))
                {
                    if (fs.CanRead)
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer);
                        contenido = UTF8Encoding.UTF8.GetString(buffer);
                    }
                };
            }
            return contenido;

        }

    }
}
