using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{

    public class DescriptionViewModel
    {

        [Required]
        public string Description { get; set; }

    }

    public class TitleViewModel
    {
        [Required]
        public string Title { get; set; }

    }

    public class NameViewModel
    {
        [Required]
        public string Name { get; set; }

    }


    public class BudgetViewModel
    {

        [Required]
        public int MinBudget { get; set; }

    }


    public class LocationViewModel
    {

        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

    }

}