using System;
using Moq;

namespace RomansShop.Tests.Common
{
    public abstract class UnitTestBase : IDisposable
    {
        protected UnitTestBase()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }

        protected MockRepository MockRepository { get; }

        public void Dispose()
        {
            VerifyAll();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void VerifyAll()
        {
            MockRepository.VerifyAll();
        }
    }
}