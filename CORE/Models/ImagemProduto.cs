using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("IMAGEMPRODUTO")]
    public class ImagemProduto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Produto))]
        public int IdProduto { get; set; }

        [Display(Name ="Nome da Imagem")]
        public string NomeImagem { get; set; }

        [Required(ErrorMessage ="É Obrigatório que seja informado um caminho!")]
        [Display(Name = "Caminho da Imagem")]
        public string CaminhoImagem { get; set; }

        public string UrlImagem { get; set; }

        public string CaminhoFisico(string wwwrootPath)
        {
            return wwwrootPath + "\\uploads\\" + NomeImagem; 
        }
    }
}
