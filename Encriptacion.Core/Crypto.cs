﻿using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RafaCano.Util.Encriptacion
{
    public class Crypto
    {
        private Byte[] _key;
        private Byte[] _iv;
        private Byte[] _encript;
        private string _decript;
        private string key = "";
        private StateProcessEncryption process = StateProcessEncryption.none;
        public string Result
        {
            get
            {
                string res = "";
                switch (process)
                {
                    case StateProcessEncryption.encrypting:
                        res = Convert.ToBase64String(_encript);
                        break;

                    case StateProcessEncryption.decrypting:
                        res = _decript;
                        break;

                    case StateProcessEncryption.none:
                        res = "";
                        break;
                }
                return res;
            }
        }
        public Crypto(string key, string pwd)
        {
            this.key = key; 
            _key = Encoding.UTF8.GetBytes(pwd);
            Array.Resize(ref _key, 32);
            _iv = Encoding.UTF8.GetBytes(key);
            Array.Resize(ref _iv, 16);
        }
        public Crypto()
        {
            _key = new byte[] { 0x00 };
            Array.Resize(ref _key, 32);
            _iv = new byte[] { 0x00 };
            Array.Resize(ref _iv, 16);
        }



        public void Encrypt(string text, string enc = "utf-8")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(enc);

            try
            {
                process = StateProcessEncryption.encrypting;

                using (Rijndael myRijndael = Rijndael.Create())
                {
                    myRijndael.Key = _key;
                    myRijndael.IV = _iv;

                    ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, encoding))
                            {
                                swEncrypt.Write(text);
                            }
                            _encript = msEncrypt.ToArray();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw e; ;
            }

        }
        public void Decrypt(string cipherText, string enc = "utf-8")
        {
            process = StateProcessEncryption.decrypting;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(enc);
            byte[] encoded = Convert.FromBase64String(cipherText);

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = _key;
                rijAlg.IV = _iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encoded))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, encoding))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            _decript = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

        }

        public void EncryptFile(FileInfo fichero, string enc = "")
        {
            bool checkBOM = false;
            if (enc.Length == 0)
            {
                enc = "utf-8";
                checkBOM = true;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(enc);

            try
            {
                Encoding currentEncode = GetFileEncode(fichero.FullName);

                if (checkBOM && !encoding.Equals(currentEncode))
                {
                    throw new DistinctCodePageException(String.Format("La codificación del fichero {0} es: {1}, se esperaba una codificación: {2}"
                        , fichero.Name, currentEncode.EncodingName, encoding.EncodingName));
                }

                using (StreamReader contenido = fichero.OpenText())
                {

                    string texto = contenido.ReadToEnd();
                    _key = encoding.GetBytes(key + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    Array.Resize(ref _key, 32);
                    _iv = encoding.GetBytes(fichero.Name);
                    Array.Resize(ref _iv, 16);

                    FileInfo ficheroCodificado = new FileInfo(fichero.DirectoryName + "/" + Path.ChangeExtension(fichero.Name, ".cod"));
                    Encrypt(texto);


                    using (StreamWriter sw = ficheroCodificado.CreateText())
                    {

                        sw.Write("_" + fichero.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\t" + Result);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: {0}", ex.Message);
                throw ex;
            }
        }

        public void DecryptFile(FileInfo file, string enc = "")
        {
            if (String.IsNullOrWhiteSpace(enc))
                enc = "utf-8";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(enc);
            try
            {
                byte[] contentFile = ReadFileBinary(file);
                string header, cipherText, name, date;
                header = cipherText = name = date = "";

                int endHeader = Array.IndexOf(contentFile, (byte)0x09, 0, (contentFile.Length > 56 ? 56 : contentFile.Length));

                if (endHeader >= 0)
                {
                    header = encoding.GetString(contentFile, 0, endHeader + 1);
                    name = header.Substring(1, header.IndexOf("_", 1) - 1);
                    date = header.Substring(header.IndexOf("_", 1) + 1, 14);
                    cipherText = encoding.GetString(contentFile, endHeader + 1, contentFile.Length - endHeader - 1);
                }

                FileInfo DecodeFile = new FileInfo(file.DirectoryName + "/" + name + ".decod");

                _key = encoding.GetBytes(key + date);
                _iv = encoding.GetBytes(name);

                Array.Resize(ref _key, 32);
                Array.Resize(ref _iv, 16);

                Decrypt(cipherText);

                using (StreamWriter sw = DecodeFile.CreateText())
                {
                    sw.Write(Result);
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: {0}", ex.Message);
                throw ex;
            }

        }
        private byte[] ReadFileBinary(FileInfo fileRead)
        {
            byte[] contentFile = new byte[fileRead.Length];
            using (FileStream contained = fileRead.OpenRead())
            {
                int numBytesToRead = (int)fileRead.Length;
                int numBytesReaded = 0;

                while (numBytesToRead > 0)
                {
                    int n = contained.Read(contentFile, numBytesReaded, numBytesToRead);

                    if (n == 0)
                    {
                        break;
                    }
                    numBytesReaded += n;
                    numBytesToRead -= n;
                }
            }

            return contentFile;
        }
        private Encoding GetFileEncode(string srcFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc = Encoding.GetEncoding(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage);

            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }
    }
}
