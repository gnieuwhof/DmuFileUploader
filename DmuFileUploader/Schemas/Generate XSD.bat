
rem aanroepen van de xsd tool om een class file te genereren. 
rem Dit bestand heeft dezelfde bestandsnaam als het xsd bestand (met een andere extensie natuurlijk)
echo Genereren van classfiles
SET programFiles=%ProgramFiles%
IF DEFINED ProgramFiles(x86) SET programFiles=%ProgramFiles(x86)%
"%programFiles%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\xsd.exe" ContentTypes.xsd /c /l:cs /n:"Schemas.ContentTypes"
"%programFiles%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\xsd.exe" Data.xsd /c /l:cs /n:"Data"
"%programFiles%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\xsd.exe" DataSchema.xsd /c /l:cs /n:"Schema"
