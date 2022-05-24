using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("USUARIOS")]
    public class Usuario : IUsuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome de Usuário é obrigatório")]
        [Display(Name="Usuário")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo E-mail é obrigatório"), Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(8, ErrorMessage ="Digite ao menos 8 caracteres")]
        [Display(Name="Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(8, ErrorMessage = "Digite ao menos 8 caracteres")]
        [Display(Name = "Repita a Senha")]
        [Compare("Password", ErrorMessage = "A senhe e a confirmação da senha são diferentes")]
        public virtual string RepitaPassword { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório")]
        [Display(Name ="Tipo de Usuário")]
        public TipoUsuario? TipoUsuario { get; set; }

        public override bool Equals(object obj)
        {
            var u = obj as Usuario;

            if (u == null)
            {
                return false;
            }

            return (u.Username.Equals(this.Username) || u.Email.Equals(this.Email)) 
                && u.Password.Equals(this.Password);
        }

        public override int GetHashCode()
        {
            var code = string.Concat(this.Username,this.Email);

            return code.GetHashCode();
        }
    }

    public enum TipoUsuario
    {
        Consumidor = 0,
        Comerciante = 1,
        Entregador = 2,
        Administrador = 3
    }

    public class User : IUsuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IUsuario
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}