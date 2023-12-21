using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public float Price { get; set; }

    public int ProductTypeId { get; set; }

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<PurchaseProposal> PurchaseProposals { get; set; } = new List<PurchaseProposal>();
}
