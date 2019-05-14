using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafaCano.Util.Encriptacion;
using Encriptacion.api.Services;
using Encriptacion.api.Services.Model;
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
            return Content("Service Started");
        }

        [HttpPost("encrypt")]
        public ActionResult<ValueEncrypt> Encrypt([FromBody] ValueEncrypt data)
        {
            if (data == null || String.IsNullOrWhiteSpace(data.Value))
            {
                return BadRequest();
            }
            ValueEncrypt res = new ValueEncrypt();
            _crypto.Encrypt(data.Value, "utf-8");
            res.Value = _crypto.Result;
            return res;
        }

        // POST api/encription/decript
        [HttpPost("decrypt")]
        public ActionResult<ValueDecrypt> Decrypt([FromBody] ValueDecrypt data)
        {
            if (data == null || String.IsNullOrWhiteSpace(data.Value))
            {
                return BadRequest();
            }
            ValueDecrypt res = new ValueDecrypt();
            _crypto.Decrypt(data.Value, "utf-8");
            res.Value = _crypto.Result;
            return res;
        }
    }
}
