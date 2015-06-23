using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEATS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
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

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:

        public EditUserViewModel(ApplicationUser user)
        {
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.Id = user.Id;
        }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Id { get; set; }

    }


    public class SelectUserRolesViewModel
    {

        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        // Enable initialization with an instance of ApplicationUser:

        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            
            this.Email = user.Email;
            this.Username = user.UserName;
            this.Id = user.Id;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:

            var allRoles = Db.Roles;

            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 

            // which the current user is a member:

            foreach (var userRole in user.Roles)
            {
                var checkUserRole =
                    this.Roles.Find(r => r.RoleId == userRole.RoleId);

                checkUserRole.Selected = true;
            }

        }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }

    }

    // Used to display a single role with a checkbox, within a list structure:

    public class SelectRoleEditorViewModel
    {

        public SelectRoleEditorViewModel() { }

        public SelectRoleEditorViewModel(IdentityRole role)
        {

            this.RoleName = role.Name;
            this.RoleId = role.Id;

        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

    }

}
