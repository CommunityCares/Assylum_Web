using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Collect")]
public partial class Collect
{
    [Key]
    public int Id { get; set; }

    [Column("date", TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("idCampaign")]
    public int IdCampaign { get; set; }

    [ForeignKey("IdCampaign")]
    [InverseProperty("Collects")]
    public virtual Campaign? IdCampaignNavigation { get; set; } = null!;
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
