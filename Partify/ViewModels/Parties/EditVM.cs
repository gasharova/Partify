using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Partify.ViewModels.Parties
{
    public class EditVM
    {
        public int Id { get; set; }

        [DisplayName("Title: ")]
        [Required(ErrorMessage = "This field is required!")]
        public string Title { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "This field is required!")]
        public string Description { get; set; }
        public string LastName { get; set; }
    }
}