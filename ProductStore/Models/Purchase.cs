using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class Purchase
{
    public int Id { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int CustomerId { get; set; }

    public int? RecommenderId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual Customer? Recommender { get; set; }
}
