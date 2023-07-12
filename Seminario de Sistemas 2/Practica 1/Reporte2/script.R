#REPORTE2

#Leemos los datos del archivo Ventas del csv
DESEMPENO <- read.csv(file = 'C:/Users/Aldo_/Documents/Usac/SS2/Practica1/DesempenioCPU.csv', header = TRUE)
DESEMPENO <- data.frame(DESEMPENO)



#*****INCISO A*****
#Obtenemos los datos de la compania amdahl
amdahl <- DESEMPENO[DESEMPENO$Compania == "amdahl" ,]

#Calculamos las metricas
maxFiltro <- max(amdahl$PRP)
minFiltro <- min(amdahl$PRP)
meanFiltro <- mean(amdahl$PRP)

#Graficamos las metricas
barplot(c(maxFiltro,meanFiltro,minFiltro), main = "Rendimiento de CPUs amdahl", col = "cyan", xlab = "Métricas", ylab = "PRP", names.arg = c("Máximo","Promedio","Mínimo"))


#*****INCISO B*****
#Instalamos las librerias para hacer el analisis correctamente
install.packages("dplyr")
library("dplyr")

#Agrupamos los datos por compania y calculamos la media de los datos agrupados
query <- DESEMPENO %>%  group_by(Compania) %>% summarise(mean = mean(PRP))

#Sacamos el maximo y el minimo
filtroMax <- max(query$mean)
filtroMin <- min(query$mean)
filtroTabla <- query[query$mean == filtroMax | query$mean == filtroMin ,]


barplot(filtroTabla$mean, main = "Rendimiento", col = "pink", xlab = "Compania", ylab = "Rendimiento Medio", names.arg = filtroTabla$Compania)

# 201800585 - ALDO RIGOBERTO HERNANDEZ AVILA

