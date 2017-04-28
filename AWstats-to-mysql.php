<?php
/*
* Allow IUSER and IIS_USERS to hmailserver/Logs folder
*/

function Conn(){
$connection = new PDO('mysql:host=localhost;dbname=hmailserver', "root","toor");
//$connection = new PDO('mysql:host=localhost;dbname=newsletter', 'root', 'toor');
// don't cache query
$connection->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
// show warning text
$connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);
// throw error exception
$connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
// don't colose connecion on script end
$connection->setAttribute(PDO::ATTR_PERSISTENT, false);
// set utf for connection
$connection->setAttribute(PDO::MYSQL_ATTR_INIT_COMMAND, "SET NAMES 'utf8' COLLATE 'utf8'");
return $connection;
}

$db = Conn();

// Allow IUSER and IIS_USERS to hmailserver/Logs folder
if (file_exists("C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats.log")) {

	copy("C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats.log", "C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats_".time().".log");
	rename("C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats.log", "C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats_old.log");

	$file = fopen("C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats_old.log","r");

	while(! feof($file))
	  {
	  	echo $l = fgets($file);
	  	$l . '<br>';
	  	$a = explode("\t", $l);

	  	$a1 = $a[0];
	  	$a2 = $a[1];
	  	$a3 = $a[2];
	  	$a4 = $a[3];
	  	$a5 = $a[4];
	  	$a6 = $a[5];
	  	$a7 = $a[6];
	  	$a8 = $a[7];
	  	$a9 = $a[8];

	    $res = $db->query("INSERT INTO awstats(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10) VALUES('$a1','$a2','$a3','$a4','$a5','$a6','$a7','$a8','$a9','')");    
	  }

	fclose($file);
	unlink("C:\Program Files (x86)\hMailServer\Logs\hmailserver_awstats_old.log");
}else{
	echo "Empty logs";
}

// remove empty lines
$res = $db->query("DELETE FROM awstats WHERE a1 = ''");

/*
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

CREATE TABLE `awstats` (
  `a1` varchar(255) NOT NULL,
  `a2` text NOT NULL,
  `a3` text NOT NULL,
  `a4` text NOT NULL,
  `a5` text NOT NULL,
  `a6` varchar(255) NOT NULL,
  `a7` varchar(255) NOT NULL,
  `a8` varchar(255) NOT NULL,
  `a9` varchar(255) NOT NULL,
  `a10` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/


?> 
