using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Módulo_7.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Módulo_7.Controllers
{
    public class ValuesControllers : ControllerBase
    {

        private readonly IDataProtector _protector;
        private readonly HashService _hashservice;

        public ValuesControllers(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("cógido_oculto_y_único");
            _hashservice = hashService;
        }


        [HttpGet("hash")]
        public ActionResult GetHash()
        {
            string textoPlano = "Hello World!";
            var hashResult1 = _hashservice.Hash(textoPlano).Hash;
            var hashResult2 = _hashservice.Hash(textoPlano).Hash;
            return Ok(new { textoPlano, hashResult1, hashResult2 });
        }


        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();

            string textoPlano = "Hello World!";
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            string textoDescifrado = _protector.Unprotect(textoPlano);
            return Ok(new { textoPlano, textoCifrado, textoDescifrado});
        }
    }
}
