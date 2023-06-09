using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityCares_Web.Models;

[Table("Image")]
public partial class Image
{
    [Key]
    public int Id { get; set; }

    

    [StringLength(200)]
    [Unicode(false)]
    public string? Url { get; set; } 

    public int? IdCampaign { get; set; }
    
    [NotMapped]
    public IFormFile? ImagenFile { get; set; } 

    [ForeignKey("IdCampaign")]
    [InverseProperty("Images")]
    public virtual Campaign? IdCampaignNavigation { get; set; }
}
