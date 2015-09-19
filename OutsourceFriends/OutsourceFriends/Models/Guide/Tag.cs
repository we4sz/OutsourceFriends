using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourceFriends.Models
{
    public class Tag
    {
        [Key]
        [Column(Order = 0)]
        public string GuidId { get; set; }

   
        public virtual Guid Guid { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }
    }
}