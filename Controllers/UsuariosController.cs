using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Módulo_7.Context;
using Módulo_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Módulo_7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors("PermitirApiRequest")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        /*
        private readonly IDataProtector _protector; 
         
        public ValuesController (IDataProtectionProvider protectionProvider)
        {
            _protector = protectionProvider.CreateProtector("valor_único_y_quizás_secreto")
        }         
        */


        [HttpPost("AsignarUsuarioRol")]
        [EnableCors("PermitirApiRequest")] 
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsereId);
            if (usuario == null) { return NotFound(); }
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RoleName);
            return Ok();
        }


        [HttpPost("RemoverUsuarioRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsereId);
            if (usuario == null) { return NotFound(); }
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleName);
            return Ok();
        }

    }
}
