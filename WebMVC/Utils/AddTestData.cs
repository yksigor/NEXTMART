using System;
using Domain.Models;
using Repository.Data;
using System.Linq;

namespace WebMVC.Utils
{
    public class AddTestData
    {
        private readonly LojaDBContext context;

        public AddTestData(LojaDBContext context)
        {
            this.context = context;
        }

        public void Iniciar()
        {
            if (context.PessoasJuridicas.Any())
                return;

            CriarUsuarios();
            CriarEnderecos();
            CriarComerciante();
            CriarCategoria();
        }

        private void CriarCategoria()
        {
            var pj = context.PessoasJuridicas.FirstOrDefault(a => a.Usuario.Username == "alan");

            var cat = new CategoriaProduto
            {
                IdComerciante = pj.Id,
                Descricao = "Categoria Teste",
                Nome = "Teste",
                Loja = pj
            };

            context.Add(cat);
            context.SaveChanges();
        }

        private void CriarComerciante()
        {
            var user = context.Usuarios.FirstOrDefault(a => a.Username == "alan");
            var end = context.Enderecos.FirstOrDefault(a => a.UF == UF.SP);

            var pj = new PessoaJuridica
            {
                Usuario = user,
                IdUsuario = user.Id,
                Endereco = end,
                IdEndereco = end.Id,
                Cnpj = "58214834000180",
                RazaoSocial = "Testando",
                Saldo = 100,
                Segmento = "Teste",
                NomeFantasia = "Teste Fantasia",
                Telefone = "1122223333"
            };

            context.PessoasJuridicas.Add(pj);
            context.SaveChanges();
        }

        private void CriarEnderecos()
        {
            var end = new Endereco
            {
                Bairro = "Jd. das Oliveiras",
                CEP = "06867370",
                Complemento = "Casa",
                Logradouro = "Rua Felipe Mendes Rodrigues",
                Municipio = "Itapecerica da Serra",
                Numero = "123",
                UF = UF.SP
            };

            context.Enderecos.Add(end);
            context.SaveChanges();
        }

        private void CriarUsuarios()
        {
            var user = new Usuario
            {
                Email = "alan@alan.com",
                Password = "123456",
                TipoUsuario = TipoUsuario.Comerciante,
                RepitaPassword = "123456",
                Username = "alan"
            };
            
            context.Add(user);
            context.SaveChanges();
        }
    }
}