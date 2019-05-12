using RafaCano.Util.Encriptacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encriptacion.api.Services
{
    public class CryptoService: Crypto
    {
        public CryptoService():base("MiKey", "MiP455w0rd")
        {
            
        }
    }
}
