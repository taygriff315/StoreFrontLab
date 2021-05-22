using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreFrontLab.UI.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Hey buddy, the name field is required")]
        public string Name { get; set; }

        public string Subject { get; set; }
        [Required(ErrorMessage = "Hey buddy, the message is required")]
        [UIHint("MultilineText")] //Creates the input field as a TextArea rather than a textbox
        public string Message { get; set; }
        [Required(ErrorMessage = "Hey buddy, the email address is required")]
        [Display(Name = "Your Email")]
        [EmailAddress(ErrorMessage = "Email address was an improper format")]
        public string EmailAddress { get; set; }
    }
}