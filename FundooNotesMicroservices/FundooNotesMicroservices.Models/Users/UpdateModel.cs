using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooNotesMicroservices.Models
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        public string Color { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        public string Image { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        public DateTime ModifiedAt { get; set; }
    }
}
