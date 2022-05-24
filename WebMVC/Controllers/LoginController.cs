using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contratos;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginServices services;
        private readonly string SessionCarrinho = "_Carrinho";

        public LoginController(ILoginServices Services)
        {
            services = Services;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logon(Usuario usuario)
        {
            if (usuario.Username == null || usuario.Password == null) return View("Index",usuario);

            try
            {
                var claims = services.Autenticar(usuario);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity),
                    new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddDays(8) });
            }
            catch
            {
                ModelState.AddModelError("Username", "Usuario ou senha inválidos");
                return View("Index");
            }

            return Redirect("~/");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString(SessionCarrinho, "");
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }

        public IActionResult CriarNovo()
        {
            var model = new LoginViewModel()
            {
                Endereco = new Endereco(),
                PessoaFisica = new PessoaFisica(),
                PessoaJuridica = new PessoaJuridica(),
                Usuario = new Usuario()
            };

            return View(model);
        }
    }
}