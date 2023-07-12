# Manual Técnico
## Instalar Linkerd (proteus, grafana)
```
curl -fsL https://run.linkerd.io/install | sh
nano ~/.bashrc <- export PATH=$PATH:/home/YOUR_USER/.linkerd2/bin

linkerd install | kubectl apply -f -
linkerd check
linkerd viz install | kubectl apply -f -
linkerd check
```

## Abrir Linkerd dashboard
```
linkerd viz dashboard
```