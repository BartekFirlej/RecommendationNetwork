using System.ComponentModel.DataAnnotations;

namespace BlazorServerFrontend.DTOs
{
    public class CustomerAuthentication
    {
        [Required]
        public string Email {  get; set; }
        [Required]
        public string PIN {  get; set; } 
    }
}
