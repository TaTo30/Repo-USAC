# MANUAL DE DESPLIEGUE

## **INSTALAR LA APLICACION**

`kubectl create -f deploy.yaml`

## **COMPROBAR LA INSTALACION**

### DEPLOYMENTS

`kubectl get deployments`

### SERVICES

`kubectl get services`

### PODS

`kubectl get pods`

##### PODS CON DETALLE

`kubectl describe pods <deployment>`

## **ACTUALIZACIONES**

### ROLLING UPDATE

`kubectl set image deployments/so2-frontend webapp=tato30/sopes2-p1-frontend:2.0`

### ROLLBACK

`kubectl rollout undo deployments/so2-frontend`