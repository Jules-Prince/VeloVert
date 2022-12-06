# Fonctionnalité

Nous possédons un serveur (routing server) qui est le point d'entré pour un client. 
Un proxy est mis en place pour mettre en cache les reponses aux requetes faites à JCDecaux. 
Lors que le client fait une requete, il recoit une string qui comporte un GUID.
Ce GUID permet au client de se connecté à activeMQ pour collecter les étapes relatives à sa demande. 
Si il n'y pas de vélo dans la station de départ, alors elle est retiré de la liste des stations potentilles. 
Si il n'y a pas de place dans la station d'arrivé, alors elle est retiré de la liste des stations potentielles.

# Gestion des erreurs.

Si le client met n'importe quoi, le routing server renvois un string avec "unknow city"
Si le routing server ne parvient pas à trouvé de station de vélo dans l'une des villes 
renseigné par le client, il renvoit une erreur "No station".
Si le proxy rencontre des porblemes, le routing serveur renvoie une erreur "No station"

# Procedure d'execution 

### Pour executer le back

RoutingServer + Proxy + ActiveMQ
Le script run_proxy_activeMQ_rontingServer.bat permet de tout executer en même temps.

Sinon, 
Pour exe le serveur : RoutingServer/bin/Debug/RoutingServer.exe
Pour exe le proxy : Proxy/bin/Debug/Proxy.exe
Pour exe ActiveMQ : apache-activemq-5.17.2/bin/activemq start

### Pour executer le front

