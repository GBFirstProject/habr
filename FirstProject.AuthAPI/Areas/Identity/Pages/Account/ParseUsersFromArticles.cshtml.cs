using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Threading;
using System;
using Duende.IdentityServer.Models;
using FirstProject.AuthAPI.Articles.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account
{
    public class ParseUsersFromArticlesModel : PageModel
    {
        private readonly IMigrationService _migrationService;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ArticlesDBContext _articlesDbContext;

        public ParseUsersFromArticlesModel(IMigrationService migrationService, ArticlesDBContext articlesDbContext)
        {
            _migrationService = migrationService;
            _cancellationTokenSource = new CancellationTokenSource();
            _articlesDbContext = articlesDbContext;
        }

        public bool IsMigrating { get; set; }
        public int ProgressPercentage { get; set; }
        public string MigrationResult { get; set; } = string.Empty;
        public bool IsInProgress { get; set; }
        [BindProperty]
        public double Progress { get; set; }
        public int UserCountInArticles { get; set; }

        public async Task OnGetAsync()
        {
            UserCountInArticles = _articlesDbContext.Authors.ToListAsync().Result.Count;
            Progress = 0;
            IsInProgress = false;
            MigrationResult = null;
        }
        
        //public async Task<IActionResult> OnPostMigrateAsync()
        public async Task<IActionResult> OnPostAsync()
        {
            IsInProgress = true;
            Progress = 0;

            try
            {
                var progress = new Progress<double>(percentage => this.Progress = percentage);
                var migrationResult = await _migrationService.MigrateAuthorsToUsersAsync(progress,
                    cancellationToken: HttpContext?.RequestAborted ?? CancellationToken.None);

                MigrationResult = $"Migration completed successfully. {migrationResult} users were created.";
            }
            catch (OperationCanceledException)
            {
                MigrationResult = "Migration was canceled.";
            }
            catch (Exception ex)
            {
                MigrationResult = $"Migration failed with error: {ex.Message}.";
            }

            IsInProgress = false;

            return Page();
        }

        public IActionResult Cancel()
        {
            _migrationService.CancelMigration();
            IsInProgress = false;
            Progress = 0;
            MigrationResult = "Migration was canceled.";

            return Page();
        }

        /*public async Task MigrateAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            IsMigrating = true;

            try
            {
                var progress = new Progress<int>(percentage =>this.ProgressPercentage = percentage);
                await _migrationService.MigrateAuthorsToUsersAsync(progress, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // The operation was cancelled, do any necessary cleanup
            }

            IsMigrating = false;
            ProgressPercentage = 0;
        }

        public async Task CancelAsync()
        {
            _cancellationTokenSource.Cancel();
            IsMigrating = false;
            ProgressPercentage = 0;
            await Task.CompletedTask;
        }*/
    }
}