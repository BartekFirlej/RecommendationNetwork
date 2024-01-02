using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class Product
{
    public int Id { get; set; } // robi baza za nas nie chce wysylac, chce wyswietlac

    public string Name { get; set; } = null!; // chce wysylac chce wyswietal

    public float Price { get; set; }// chce wysylac chce wyswietal

    public int ProductTypeId { get; set; } // chce wysylac chce wyswietal

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<PurchaseProposal> PurchaseProposals { get; set; } = new List<PurchaseProposal>();
}
