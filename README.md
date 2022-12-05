# Serveur

RoutingServer + Proxy + ActiveMQ

Le script run_proxy_activeMQ_rontingServer.bat permet de tout executer en même temps.

Sinon, 
Pour exe le serveur : RoutingServer/bin/Debug/RoutingServer.exe
Pour exe le proxy : Proxy/bin/Debug/Proxy.exe
Pour exe ActiveMQ : apache-activemq-5.17.2/bin/activemq start


# Client

Le Depart et Arrivee sont marqués en dur dans le code ligne 52.
La méthode getCheminAVelo renvoie un string qui comporte un GUID. 
Ce GUID permet de se co à activeMQ avec la demande du client.
A la ligne 93 / 94, le message de retour et parsé pour obtenir la latitude et la longitude.

# Etat

## Client 

Pour le moment le client recoit toute les données d'un coup. 
Evolution : Avoir un bouton sur l'interface pour prendre le chemin packet par packet. 
Faire un script ou l'ajouter dans le script des les serveurs pour son execution

## Serveur

Le serveur ne répond qu'a une user story. Celle ou le client entre 2 adresses valident et où aucun probleme n'est rencontré. 
Evolution : Prendre en consideration tous les cas où le service peut rencontrer un probleme. 