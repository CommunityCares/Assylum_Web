using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Admin")]
public partial class Admin
{
    [Key]
    public int Id { get; set; }

    public byte IdAssylum { get; set; }

    [ForeignKey("IdAssylum")]
    [InverseProperty("Admins")]
    public virtual Assylum? IdAssylumNavigation { get; set; } 

    [ForeignKey("Id")]
    [InverseProperty("Admin")]
    public virtual Person? IdNavigation { get; set; } 
}
