using System.ComponentModel.DataAnnotations;

namespace web1.Models
{
    public class OrderDetailModel
    {
        [Required(ErrorMessage = "Firstname is required")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [RegularExpression("[a-öA-Ö 0-9]+", ErrorMessage = "Address is invalid")]
        public string PostAddress { get; set; }

        [Required(ErrorMessage = "Post code is required")]
        [Display(Name = "Post code")]
        [RegularExpression("[0-9]+", ErrorMessage = "Post code is invalid")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Post town is required")]
        [Display(Name = "Post town")]
        [RegularExpression("[a-öA-Ö ]+", ErrorMessage = "Post town is invalid")]
        public string PostTown { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email address is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Phone number is invalid")]
        public string PhoneNumber { get; set; }
    }
}