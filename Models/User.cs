using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationLogin.Models
{
    public class User
    {
        
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(2)]
        public string Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        
        [Required]
        [MinLength(8)]
        [DataType("Password")]
        public string Password {get;set;}

        // We use the NotMapped Annotation so that this variable doesn't end up in our database.
        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType("Password")]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Activity> MyActivities { get; set; }
        public List<WatchParty> MyParties { get; set; }
        
        
        
            
    }
}