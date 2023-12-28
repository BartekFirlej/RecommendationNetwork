using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Town { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int VoivodeshipId { get; set; }

    public virtual ICollection<Purchase> PurchaseCustomers { get; set; } = new List<Purchase>();

    public virtual ICollection<PurchaseProposal> PurchaseProposals { get; set; } = new List<PurchaseProposal>();

    public virtual ICollection<Purchase> PurchaseRecommenders { get; set; } = new List<Purchase>();

    public virtual Voivodeship Voivodeship { get; set; } = null!;
}
