#REPORTE3

#Leemos los datos del archivo CSV
MUERTES <- read.csv(file = 'C:/Users/Aldo_/Documents/Usac/SS2/Practica1/MuertesSexoEdad.csv', header = TRUE)
MUERTES <- data.frame(MUERTES)

#Eliminamos la palabra Total
MUERTES <- MUERTES[MUERTES$Sex != "Total",]
MUERTES <- MUERTES[MUERTES$Age != "Total",]



#*****INCISO A*****

# Sacamos la frecuencia de los rangos de edad
frecuencia_absoluta <- table(MUERTES$Age)
frecuencia_absoluta


#*****INCISO B*****
hist(frecuencia_absoluta, main = "Histograma de Frecuencia Absoluta", ylab = "Edad", col = "yellow", xlab = "Muertes")


#*****INCISO C*****
plot(frecuencia_absoluta,type="l",col="red", ylab=" ", main="Poligono de frecuencias")


#*****INCISO D*****
frecuencia_acumulada <- cumsum(frecuencia_absoluta)
frecuencia_acumulada

barplot(frecuencia_acumulada, main = "Frecuencia Acumulada", col = "orange", xlab = "Edad", ylab = "Muertes")

# 201800585 - ALDO RIGOBERTO HERNANDEZ AVILA

