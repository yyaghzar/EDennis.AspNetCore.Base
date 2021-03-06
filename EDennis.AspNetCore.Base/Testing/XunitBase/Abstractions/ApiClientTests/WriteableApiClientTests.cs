﻿using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class WriteableApiClientTests<TClient,TStartup>: IDisposable
        where TStartup : class
        where TClient: ApiClient {

        protected ITestOutputHelper Output { get; }
        protected string InstanceName { get; }
        protected HttpClient HttpClient { get; }

        protected TClient ApiClient { get; }

        public WriteableApiClientTests(ITestOutputHelper output,
                ApiLauncherFactory<TStartup> fixture,
                string testUser = "tester@example.org") {

            Output = output;
            ApiClient = TestApiClientFactory.CreateWriteableClient<TClient, TStartup>(fixture, testUser);
            HttpClient = ApiClient.HttpClient;
            InstanceName = ApiClient.GetInstanceName();

        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    HttpClient.SendResetAsync(Interceptor.HDR_DROP_INMEMORY, InstanceName);
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

