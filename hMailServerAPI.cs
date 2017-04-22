using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hMailServer;

//Add MySql Library
using MySql.Data.MySqlClient;


// https://www.codeproject.com/Articles/1101963/Tips-on-hMailServer-Manipulation-using-Csharp
// Add reference to hMailserver Server COM Library

namespace hMailAddAccount
{
    class Program
    {
        

        private MySqlConnection Conn(){                                
            string connectionString = "SERVER=localhost;DATABASE=hmail;UID=rooot;PASSWORD=toor;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }


        // INSERT INTO `hm_domains` (`domainid`, `domainname`, `domainactive`, `domainpostmaster`, `domainmaxsize`, `domainaddomain`, `domainmaxmessagesize`, `domainuseplusaddressing`, `domainplusaddressingchar`, `domainantispamoptions`, `domainenablesignature`, `domainsignaturemethod`, `domainsignatureplaintext`, `domainsignaturehtml`, `domainaddsignaturestoreplies`, `domainaddsignaturestolocalemail`, `domainmaxnoofaccounts`, `domainmaxnoofaliases`, `domainmaxnoofdistributionlists`, `domainlimitationsenabled`, `domainmaxaccountsize`, `domaindkimselector`, `domaindkimprivatekeyfile`) VALUES (NULL, 'breakermind.com', '1', '', '0', '', '0', '0', '', '0', '0', '1', '', '', '0', '0', '0', '0', '0', '0', '0', '', '');
        // INSERT INTO `hm_accounts` (`accountid`, `accountdomainid`, `accountadminlevel`, `accountaddress`, `accountpassword`, `accountactive`, `accountisad`, `accountaddomain`, `accountadusername`, `accountmaxsize`, `accountvacationmessageon`, `accountvacationmessage`, `accountvacationsubject`, `accountpwencryption`, `accountforwardenabled`, `accountforwardaddress`, `accountforwardkeeporiginal`, `accountenablesignature`, `accountsignatureplaintext`, `accountsignaturehtml`, `accountlastlogontime`, `accountvacationexpires`, `accountvacationexpiredate`, `accountpersonfirstname`, `accountpersonlastname`) VALUES (NULL, '8', '0', 'info@breakermind.com', '09598b56eddb0b17ca5e9585d70e989f343dc58706fdfd95fceba397f18dd100f084b5', '1', '0', '', '', '0', '0', '', '', '3', '0', '', '0', '0', '', '', '2017-04-06 19:45:44', '0', '2017-04-03 00:00:00', '', '');

        private void GetAccounts(MySqlConnection conn){
            
            conn.Open();
            string query = "SELECT * FROM NewsletterAccounts WHERE Name=@Name1";            
            
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name1", "Michikolta");

            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            while (dataReader.Read())
            {
                string account = dataReader["id"].ToString() + dataReader["name"] + dataReader["age"];
            }

            //cmd.ExecuteNonQuery();
            dataReader.Close();
            conn.Close();
        }


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
            domain.Postmaster = "catchall@" + domainName; // add catch all address, mailbox
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

        // Domain set catchall mailbox
        private static bool DomainActivateCatchAll(string userName, string password, string domainName, string mailboxEmail = "catchall", bool active = true)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            myDomain.Active = active;
            myDomain.Postmaster = mailboxEmail + "@" + domainName;
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
        private static bool AccountCreate(string userName, string password, string domainName, string accountAddress, string accountPassword, bool accountActive = true, int maxSize = 1000) {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            if (myDomain != null)
            {
                hMailServer.Account account = myDomain.Accounts.Add();
                account.Address = accountAddress;
                account.Password = accountPassword;
                account.Active = accountActive;
                account.MaxSize = maxSize;
                account.Active = true;
                account.Save();
                return true;
            }
            else
                return false;
        }

        // Deactivate account
        private static bool AccountDeactivate(string userName, string password, string domainName, string accountAddress, bool active = false)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Account account = myDomain.Accounts.ItemByAddress[accountAddress];
            account.Active = active;
            account.Save();
            return true;
        }

        // Deactivate account
        private static bool AccountActivate(string userName, string password, string domainName, string accountAddress, bool active = true)
        {
            hMailServer.Application hMailApp = Authenticate(userName, password);
            hMailServer.Domain myDomain = hMailApp.Domains.ItemByName[domainName];
            hMailServer.Account account = myDomain.Accounts.ItemByAddress[accountAddress];
            account.Active = active;
            account.Save();
            return true;
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

            try
            {
                // Create new domain
                DomainCreate("Administrator", "xxxx", "xxxxx.xx");

            }catch(Exception e){
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }

        }
    }
}
