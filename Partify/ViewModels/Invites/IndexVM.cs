using Partify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Partify.ViewModels.Invites
{
    public class IndexVM
    {
        public Party[] Items { get; set; }
        public List<string>[] Invited { get; set; }
    }
}