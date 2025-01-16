using GrowthTracker.Repositories;
using GrowthTracker.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthTracker.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccountsAsync();
        Task<Account> GetAccountByIdAsync(string id);
        Task<int> UpdateAccountAsync(Account account);
        Task<int> CreateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(string id);
        Task<Account> LoginAccount(string username, string password);
    }
    public class AccountService : IAccountService
    {
        private readonly AccountRepository accountRepository;
        public AccountService()
        {
            this.accountRepository = new AccountRepository(); 
        }
        public async Task<int> CreateAccountAsync(Account account)
        {
            return await accountRepository.CreateAsync(account);
        }

        public async Task<bool> DeleteAccountAsync(string id)
        {
            var account = accountRepository.GetById(id);
            if (account != null)
            {
                return await accountRepository.RemoveAsync(account);
            }
            return false;
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            return await accountRepository.GetByIdAsync(id);
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            return await accountRepository.GetAllAsync();
        }

        public async Task<Account> LoginAccount(string username, string password)
        {
            return await accountRepository.LoginAccount(username, password);
        }

        public async Task<int> UpdateAccountAsync(Account account)
        {
            var upAccount = accountRepository.GetById(account.AccountId);
            if (account == null)
            {
                throw new Exception();
            }
            return await accountRepository.UpdateAsync(upAccount);
        }
    }
}
