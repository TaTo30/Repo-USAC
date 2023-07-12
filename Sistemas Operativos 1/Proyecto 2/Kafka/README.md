# Kafka

Deploy del kafka con kubernetes

## Crear el namespace

`kubectl create namespace squidgame`

## Desplegar cluster.yaml

`kubectl create -f ./cluster.yaml`

## Obtener y copiar la ip del LoadBalancer

Una vez desplegado el cluster se debe ejecutar el siguiente comando para obtener informacion del servicio donde se expone el cluster de kafka, copiar el valor de la propiedad **IP**

`kubectl describe service kafka-service -n squidgame`

## Configurar kafka.yaml

En el archivo de kafka.yaml se debe reemplazar las siguentes variables de entorono

- KAFKA_ADVERTISED_HOST_NAME
  - Colocar la IP del paso anterior
- KAFKA_URL
  - Colocar la IP mas el puerto 9092, es decir, IP:9092

## Desplegar el broker de kafka.yaml

`kubectl create -f ./kafka.yaml`
