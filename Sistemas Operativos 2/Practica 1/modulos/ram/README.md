# Instalacion
## Instalacion de dependencias
 `$ sudo apt-get linux-headers-$(uname -r) make gcc`

## Compilar el modulo
`$ sudo make`

## Instalar el modulo
`$ sudo insmod ram_mem_g6.ko`

## Usar el modulo
`$ sudo cat /proc/ram_mem_g6`

### Devuelve la siguiente salida
```json
{
    'total': 11759156,
    'free': 5674016,
    'use': 6085140
}
```