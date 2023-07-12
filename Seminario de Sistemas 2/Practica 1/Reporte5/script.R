#REPORTE 5

#Leemos los datos del archivo CSV

DATOS <- read.csv(file = 'C:/Users/Aldo_/Documents/Usac/SS2/Practica1/PobrezaDesempleoAsesinatos.csv', header = TRUE)
DATOS <- data.frame(DATOS)

#*****INCISO A*****
#Sacamos los datos reales quitando los porcentajes

pobreza <- DATOS$Habitantes * (DATOS$Porcentaje.con.ingresos.debajo.de.5000 / 100)
pobreza <- data.frame(pobreza)

desempleo <- DATOS$Habitantes * (DATOS$Porcentaje.desempleado / 100)
desempleo <- data.frame(desempleo)

asesinatos <- DATOS$Habitantes * (DATOS$Asesinatos.por.1000000.habitantes / 100)
asesinatos <- data.frame(asesinatos)

#Hacemos una analisis correlativo entre pobreza-desempleo y pobreza-asesinato

pobrezaDesempleo <- cor(pobreza, desempleo)
pobrezaAsesinato <- cor(pobreza, asesinatos)

#La correlacion entre pobreza y desempleo es:
#0.995
#La correlacion entre pobreza y asesinatos es:
#0.923
#En ambos casos los datos esta muy relacionados

#*****INCISO B*****

#Hacemos la regresion lineal para los datos y extraemos el modelo
regresionPD <- lm(pobreza ~ desempleo, data = data.frame(pobreza, desempleo))
summary(regresionPD)
plot(regresionPD)
# El modelo obtenido es:
# Pobreza = -8934 + 2.984 * Desempleo

regresionPA <- lm(pobreza ~ asesinatos, data = data.frame(pobreza, asesinatos))
summary(regresionPA)
plot(regresionPA)
# El modelo obtenido es:
# Pobreza = -5015 + 0.9823 * Asesinatos

# 201800585 - ALDO RIGOBERTO HERNANDEZ AVILA
