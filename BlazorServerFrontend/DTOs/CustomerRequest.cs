using System.ComponentModel.DataAnnotations;

public class CustomerRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PIN { get; set; }

    [Required]
    public string Town { get; set; }

    [Required]
    public string ZipCode { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public int VoivodeshipId { get; set; }

    public int? RecommenderId { get; set; }
}
