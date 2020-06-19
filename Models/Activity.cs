using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistrationLogin.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required(ErrorMessage="Title is required")]
        [MinLength(2,ErrorMessage="Title must be at least 2 characters")]
        public string Title { get;set; }

        [Required(ErrorMessage="Description is required!")]
        public string Description { get; set; }

        [Required(ErrorMessage="Date is required!")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage="Time is required!")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage="Duration is required!")]
        public int Duration { get; set; }
        
        //public int NumParticipants { get; set; }

        public int UserId  {get;set;}
        public User Organizer { get; set; }

        public List<WatchParty> Guests { get; set; }

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}