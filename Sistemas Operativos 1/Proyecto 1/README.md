# Proyecto 1

## Integrantes

|          Nombre              |    Carne    |
|------------------------------|-------------|
|Aldo Rigoberto Hernandez Avila|**201800585**|
|Cinthya Andrea Palomo Galvez  |**201700670**|
|Jose Fernando Guerra Muñoz    |**201731087**|


## Cuentas de Nube

> **Azure**
> > *Cuenta:* gruporedes24@outlook.com
> >
> > *Pass:* ##2021Sopes
> 
> **Google Cloud**
> >  *Cuenta:* gruporedes24@gmail.com
> >
> > *Pass:* ##2021Sopes


# Manual Técnico
> ¿Qué generador de tráfico es más rápido? ¿Qué diferencias hay entre las implementaciones de los generadores de tráfico?
> > >
> ![image](https://user-images.githubusercontent.com/36779113/135778402-7175be2d-7c42-4eae-bd94-ba7d68c34038.png)
> ![image](https://user-images.githubusercontent.com/36779113/135778881-bee94a8b-65b4-4822-a4d2-32cc3df70fe8.png)
> ![image](https://user-images.githubusercontent.com/36779113/135877604-58c9c2ca-453b-4fa3-981d-99f49a7abbfb.png)
> - El generador más rápido a nuestra consideración fue el de golang debido a que tuvo menores tiempos en que se incertaron los datos, por lo que respecta al tiempo fue más eficiente.
> > >
> ¿Qué lenguaje de programación utilizado para las APIs fue más óptimo con relación al tiempo de respuesta entre peticiones? ¿Qué lenguaje tuvo el performance menos óptimo?
> Según las gráficas pudimos observar que la de mayor eficiencia y óptimo corresponde a la de python, en cambio el que obtuvo menos performance menos óptimo fue el de Rust.
> - ![image](https://user-images.githubusercontent.com/36779113/136286634-dec516c4-326c-4374-b5d7-727d2264eeda.png)

> > >
> ¿Cuál de los servicios de Google Cloud Platform fue de mejor para la implementación de las APIs? ¿Cuál fue el peor? ¿Por qué?
> - La mejor a nuestro parecer fue el Cloud Run, porque hace uso de contenedores, y el peor fue el de Cloud Function, porque depende como este preparada la plataforma para ejecutar esa aplicación y no es versátil.
> > > 
> ¿Considera que es mejor utilizar Containerd o Docker y por qué?
> - Es mejor usar docker, porque facilidad de uso, accesible y está orientado a los usuarios menos experimentados.
> > >
> ¿Qué base de datos tuvo la menor latencia entre respuestas y soportó más carga en un determinado momento? ¿Cuál de las dos recomendaría para un proyecto de esta índole?
> - La que soportó más carga en un determinado momento fue la de CosmoDB, además de el tiempo por consulta fue más rápido en comparación con MySQL. La que recomendamos es la de CosmoDB.  
> ![image](https://user-images.githubusercontent.com/36779113/136142472-c89d7fc7-e036-4a0e-9a51-feddc58caa62.png)
> ![image](https://user-images.githubusercontent.com/36779113/136142501-2f9dfd7f-bb14-4aba-b462-cba92160c7f3.png)
> > >
> Considera de utilidad la utilización de Prometheus y Grafana para crear dashboards, ¿Por qué?
> - Si, lo consideramos muy útil porque ayuda a ver de forma gráfica y sencilla cada dato y así poderlo analizar con detenimiento y ver el factor de utilización de cada variable o proceso del sistema.
> - - ![image](https://user-images.githubusercontent.com/36779113/135707599-e81eceee-d4c5-4152-ad39-c4cc425f2dcb.png)

### Base de Datos ER
![image](https://user-images.githubusercontent.com/36779113/135707489-ee4e7dce-aa2d-4514-b63d-06fcd8def95f.png)

### Screenshoots de participacion para extras

![image](https://user-images.githubusercontent.com/36779113/136140940-975bea6a-452a-49ba-a1a1-625e3497dc6c.png)
![image](https://user-images.githubusercontent.com/36779113/136141155-06c2c189-8886-4bd7-99cb-4f2b58b57fd2.png)


# Manual de Usuario
### Página Principal:
#### Por DB:
> En esta vista se presentan todos los twits en base a la DB elegida
> ![image](https://user-images.githubusercontent.com/36779113/136143073-ffd08811-3c78-4337-a923-8989f4be5632.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143089-5e32f6b5-4398-4562-b3d1-77ca51b0bf73.png)

### Reportes:
> Este contiene todos los reportes con sus respectivas gráficas, respecto a la DB 
> Las funciones principales son:
> - Mostrar el total de noticias, upvotes y hashtags (diferentes) que hay en el sistema.
> - Tabla con los datos que están en el sistema.
> - Gráfica circular del top 5 de hashtags. Se calculó a partir de la cantidad de upvotes que tiene cada hashtag.
> - Gráfica (de barras, líneas, stackeada, etc…) que compare la cantidad de upvotes y downvotes por día.
> ![image](https://user-images.githubusercontent.com/36779113/136143347-11c15888-3aaf-491d-bdb0-52e4fb3b8cea.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143401-112eeb28-ddac-459c-8228-21420dcf02f3.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143421-fb3c67f2-5f28-4c12-b8ed-89f1f2c16374.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143435-5801820e-cc64-4582-9812-0bffaad17f25.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143447-eca15646-d657-4699-b419-4bb05071374a.png)
> ![image](https://user-images.githubusercontent.com/36779113/136143464-472da339-4a07-44bf-91d8-c73d20f56b94.png)


### Grafana
#### Para instalar el programa se realiza con los siguientes comandos para instalar Prometheus:
``` python
wget https://github.com/prometheus/prometheus/releases/download/v2.30.1/prometheus-2.30.1.linux-amd64.tar.gz

tar -xzf prometheus-2.30.1.linux-amd64.tar.gz

cd prometheus-2.30.1.linux-amd64

./prometheus
```
> - Se desplegará una pantalla en el localhost con el puerto 9090
> - Se configura el prometheus.yml para desplegar la los datos y páginas
``` java
 global:
  scrape_interval:     15s # By default, scrape targets every 15 seconds.

  # Attach these labels to any time series or alerts when communicating with
  # external systems (federation, remote storage, Alertmanager).
  external_labels:
    monitor: 'codelab-monitor'

# A scrape configuration containing exactly one endpoint to scrape:
# Here it's Prometheus itself.
scrape_configs:
  # The job name is added as a label `job=<job_name>` to any timeseries scraped from this config.
  - job_name: 'prometheus'

    # Override the global default and scrape targets from this job every 5 seconds.
    scrape_interval: 5s

    static_configs:
      - targets: ['localhost:9090']
```
> ![image](https://user-images.githubusercontent.com/36779113/136144462-ed00d9bb-ee6c-4cc7-808e-bf5ead322d80.png)

#### Para instalar grafana se necesita:
``` python
wget https://dl.grafana.com/enterprise/release/grafana-enterprise-8.1.5.linux-amd64.tar.gz

tar -zxvf grafana-enterprise-8.1.5.linux-amd64.tar.gz

cd grafana-8.1.5/

./bin/grafana-server
```
> - Este se desplegará en el puerto 3000 y enviará a la pantalla de login para ingresar colocar los datos mostrarodos en la siguiente imagen
> ![image](https://user-images.githubusercontent.com/36779113/136144723-d2dc3e03-7998-49fb-a3a8-143370fa9333.png)
> - Seguido de esto redirige a la página principal donde se muestran los dashboard, en la parte superior izquierda se puede agregar carpetas a los dashboard
> ![image](https://user-images.githubusercontent.com/36779113/136145072-caf32c6b-8db9-46db-89fb-fb9befac590c.png)
> - Se agregará como base de datos Prometheus que está en constante escucha de los tags agregados
> ![image](https://user-images.githubusercontent.com/36779113/136145185-b1b510d7-b9b7-4e0c-9b23-11fe9180b888.png)
> ![image](https://user-images.githubusercontent.com/36779113/136145264-f3151984-8817-429d-9a7d-1de60e9c5550.png)
> - Cada Dashboard se pueden guardar en carpetas
> ![image](https://user-images.githubusercontent.com/36779113/136145522-a69293bd-07d9-4715-9cba-89efd7462c35.png)
> - Se puede agregar cada panel con su respectiva gráfica
> ![image](https://user-images.githubusercontent.com/36779113/136145583-f16de3aa-d22f-414a-b453-5244f3c40cae.png)
> - La configuración de cada gráfica corresponde a cada ruta de tag de prometheus con su ruta /metrics, el cual se despliega las variables y datos de esta manera
``` javascript
# HELP go_total_ram cantidad total.
# TYPE go_total_ram gauge
go_total_ram 1993
```
> - La variable go_total_ram debería aparecer cuando se quiere agregar una gráfica por medio de querys 
> ![image](https://user-images.githubusercontent.com/36779113/136145861-164cd3c3-3d13-40d8-877f-d70284780800.png)
> - Luego salvará y aplicará para que aparezcan las gráficas con respecto a las variables 
> ![image](https://user-images.githubusercontent.com/36779113/136146049-7d76abd5-0533-4cdb-b3cc-e6cbba6fea02.png)





