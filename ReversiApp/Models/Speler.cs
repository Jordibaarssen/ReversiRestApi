using Microsoft.AspNetCore.Identity;
using ReversiApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class Speler : IdentityUser
    {
      
        public string Token { get; set; }

        public Kleur Kleur { get; set; }


    }
}
