using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Role { get; set; } = null!;
    }
}
