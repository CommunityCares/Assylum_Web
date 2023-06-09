using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Donation")]
public partial class Donation
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? DescriptionItems { get; set; }

    [Column(TypeName = "decimal(10, 0)")]
    public decimal? DescriptionMonto { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? IsAnonymus { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? IsReceived { get; set; }

    public int IdCollect { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Hour { get; set; } = null!;

    public int IdCampaign { get; set; }

    public int IdDonnors { get; set; }

 
    
    public virtual Campaign? IdCampaignNavigation { get; set; } = null!;
    
  
    public virtual Collect? IdCollectsNavigation { get; set; } = null!;

    
    public virtual Donor? IdDonnorsNavigation { get; set; } = null!;
}
