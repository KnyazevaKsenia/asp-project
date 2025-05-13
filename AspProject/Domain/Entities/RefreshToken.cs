using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspProject.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }
    
    public bool IsRevoked { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}