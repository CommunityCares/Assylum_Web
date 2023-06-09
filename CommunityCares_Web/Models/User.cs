using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    //[Unicode(false)]
    public string? Password { get; set; } 

    //[StringLength(50)]
    //[Unicode(false)]
    public string? Role { get; set; } 

    //[StringLength(150)]
    //[Unicode(false)]
    public string? Email { get; set; } 

    [ForeignKey("Id")]
    [InverseProperty("User")]
    public virtual Person? IdNavigation { get; set; } 
}
