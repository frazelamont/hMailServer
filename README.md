# hMailServer
hMailServer c# api


## PHPWebAdmin on IIS Windows server
How to run hmailserver admin panel from iis on windows server 2012 https://www.hmailserver.com/documentation/latest/?page=howto_install_phpwebadmin

### Copy PhpWebAdmin from hMailServer folder to wwwroot and create page or copy to localhost folder
In PhpWebAdmin create a copy of the file named config-dist.php and give it the name config.php.

### Add to php.ini
[COM]
com.allow_dcom = true

[ExtensionList]
extension=php_com_dotnet.dll

### Run DCOM in windows server
https://www.hmailserver.com/documentation/latest/?page=howto_dcom_permissions
