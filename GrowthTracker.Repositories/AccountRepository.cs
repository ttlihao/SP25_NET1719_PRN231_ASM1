using GrowthTracker.Repositories.Base;
using GrowthTracker.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthTracker.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository() { }
        
        public async Task<Account> LoginAccount(string username, string password)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.Username == username && u.Password == password); //if exist isActive, u.isActive == true

            //return await _context.Accounts.FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode && u.Password == password); //if exist isActive, u.isActive == true and if change userName, change it
        }
    }
}
