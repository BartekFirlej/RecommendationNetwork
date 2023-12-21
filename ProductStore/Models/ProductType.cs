using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.Models;

public partial class ProductType
{
    [Key]
    public int Id { get; set; }                             

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); 
}
