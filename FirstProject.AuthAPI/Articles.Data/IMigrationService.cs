using System.Threading.Tasks;
using System.Threading;
using System;

namespace FirstProject.AuthAPI.Articles.Data
{
    public interface IMigrationService
    {
        public Task<int> MigrateAuthorsToUsersAsync(IProgress<double> progress, CancellationToken cancellationToken);
        public void CancelMigration();
    }
}
