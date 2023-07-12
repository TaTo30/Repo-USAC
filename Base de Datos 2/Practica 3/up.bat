docker run -h publisher --name publisher -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pa55w0rd!' -p 1470:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04
docker run -h distributor --name distributor -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pa55w0rd!' -p 1471:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04
docker run -h subscriber1 --name subscriber1 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pa55w0rd!' -p 1472:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04
docker run -h subscriber2 --name subscriber2 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pa55w0rd!' -p 1473:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04


docker exec -it -u 0 publisher  /opt/mssql/bin/mssql-conf set sqlagent.enabled true 
docker exec -it -u 0 distributor /opt/mssql/bin/mssql-conf set sqlagent.enabled true
docker exec -it -u 0 subscriber1  /opt/mssql/bin/mssql-conf set sqlagent.enabled true 
docker exec -it -u 0 subscriber2  /opt/mssql/bin/mssql-conf set sqlagent.enabled true 

docker exec -u 0 distributor mkdir /var/opt/mssql/ReplData

docker network connect replication publisher
docker network connect replication distributor
docker network connect replication subscriber1
docker network connect replication subscriber2


