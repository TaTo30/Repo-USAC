#REPORTE 4

#Leemos los datos del archivo CSV

PRESION <- read.csv(file = 'C:/Users/Aldo_/Documents/Usac/SS2/Practica1/PresionSanguineaEdad.csv', header = TRUE)


#*****INCISO A*****

#Hacemos una correlacion entre la presion y la edad
correlacion <- cor(PRESION$Edad, PRESION$Systolic.Blood.Pressure)
correlacion

#La correlacion es igual 0.6575, 
#Por lo tanto podemos considerar que existe cierto grado de correlacion entre ambas variables

#*****INCISO B*****

#Hacemos una regresion lineal entre los datos
Edad <- c(PRESION$Edad)
Presion <- c(PRESION$Systolic.Blood.Pressure)
EdadPresion <- data.frame(Presion, Edad)

regresion <- lm(Presion ~ Edad, data = EdadPresion)
summary(regresion)

# Modelo obtenido:
# Presion = 98.7147 + 0.9709 * Edad

# Graficamos la regresion Normal Q-Q
plot(regresion)

# 201800585 - ALDO RIGOBERTO HERNANDEZ AVILA 
