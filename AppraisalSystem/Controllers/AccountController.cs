using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using AppraisalSystem.Models;
using AppraisalSystem.Providers;
using AppraisalSystem.Results;
using AppraisalSln.Models;
using RepositoryPattern;
using Appraisal.BusinessLogicLayer.Employee;

namespace AppraisalSystem.Controllers
{

    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser() { UserName = model.EmployeeId, Email = model.Email, EmailConfirmed = true };

            string password = UniqueNumbers.GeneratePassword();

            try
            {
                IdentityResult result = await UserManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var res = await UserManager.AddToRoleAsync(user.Id, model.RoleName);
                    EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
                    employees.CreatedBy = User.Identity.GetUserName();

                    employees.Save(new Employee { EmployeeId = model.EmployeeId, EmployeeName = model.EmployeeName, DesignationId = model.DesignationId, SectionId = model.SectionId, Location = model.Location, ReportTo = model.ReportTo, JoiningDate = model.JoiningDate, Email = model.Email, groups = model.groups });
                }

                var employee = await UserManager.FindByNameAsync(model.EmployeeId);

                if (employee?.Email != null)
                {
                    var url = @"/#/login";
                    await UserManager.SendEmailAsync(employee.Id, "Regisatration Confirmation", "Mr/s " + model.EmployeeName + "<br/> Your Registration is complete. Your password is: "+password+". please click <a href=\"" + new Uri(url) + "\">here</a> to login");
                }
                return Ok("The Employee Password is " + password + " and Role is " + UserManager.GetRoles(user.Id).SingleOrDefault());

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Route("RecoveryPassword")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> RecoveryPassword(SetPasswordBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.EmployeeId);
                if (user == null)
                {
                    return BadRequest("User is not exist");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var result = await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password reset Successfully");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                }
            }
            return BadRequest(ModelState);
        }

        [Route("ForgotPassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.EmployeeId);
                if (user == null)
                {
                    return BadRequest("Sorry! Employee Id is not exist");
                }
                if (user.Email == null)
                {
                    return BadRequest("Sorry! You have no mail attach. Please contact admin to recover Password");
                }
                try
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    code = WebUtility.UrlEncode(code);
                    var url = "/#/ResetPassword?id=" + user.Id + "&code=" + code;

                    await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + new Uri(url) + "\">here</a>");
                    return Ok("Please check your Email and recovery Password");
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }

            return BadRequest("Employee Id is required");
        }

        [Route("ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.id);

                if (user == null)
                {
                    return BadRequest("User is not exists");
                }


                var result = await UserManager.ResetPasswordAsync(user.Id, model.code, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password Reset Successfully");

                }
                else
                {
                    BadRequest("Code or id is incorrect plz resend again");
                }
            }
            return BadRequest("Internal Server Problem");
        }

        [Route("UpdateEmployeeRole")]
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IHttpActionResult> UpdateRole(UpdateRole role)
        {
                var user = await UserManager.FindByNameAsync(role.EmployeeId); //UserManager.FindById(role.EmployeeId);
                var oldRoleName = UserManager.GetRoles(user.Id).FirstOrDefault();

                if (oldRoleName.Equals(role.Role))
                {
                    return BadRequest("The Role is already assign to this employee");
                }

                UserManager.RemoveFromRole(user.Id, oldRoleName);
                UserManager.AddToRole(user.Id, role.Role);

                return Ok();
        }


        [Route("SetLocked")]
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IHttpActionResult> SetLockedUser(LockModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.EmployeeId);

                if (user == null)
                {
                    return BadRequest("User is not Exist");
                }

                user.isLocked = model.isLocked;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok("Successfully locked User");
                }

                return BadRequest("Something is Problem");
            }
            return BadRequest("Internal Server Error");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
