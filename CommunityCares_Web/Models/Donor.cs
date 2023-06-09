using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Donor")]
public partial class Donor
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Region { get; set; } = null!;

    
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    [ForeignKey("Id")]
    [InverseProperty("Donor")]
    public virtual Person? IdNavigation { get; set; } = null!;
}
