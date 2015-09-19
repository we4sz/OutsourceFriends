using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OutsourceFriends.Models
{
    public class Guide
    {
        [Index(IsUnique = true)]
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Index]
        [MaxLength(256)]
        public string Name { get; set; }


        [MaxLength(256)]
        public string CurrentTitle { get; set; }

        [Index]
        [DefaultValue(false)]
        public bool Removed { get; set; }

        [Index]
        public DateTime? UpdatedTime { get; set; }

        [Index]
        public DateTime? LastActive { get; set; }

        [Index]
        public int Age { get; set; }


        [Index]
        [MaxLength(256)]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime? CreatedTime { get; set; }

        public DbGeography Location { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(2)]
        public string CreatedInLanguage { get; set; }

        public Guide()
        {
        }

        public void beforeSave(bool fullyLoaded)
        {
            UpdatedTime = DateTime.UtcNow;
        }

    }
}