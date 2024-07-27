using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace cinema_web_app.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    // Additional properties
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    // Navigation properties
    public virtual ICollection<ApplicationAdmin> ApplicationAdmins { get; set; }
    public virtual ICollection<ContentCinemaAdmin> ContentCinemaAdmins { get; set; }
    public virtual ICollection<ContentAppAdmin> ContentAppAdmins { get; set; }
    public virtual ICollection<Customer> Customers { get; set; }
}