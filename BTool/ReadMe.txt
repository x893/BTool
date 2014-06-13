==================================================
Texas Instruments BTool
==================================================
--------------------------------------------
BTool - (For Version 01.40.5 or higher)
--------------------------------------------

--------------------------------------------
Integrated Cross Platform Runtime Executable 
(Using Mono 2.10.5 or higher)
--------------------------------------------
1. Install BTool in Windows. (Note: Mono is NOT needed in Windows) 

2. Copy the installed files 
BTool.exe 
BTool.exe.config
BToolGattUuid.xml
as is, to a different OS with Mono 2.10.5 or higher installed.

3. Run "BTool.exe" using Mono on the different OS.

Tested using Windows XP, Windows 7, Ubuntu 12.04 LTS, Ubuntu 11.10.
Should be usable under Linux, Mac OS X, BSD, Sun Solaris, Android, PS3, Xbox 360 and more!
See http://mono-project.com/What_is_Mono for more information. 

==================================================
Getting BTool Running In A OS Other Than Windows
==================================================

Install Mono 2.10.5 or higher
(ubuntu) sudo apt-get install mono

Install WinForms 4.0 for Mono
(ubuntu) sudo apt-get install libmono-system-windows-forms4.0-cil 

Using a terminal window, change to the directory where the files are and issue the following command:
mono BTool.exe 




