using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafaCano.Util.Encriptacion;
using RafaCano.Util.Encriptacion.Model;
using Encriptacion.api.Services;
namespace Encriptacion.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EncriptionController : ControllerBase
    {
        private readonly CryptoService _crypto;
        public EncriptionController(CryptoService crypto)
        {
            _crypto = crypto;
        }
        // POST api/encription
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Service Started";
        }
      
        [HttpPost("encript")]
        public ActionResult<ValueEncript> Encript([FromBody] ValueEncript data)
        {
            ValueEncript res = new ValueEncript();
            _crypto.Encrypt(data.Value, "utf-8");
            res.Value = _crypto.Result;
            return res;
        }

        // POST api/encription/decript
        [HttpPost("decript")]
        public ActionResult<ValueEncript> Decript([FromBody] ValueEncript data)
        {
            ValueEncript res = new ValueEncript();
            _crypto.Decrypt(data.Value, "utf-8");
            res.Value = _crypto.Result;
            return res;
        }
    }
}
