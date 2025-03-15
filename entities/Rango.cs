using System.ComponentModel.DataAnnotations;

namespace dot_net_api.entities;

public class Rango{

    [Key]
    public int Id{get; set;}

    [Required]
    [MaxLength(200)]
    public string? Nome { get; set; } 
    public ICollection<Ingrediente> Ingredientes {get; set;} = new List<Ingrediente>();


    public Rango()
    {

    }

    public Rango(int id, string Nome)
    {
        this.Nome = Nome;
        this.Id = id;
    }

}
