using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MembershipWebsite.Entities
{
    [Table("SubscriptionProduct")]
    public class SubscriptionProduct
    {
        // composite primary key of ProductId and ItemId
        [Required]
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        [Required]
        [Key, Column(Order = 2)]
        public int SubscriptionId { get; set; }

        // used only to send data from server to client
        // are not part of the table
        [NotMapped]
        public int OldProductId { get; set; }

        [NotMapped]
        public int OldSubscriptionId { get; set; }

    }
}