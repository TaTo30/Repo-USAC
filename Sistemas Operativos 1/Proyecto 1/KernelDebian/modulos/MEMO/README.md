se instalaron las librerias:
sudo apt-get gcc
sudo apt-get g++
sudo apt-get make
sudo apt-get golang
sudo apt-get git

----------------------------------
se utilizaron los comandos:
make all 
para crear el archivo .KO, el cual permite montar el archivo de la memoria

make clean
para eliminar el archivo .KO
--------------------------------------------
para montar el archivo memo_carnet se utilizo:

sudo insmod memo_carnet.KO

para desmontar el archivo memo_carnet se utilizo:

sudo rmmod memo_carnet.KO
----------------------------------------
vara visualiar el contenido del archivo memo_carnet, se debe redirigir a la carpeta /proc y en dicha carpeta escribir el comando
cat memo_carnet
----------------------------
el archivo makefile se copia tal cual
