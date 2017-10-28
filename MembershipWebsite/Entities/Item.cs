using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipWebsite.Entities
{
    [Table("Item")]
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [MaxLength(1024)]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string HTML { get; set; }

        // users don't have to wait to access content unless otherwise specified
        [DefaultValue(0)]
        [DisplayName("Wait Days")]
        public int WaitDays { get; set; }

        public string HTMLShort
        {
            get
            {
                return HTML == null || HTML.Length < 50 ?
                  HTML : HTML.Substring(0, 50);
            }
        }

        [DisplayName("Product Id")]
        public int ProductId { get; set; }

        [DisplayName("Item Type Id")]
        public int ItemTypeId { get; set; }

        [DisplayName("Section Id")]
        public int SectionId { get; set; }

        [DisplayName("Part Id")]
        public int PartId { get; set; }

        [DisplayName("Is Free")]
        public bool IsFree { get; set; }

        [DisplayName("Item Type")]
        public ICollection<ItemType> ItemTypes { get; set; }

        [DisplayName("Sections")]
        public ICollection<Section> Sections { get; set; }

        [DisplayName("Parts")]
        public ICollection<Part> Parts { get; set; }




    }
}