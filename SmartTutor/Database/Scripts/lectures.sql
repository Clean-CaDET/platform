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