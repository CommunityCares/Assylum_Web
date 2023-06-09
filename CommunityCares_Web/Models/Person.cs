using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Person")]
public partial class Person
{
    [Key]
    public int Id { get; set; }

    //[StringLength(60)]
    //[Unicode(false)]
    public string? Name { get; set; }

    //[StringLength(50)]
    //[Unicode(false)]
    public string? LastName { get; set; } 

    //[StringLength(50)]
    //[Unicode(false)]
    public string? SecondLastName { get; set; }

    public byte Status { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }= DateTime.UtcNow;

    //[StringLength(20)]
    //[Unicode(false)]
    public string? Ci { get; set; } 

    //[StringLength(15)]
    //[Unicode(false)]
    public string? PhoneNumber { get; set; } 

    [InverseProperty("IdNavigation")]
    public virtual Admin? Admin { get; set; } 

    [InverseProperty("IdNavigation")]
    public virtual Donor? Donor { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual User? User { get; set; }
}
