using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class Voivodeship
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
