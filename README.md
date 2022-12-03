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