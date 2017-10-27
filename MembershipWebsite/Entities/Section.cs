using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MembershipWebsite.Entities
{
    [Table("Section")]
    public class Section
    {
        // Makes it required and causes automatic incrementing when adding rows to the table
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

    }
}