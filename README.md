# Laboration 2OOP
Projekt
Laboration_2OOP är en WPF-applikation för att hantera:
-medlemmar
-spel
-spelträffar
anmälningar och avanmälningar till spelträffar
Projektet bygger vidare på domänmodellen från Laboration 1 och vidareutvecklar den till en applikation med WPF, MVVM, Entity Framework och persistent lagring
Funktioner
UC1 – Anmälningar
-välj medlem och spelträff och klicka Anmäl / Avanmäl
-visar deltagare för vald träff
-visar status- och felmeddelanden i gränssnittet
UC2 – Spelträffar
-skapa en ny spelträff
-ange datum, tid, plats och aktivitetstyp
-välja ansvarig arrangör
-visa kommande spelträffar
-uppdatera listan över spelträffar
UC3 – Medlemmar
-registrera ny medlem
-uppdatera en befintlig medlem
-visa lista över medlemmar
-filtrera fram endast aktiva medlemmar
-Spel
-registrera nytt spel
-uppdatera befintligt spel
-visa lista över spel
-gruppera spel efter kategori med LINQ
Teknik
Projektet använder följande tekniker:
-C# / .NET
-WPF
-MVVM
-Databindning med INotifyPropertyChanged, ObservableCollection<T>, ItemsSource och SelectedItem
-Commands i ViewModels
-Entity Framework
-SQL Server LocalDB
-LINQ för filtrering, sortering och gruppering

DATABAS
Hur programmet körs
Krav
-Windows
-Visual Studio
-SQL Server LocalDB installerade
Starta projektet
-Öppna lösningen i Visual Studio.
-Kontrollera att LocalDB finns installerad.
-Kör projektet med F5.
-Vid första uppstart skapas databasen automatiskt och demonstrations data läses in.
Användning
UC1 – Anmälningar
-Välj en medlem i listan Medlemmar
-Välj en spelträff i listan Spelträffar
-Deltagare visas i rutan Deltagare
-Klicka på Anmäl eller Avanmäl
-Vid fel visas ett tydligt meddelande i statusrutan
UC2 – Skapa spelträff
-Välj datum
-Ange tid och plats
-Välj aktivitetstyp
-Välj arrangör
-Klicka på Skapa spelträff
UC3 – Medlemmar
-Fyll i medlemsuppgifter
-Klicka på Registrera
-Välj en medlem för att uppdatera uppgifter
-Kryssa i Visa endast aktiva för att filtrera listan
Spel
-Fyll i spelets uppgifter
-Klicka på Registrera spel
-Välj ett spel för att uppdatera det
-Klicka på knappen för att visa spel grupperade efter kategori
Validering och felhantering
Applikationen innehåller validering och tydlig återkoppling för vanliga fel, till exempel:
-tomma eller ogiltiga fält
-dubbelanmälan
-fullbokad spelträff
-försök att anmäla inaktiv medlem
-uppdatering utan valt objekt
Project Struktur
Projektet är organiserat i tydliga delar, till exempel:
-Domän – domän klasser och regler
-ViewModels – tillstånd, databindning och commands
-Services – applikationslogik och dataåtkomst
-Data – AppDbContext, seedning och databas relaterad logik
-DemoData – demonstrations data
-Views – XAML och WPF-fönster
