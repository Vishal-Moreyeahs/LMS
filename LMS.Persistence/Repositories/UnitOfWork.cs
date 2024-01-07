using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Persistence.Repositories
{
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _dbContext;
        public IUserRepository Admin { get; private set; }

        public ICompanyRepository InvoiceMaster { get; private set; }

        public UnitOfWork(DataBaseContext context)
        {
            _dbContext = context;
        }

        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }
    }
}