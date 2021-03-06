﻿
namespace ContosoUniversity.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ContosoUniversity.DataAccess;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Infrastructure;

    public class ContosoUniversityTestContext : ContosoUniversityContext
    {
        public event Action ChangesSaved = delegate { };

        public ContosoUniversityTestContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            ChangesSaved();
            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangesSaved();
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = base.SaveChangesAsync(cancellationToken);
            ChangesSaved();
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangesSaved();
            return result;
        }
    }
}
