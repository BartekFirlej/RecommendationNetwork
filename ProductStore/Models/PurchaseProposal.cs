namespace ProductStore.Models;

public partial class PurchaseProposal
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
