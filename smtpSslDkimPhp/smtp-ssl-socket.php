<?php
/*
$filename = "bg.jpg";
$handle = fopen($filename, "rb");
$contents = fread($handle, filesize($filename));
fclose($handle);
*/
?>


<?php
try{
$socket = stream_socket_client("ssl://qflash.pl:587", $errno, $errstr);
if ($socket) {		
	// fwrite($socket, "GET / HTTP/1.0\r\nHost: www.example.com\r\nAccept: */*\r\n\r\n");
	echo fwrite($socket, "Hello localhost");  
	echo fread($socket, 1024);
  echo fwrite($socket, "<QUIT>");
  echo fread($socket, 1024);
  /*
    echo fwrite($socket, "hello@breakermind.com<|>hello@breakermind.com<|>hello@breakermind.com");
   	echo fread($socket, 8192);
    echo fwrite($socket, "Temat wiadomości<|>Message<h1>Trochę html</h1><p>Footer wiadomości<p>");
   	echo fread($socket, 8192);
   	// filename
   	echo fwrite($socket, "bg.jpg");
   	echo fread($socket, 8192);
   	echo fwrite($socket, strlen($contents));   	
   	echo fwrite($socket, $contents);
   	echo fread($socket, 8192);
   	echo fwrite($socket, "<QUIT>");
    */
}else{
	echo "ERROR: $errno - $errstr\n";
}
}catch(Exception $e){
	echo $e;
}
?>
