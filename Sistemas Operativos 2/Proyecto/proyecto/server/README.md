# Proyecto 1 Sistemas Operativos 2

La práctica consiste en la creación de una aplicación web, tipo red social. La cual
tendrá funciones básicas, como inicio de sesión, postear fotos y agregar amigos
(estas se detallan más adelante). La parte importante es el uso de pruebas
unitarias p

## API
---
### Crear publicación

`https://localhost:3000/posts/:username/create`

**Petición**

```
    {
        "text": "Mi primera publicación",
        "image": "/9j/4AAQSkZJRgABAQEAYABgAAD/4R..."
    }
```

**Respuesta**
```
    {
        message: "Éxito/Fracaso"
    }
```
### Obtener publicaciones del usuario (propias)

`https://localhost:3000/posts/:username/`

**Petición**

```    
    https://localhost:3000/posts/ana12/
```
**Respuesta**
```
    {
        "posts": [
            {
                "_id": "623a1034be40b39da811178e",
                "date": "2022-03-22T18:06:44.354Z",
                "text": "Adiós, esta es mi última publicación",
                "image": "2022-03-22T18:06:44.339Z-30220263-814a-46aa-ac9d-b25c6ce929b0.jpg",
                "__v": 0
            },
            {
                "_id": "623a0fcbbe40b39da811178a",
                "date": "2022-03-22T18:04:59.983Z",
                "text": "Hola, esta es mi primer publicacion",
                "image": "2022-03-22T18:04:59.952Z-683ae930-b4fb-4819-827e-0cd4c8f30308.jpg",
                "__v": 0
            }            
        ]
    }
```
---
### Enviar Solicitud de amistad

`https://localhost:3000/requests/send`

**Petición (POST)**

```
    {
        "from" : "pedro",   --> El username que esta enviando la solicitud
        "to": "juan"        --> El username a quien se le esta enviando la solicitud
    }
```

**Respuesta**
```
    {
        message: "Éxito/Fracaso"
    }
```
---
### Obtener solicitudes de amistad

`https://localhost:3000/requests/:username`

**Petición (GET)**

```    
    https://localhost:3000/requests/juan
```

**Respuesta**
```
    {
        "requests": [
             {
                "_id": "62422eec0dc0f909bf9708ef",
                "from": "pedro",         --> Username quien la envio
                "to": "juan",           --> Username a quien le llego
                "status": "pending",    --> status: pending, rejected, accepted
                "__v": 0
            }
        ]
    }
```
---
### Rechazar solicitud de amistad

`https://localhost:3000/requests/:idRequest/reject`

El id debe ser el ID de la solicitud, ese se obtiene con la de listar solicitudes

**Petición (POST)**

```
    https://localhost:3000/requests/623f63507f0875de4196576e/reject
```

**Respuesta**
```
    {
        message: "Éxito/Fracaso"
    }
```
### Aceptar Solicitud de Amistad

`https://localhost:3000/requests/:idMyUser/accept/:idRequest`

    idMyUser: El id del usuario que esta aceptando la solicitud
    idRequest: El id de la solicitud que esta aceptando

**Petición (POST)**

```
   http://localhost:3000/requests/62422ed70dc0f909bf9708ec/accept/62422eec0dc0f909bf9708ef
```

**Respuesta**
```
    {
        message: "Éxito/Fracaso"
    }
```

### Obtener lista de no amigos

`https://localhost:3000/notFriends/:id`

    id: Id del usuario que se desea consultar

**Petición (GET)**

```
  http://localhost:3000/notFriends/62422ecc0dc0f909bf9708e9
```

**Respuesta**
```
     [
        {
            "_id": "624236f3f0f2dbec5c9dd61b",
            "name": "carlos",
            "username": "carlos",
            "password": "$2b$10$ce5pUeVk46D87B0fjkC.WuR6FTfaXJyYf913OUEixl/cQY8.CL67a",
            "posts": [],
            "requests": [],
            "friends": [],
            "photo": "https://ayd1-practica2.s3.amazonaws.com/2022-03-28T22:30:11.463Z-fdc00859-d4f1-4fe3-99b5-f501ac97a891.jpg",
            "__v": 0
        }
    ]
```

### Editar usuario por id

`http://localhost:3100/user/update/:id`

El id debe ser el ID del usuario, si se desea cambiar la contraseña se debe conocer previamente ya que en la bd momentaneamente esta unicamente para que se muestre encriptada. Se debe mandar el username, la password, el name y la foto. Si alguna de estas no se cambia se deja el mismo dato

**Petición (POST)**

```
    http://localhost:3100/user/update/6241637cf5402c73a0616781

```

**Respuesta**
```
    {
    "status": 200,
    "message": "El usuario kirbyyRosado ha sido modificado"
    }
```