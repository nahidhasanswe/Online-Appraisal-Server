using System;
using System.ComponentModel.DataAnnotations;

namespace AppraisalSystem.Models
{
    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "EmployeeId")]
        public string EmployeeId { get; set; }
        [Required]
        [Display(Name = "EmployeeName")]
        public string EmployeeName { get; set; }
        [Required]
        [Display(Name = "Section")]
        public Guid SectionId { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public Guid DesignationId { get; set; }

        [Required]
        [Display(Name = "JoiningDate")]
        public DateTime JoiningDate { get; set; }
        [Required]
        [Display(Name = "ReportTo")]
        public string ReportTo { get; set; }
        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string groups { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        public string EmployeeId { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
      

    }

    public class LockModel
    {
        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public bool isLocked { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [Display(Name = "EmployeeId")]
        public string EmployeeId { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateRole
    {
        public string EmployeeId { get; set; }
        public string Role { get; set; }
    }
}