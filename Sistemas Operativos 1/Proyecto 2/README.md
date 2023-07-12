# Sistemas Operativos - Proyecto 2

## Integrantes

| Nombre                         | Carne               |
| ------------------------------ | ------------------- |
| Aldo Rigoberto Hernandez Avila | **201800585** |
| Cinthya Andrea Palomo Galvez   | **201700670** |
| Jose Fernando Guerra Muñoz    | **201731087** |

# Manual Tecnico

### Preguntas

> - Cómo funcionan las métricas de oro, cómo puedes interpretar las 7 pruebas de faulty traffic, usando como base los gráficos y métricas que muestra el tablero de Linkerd Grafana.
> - Menciona al menos 3 patrones de comportamiento que hayas descubierto en las pruebas de faulty traffic
> - ¿Qué sistema de mensajería es más rápido? ¿Por qué?
> - ¿Cuántos recursos utiliza cada sistema de mensajería?
> - ¿Cuáles son las ventajas y desventajas de cada servicio de mensajería?
> - ¿Cuál es el mejor sistema de mensajería?
> - ¿Cuál de las dos bases de datos se desempeña mejor y por qué?
> - ¿Cómo se reflejan en los dashboards de Linkerd los experimentos de Chaos Mesh?
> - ¿En qué se diferencia cada uno de los experimentos realizados?
> - ¿Cuál de todos los experimentos es el más dañino?

## Modelo de Base de Datos

> - MongoDB: es una base de datos NoSQL documental que almacena la información utilizando el formato de datos JSON. Un ejemplo de log se
> - Redis: es una base de datos NoSQL de clave-valor que implementa distintos tipos de estructuras de datos como listas, conjuntos, conjuntos ordenados, etc.

```JSON

{
  "request_number": number
  "game": number
  "gamename": string
  "winner": string
  "players": number
  "worker": string
} 
```

### Preguntas Base de Datos

> - ¿Cuál de las dos bases se desempeña mejor y por qué?
>
> - - debido a que están diseñados para diferentes propósitos, las capacidades mejoradas de Redis aumentan significativamente las capacidades de MongoDB, ya que MongoDB es una base de datos basada en disco y orientada a documentos optimizada para la simplicidad operativa, mientras que Redis es un almacén de estructura de datos persistente en memoria que permite realizar operaciones comunes con una complejidad mínima y un rendimiento máximo.

# Manual de Usuario

## Puntos Extras

![image](https://user-images.githubusercontent.com/36779113/141734871-47997ebe-bd4a-41b4-99e3-21d40fc5a6d4.png)
