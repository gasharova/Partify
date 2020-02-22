using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Partify.ViewModels.Invites
{
    public class CreateVM
    {
        [DisplayName("Username of person to be invited: ")]
        [Required(ErrorMessage = "This field is required!")]
        public string ReceiverUsername { get; set; }
        public int PartyId { get; set; }
    }
}