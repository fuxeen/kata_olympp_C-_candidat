
=== Voici ce que j'ai fait :

J'ai implémenté les différentes classes en respectant l'architecture demandée,
afin qu'elles aient le comportement attendu.

J'ai créé des tests pour tester les différentes classes,
spécialement les objets Clan, Army, BattleReport
BattleRepository, ClanRepository
et BattleService.

La classe BattleService permet tout spécialement de tester le moteur principal du sujet, à savoir
la simulation des batailles avec les différents tours de jeu.

La bataille actuellement simulée dans la classe de test est intéressante :
Athen à une armé, Troy deux.
La première armée d'Athen bat la première de Troy en deux tour, puis la seconde armée de Troy bat celle d'Athen.
Donc finalement, Troy gagne.

Ce test montre que la simulation fonctionne correctement.
Le rapport de bataille est également bien généré.

Globalement, tout semble fonctionner dans les différentes parties du programme.

Le seul soucis que je rencontre est lorsque l'on veut utiliser l'API en tant que telle.

=== Problème rencontré  ===

Lorsque je démarre l'application, elle se lance bien,
il n'y a pas d'erreurs dans la console,
cependant, la page https://localhost:44349/swagger 
est blanche.

De même pour la page https://localhost:44349/swagger/v1/swagger.json,
Cette page est blanche aussi, alors qu'elle devrait montrer le Json généré par swagger.
Cela signifie que swagger n'arrive pas à le générer.

J'ai pas mal cherché sur internet et avec un assistant IA, j'ai essayé différentes solutions, pour configurer swagger différement mais cela n'a rien donné.
Swagger ne semble pas non plus lever d'exception à son lancement ce qui est étonnant.

Mon hypothèse principale est que le problème viens de mes APIs qui ne doivent pas correspondre à ce que swagger attends, ce qui l'empecherait de générer son JSON.
Cependant, je ne m'y connait pas assez bien pour pouvoir diagnostiquer  d'avantage le problème, d'autant plus que le temps me manque.
C'est bien la première fois que j'utilise swagger.