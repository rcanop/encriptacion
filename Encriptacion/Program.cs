using System;
using System.IO;
using System.Text;
using RafaCano.Util.Encriptacion;

namespace Encriptacion
{
    class Program
    {
        static void Main(string[] args)
        {
            string nombreFichero = "";
            string accion = "";
            for (var i = 0; i < args.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        nombreFichero = args[0];
                        break;
                    case 1:
                        accion = args[1];
                        break;
                }

            }

            if (File.Exists(nombreFichero))
            {
                FileInfo fichero = new FileInfo(nombreFichero);
                bool procesar = accion.Length == 0;
                bool descodificar = false;
                if (!procesar)
                {
                    procesar = (accion.ToLower() == "d");
                    descodificar = true;
                }

                if (procesar)
                    Proceder(fichero, descodificar);
                else
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Uso: Encriptacion <fichero> [d] ");
                    Console.WriteLine();
                    Console.WriteLine("d para desencriptar el fichero.");
                    Console.WriteLine("---------------------------------------------");

                }
            }
            else
            {
                Console.WriteLine("El Fichero: {0} no existe", nombreFichero.Trim());
            }
            ejemplocadenas();
            Console.ReadKey();

        }
        static void Proceder(FileInfo fichero, bool descodificar = false, string enc = "utf-8", string key = "M1P455w0rd", string iv = "NetCore")
        {
            Crypto cr = new Crypto(key, iv);
       
            if (descodificar)
            { 
                cr.DecryptFile(fichero, enc, true, true);
            }
            else
            {
                cr.EncryptFile(fichero, enc);
            }
        }
        
        static void ejemplocadenas()
        {
            string cadena = "El pico de la cigüeña es largo.";
            byte[] utfBytes = Encoding.UTF8.GetBytes(cadena);
            cadena = Convert.ToBase64String(utfBytes);
            Crypto cr = new Crypto("MiKey", "MiP455w0rd");

            try
            {
                Console.WriteLine(cadena);

                cr.Encrypt(cadena);
                string cipherText = cr.Result;
                Console.WriteLine(cipherText);

                cr.Decrypt(cipherText);
                Console.WriteLine(cr.Result);
                cr.Decode64();
                Console.WriteLine(cr.Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            //Console.ReadKey();
        }
    }
}
