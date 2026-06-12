# Grupa8-Tim3
# StudyBuddy

StudyBuddy je web aplikacija razvijena u ASP.NET Core MVC tehnologiji, namijenjena organizaciji i praćenju sesija učenja. Sistem omogućava studentima kreiranje i pregled sesija učenja, prijavu na dostupne sesije, evidenciju prisustva, pregled statistike i primanje obavještenja.

Aplikacija koristi PostgreSQL bazu podataka, ASP.NET Core Identity za autentifikaciju i autorizaciju korisnika, te role-based pristup za rad sa različitim tipovima korisnika.

## Pristup aplikaciji

Aplikacija je deployana na Render platformi.

Link aplikacije:

https://grupa8-tim3.onrender.com

## Pristupni podaci za testiranje

Za testiranje aplikacije mogu se koristiti sljedeći korisnički nalozi:

### Student

Email: `student@test.com`
Lozinka: `Student123!`

### Moderator

Email: `mod@test.com`
Lozinka: `Mod123!`

### Administrator

Email: `admin@test.com`
Lozinka: `Admin123!`

Registracija novih korisnika je funkcionalna. Svaki novi registrovani korisnik automatski dobija studentsku ulogu.

## Korisničke uloge

Aplikacija podržava tri korisničke uloge:

* Student
* Moderator
* Administrator

### Student

Student može:

* registrovati se i prijaviti u sistem,
* pregledati dostupne sesije učenja,
* filtrirati sesije po godini studija, smjeru, predmetu, statusu i slobodnim mjestima,
* kreirati novu sesiju učenja,
* odabrati postojeću lokaciju ili dodati novu lokaciju pri kreiranju sesije,
* prijaviti se na sesiju,
* otkazati prijavu na sesiju,
* pregledati svoja obavještenja,
* pregledati statistiku učenja.

### Moderator

Moderator može:

* pregledati sesije učenja,
* pregledati detalje sesija,
* evidentirati prisustvo korisnika,
* pregledati obavještenja.

Moderator nema pristup administratorskim funkcionalnostima i nema pristup statistici učenja, jer je njegova uloga fokusirana na evidenciju prisustva.

### Administrator

Administrator ima puni pristup sistemu i može:

* upravljati korisnicima,
* upravljati predmetima,
* upravljati lokacijama,
* upravljati sesijama učenja,
* pregledati i uređivati prijave na sesije,
* upravljati evidencijom prisustva,
* pregledati i brisati obavještenja,
* pregledati statistiku korisnika,
* upravljati ulogama.

## Glavni entiteti sistema

Aplikacija koristi sljedeće glavne entitete:

* Korisnik
* Predmet
* Lokacija
* SesijaUcenja
* PrijavaNaSesiju
* Prisustvo
* Obavjestenje
* Uloga

Predmeti su povezani sa godinom studija i smjerom, što omogućava filtriranje sesija prema nastavnom planu. Lokacije mogu biti fizičke ili online. Online lokacije koriste link za pristup, dok fizičke lokacije koriste adresu.

## Sesije učenja

Sesija učenja predstavlja centralni dio aplikacije. Svaka sesija sadrži:

* naziv sesije,
* opis,
* datum i vrijeme održavanja,
* trajanje,
* predmet,
* lokaciju,
* maksimalan broj učesnika,
* broj slobodnih mjesta,
* status sesije,
* kreatora sesije.

Prilikom kreiranja sesije korisnik može odabrati jednu od ponuđenih lokacija ili dodati novu lokaciju. Sistem provjerava da li je odabrana lokacija zauzeta u istom terminu, čime se sprječava preklapanje sesija na istoj lokaciji.

## Lokacije

Sistem podržava fizičke i online lokacije.

Primjeri fizičkih lokacija:

* Homework Hub
* Čitaonica STELEKS
* Campus Lounge
* Čitaonica "Safet Zajko"
* Gazi Husrev-begova biblioteka

Primjeri online lokacija:

* Online StudyBuddy soba 1
* Online StudyBuddy soba 2
* Online StudyBuddy soba 3
* Online StudyBuddy soba 4
* Online StudyBuddy soba 5

Za online sesije koristi se link za pristup online sobi. U demo verziji aplikacije koristi se Google Meet generator linkova.

## Obavještenja

Sistem automatski generiše obavještenja za važne događaje, kao što su:

* prijava korisnika na sesiju,
* otkazivanje prijave,
* promjena termina sesije,
* promjena lokacije sesije,
* promjena statusa sesije.

Korisnik može pregledati obavještenja, a u navigaciji se prikazuje oznaka za nepročitana obavještenja.

## Statistika

Aplikacija omogućava pregled statistike učenja. Student može pregledati vlastitu statistiku, dok administrator može pregledati statistiku za korisnike u sistemu.

Statistika uključuje podatke kao što su:

* broj kreiranih sesija,
* broj evidentiranih prisustava,
* ukupno vrijeme učenja.

## Dizajn paterni

U projektu su implementirani sljedeći dizajn paterni:

### Factory

Factory patern se koristi za kreiranje obavještenja. Time je logika kreiranja različitih tipova obavještenja izdvojena iz kontrolera i smještena u posebnu klasu.

### Builder

Builder patern se koristi za kreiranje objekta sesije učenja. Ovim pristupom se omogućava postepeno i pregledno postavljanje podataka potrebnih za sesiju.

### Facade

Facade patern se koristi za operacije nad sesijama učenja. Kontroler ne izvršava direktno svu poslovnu logiku, nego poziva facade koji objedinjuje kreiranje sesije, prijavu korisnika i otkazivanje prijave.

### Adapter

Adapter patern se koristi za prilagođavanje servisa za slanje obavještenja. Na taj način aplikacija može koristiti vanjski servis kroz interfejs prilagođen potrebama sistema.

### Observer

Observer patern se koristi za obavještavanje korisnika kada dođe do promjene sesije. Kada se promijeni datum, lokacija ili status sesije, registrovani korisnici dobijaju obavještenje.

### Strategy

Strategy patern se koristi za računanje različitih tipova statistike. Svaka strategija predstavlja poseban način izračunavanja statističkih podataka.

## Tehnologije

U projektu su korištene sljedeće tehnologije:

* ASP.NET Core MVC
* C#
* Entity Framework Core
* PostgreSQL
* ASP.NET Core Identity
* Razor Views
* HTML
* CSS
* JavaScript
* Bootstrap
* Render
* Docker
* Git i GitHub

## Baza podataka

Aplikacija koristi PostgreSQL bazu podataka. Entity Framework Core se koristi za rad sa bazom, migracije i mapiranje modela na tabele.

Baza podataka je postavljena na Render platformi. Početni podaci za predmete i lokacije dodaju se kroz seeder klase.

Aplikacija koristi ASP.NET Core Identity tabele za autentifikaciju i autorizaciju korisnika, uključujući tabele za korisnike, role i povezivanje korisnika sa rolama.

## Deployment

Aplikacija je deployana na Render platformi korištenjem Docker konfiguracije.

Deployment koristi:

* GitHub repozitorij,
* Dockerfile,
* Render Web Service,
* Render PostgreSQL bazu,
* environment varijable za konekciju na bazu.

Konekcijski string nije naveden javno u README datoteci zbog sigurnosti.

## Struktura projekta

Najvažniji dijelovi projekta su:

* `Controllers` — kontroleri aplikacije
* `Models` — modeli i enum klase
* `Views` — Razor pogledi
* `Data` — ApplicationDbContext i seed podaci
* `Services` — poslovna logika i implementacija dizajn paterna
* `wwwroot` — CSS, JavaScript i statički resursi
* `Areas/Identity` — stranice za autentifikaciju i registraciju korisnika

## UI dizajn

Korisnički interfejs aplikacije je prilagođen StudyBuddy temi. Aplikacija koristi ružičasto-bijelu paletu boja, kartice, forme i pregledne tabele. Posebna pažnja posvećena je stranicama za kreiranje sesija, pregled sesija, detalje entiteta, brisanje entiteta, prijavu i registraciju korisnika.

## Napomene

Dodjeljivanje uloga kroz poseban korisnički interfejs nije implementirano. Novi korisnici koji se registruju automatski dobijaju studentsku ulogu. Administratorski i moderatorski nalozi su unaprijed pripremljeni za potrebe testiranja i demonstracije.

Aplikacija ne koristi vanjski API za mape. Online lokacije koriste linkove za online sobe, a u demo verziji koristi se Google Meet generator linkova.

Korisnički interfejs je prilagođen prikazu na računaru na kojem se aplikacija prezentuje.

## Zaključak

StudyBuddy predstavlja sistem za organizaciju zajedničkog učenja, sa podrškom za različite korisničke uloge, kreiranje i praćenje sesija, evidenciju prisustva, obavještenja, statistiku i upravljanje osnovnim podacima. Projekat demonstrira primjenu ASP.NET Core MVC arhitekture, rada sa bazom podataka, autentifikacije i autorizacije, kao i primjenu više dizajn paterna u konkretnom softverskom sistemu.
