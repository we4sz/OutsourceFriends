using OutsourceFriends;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace OutsourceFriends.Models
{


    public class RegisterBindingModelAPI : RegisterBindingModel {
        public bool? isApplier { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = ApplicationUserManager.PasswordLength)]
        [DataType(DataType.Password)]
        [Display]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
    }


    public class SetPasswordBindingModelId : SetPasswordBindingModel
    {
        public string Id { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required(ErrorMessageResourceName = "required")]
        [StringLength(100, MinimumLength = ApplicationUserManager.PasswordLength)]
        [DataType(DataType.Password)]
        [Display]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }


    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }





    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display]
        public string Email { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display]
        public string Email { get; set; }

        [Required]
        [Display]
        public string Code { get; set; }
    }

    public class ResendEmailConfirmViewModel
    {
        [Required]
        [EmailAddress]
        [Display]
        public string Email { get; set; }

    }

    public class FacebookTokenViewModel
    {
        [Required]
        [Display]
        public string Token { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = ApplicationUserManager.PasswordLength)]
        [DataType(DataType.Password)]
        [Display]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }


    public class LoginViewModel
    {
        [EmailAddress]
        [Display]
        public string Email { get; set; }

        [Display]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display]
        public bool RememberMe { get; set; }

    }


    public class LoginFacebookViewModel
    {
        [Required]
        public string Token { get; set; }

        public ProfileType ProfileType { get; set; }

    }






    public class UserMeViewModel
    {

        public Int64 Registered { get; set; }

        public string Email { get; set; }

        public ICollection<string> Roles { get; set; }

        public bool HasPassword { get; set; }

        public string PhoneNumber { get; set; }

        public string Id { get; set; }

        [MaxLength(3)]
        public string UserLanguage { get; set; }

        [MaxLength(3)]
        public string Currency { get; set; }

    }

}
