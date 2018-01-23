@echo off
rem START or STOP Services
rem ----------------------------------
rem Check if argument is STOP or START

if not ""%1"" == ""START"" goto stop

if exist D:\School\xampp\hypersonic\scripts\ctl.bat (start /MIN /B D:\School\xampp\server\hsql-sample-database\scripts\ctl.bat START)
if exist D:\School\xampp\ingres\scripts\ctl.bat (start /MIN /B D:\School\xampp\ingres\scripts\ctl.bat START)
if exist D:\School\xampp\mysql\scripts\ctl.bat (start /MIN /B D:\School\xampp\mysql\scripts\ctl.bat START)
if exist D:\School\xampp\postgresql\scripts\ctl.bat (start /MIN /B D:\School\xampp\postgresql\scripts\ctl.bat START)
if exist D:\School\xampp\apache\scripts\ctl.bat (start /MIN /B D:\School\xampp\apache\scripts\ctl.bat START)
if exist D:\School\xampp\openoffice\scripts\ctl.bat (start /MIN /B D:\School\xampp\openoffice\scripts\ctl.bat START)
if exist D:\School\xampp\apache-tomcat\scripts\ctl.bat (start /MIN /B D:\School\xampp\apache-tomcat\scripts\ctl.bat START)
if exist D:\School\xampp\resin\scripts\ctl.bat (start /MIN /B D:\School\xampp\resin\scripts\ctl.bat START)
if exist D:\School\xampp\jboss\scripts\ctl.bat (start /MIN /B D:\School\xampp\jboss\scripts\ctl.bat START)
if exist D:\School\xampp\jetty\scripts\ctl.bat (start /MIN /B D:\School\xampp\jetty\scripts\ctl.bat START)
if exist D:\School\xampp\subversion\scripts\ctl.bat (start /MIN /B D:\School\xampp\subversion\scripts\ctl.bat START)
rem RUBY_APPLICATION_START
if exist D:\School\xampp\lucene\scripts\ctl.bat (start /MIN /B D:\School\xampp\lucene\scripts\ctl.bat START)
if exist D:\School\xampp\third_application\scripts\ctl.bat (start /MIN /B D:\School\xampp\third_application\scripts\ctl.bat START)
goto end

:stop
echo "Stopping services ..."
if exist D:\School\xampp\third_application\scripts\ctl.bat (start /MIN /B D:\School\xampp\third_application\scripts\ctl.bat STOP)
if exist D:\School\xampp\lucene\scripts\ctl.bat (start /MIN /B D:\School\xampp\lucene\scripts\ctl.bat STOP)
rem RUBY_APPLICATION_STOP
if exist D:\School\xampp\subversion\scripts\ctl.bat (start /MIN /B D:\School\xampp\subversion\scripts\ctl.bat STOP)
if exist D:\School\xampp\jetty\scripts\ctl.bat (start /MIN /B D:\School\xampp\jetty\scripts\ctl.bat STOP)
if exist D:\School\xampp\hypersonic\scripts\ctl.bat (start /MIN /B D:\School\xampp\server\hsql-sample-database\scripts\ctl.bat STOP)
if exist D:\School\xampp\jboss\scripts\ctl.bat (start /MIN /B D:\School\xampp\jboss\scripts\ctl.bat STOP)
if exist D:\School\xampp\resin\scripts\ctl.bat (start /MIN /B D:\School\xampp\resin\scripts\ctl.bat STOP)
if exist D:\School\xampp\apache-tomcat\scripts\ctl.bat (start /MIN /B /WAIT D:\School\xampp\apache-tomcat\scripts\ctl.bat STOP)
if exist D:\School\xampp\openoffice\scripts\ctl.bat (start /MIN /B D:\School\xampp\openoffice\scripts\ctl.bat STOP)
if exist D:\School\xampp\apache\scripts\ctl.bat (start /MIN /B D:\School\xampp\apache\scripts\ctl.bat STOP)
if exist D:\School\xampp\ingres\scripts\ctl.bat (start /MIN /B D:\School\xampp\ingres\scripts\ctl.bat STOP)
if exist D:\School\xampp\mysql\scripts\ctl.bat (start /MIN /B D:\School\xampp\mysql\scripts\ctl.bat STOP)
if exist D:\School\xampp\postgresql\scripts\ctl.bat (start /MIN /B D:\School\xampp\postgresql\scripts\ctl.bat STOP)

:end

