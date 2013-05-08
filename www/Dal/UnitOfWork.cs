using System;
using System.Data.Entity.Infrastructure;
using www.Models;

namespace www.Dal
{
    public class UnitOfWork : IDisposable
    {
        private readonly SubCrmContext _context = new SubCrmContext();
        private GenericRepository<Submission> _submissionRepository;

        public GenericRepository<Submission> SubmissionRepository
        {
            get { return _submissionRepository ?? (_submissionRepository = new GenericRepository<Submission>(_context)); }
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.Detach(entity);
        }
    }
}
