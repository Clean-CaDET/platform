DELETE FROM public."Texts";
DELETE FROM public."Images";
DELETE FROM public."Videos";
DELETE FROM public."MetricRangeRules";
DELETE FROM public."ChallengeHints";
DELETE FROM public."BasicMetricCheckers";
DELETE FROM public."ChallengeFulfillmentStrategies";
DELETE FROM public."Challenges";
DELETE FROM public."QuestionAnswers";
DELETE FROM public."Questions";
DELETE FROM public."ArrangeTaskElements";
DELETE FROM public."ArrangeTaskContainers";
DELETE FROM public."ArrangeTasks";
DELETE FROM public."LearningObjects";

DELETE FROM public."LearningObjectSummaries";
DELETE FROM public."KnowledgeNodes";
DELETE FROM public."Lectures";

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
	VALUES (3, 'Imena funkcija predstavljaju glagol, što označava da rade nešto (npr. *Buy(Product product), Create(WebPage newPage), Open(File file)*). Ostali identifikatori, poput promenljiva, polja, klasa i paketa, predstavljaju imenice (npr. *Invoice, PageHeader, FileParser*). Kod koji implementira poslovnu logiku treba da sadrži imenice i akcije iz poslovnog domena (npr. *Invoice, Order, OrderItem, submit(Payment p), cancel(Order o)*).

Pošto ime treba jasno da odredi neku operaciju ili pojam, treba da izbegavamo sinonime i da poštujemo timske konvencije. Ako za većinu entiteta koristimo metodu čije ime počinje sa "Get", programer će morati da zastane ako naleti na metodu koja učitava određeni entitet i ime joj počinje sa "Load". Postaviće se pitanje da li je Load drugačije od Get i da li treba nešto drugačije uraditi sa Load metodama.');
	
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
	VALUES (2, 'Primeni heuristiku odbacivanja beznačajnih reči radi formiranje boljih imena u kodu.', 1, NULL, 1);
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
INSERT INTO public."Challenges"(
	"Id", "Description", "Url")
	VALUES (12, 'Često definišemo naša imena uz pomoć generičnih i beznačajnih reči koji ponavljaju jasnu informaciju ili ništa posebno ne kažu. U sklopu direktorijuma "Naming/01. Noise Words" isprati zadatke u zaglavlju klase i ukloni suvišne reči iz imena u kodu.', 'https://github.com/Clean-CaDET/challenge-repository');
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (2, 12);
-- TODO: NameStrategy, rules, hints
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (13, 11);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (13, 'https://youtu.be/sR8hjHldAfI');
	


--== Naming ==- PK Node 2
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (3, 'Primeni osnovne tehnike refaktorisanja za formiranje boljih imena u kodu.', 1, NULL, 1);

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
INSERT INTO public."Challenges"(
	"Id", "Description", "Url")
	VALUES (7, 'U svojoj brzopletosti, često nabacamo kratka imena kako bismo što pre ispisali kod koji radi. U sklopu direktorijuma "Naming/02. Meaningful Words" proširi kod korisnim imenima koji uklanjaju potrebe za komentarima i isprati zadatke u zaglavlju klase.', 'https://github.com/Clean-CaDET/challenge-repository');
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (1, 7);
-- TODO: NameStrategy, rules, hints (doc)

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
	VALUES (4, 'Razumi uticaj jasnih i misterioznih imena u kodu na rad programera.', 2, NULL, 1);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (12, 'Complex Example', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (13, 'Programmer Motivation', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (14, 'Conclusion', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (15, 'Recap', 4);
	
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
	
--== Methods ==- FK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (10, 'Analiziraj ideju da je funkcija fokusirana na jedan zadatak.', 0, NULL, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId") -- Not sure what to do with this Description field.
	VALUES (30, 'Clean Function Definition', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (31, 'Long Function Example', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (32, 'Short Function Example', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (33, 'One Thing', 10);
-- Methods - FK Node
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (30, 30);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (30, 'Funkcija predstavlja *imenovani* blok koda koji enkapsulira smislen zadatak. Ona predstavlja najjednostavniju grupaciju koda koja može samostalno da postoji. U objektno-orijentisanom programiranju funkcije su često metode koje definišu ponašanje objekta nad kojim se pozivaju. Principi koje poštujemo za formiranje čistih funkcija su jednako primenjivi na metode.

Čista funkcija je *fokusirana na jedan zadatak*. Ovaj zadatak je jasno opisan kroz imena zaglavlja funkcije, što uključuje samo ime funkcije i imena njenih parametra. Čista funkcija ima jednostavno telo i sastoji se od koda koji zahteva malo mentalnog napora da se razume. Kao posledica, ovakve funkcije često sadrže mali broj linija koda.');
		
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (31, 31);
INSERT INTO public."Images"( --TODO: CodeSnippet gist
	"Id", "Url", "Caption")
	VALUES (31, 'https://i.ibb.co/dbthB3H/RS-Methods-Long.png', 'Izdvoj vreme da opišeš sve high-level i low-level zadatke (segmente koda) koje "getFree" izvršava u pratećem kodu. Koje od navedenih zadataka vidiš da bi koristio u drugim kontekstima (i time duplirao kod)?');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (32, 32);
INSERT INTO public."ArrangeTasks"(
	"Id", "Text")
	VALUES (32, 'Prateći kod predstavlja primer čiste funkcije.

    public List<Doctor> GetSuitableDoctors(Operation operation){
    	List<Doctor> doctors = doctorRepository.FindAll();
    
    	List<Doctor> suitableDoctors = new ArrayList<>();
    	foreach(Doctor doctor in doctors){
    		if(IsCapable(doctor, operation.GetRequiredCapabilities())
    		    && IsAvailable(doctor, operation.GetTimeSlot())){
    			suitableDoctors.Add(doctor);
    		}
    	}
    
    	return suitableDoctors;
    }

Rasporedi zahteve za izmenu softvera tako da su vezani za funkcije koje bismo verovatno menjali da bismo ih ispoštovali.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (1, 32, 'Nijedna od navedenih');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (1, 1, 'Dopuni format data transfer objekta da prikaže podatke novoj klijentskoj aplikaciji.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (2, 32, 'FindAll');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (2, 2, 'Potrebno je sačuvati podatke o lekarima u NoSQL bazi umesto u SQL bazi.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (3, 32, 'GetSuitableDoctors');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (3, 3, 'Od mogućih, uvek odabrati lekara koji ima najveći stepen uspeha za dati tip operacije, a prvog kada je nerešeno.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (4, 32, 'IsCapable');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (4, 4, 'Za izazovnu operaciju je potreban hirurg koji je bar jednom izveo dati tip operacije.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (5, 32, 'IsAvailable');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (5, 5, 'Uzima u obzir da li je lekar na bitnom sastanku u traženo vreme.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (33, 33);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (33, 'Šta tačno znači biti fokusiran na jedan zadatak? Idealno, funkcija koja radi jedan zadatak zna koji su koraci potrebni da se uradi taj zadatak, bez da poznaje detalje svakog od navedenih koraka (npr. bez da zna koji su koraci potrebni da bi se rešio njen prvi korak).

Ovako će `getSuitableDoctors` da zna da je za operaciju potrebno pronaći lekare koji su sposobni da urade operaciju i dostupni u predloženom terminu. Ova funkcija neće znati šta znači da je "lekar sposoban", niti kako da interaguje sa skladištem podataka kako bi dobavila lekare. Dalje, `IsAvailable` će znati šta sve treba proveriti da se odredi da li je lekar dostupan, što podrazumeva pregled njegovog radnog vremena i razmatranje da li već ima bitne obaveze u datom vremenskom opsegu. Ova funkcija neće poznavati detalje ovih koraka, poput logike koja je potrebna da se uporede datum i vreme, a možda ni logike koja proverava tačno radno vreme lekara i uključuje ispitivanje da li je na godišnjem odmoru.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (34, 33);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (34, 'https://i.ibb.co/hd5ktG6/RS-Methods-One-Thing.png', '"Zadatak" može da opiše logiku na raznim nivoima apstrakcije - od "GetSuitableDoctors(operation)" do "Sum(a,b)".');


--== Methods ==- PK Node 1
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (11, 'Primeni extract method refaktorisanje za formiranje kraćih funkcija.', 1, NULL, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId") -- Not sure what to do with this Description field.
	VALUES (40, 'Function Length Heuristic', 11);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (41, 'Function Length Challenge', 11);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (42, 'Function Length Solution'); -- Hidden

--== Methods ==- PK Node 2	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (12, 'Primeni extract method refaktorisanje za formiranje jednostavnijih funkcija.', 1, NULL, 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (43, 'Function Comments Heuristic', 12);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (44, 'Function Complexity Challenge', 12);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (45, 'Function Complexity Heuristic'); -- Hidden hint
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (46, 'Function Complexity Challenge Solution');
	
--== Methods ==- PK Node 3	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (13, 'Primeni strategije za redukciju broja parametra za formiranje čistijih funkcija.', 1, NULL, 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (47, 'Parameter List Heuristic', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (48, 'Parameter List Reduction Strategy', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (49, 'Parameter List Challenge', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (50, 'Parameter List Challenge Solution');



-- Methods LO - PK Node 1
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (40, 40);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (40, 'Najjednostavnija heuristika za ispitivanje da li funkcija radi više zadataka podrazumeva posmatranje dužine funkcije. Svakako je moguće da funkcija sa 10 linija koda rešava više problema, ali je sasvim sigurno da funkcija sa 50 linija koda radi previše.
	
Kod dugačkih funkcija potrebno je identifikovati regione logički povezanog koda koji se može ekstrahovati u zasebnu funkciju za koju možemo da odredimo smisleno ime. Savremena integrisana razvojna okruženja nude Extract Method komandu sa kojom možemo označeni kod jednostavno izdvojiti u posebnu funkciju.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (41, 41);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description")
	VALUES (41, 'https://github.com/Clean-CaDET/challenge-repository', 'Da imamo kratke metode ne treba da bude naš konačan cilj, već posledica praćenja dobrih praksi. Ipak, funkcija koja prevazilazi nekoliko desetina linija je dobar kandidat za refaktorisanje. U sklopu direktorijuma "Methods/01. Small Methods" ekstrahuj logički povezan kod tako da završiš sa kolekcijom sitnijih metoda čije ime jasno označava njihovu svrhu.');
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (3, 41);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (3);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (42, 42);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (42, 'https://youtu.be/79a8Zp6FBfU');
	
-- Methods LO - PK Node 2
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (43, 43);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (43, 'Komentari i prazan prostor (npr. dva prazna reda) u telu funkcije su *često* (ali ne uvek) signal da funkcija izvršava više zadataka. Takvi regioni se mogu ekstrahovati, gde će postojeći komentar pomoći pri formiranju imena za novu funkciju.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (44, 44);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description")
	VALUES (44, 'https://github.com/Clean-CaDET/challenge-repository', 'Složene funkcije su one koje zahtevaju visok mentalni napor da se razume sva logika i tokovi kontrole. Mnogi aspekti koda doprinose otežanom razumevanju - čudna imena, dugački izrazi, duboko ugnježdavanje. U sklopu direktorijuma "Methods/01. Simple Methods" refaktoriši funkcije tako da ih pojednostaviš i smanjiš dupliranje koda.');
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (4, 44);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (4);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (45, 45);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (45, 'Kada pravimo male, fokusirane funkcije možemo lako da apstrahujemo složenu logiku iza značajnog imena. Tako možemo da zamenimo složen uslovni izraz (npr. uslov u IF-u koji broji ima više relacionih i logičkih operatora) sa pozivom funkcije čije ime opisuje nameru iza tog izraza. Sa sličnim pristupom možemo zameniti petlje i kalkulacije.
	
Kada naletimo na kod sa dubokim ugnježdavanjem (npr. FOR u IF u FOR u TRY) treba da se zapitamo da li bi se jasnoća koda povećala sa ekstrakcijom nekog segmenta u zasebnu metodu.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (46, 46);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (46, 'https://youtu.be/-TF5b_R9JG4');
	
-- Methods LO - PK Node 3
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (47, 47);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (47, 'Razumljive funkcije treba da teže ka što manjem broju parametara. Ovaj broj je idealno nula, no svakako su funkcije sa jednim ili dva parametra prihvatljive i česte. Tako imamo funkcije koje prihvataju parametar da bi ga transformisali u novi objekat (npr. deserijalizacija stringa u objekat), funkcije koje ispituju neko svojstvo parametra, kao i funkcije koje izvršavaju komandu spram ulaznog podatka ili obrađuju prosleđeni događaj.

Sve preko dva parametra je ozbiljan kandidat za refaktorisanje. Takve funkcije obično rešavaju više zadataka i konfigurišu svoje ponašanje spram ulaznih podataka (tipičan primer su tzv. flag parametri bool tipa) što ih čini manje razumljivim. Izuzetak na ovo pravilo su konstruktori koji često prihvataju više podataka kako bi formirali složeni objekat.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (48, 48);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (48, 'https://i.ibb.co/5cFm6Np/params-strategy-rs.png', 'Kroz navedene strategije vidimo kako je broj parametra metode u interakciji sa klasama i može biti signal da li neku klasu treba uvesti ili izmeniti.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (49, 49);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description")
	VALUES (49, 'https://github.com/Clean-CaDET/challenge-repository', 'Redukcija broja parametra pozitivno utiče na razumevanje samog zaglavlja funkcije i informacije šta ona radi. Pored toga, redukcijom liste parametra često smanjujemo broj zadataka koje funkcija radi. U sklopu direktorijuma "Methods/03. Parameter Lists" primeni strategije za redukciju parametra i refaktoriši funkcije.');
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (5, 49);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (5);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (50, 50);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (50, 'https://youtu.be/yKnxsH0CJzY');

-- HINTS
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (1, 'Ispitaj da li možeš datu funkciju da pojednostaviš reorganizacijom logike ili ekstrakcijom smislenog podskupa koda u funkciju kojoj možeš dati jasno ime.', 43);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (2, 'Razmisli kako bi pojednostavio datu metodu tako da smanjiš ugnježdavanje koda.', 45);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (3, 'Ne zaboravi da vodiš računa o linijama koda.', 40);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (4, 'Funkcija ti i dalje ima previše parametra. Ispitaj svaku od četiri strategije za redukciju parametra i razmisli koja bi bila najpogodnija, pa je onda primeni.', 48);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (5, 'Vredna strategija za redukciju parametra podrazumeva premeštanje metoda i polja klase tako da se ukloni potreba za parametrom. Razmisli da li ima smisla premestiti neku metodu iz ove klase u drugu.', 49);
	
-- Challenge rules
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (1, 'ELOC', 1, 18, 1, NULL, 3);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (2, 'ELOC', 1, 12, 3, NULL, 4);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (3, 'CYCLO', 1, 5, 2, NULL, 4);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (4, 'NOP', 0, 1, 4, NULL, 5);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (5, 'NMD', 0, 2, 5, 5, NULL);
	
	
--== Methods ==- CK Node
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId") -- TODO: KN many to many prerequisites (after experiment)
	VALUES (14, 'Razumi posledice održavanja koda koji se sastoji od funkcija gde je svaka fokusirana na manji broj zadataka (idealno jedan).', 2, NULL, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (60, 'Refactoring Extract Method', 14);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId") -- Not sure what to do with this Description field.
	VALUES (61, 'Function LINQ', 14);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (62, 'Function Hierarchy', 14); -- Dva LO - slika i task
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (63, 'Refactoring One Thing', 14);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (64, 'Function Big Picture', 14);
	
-- Methods LO - CK Node
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (60, 60);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (60, 'https://youtu.be/2goLaolzEV0');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (61, 61);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (61, 'https://i.ibb.co/Y8SM4qV/RS-Methods-LINQ.png', 'Razmisli na koji način nam IsActive funkcija apstrahuje logiku, a na koji način je enkapsulira.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (62, 62);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (62, 'https://i.ibb.co/rFJK6Z8/RS-Methods-Hierarchy.png', 'Kažemo da dobre metode treba da rade na jednom zadatku. Pitanje je kako definišemo zadatak. Uzmi po jednu metodu svake boje - opiši u rečenici šta rade, a šta ne rade. Da li uočavaš razliku u apstraktnosti tvog opisa?');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (63, 62);
INSERT INTO public."ArrangeTasks"(
	"Id", "Text")
	VALUES (63, 'Zamisli sledeću strukturu funkcija:
	
- `GetSuitableDoctors` poziva `FindAllDoctors` i za svakog lekara `IsCapable` i `IsAvailable` metode.
- `FindAllDoctors` poziva `ConnectToStorage` i `ParseDoctor` metode.
- `IsAvailable` poziva `DoesTimeOverlap` metodu.

Uzmimo da svaka od navedeninih 7 funkcija ima 10 linija koda. Dolazi programer koji nije familijaran sa kodom sa ciljem da implementira izmenu kako bi se ispoštovao novi zahtev. Programer kreće od `GetSuitableDoctors` funkcije i spušta se kroz funkcije koje ona poziva. Organizuj zahteve u kolone koje ističu koliko će linija starog koda (iz navedenih funkcija) programer morati da pročita pre nego što će naći linije koje treba da izmeni..');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (6, 63, '1-10');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (6, 6, 'Od mogućih, uvek odabrati lekara koji ima najveći stepen uspeha za dati tip operacije, a prvog kada je nerešeno.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (7, 63, '11-20');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (7, 7, 'Uzima u obzir da li je lekar na bitnom sastanku u traženo vreme.');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (8, 7, 'Za izazovnu operaciju je potreban hirurg koji je bar jednom izveo dati tip operacije.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (8, 63, '21-30');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (9, 8, 'Potrebno je sačuvati podatke o lekarima u NoSQL bazi umesto u SQL bazi.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (9, 63, '31-40');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (10, 63, '41-50');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (64, 63);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (64, 'https://youtu.be/JQPIh0VcLK4');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (65, 64);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (65, 'Zamisli aplikaciju koja se razvija duže vreme. Sadrži mnogo klasa, gde se većina sastoji od više čistih metoda koje su fokusirane na jedan zadatak. Kompletan sistem podržava razne slučajeve korišćenja kroz koordinaciju ovih mnogobrojnih funkcija.
Stiže novi zahtev koji zahteva izmenu kako se neki zadatak izvršava. Programer kreće od korisničke kontrole (forme, tastera) koja će pružiti drugačije ponašanje/podatke kada se ispuni zahtev i ispituje telo povezane metode/endpoint-a. Potom rekurzivno primenjuje sledeći algoritam:

1. Ispitaj telo metode u potrazi za logikom koju treba menjati.
2. Za svako ime metode koja se poziva, analiziraj njegovo značenje. Ako ime određuje logiku relevantnu za novi zahtev skoči na telo date funkcije i primeni korak 1. U suprotnom ignoriši.

Ovako programer izbegava analizu stotina linija koda i svede posao na analizu nekoliko imena. Programer leti kroz kod uz pomoć prečica svog integrisanog razvojnog okruženja i ne zamara se sa detaljima funkcija koje nisu relevantne za dati zahtev.
Kada konačno pronađu funkciju čije ponašanje treba promeniti, izmena je sitna i fokusirana, što smanjuje šansu da se uvede *bug*. Programer ispunjuje novi zahtev bez da je potrošio mnogo mentalne energije.

Sada zamisli sličnu aplikaciju koja je sastavljena od golemih funkcija koje broje stotine linija koda i poseduju misteriozna imena. Nova izmena zahteva od programera da istražuje kod. Potreban je veliki mentalni napor kako bi se očuvao visok fokus dok se identifikuje koje deo kompleksne logike treba izmeniti, a koji ignorisati. Sama izmena je rizična aktivnost i u ovakvom kodu se *bug* lako potkrade. Zbog toga je potrebno pažljivo raditi ovaj mukotrpan proces, koji može trajati više sati (na dovoljno složenom problemu i više dana). Na kraju, programer je istrošen i opterećen mislima da je nešto prevideo i da će njegova izmena srušiti sistem u produkciji.

Pisanje čistih funkcija je teže nego kucanje koda dok ne proradi. Za čiste funkcije svakako treba napraviti nešto što radi, a onda još treba organizovati tu logiku u smislene celine ispred kojih stavljamo jasno ime. Sa druge strane, čitanje aljkavo napisanih funkcija je teže od čistanja čistih funkcija. Postavlja se pitanje - u koji aspekt vredi uložiti više truda i energije? Za softver koji je dovoljno dugo u razvoju (npr. više od mesec dana, razvijan od strane pet ili više ljudi) većina programerovog vremena odlazi na čitanje koda.');

-- TODO: Final question