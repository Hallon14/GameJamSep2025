# GameJamSep2025

Come on and Slam and welcome to the Jam.
GameJam Gp25 Sep2025, MadeBy Ludvid, Nicolas och Otto

Inledande "möte" anteckningar

**Första ide.**

Necromancer-ish
Spelar som wizard och använder offer till sin nyttja. Spelet går ut på att fylla skärmen med allierade.
2D topdown "shooter". Wave defence. Neverending.
Återupplivade offer baseras på vad de dödas av.
Alla attackerar med ett visst intervall mot samma target. 

Anväda circle collider som "radie" för att sortera allies runt om kring oss.
En brute och en archer till och börja med


**(Utesluten) Andra ide. Omvänd vampire survivor**

Portal i mitten som spawnar motståndare. 
Ditt jobb som spelare är att stoppa demonerna från att komma igenom och utvinna deras krafter. Uppgradera försvar och snurra på en yttre cirkel för att försvara. 


**(Utesluten) Tredje ide, Boomershooter**
Fast i mitten av en arena. Wave defence av motståndare som du ska försvara dig mot för att inte bli offrad.

3D.
Rewards baserad på resultat från waves. 
Begränsad movement från altaret i mitten.

**Förväntningar & Lärandemål**
Ludvid
Vill lära sig att göra spelet smartare än att hårdkoda skit. Utveckla dynamiska system för händelser i spelet. Ha en central kul grej att centrera spelet runt om det för att maximera den effekten. Samarbeta och lära sig git.

Nicolas
Inget speciellt. En bredare överblick över hur olika aspekter av ett spel samarbetar. Intresserad av radien vi snackade om och muspekarens funktion.

Otto
Leveldesign och hur man kan implementera olika element och faror för att ge spelet karaktär. 

**Ambitioner**

Vi ses i skolan 9-16 på tis & tors. Då schemat e tomt.

Första mål. (Ons eftermiddag) 

utveckla 2 typer av motståndare en ranger och en brute. Som vid sin död reanimeras och samlas i olika radier kring spelaren. Ett attacksystem som baseras på tid och muspekarens funktion.

Till detta önskas två olika "levels" varav en lite mer öppen. Så våra spelare kan lära sig grunderna till spelet. 

**(Uppdelningar)**
Otto börjar på level design

TODO:
Game ploish
  - Sounds
  - Better transitions (Otto has an idea about this)
  - Hit indicators. ( For all enemies and allies)
  - Deathanimations?
  - Particles for projectiles.
  - Credits
  - Stats display (On death and complete game)


  TODO:
    - Player: Fixa movement, få bort problem med att man t.ex kan låsas i ett "driftande" läge. Lägg till mjukare rörelse med "acceleration" etc. *Solved: Ändrade spelarens rörelser till att använda fysiksystemet.*
    - Player: flippa sprite beroende på rörelseriktning. *Solved*
    - Kolla bugg med piercande fiendeprojektiler. *Solved*
    - Kamerarörelser - follow-kamera istället för parentad kamera?
    - Hit indicators. ( For all enemies and allies)
    






