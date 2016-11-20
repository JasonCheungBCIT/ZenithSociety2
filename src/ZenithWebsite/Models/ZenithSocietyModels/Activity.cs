using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenithWebsite.Models.ZenithSocietyModels
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]   // Note?(1/2): Making this DataType.Date will change editor controls too! (so not just a text box)
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm}")]    // Note?(2/2): however hh:mm does not show sadly. 
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }  
    }
}
