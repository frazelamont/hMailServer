<?php

// Connect to hMailServer !!!!!
// See PHPWebAdmin folder in hmail server installation !!!!!

try
{
	$domainid = 14;
	$accountid = 54;

	$obBaseApp = new COM("hMailServer.Application", NULL, CP_UTF8);
	$obBaseApp->Connect();

	// Authenticate the user
   	$obBaseApp->Authenticate('Administrator', 'SMTP)(*&^');

   	// Get domain with id
	$obDomain = $obBaseApp->Domains->ItemByDBID($domainid);
	
	// Get account with id
	$obAccount = $obDomain->Accounts->ItemByDBID($accountid);

	//$obAccount->Save();
	// $accountid = $obAccount->ID;

}
catch(Exception $e)
{
   echo $e->getMessage();
   echo "<br>";
   echo "This problem is often caused by DCOM permissions not being set.";
   die;
}

?>
