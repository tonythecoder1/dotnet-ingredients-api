using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace dot_net_api.entities;

public class Ingrediente{

    public int Id{get; set;}

    [Required]
    [MaxLength(200)]
    public string Nome {get;set;}

    public Ingrediente() { }

    public ICollection<Rango> Rangos{get; set;} = new List<Rango>();

    [SetsRequiredMembers]
    public Ingrediente(int id, string nome){
        this.Nome = nome;
        this.Id = id;
    }

}


