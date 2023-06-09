using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Assylum")]
public partial class Assylum
{
    [Key]
    public byte Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Nit { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string RepresentativeName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string BussinessEmail { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CellphoneNumber { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Region { get; set; } = null!;

    [InverseProperty("IdAssylumNavigation")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("IdAssylumNavigation")]
    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
}
