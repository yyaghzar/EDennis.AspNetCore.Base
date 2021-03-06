﻿using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class WriteableRepoTests<TRepo, TEntity, TContext> 
        : IClassFixture<ConfigurationFactory<TRepo>>, IDisposable
        
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : WriteableRepo<TEntity, TContext> 
        
        {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected string InstanceName { get; }


        public WriteableRepoTests(ITestOutputHelper output, 
            ConfigurationFactory<TRepo> fixture,
            string testUser = "tester@example.org") {

            Output = output;
            Repo = TestRepoFactory.CreateWriteableRepo<TRepo,TEntity,TContext,TRepo>(fixture,testUser) as TRepo;
            InstanceName = Repo.GetInstanceName();

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    TestDbContextManager<TContext>.DropInMemoryDatabase(Repo.Context);
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
