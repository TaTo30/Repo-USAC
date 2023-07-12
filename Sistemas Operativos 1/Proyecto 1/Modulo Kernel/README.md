# Instalacion

1. Instalar los headers de linux
   
   Ubuntu-Debian:
   ```
   $ sudo apt-get linux-headers-$(uname -r)
   ```

   CentOS: 
   ```
   $ sudo yum kernel-devel
   
   ```

2. Instalar la libreria Make

    Ubuntu-Debian:
    ```
    $ sudo apt-get -y make
    ```
    CentOS: 
    ```
    $ sudo yum -y make
    ```

3. Navegar al directorio donde tienen el `Makefile` y el `hardinfo.c` y compilar el modulo ejecutando el siguiente comando

    ```bash
    $ sudo make
    ```

5. Instalar el modulo con el siguiente comando

    ```bash
    $ sudo insmod hardinfo.ko
    ```

6. Comprobar la instalacion con el siguiente comando

    ```
    $ sudo cat /proc/hardinfo
    ```