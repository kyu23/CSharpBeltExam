using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistrationLogin.Models
{
    public class WatchParty
    {
        [Key]
        public int WatchPartyId { get; set; }

        public int UserId  {get; set;}
        public int ActivityId { get; set; }

        public User ActivityGoer { get; set;}
        public Activity Feature { get; set; }

    }
}