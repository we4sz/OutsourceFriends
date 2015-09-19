using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class ForgotPasswordCode
    {
        [Key]
        [Required]
        public string UserId { get; set; }


        [Required]
        public string Code { get; set; }

        [Required]
        public Int64 Created { get; set; }

    }
}