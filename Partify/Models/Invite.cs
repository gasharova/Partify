using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Partify.Models
{
    public class Invite
    {
        [Key]
        public int Id { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public Party Party { get; set; }
    }
}