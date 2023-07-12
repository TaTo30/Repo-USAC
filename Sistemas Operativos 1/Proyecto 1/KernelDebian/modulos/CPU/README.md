se instalaron las librerias:
sudo apt-get gcc
sudo apt-get g++
sudo apt-get make
sudo apt-get golang
sudo apt-get git

----------------------------------
se utilizaron los comandos:
make all 
para crear el archivo .KO, el cual permite montar el archivo del cpu

make clean
para eliminar el archivo .KO
--------------------------------------------
para montar el archivo cpu_carnet se utilizo:

sudo insmod cpu_carnet.KO

para desmontar el archivo cpu_carnet se utilizo:

sudo rmmod cpu_carnet.KO
----------------------------------------
vara visualiar el contenido del archivo cpu_carnet, se debe redirigir a la carpeta /proc y en dicha carpeta escribir el comando
cat cpu_carnet
----------------------------
el archivo makefile se copia tal cual


