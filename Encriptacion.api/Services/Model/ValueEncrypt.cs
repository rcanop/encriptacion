using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Encriptacion.api.Services.Model
{
    public class ValueEncrypt
    {
        [Required]
        public string Value { get; set; }

    }
}
