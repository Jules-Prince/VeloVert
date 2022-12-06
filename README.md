# Etudiants FISA SI4

* Théophile Yvars<br/>
* Jules Prince

# Fonctionnalité

Nous possédons un serveur (routing server) qui est le point d'entré pour un client. <br/>
Un proxy est mis en place pour mettre en cache les reponses aux requetes faites à JCDecaux. <br/>
Lors que le client fait une requete, il recoit une string qui comporte un GUID.<br/>
Ce GUID permet au client de se connecté à activeMQ pour collecter les étapes relatives à sa demande. <br/>
Si il n'y pas de vélo dans la station de départ, alors elle est retiré de la liste des stations potentilles. <br/>
Si il n'y a pas de place dans la station d'arrivé, alors elle est retiré de la liste des stations potentielles.<br/>

# Gestion des erreurs.

Si le client met n'importe quoi, le routing server renvois un string avec "unknow city"<br/>
Si le routing server ne parvient pas à trouvé de station de vélo dans l'une des villes <br/>
renseigné par le client, il renvoit une erreur "No station".<br/>
Si le proxy rencontre des porblemes, le routing serveur renvoie une erreur "No station"<br/>

# Procedure d'execution 

### Pour executer le back

RoutingServer + Proxy + ActiveMQ<br/>
Le script run_proxy_activeMQ_rontingServer.bat permet de tout executer en même temps.<br/>

Sinon, 
Pour exe le serveur : RoutingServer/bin/Debug/RoutingServer.exe<br/>
Pour exe le proxy : Proxy/bin/Debug/Proxy.exe<br/>
Pour exe ActiveMQ : apache-activemq-5.17.2/bin/activemq start<br/>

### Pour executer le front

