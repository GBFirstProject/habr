using FirstProject.AuthAPI.Data;
using FirstProject.AuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.Encodings.Web;
using System.Text;
using System.Linq;

namespace FirstProject.AuthAPI.Articles.Data
{
    public class MigrationService : IMigrationService
    {
        private readonly ArticlesDBContext _articlesDbContext;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private CancellationTokenSource _cancellationTokenSource;

        public MigrationService(ArticlesDBContext articlesDbContext, 
                                ApplicationDbContext applicationDbContext, 
                                SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager)
        {
            _articlesDbContext = articlesDbContext;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<int> MigrateAuthorsToUsersAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            /*if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _userManager.AddToRoleAsync(user, Config.User);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {


                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }*/
            try
            {
                var authors = _articlesDbContext.Authors.ToList();
                var users = new List<ApplicationUser>();
                var migratedAuthors = 0;

                for (var i = 0; i < authors.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var author = authors[i];

                    var existAuthor = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == $"{author.NickName}@gmail.com");
                    if (existAuthor != null)
                    {
                        /*var articlesToUpdate = _articlesDbContext.Articles.Where(a => a.AuthorId == author.Id).ToList();
                        //author.Id = Guid.Parse(existAuthor.Id);
                        foreach (var article in articlesToUpdate)
                        {
                            article.AuthorId = Guid.Parse(existAuthor.Id);
                            var authorN = _articlesDbContext.Authors.Find(existAuthor.Id);
                            authorN.Articles.Add(article);
                        }*/
                        continue;
                    }
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = $"{author.NickName}@gmail.com",
                        Email = $"{author.NickName}@gmail.com",
                        FirstName = author.FirstName,
                        LastName = author.LastName,                    
                    };                
                    var result = await _userManager.CreateAsync(user, "Admin123*");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Config.User);
                        migratedAuthors++;
                    }

                    users.Add(user);

                    // Обновляем AuthorId у всех статей автора
                    var articles = _articlesDbContext.Articles.Where(a => a.AuthorId == author.Id).ToList();
                    author.Id = Guid.Parse(user.Id);
                    foreach (var article in articles)
                    {
                        article.AuthorId = Guid.Parse(user.Id);
                    }
                    
                    _articlesDbContext.SaveChanges();

                    // Обновляем прогресс выполнения
                    var percentage = (double)Math.Floor((double)(i + 1) / authors.Count * 100);
                    progress?.Report(percentage);
                }

                // Сохраняем изменения в базе данных
                //await _articlesDbContext.SaveChangesAsync(cancellationToken);
                //await _applicationDbContext.SaveChangesAsync(cancellationToken);
                

                
                return migratedAuthors;
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                _cancellationTokenSource = new CancellationTokenSource();
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                _cancellationTokenSource = new CancellationTokenSource();
                throw new MigrationException("Migration failed.", ex);
            }
        }
        public void CancelMigration()
        {
            _cancellationTokenSource.Cancel();
        }
    }

}
