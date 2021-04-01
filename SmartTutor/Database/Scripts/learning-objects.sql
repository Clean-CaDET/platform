--TODO: NodeProgressId should be removed from this table as it is a many to many relationship (explore navigation options and EF Core configuration)
-- Naming - FK Node
DELETE FROM public."LearningObjects";
DELETE FROM public."Texts";
DELETE FROM public."Images";
DELETE FROM public."Videos";

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (1, 1);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (1, 'Imenovanje je proces određivanja i dodeljivanja imena identifikatoru. Identifikatore pronalazimo svuda u kodu. Tako imenujemo datoteke, direktorijume, klase, metode i promenljive. U Java programima imenujemo pakete i JAR datoteke, dok kod C# jezika imenujemo namespace i DLL datoteke. Dobro ime treba da **objasni svrhu elementa** koji imenujemo i da pokuša da odgovori na pitanja: Zašto dati element postoji? Šta radi? Kako se koristi?');
		
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (2, 2);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (2, 'https://i.ibb.co/vqjMTyJ/simple-names-sr.png', 'U većini slučajeva kada hoćemo da stavimo komentar, pravo rešenje je smišljanje jasnijeg imena.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (3, 3);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (3, 'Imena funkcija predstavljaju glagol, što označava da rade nešto (npr. *Buy(Product product), Create(WebPage newPage), Open(File file)*). Ostali identifikatori, poput promenljiva, polja, klasa i paketa, predstavljaju imenice (npr. *Invoice, PageHeader, FileParser*). Kod koji implementira poslovnu logiku treba da sadrži imenice i akcije iz poslovnog domena (npr. *Invoice, Order, OrderItem, submit(Payment p), cancel(Order o)*).');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (4, 4);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (4, 'https://i.ibb.co/Tw9qktR/domain-names-sr.png', 'Lakše ćemo razumeti nove zahteve kada koristimo u kodu isti jezik kao naši klijenti.');
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (5, 4);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (5, 'https://i.ibb.co/f144vCk/names-example.png', 'Ovako objektno orijentisani programer imenuje stvari kada izbegava reči iz poslovnog domena.');
	
-- Naming - PK Node 1
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (6, 5);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (6, 'Izbor pravog imena je proces koji podrazumeva sledeće korake:

1. Opisivanje elementa koji želimo da imenujemo kroz rečenicu ili dve. Ovaj opis bi se mapirao na tekst koji bismo stavili u komentar uz element koji imenujemo. Tekst je lakše formirati na maternjem jeziku, a kada smo zadovoljni sa opisom možemo prevesti na engleski (sa ili bez pomoći onlajn rečnika).
2. Uklanjanje svih redundantnih reči, veznika i priloga koji nisu ključni za prenošenje svrhe elementa koji imenujemo.
3. Uklanjanje svih preostalih reči koje se lako izvlače iz **tipa** (npr. povratna vrednost, tip parametra, promenljive), **modula** kom dati element pripada (npr. kada imenujemo funkciju uzimamo u obzir ime klase, dok za klasu razmatramo paket) i preostalog konteksta (npr. parametri funkcije koju imenujemo).
4. Razmatranje da li preostale reči adekvatno opisuju element i ako da
5. Postavljanje novog imena.

Navedeni algoritam nije jednokratna aktivnost. Često je potrebno nekoliko iteracija i preimenovanja da bismo stigli do imena koje je dovoljno jasno i značajno. Na sreću, savremena IDE rešenja pružaju podršku za lako preimenovanje elemenata koda. Ako kod prolazi *build* posle preimenovanja, možemo biti uvereni da nismo uveli nove greške u kod.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (10, 9);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (10, 'https://youtu.be/IusayOJt79E');
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (11, 9);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (11, 'Prilikom formiranja imena treba da izbegnemo dodavanje beznačajnih prefiksa i sufiksa. Tipičan primer nepotrebnog sufiksa nastaje kada se radi *copy & paste* izraza koji definiše promenljivu, gde se na kraj kopirane promenljive doda "1". Ovaj potez rezultuje brzom pisanju nove instrukcije, ali usporava svako naknadno čitanje.

Treba da izbegavamo redundantne reči koje ponovo ističu tip elementa (npr. *str, string, obj, list, set*). Takođe treba da izbegavamo previše generične reči, poput *Manager, Coordinator, Data, Info*, jer ovakve reči važe za mnoge elemente koda. Dodatno treba izbegavati akronime i skraćenice osim kada su one opšte poznate (npr., ID, VAT).

Najzad, reči poput *Controller* i *Service* treba pažljivo koristiti, prateći konvencije tima. U opštem slučaju, prefiksi i sufiksi su prihvatljivi samo kada se prati konvencija tima, no i tada se moramo postarati da imamo značajno ime pre nego što uvedemo ovakve dodatke.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (12, 10);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id")
	VALUES (2);
-- TODO: NameStrategy and rules
INSERT INTO public."Challenges"(
	"Id", "Url", "FulfillmentStrategyId")
	VALUES (12, 'https://github.com/Clean-CaDET/challenge-repository', 2);
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (13, 11);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (13, 'https://youtu.be/sR8hjHldAfI');
	
	
-- Naming - PK Node 2
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (7, 6);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id")
	VALUES (1);
-- TODO: NameStrategy and rules
INSERT INTO public."Challenges"(
	"Id", "Url", "FulfillmentStrategyId")
	VALUES (7, 'https://github.com/Clean-CaDET/challenge-repository', 1);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (8, 7);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (8, 'Dodeljivanje značajnih imena elementima koda pomaže sa aspekta pretrage koda. Kada u velikom projektu želimo pretragom da lociramo promenljivu ili funkciju, ako je ime tog elementa kratko (broji par karaktera), mala je šansa da ćemo brzo doći do datog elementa. Naspram toga, ako ime broji nekoliko reči, pretraga nam može u istom momentu locirati dati element.

Slična je priča sa takozvanim *magic numbers*, što predstavljaju vrednosti zakucane u kodu (literal), poput broja 10 ili teksta "TEST". Pored što je teško pretragom pronaći ovakve vrednosti, brzo se zaboravi namera iza datog podatka. Kada naiđemo na kod koji proverava da li je vrednost jednaka "EXIT -1" ili je dužina veća od 12, dobra je šansa da ćemo morati pitati kolegu šta podatak znači ili na drugi način trošiti vreme u potrazi za odgovorom. Kad bismo zamenili zakucanu vrednost sa konstantom  (npr. umesto 12 definišemo *MinimumPasswordLength*) unapredićemo razumljivost koda, centralizovaćemo vrednost u konstanti (umesto da ga kopiramo na više mesta) i omogućićemo pronalazak podatka uz pomoć pretrage.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (9, 8);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (9, 'https://youtu.be/8OYsu0dza0k');


-- Naming - CK Node
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (14, 12);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (14, 'https://youtu.be/wcIJOmP0R7I');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (15, 13);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (15, 'https://i.ibb.co/pbwFLL1/RS-motivation.png', 'Kako bi ti radije provodio svoje radno vreme?');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (16, 14);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (16, 'Jasno ime objašnjava **šta** imenovano parče koda radi, ne **kako** to radi. Tako jedna funkcija može imati sledeća imena:

 - `List<KeyValuePair<EmployeeId, decimal>> SumBonusesAndSickDaysForSalaryCalculation`, gde iz imena možemo pretpostaviti kako izgleda kod;
 - `List<KeyValuePair<EmployeeId, decimal>> ComputeSalariesPairList()`, gde ime redundantno opisuje tip povratne vrednosti i ništa van toga; i
 - `List<KeyValuePair<EmployeeId, decimal>> ComputeEmployeeSalaries()` gde je istaknuta suština operacije bez nepotrebnih dodatnih detalja.

Iz perspektive programera koji radi sa ovim kodom (a inače je zadužen za drugu komponentu), razumemo nameru ovog koda bez da ulazimo u detalje vezane za korake ostvarivanja date namere ili izgleda konačnog rezultata (povratne vrednosti).

Ime predstavlja nešto apstraktnije od skupa koraka i tipa povretne vrednost - rezultat je kolekcija plata zaposlenih, a ne bilo kakav spisak parova. Na isti način će neki `int` često predstavljati nešto više od `int` vrednosti - može da predstavlja godine osobe ili broj elemenata kolekcije i da postoje ograničenja koje vrednosti iz skupa svih celih brojeva može da uzme. Tako objekat tipa `Employee` može biti menadžer tima ili njegov član, dok lista `double` vrednost može da bude kolekcija merenja prosečne temperature u Novom Sadu u trenutnoj godini. U svim slučajevima značajna imena će nam pomoći da razumemo šta su ti podaci i ponašanje koje imenujemo.');
