#REPORTE1

#Leemos los datos del archivo Ventas del csv
VENTAS <- read.csv(file = 'C:/Users/Aldo_/Documents/Usac/SS2/Practica1/VENTAS.csv', header = TRUE)
VENTAS <- data.frame(VENTAS)

#*******INCISO A*********

#Se aplica el filtro para poder obtener unicamente los datos necesarios
filtro <-  VENTAS[VENTAS$Country == "Guatemala"| VENTAS$Country == "El Salvador" | VENTAS$Country == "Honduras" | VENTAS$Country == "Nicaragua" | VENTAS$Country == "Costa Rica" | VENTAS$Country == "Panama" | VENTAS$Country == "Belice" ,]

# Se obtiene la frecuencia absoluta y se guarda en una tabla
frecuencia_absoluta <- table(filtro$Country)
frecuencia_absoluta

#*******INCISO B*********

# Generamos una grafica de barras para la frecuencia absoluta
barplot(frecuencia_absoluta, main = "Ventas Centroamerica", col = "cyan", xlab = "Paises", ylab = "Frecuencia")

#*******INCISO C*********

# Se obtiene la frecuencia acumulada, a partir de la frecuencia absoluta
frecuencia_acumulada <- cumsum(frecuencia_absoluta)
frecuencia_acumulada

#Generamos el histograma para las frecuencias absolutas
hist(frecuencia_acumulada, main = "Histograma", ylab = "Paises", col = "pink", xlab = "Ventas")

# 201800585 - ALDO RIGOBERTO HERNANDEZ AVILA