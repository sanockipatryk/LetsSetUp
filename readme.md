Utworzenie bazy danych wymaga zainstalowania MySQL Server i MySQL Workbench, połączenia z serwerem, zaktualizaowania kodu połączenia w pliku appsettings.json.
Następnym krokiem jest użycie polecenia: 
dotnet ef database update
i uruchomienia serwera backendu dla zainicjowania bazy danych.

Usunięcie dotychczasowej bazy danych odbywa się poleceniem:
dotnet ef database drop