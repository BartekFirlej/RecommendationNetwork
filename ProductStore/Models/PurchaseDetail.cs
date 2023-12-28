using System;
using System.Collections.Generic;

namespace ProductStore.Models;

public partial class PurchaseDetail
{
    public int Id { get; set; }

    public decimal Number { get; set; }

    public float PriceForOnePiece { get; set; }

    public int ProductId { get; set; }

    public int PurchaseId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
