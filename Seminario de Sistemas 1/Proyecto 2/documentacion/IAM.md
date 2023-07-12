# Descripción de Usuarios

### Administrador

El usuario llamado `Administrador_201800585` tiene asiganda una política llamada `AdministratorAccess`. Tiene los permisos necesarios para poder administrar todos los servicion que ofrece Amazon, para este proyecto dicho usuario se utilizó para crear las instacias de EC2 para alojar los servicios desarrollados. 

### Rekognition 
El usuario llamado `rekognitionManger` tiene asiganda una política administrada por AWS, llamada `AmazonRekognitionFullAccess`. Utiliza el servicio rekognition para poder obtener todas las posibles etiquetas que tiene una imagen y crear la funcionalidad del filtrado de las publicaciones. 

### Translate 
El usuario llamado `translateManager ` tiene asiganda una política administrada por AWS, llamada `TranslateFullAccess`. Utiliza el servicio Translate para poder traducir el texto de una publicación a español desde cualquier idioma. 

### S3 
El usuario llamado `s3manager ` tiene asiganda una política administrada por AWS, llamada `AmazonS3FullAccess`. Administra un Bucket de S3 para poder almacenar y obtener todas la imagenes de perfil de los usuario e imagenes subidas en las publicaciones. 

