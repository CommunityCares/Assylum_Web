using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Campaign")]
public partial class Campaign
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; } 

    [StringLength(50)]
    [Unicode(false)]
    public string? Requirement { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InitialDate { get; set; }= DateTime.Now;

    [Column(TypeName = "datetime")]
    public DateTime CloseDate { get; set; }=DateTime.Now;

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    public byte IdAssylum { get; set; }

    [InverseProperty("IdCampaignNavigation")]
    public virtual ICollection<Collect> Collects { get; set; } = new List<Collect>();

   
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    [ForeignKey("IdAssylum")]
    [InverseProperty("Campaigns")]
    public virtual Assylum? IdAssylumNavigation { get; set; } = null!;

    [InverseProperty("IdCampaignNavigation")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
