DELETE FROM public."Lectures";
DELETE FROM public."KnowledgeNodes";
DELETE FROM public."LearningObjectSummaries";

DELETE FROM public."LearningObjects";
DELETE FROM public."Texts";
DELETE FROM public."Images";
DELETE FROM public."Videos";
DELETE FROM public."Challenges";
DELETE FROM public."QuestionAnswers";
DELETE FROM public."Questions";

INSERT INTO public."Lectures"(
	"Id", "Name", "Description")
	VALUES (1, 'Jasna imena', 'Imena pronalazimo u svim segmentima razvoja softvera - kao identifikator promenljive, funkcije, klase, ali i biblioteke i aplikacije. Jasno ime funkcije nas oslobađa od čitanja njenog tela, dok će misteriozno ime zahtevati dodatan mentalni napor da razumemo svrhu koncepta koji opisuje. U najgorem slučaju, loše ime će nas navesti na pogrešan put i drastično nam produžiti vreme razvoja. Kroz ovu lekciju ispitujemo dobre i loše prakse za imenovanje elemenata našeg koda.');
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description")
	VALUES (2, 'Kratke funkcije', 'Čest savet je da naše funkcije treba da imaju mali broj linija koda. Ovako povećavamo fokusiranost i jasnoću funkcije, kao i mogućnost ponovne upotrebe ovog parčeta koda. Međutim, greška je reći da nam je cilj da imamo kratke funkcije. Nama je cilj da ispoštujemo više dobrih praksi za formiranje čistih funkcija, a kao posledicu primene tih praksi ćemo dobiti kratke funkcije. Kroz ovu lekciju analiziramo dobre i loše prakse za formiranje čistih funkcija.');
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description")
	VALUES (3, 'Kohezija', 'U visoko kohezivnim timovima, svako član ima dobro razvijen odnos sa svakim članom tima pojedinačno. Ovo pre svega podrazumeva da svaki član dobro poznaje kako drugi članovi rade, gde su manje, a gde više efikasni i kako najbolje da sarađuju sa njima na nekom zadatku. Ovo svojstvo je primenljivo na softverske module, gde se svaki sastoji od elemenata koji međusobno sarađuju. Tako je visoko kohezivna klasa ona čija polja i metode su gusto umrežene. Kroz ovu lekciju ćemo ispitati kako takve module da formiramo.');
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description")
	VALUES (4, 'Spregnutost', 'Dva gusto spregnuta softverska modula u velikom stepenu zavise jedan od drugog i često se ponašaju kao jedan veliki modul. Ovakvi moduli imaju puno međusobnih veza (poput upotrebe atributa ili poziva metoda) i znaju razne detalje jedan o drugom. Izmena jednog takvog modula gotovo uvek povlači modifikaciju (i bagove) drugog. Ako je sistem prepun ovakvih sprega, postaje trom i težak za izmenu. Sa druge strane, ako modul sakriva mnoštvo logike iza jednostavnog API-a, značajno je ograničena mogućnost sprezanja sa takvim modulom. Kroz ovu lekciju učimo da pravimo ovakve module.');
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description")
	VALUES (5, 'Princip jedne odgovornosti', 'Vodeći princip inženjerstva softvera ističe da svaki modul treba da ispunjava jedan cilj koji je onoliko apstraktan koliko i sam modul. Privatna metoda koja radi sa detaljima će imati veoma konkretan cilj. Aplikacija ima jedan cilj na visokom nivou asptrakcije koji se razlaže na slučajeve korišćenja koje potom podržavamo sa manje apstraktnim objektima i njihovima metodama. U tako dizajniranom sistemu, svaka izmena je fokusirana samo na one module koji su vezani za cilj koji je afektovan. Kroz ovu lekciju razlažemo ovaj vodeći princip i gledamo kako vodi naš razvoj softvera.');
	

	
--== Naming ==- FK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (1, 'Navedi osnovne vodilje za definisanje značajnih imena.', 0, NULL, 1);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (1, 'Definition', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (2, 'Basic Example', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (3, 'Word Type Heuristic', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (4, 'Example', 1);
-- Naming - FK Node
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
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (17, 4);
INSERT INTO public."Questions"(
	"Id", "Text")
	VALUES (17, 'Kada dizajniramo softver, bitan cilj je da razdvojimo funkcionalnosti poslovne logike (npr. pravilo kako se stornira kupovina) od logike koja je vezana za tehnološke detalje (npr. kod za formiranje HTTP zahteva). Ovo pravilo važi i za sama imena, gde objekti i funkcije poslovne logike treba da sadrže samo reči koje potiču iz domena problema, odnosno od naših klijenata. Označi imena funkcija koje smatraš da potiču iz domena poslovne logike:');
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (1, 'string.join(":", patientRecord)', false, 'Ovo nije ime iz domena problema već funkcija za spajanje elementa niza u string - tehnološki detalj.', 17);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (2, 'Close(Account account)', true, 'Ovo ime je iz domena problema bankarskog poslovanja. Zatvaranje naloga je poslovna operacija koja se praktikovala i pre nego što su banke imale softver.', 17);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (3, 'SaveToFile(MedicalHistory patientCard)', false, 'Ovo ime ističe tehnološki detalj - datoteku kao način perzistencije podataka. Da je ime metode bilo samo "Save" mogli bismo ga svrstati u domen problema kao poslovnu želju da se trajno sačuva ova informacija. Međutim, poslovnu logiku ne zanima da li je to čuvanje u datoteci, bazi podataka ili na cloud-u.', 17);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (4, 'RefreshEmployeeRegistryView()', false, 'Po imenu možemo zaključiti da ova metoda osvežava neki prikaz (npr. tabele) što je tehnološki detalj.', 17);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (5, 'RegisterMember(Member newMember)', true, 'Ovakva funkcija je deo poslovne logike oko registracije novog člana (npr. biblioteke). Naspram toga, operacija registracije korisnika aplikacije (npr. RegisterUser) bi podrazumevala tehnološku logiku koja bi mogla da uključi poslovnu ako bi se prilikom registracije formirao i član koji nije samo sistemski korisnik.', 17);
	

	
--== Naming =- PK Node 1
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (2, 'Primeni heuristiku odbacivanja beznačajnih reči radi formiranje boljih imena u kodu.', 1, 1, 1);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (5, 'Algorithm', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (9, 'Noise Word Heuristic', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (10, 'Challenge Noise', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description") -- Hidden
	VALUES (11, 'Challenge Noise Solution');
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
4. Razmatranje da li preostale reči adekvatno opisuju element.
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
	"Id", "Url", "Description", "FulfillmentStrategyId")
	VALUES (12, 'Često definišemo naša imena uz pomoć generičnih i beznačajnih reči koji ponavljaju jasnu informaciju ili ništa posebno ne kažu. U sklopu direktorijuma "01. Noise Words" isprati zadatke u zaglavlju klase i ukloni suvišne reči iz imena u kodu.', 'https://github.com/Clean-CaDET/challenge-repository', 2);
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (13, 11);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (13, 'https://youtu.be/sR8hjHldAfI');
	


--== Naming ==- PK Node 2
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (3, 'Primeni osnovne tehnike refaktorisanja za formiranje boljih imena u kodu.', 1, 2, 1);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (6, 'Challenge Meaning', 3);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (7, 'Magic Numbers Heuristic', 3);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description") -- Hidden
	VALUES (8, 'Challenge Meaning Solution');
-- Naming - PK Node 2
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (7, 6);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id")
	VALUES (1);
-- TODO: NameStrategy and rules
INSERT INTO public."Challenges"(
	"Id", "Url", "Description", "FulfillmentStrategyId")
	VALUES (7, 'U svojoj brzopletosti, često nabacamo kratka imena kako bismo što pre ispisali kod koji radi. U sklopu direktorijuma "02. Meaningful Words" proširi kod korisnim imenima koji uklanjaju potrebe za komentarima i isprati zadatke u zaglavlju klase.', 'https://github.com/Clean-CaDET/challenge-repository', 1);

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
	


--== Naming ==- CK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (4, 'Razumi uticaj jasnih i misterioznih imena u kodu na rad programera.', 2, 3, 1);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (12, 'Complex Example', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (13, 'Programmer Motivation', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (14, 'Conclusion', 4);
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

 - `List<KeyValuePair<EmployeeId, decimal>> SumBonusesAndSickDaysForSalaryCalculation()`, gde iz imena možemo pretpostaviti kako izgleda kod;
 - `List<KeyValuePair<EmployeeId, decimal>> ComputeSalariesPairList()`, gde ime redundantno opisuje tip povratne vrednosti i ništa van toga; i
 - `List<KeyValuePair<EmployeeId, decimal>> ComputeEmployeeSalaries()` gde je istaknuta suština operacije bez nepotrebnih dodatnih detalja.

Iz perspektive programera koji radi sa ovim kodom (a inače je zadužen za drugu komponentu), razumemo nameru ovog koda bez da ulazimo u detalje vezane za korake ostvarivanja date namere ili izgleda konačnog rezultata (povratne vrednosti).

Ime predstavlja nešto apstraktnije od skupa koraka i tipa povretne vrednost - rezultat je kolekcija plata zaposlenih, a ne bilo kakav spisak parova. Na isti način će neki `int` često predstavljati nešto više od `int` vrednosti - može da predstavlja godine osobe ili broj elemenata kolekcije i da postoje ograničenja koje vrednosti iz skupa svih celih brojeva može da uzme. Tako objekat tipa `Employee` može biti menadžer tima ili njegov član, dok lista `double` vrednost može da bude kolekcija merenja prosečne temperature u Novom Sadu u trenutnoj godini. U svim slučajevima značajna imena će nam pomoći da razumemo šta su ti podaci i ponašanje koje imenujemo.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (18, 14);
INSERT INTO public."Questions"(
	"Id", "Text")
	VALUES (18, 'Ako tvrdimo da jasno ime objašnjava šta imenovano parče koda radi, a ne kako to radi, označi funkcije koje imaju jasno ime?');
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (6, 'Dictionary<MetricId, double>> ComputeMetricValueMap()', false, 'Ovo nije dobro ime, jer redundantno opisuje tip povratne vrednosti i ništa van toga. "GetMethodMetrics()" bi jasnije opisalo nameru funkcije.', 18);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (7, 'void Save(Employee employee)', true, 'Ovo je dobro ime, pod pretpostavkom da se glagol "Save" po konvenciji koristi za svako čuvanje entiteta.', 18);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (8, 'List<DatasetInstance> FindInstancesRequiringAdditionalAnnotation(Dataset dataset)', true, 'Ovo je dobro ime, jer apstrahuje detalje koji određuju šta znači da instanca nije dovoljno anotirana, a opet dodatno opisuje šta će ta povratna vrednost da predstavlja, odnosno da nije bilo kakva lista instanci.', 18);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (9, 'decimal CalculatePriceFromOrderSizeAndDiscount(Order order, Discount discount)', false, 'Ovo nije dobro ime, jer opisuje kako se cena računa. Bolje ime bi bilo "CalculatePrice(Order order, Discount discount)" koje ističe nameru funkcije, a iz parametra se vidi da se popust uvažava.', 18);
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (19, 15);
INSERT INTO public."Questions"(
	"Id", "Text")
	VALUES (19, 'Iz sledećeg spiska odaberi istinite tvrdnje:');
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (10, 'Uvek je bolje dodeliti identifikatoru duže ime.', false, 'Izjava nije tačna. Iako je često bolje ime koje se sastoji od nekoliko reči umesto od par karaktera, treba da izbegavamo suvišne, redundantne i generične reči i da budemo koncizni. U određenim okolnostima, čak i ime od par karaktera je dopustivo (npr. kada je scope identifikatora ograničen na par linija koda).', 19);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (11, 'Formiranje jasnog imena ubrzava čitanje i pisanje koda.', false, 'Izjava nije tačna. Dobro ime značajno olakšava čitanje koda, no formulisanje jasnog imena je često zahtevan posao i može uzeti više minuta, dok je postavljanje besmislenog imena instant operacija.', 19);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (12, 'Kada formiramo ime za neki element, treba da uzmemo u obzir njegov kontekst (tip, modul kom pripada, itd.)', true, 'Izjava je tačna. Kada zadajemo ime treba da izbegnemo ponavljanje informacije koja je jasno vidljiva iz npr. tipa povratne vrednosti funkcije ili objekta nad kojim se poziva metoda.', 19);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (13, 'Treba da izbegavamo tehnološke reči u našim imenima.', false, 'Izjava nije tačna. U redu je da imamo reči poput "File", "HTTP" i "Queue" u našim nazivima, ali ciljamo da ove koncepte izdvojimo od poslovne logike.', 19);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (14, 'Kod koji opisuje poslovnu logiku treba da bude jasan našem klijentu.', true, 'Izjava je tačna. Kada koristimo imenice iz domena da opišemo klase i promenljive i glagole da opišemo metode, naš kod je čitljiv od strane osobe koja poznaje domen - naš klijent.', 19);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (15, 'Potrebno je dobro ime izabrati u startu, zato što je naknadna promena skupa.', false, 'Izjava nije tačna. Promena imena za gotovo svaki element koda je trivijalna operacija uz pomoć savremenih editora koda.', 19);
	
	