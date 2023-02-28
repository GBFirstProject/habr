using FirstProject.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using FirstProject.AuthAPI.Articles.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading;
using System;
using IdentityModel;
using System.Security.Claims;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account
{
    public class RapseArticleUsersModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ArticlesDBContext _articlesDbContext;

 
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }               
        public int UserCountInArticles { get; set; }        
        public bool IsParsing { get; set; }
        public int ProgressPercent { get; set; }
        public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public RapseArticleUsersModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ArticlesDBContext articlesDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _articlesDbContext = articlesDbContext;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            UserCountInArticles = _articlesDbContext.Authors.ToListAsync().Result.Count;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }       

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach (var author in _articlesDbContext.Authors)
            {
                if (_userManager.Users.FirstOrDefault(u => u.UserName == author.NickName) != null)
                {
                    continue;
                }

                var user = new ApplicationUser 
                { 
                    UserName = $"{author.NickName}@gmail.com", 
                    Email = $"{author.NickName}@gmail.com",
                    FirstName = author.FirstName,
                    LastName = author.LastName
                };
                var result = await _userManager.CreateAsync(user, "Admin123*");
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, Config.User);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    /*var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);*/

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }                

                var resUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == author.NickName);
                var articleToUpdate = await _articlesDbContext.Articles.FirstOrDefaultAsync(a => a.AuthorId == author.Id);
                if(articleToUpdate != null)
                {
                    articleToUpdate.AuthorId = System.Guid.Parse(resUser.Id);
                    _articlesDbContext.Articles.Update(articleToUpdate);                    
                }                
            }
            await _articlesDbContext.SaveChangesAsync();
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
