using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hMailServer;

namespace hMailAddAccount
{
    // https://www.codeproject.com/Articles/1101963/Tips-on-hMailServer-Manipulation-using-Csharp
    // Add reference to hMailserver Server COM Library

    class Program
    {
        // Authenticate
        private static hMailServer.Application Authenticate(string userName,string password)
        {
            hMailServer.Application hMailApp = new hMailServer.Application();
            if(hMailApp != null)
                hMailApp.Authenticate(userName, password);
            return hMailApp;
        }


        // Create new domain
        private static bool DomainCreate(string user, string pass, string domainName) {
            hMailServer.Application hMailApp = Authenticate(user, pass);
            hMailServer.Domain domain = hMailApp.Domains.Add();
            domain.Name = domainName;
            domain.Active = true;
            domain.Save();
            return true;
        }

        // Domain activation or deactivation
        private static bool DomainActivate(string userName, string password, string domainName, bool active = true)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            myDomain.Active = active;
            myDomain.Save();
            return true;
        }

        // Domain delete with all mailbox accounts
        private static bool DomainDelete(string userName, string password, string domainName)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Accounts hAccounts = myDomain.Accounts;

            for (var account = 0; account < hAccounts.Count; account++)
            {
                try
                {
                    var accountInfo = hAccounts.get_ItemByDBID(account);
                    myDomain.Accounts.DeleteByDBID(accountInfo.ID);
                }
                catch (System.Exception)
                { }
            }
            myDomain.Delete();
            return true;
        }
        

        // Create mailbox, Account: mailbox@domain.com
        private static bool AccountCreate(string userName, string password, string domainName, string accountAddress, string accountPassword, bool accountActive = true, int maxSize = 0) {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            if (myDomain != null)
            {
                hMailServer.Account account = myDomain.Accounts.Add();
                account.Address = accountAddress;
                account.Password = accountPassword;
                account.Active = accountActive;
                account.MaxSize = maxSize;
                account.Save();
                return true;
            }
            else
                return false;
        }

        // Delete account mailbox
        private static bool AccountDelete(string userName, string password, string domainName, string accountAddress)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Account account = myDomain.Accounts.ItemByAddress[accountAddress];
            myDomain.Accounts.DeleteByDBID(account.ID);
            return true;
        }

        // Delete all accounts from domain
        private static bool DeleteAllAccounts(string userName, string password, string domainName)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Accounts hAccounts = myDomain.Accounts;

            for (var account = 0; account < hAccounts.Count; account++)
            {
                var accountInfo = hAccounts.get_ItemByDBID(account);
                myDomain.Accounts.DeleteByDBID(accountInfo.ID);
            }
            return true;
        }

        // Change Account mailbox password
        private static bool ChangAccountPassword(string userName, string password, string domainName, string accountAddress, string newPassword)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Account account = myDomain.Accounts.ItemByAddress[accountAddress];
            account.Password = newPassword;
            myDomain.Save();
            return true;
        }

        static void Main(string[] args)
        {
            // Create new domain
            DomainCreate("Administrator", "Pass", "zenek.xx");

        }
    }
}
