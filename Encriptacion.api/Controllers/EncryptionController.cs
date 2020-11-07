using System;
using Microsoft.AspNetCore.Mvc;
using Encriptacion.api.Services;
using Encriptacion.api.Services.Model;
namespace Encriptacion.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EncryptionController : ControllerBase
    {
        private readonly CryptoService _crypto;
        public EncryptionController(CryptoService crypto)
        {
            _crypto = crypto;
        }

        /// <summary>
        /// Encripta una cadena
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Cadena encriptada</returns>
        /// <response code="200">Devuelve la cadena encriptada</response>
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

        /// <summary>
        /// Desencripta una cadena
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Cadena desencriptada</returns>
        /// <response code="200">Devuelve la cadena encriptada</response>
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
