# Manual Técnico

## Librerias

> Para hacer uso de los siguientes modulos es necesario cargarlos y compilarlos con las siguientes librerias

```c
#include <linux/init.h>
#include <linux/module.h>
#include <linux/kernel.h>
#include <linux/debugfs.h>
#include <linux/seq_file.h>
#include <linux/spinlock.h>
#include <linux/sched/signal.h>
#include <linux/sched/task.h>
#include <linux/fs.h>
#include <linux/proc_fs.h>
#include <linux/seq_file.h>
```

## Estructura de Modulo

> El siguiente es un ejemplo de como estructurar un modulo kernel

```c
// LIBRERIAS

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Sopes 2 Grupo 6");
MODULE_DESCRIPTION("Descripicon de modulo");
MODULE_VERSION("0.01");

static int my_proc_show(struct seq_file *m, void *v)
{
 // REEMPLAZAR CON LOS METODOS my_proc_show DE CADA MODULO QUE SE DESCRIBIRA MAS ADELANTE
}

static ssize_t my_proc_write(struct file *file, const char __user *buffer, size_t count, loff_t *f_pos)
{
    return 0;
}

static int my_proc_open(struct inode *inode, struct file *file)
{
    return single_open(file, my_proc_show, NULL);
}

// ESTA ESTRUCTURA PUEDE CAMBIAR DEPENDIENDO LA VERSION DE LINUX A USAR, LOS CAMBIOS SE DESCRIBIRAN MAS ADELANTE
static struct file_operations my_fops = {
    .open = my_proc_open,
    .read = seq_read,
    .write = my_proc_write,
    .llseek = seq_lseek,
    .release = single_release
};

static int __init kernel_module_init_event(void)
{
    struct proc_dir_entry *entry;
    entry = proc_create("module", 0777, NULL, &my_fops);
    if (!entry)
    {
        return -1;
    }
    else
    {
        printk(KERN_INFO "El grupo 6 ha instalado el modulo\n");
    }
    return 0;
}

static void __exit kernel_module_exit_event(void)
{
    remove_proc_entry("module", NULL);
    printk(KERN_INFO "El grupo 6 ha removido el modulo\n");
}

module_init(kernel_module_init_event);
module_exit(kernel_module_exit_event);

```

## Consideraciones
 > Es posible que la estructura de ***my_fops*** del kernel cambie de una versión de linux a otra
 >
 > Para las versiones de linux 5.6 o superior, la estructura es la siguiente:

```c
static struct proc_ops my_fops = {
    .proc_open = my_proc_open,
    .proc_release = single_release,
    .proc_read = seq_read,
    .proc_lseek = seq_lseek,
    .proc_write = my_proc_write
};
```
> Para las versiones de linux inferiores a 5.6, la estructura es la siguiente:

```c
static struct file_operations my_fops = {
    .open = my_proc_open,
    .release = single_release,
    .read = seq_read,
    .llseek = seq_lseek,
    .write = my_proc_write
};
```

## Módulo de procesos de CPU

> Provee un resumen de los procesos del sistema y cada uno de sus hijos

```c
static int my_proc_show(struct seq_file *m, void *v){
    struct task_struct *task;
    int32_t registered = 0;
    int32_t running = 0;
    int32_t sleeping = 0;
    int32_t stopped = 0;
    int32_t zombie = 0;
    unsigned long rss;
    for_each_process(task){
        get_task_struct(task);
        registered++;
        if(task -> state == TASK_RUNNING || task -> state == TASK_TRACED ){
            running++;
        }else if(task -> state == TASK_INTERRUPTIBLE || task -> state == TASK_UNINTERRUPTIBLE ){
            sleeping++;
        }else if(task -> state ==  __TASK_STOPPED || task -> state ==  TASK_STOPPED ){
            stopped++;
        }else if(task -> state == EXIT_ZOMBIE || task -> state ==  EXIT_DEAD){
            zombie++;
        }else{
            sleeping++;
        }
  	}

    seq_printf(m, "{ \"registered\": %d , \"running\": %d , \"sleeping\": %d , \"stopped\": %d, \"zombie\": %d, \n \"data\": [", registered, running, sleeping, stopped, zombie);
    seq_printf(m, "{\"father\":null, ");
    seq_printf(m, "\"id\": 0, \"name\":\"Procesos\", \"estate\":\"Ejecutando\", \"ram\": 0, \"taskCodesize\":0, \"user\": \"0\" },\n");
    for_each_process(task){
        get_task_struct(task);    
        if (task->mm) {
            seq_printf(m, "{\"father\":%d, ", task->parent->pid);
            if(task -> state == TASK_RUNNING || task -> state == TASK_TRACED ){
                rss = get_mm_rss(task->mm) << PAGE_SHIFT;
                seq_printf(m, "\"id\": %d, \"name\":\"%s\", \"estate\":\"%s\", \"ram\": %lu, \"taskCodesize\":%lu",task->pid, task->comm, "Running", rss, task->mm->end_code - task->mm->start_code);
            }else if(task -> state == TASK_INTERRUPTIBLE || task -> state == TASK_UNINTERRUPTIBLE ){
                rss = get_mm_rss(task->mm) << PAGE_SHIFT;
                seq_printf(m, "\"id\": %d, \"name\":\"%s\", \"estate\":\"%s\", \"ram\": %lu, \"taskCodesize\":%lu",task->pid, task->comm, "Sleeping", rss, task->mm->end_code - task->mm->start_code);
            }else if(task -> state ==  __TASK_STOPPED || task -> state ==  TASK_STOPPED ){
                rss = get_mm_rss(task->mm) << PAGE_SHIFT;
                seq_printf(m, "\"id\": %d, \"name\":\"%s\", \"estate\":\"%s\", \"ram\": %lu, \"taskCodesize\":%lu",task->pid, task->comm, "Stoped", rss, task->mm->end_code - task->mm->start_code);
            }else if(task -> state == EXIT_ZOMBIE || task -> state ==  EXIT_DEAD){
                rss = get_mm_rss(task->mm) << PAGE_SHIFT;
                seq_printf(m, "\"id\": %d, \"name\":\"%s\", \"estate\":\"%s\", \"ram\": %lu, \"taskCodesize\":%lu",task->pid, task->comm, "Zombie", rss, task->mm->end_code - task->mm->start_code);
            }
           
        }else{
            seq_printf(m, "{\"father\":%d, ", task->parent->pid);
            seq_printf(m, "\"id\": %d, \"name\":\"%s\", \"estate\":\"%s\", \"ram\": 0, \"taskCodesize\":0",task->pid, task->comm, "sleeping");
        }
        if( task->cred->uid.val == 0){
            seq_printf(m,", \"user\":  \"root\"},\n");
        }else{
            seq_printf(m,", \"user\":  \"%d\"},\n", task->cred->uid.val );
        }
    }
    seq_printf(m, "] \n }");
    return 0;
}
```

> Devuelve de salida un objeto JSON con el resumen de los procesos del sistema y un arreglo de objetos con la informacion de cada proceso

```json
{ 
    "registered": 101 ,
    "running": 1 ,
    "sleeping": 100 ,
    "stopped": 0,
    "zombie": 0,
    "data": [
        {
            "father":null,
            "id": 0,
            "name":"Procesos",
            "estate":"Ejecutando",
            "ram": 0,
            "taskCodesize":0,
            "user": "0" 
        }
        // ... Mas procesos
    ]
}
```

## Modulo de memoria RAM

> Este modulo provee un resumen básico del uso de la memoria RAM del sistema

```c
static int my_proc_show(struct seq_file *m, void *v)
{
    struct sysinfo i;
    si_meminfo(&i);


    seq_printf(m, "{\"total\": %lu, \"free\": %lu, \"use\": %lu}", i.totalram * 4, i.freeram * 4, i.totalram * 4 - i.freeram * 4);
    return 0;
}
```
> Devuelve de salida un objeto JSON con la informacion correspondiente

```json
{
    "total": 11759156,
    "free": 5674016,
    "use": 6085140
}
```

## Instalacion

> Una vez escrito los modulos, se tendran que compilar e instalar en el kernel del sistema, los pasos son los siguientes:

### Instalar las dependencias

```console
# Para instalar las librerias de linux
$ sudo apt install linux-headers-$(uname -r)

# Para compilar el modulo
$ sudo apt install gcc

# Para montar el modulo en el kernel
$ sudo apt install make
```

### Compilar e instalar el modulo

```console
# Compilar el modulo
$ sudo make

# Instalar en la carpeta /proc
$ sudo insmod <NOMBRE_DEL_MODULO>.ko

# Usar el modulo
$ sudo cat /proc/<NOMBRE_DEL_MODULO>
```

# Manual de Usuario

## Dashboard - Monitor

- Memoria RAM
  - Se muestra la memoria ram del servidor, ram consumida, ram libre y el porcentaje de consumo
    ![](https://cdn.discordapp.com/attachments/804579277567819807/945117443494412298/unknown.png) 

- Utilizacion de RAM
  - Se muestra gráfica de poligono de frecuencias del porcentaje de uso de ram
    ![](https://cdn.discordapp.com/attachments/804579277567819807/945117690706690138/unknown.png) 
## Dashboard - Procesos


- Procesos
  - Se muestra la cantidad de procesos registrados, corriendo, durmiendo, parados y en estado zombie.
    ![](https://cdn.discordapp.com/attachments/804579277567819807/945118025651224637/unknown.png) 

- Arbol de procesos
  - Se muestra TreeView de procesos de la máquina, se deberá ingresar el PID del proceso que se desea ejecutar KILL o STRACE
    ![](https://cdn.discordapp.com/attachments/804579277567819807/945118194069295124/unknown.png) 

- Strace
  - Se muestra la salida del comando strace, es necesario ingresar PID en el apartado Arbol de Procesos
    ![](https://cdn.discordapp.com/attachments/804579277567819807/945118481865666620/unknown.png) 
    