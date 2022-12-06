# Etudiants FISA SI4

* Théophile Yvars<br/>
* Jules Prince

# Fonctionnalités

Nous possédons un serveur (routing server) qui est le point d'entré pour un client. <br/>
Un proxy est mis en place pour mettre en cache les reponses aux requetes faites à JCDecaux. <br/>
Lors que le client fait une requete, il recoit une string qui comporte un GUID.<br/>
Ce GUID permet au client de se connecter à activeMQ pour collecter les étapes relatives à sa demande. <br/>
Si il n'y pas de vélo dans la station de départ, alors elle est retiré de la liste des stations potentilles. <br/>
Si il n'y a pas de place dans la station d'arrivé, alors elle est retiré de la liste des stations potentielles.<br/>

# Gestion des erreurs.

Pour les erreurs, de maniere gloable, cela fonctionne avec le même principe.<br/>
Le client recoit un id pour se connecter à ActiveMQ. Un fois connecté, il recoit le message d'erreur. <br/>

Si le client entre une chaîne vide, l'application demande de recommencer la saisie. <br/>
Si le client met n'importe quoi, le routing server renvois "Unknown addresses :" avec le ou les adresses inconnus <br/>
Si le routing server ne parvient pas à trouvé de station de vélo dans l'une des villes renseigné par le client, il renvoit une erreur "No station in :" avec la ou les villes n'ayant pas de station.<br/>
Si le proxy rencontre des porblemes, le routing serveur renvoie une erreur "No station in : " avec le nom des 2 villes <br/>
Si le routing server et en defaut, alors le client catch une erreur de connexion.<br/>
Si activeMQ n'est pas en marche, alors le client en est informé. <br/>

# Procedure d'execution 

### Pour executer le back

RoutingServer + Proxy + ActiveMQ<br/>
Le script run_proxy_activeMQ_rontingServer.bat permet de tout executer en même temps.<br/>

Sinon, <br/>
Pour exe le serveur : RoutingServer/bin/Debug/RoutingServer.exe<br/>
Pour exe le proxy : Proxy/bin/Debug/Proxy.exe<br/>
Pour exe ActiveMQ : apache-activemq-5.17.2/bin/activemq start<br/>

### Pour executer le front

Avant de compiler, les serveurs doivent etre démarré. 
Pour compiler le projet : mvn clean jaxws:wsimport<br/>
Ouvrir le projet dans un IDE, Intellij de preference, puis run le projet. <br/>
