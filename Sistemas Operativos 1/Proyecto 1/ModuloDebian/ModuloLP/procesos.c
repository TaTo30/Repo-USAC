#include <linux/module.h> 
#include <linux/kernel.h> 
#include <linux/init.h>
#include <linux/types.h>
#include <linux/slab.h>
#include <linux/sched.h>
#include <linux/string.h>
#include <linux/fs.h>
#include <linux/seq_file.h>
#include <linux/proc_fs.h>
#include <asm/uaccess.h> 
#include <linux/hugetlb.h>

#include <linux/sched/signal.h>

#define FileProc "procesos"


struct task_struct *task;
struct task_struct *task_child;
struct list_head *list;
int mextra;
int mextra2;

struct task_struct *task; //estructura definida en sched.h para tareas/procesos
struct task_struct *task_child; //estructura necesaria para iterar a travez de procesos secundarios
struct task_struct *memtask; 

static int proc_llenar_archivo(struct seq_file *archivo, void *v) {
	struct sysinfo i;
        si_meminfo(&i);
        seq_printf(archivo, "%lu\n%lu\n%lu\n%lu\n",(i.totalram * i.mem_unit)/1024, ((i.totalram - i.freeram) * i.mem_unit)/1024, (i.freeram * i.mem_unit)/1024,((i.totalram - i.freeram)*100)/i.totalram);
   for_each_process( task ){
seq_printf(archivo, "{\n");
	seq_printf(archivo, "\"pid\": %d,\n",task->pid);
	seq_printf(archivo, "\"nombre\": \"%s\",\n",task->comm);
	seq_printf(archivo, "\"usuario\": \"root\",\n");
	seq_printf(archivo, "\"estado\": %ld,\n",task->state);
	seq_printf(archivo, "\"hijo\":\n");
	seq_printf(archivo, "\t[\n");
		list_for_each( list,&task->children ){
	seq_printf(archivo, "\t{\n");
			task_child= list_entry( list, struct task_struct, sibling );
	//printk(KERN_INFO "HIJO DE %s[%d]PID: %d PROCESO: %s ESTADO: %ld",task->comm,task->pid,task_child->pid, task_child->comm, task_child->state);
	seq_printf(archivo, "\t\"pid\": %d,\n",task_child->pid);
	seq_printf(archivo, "\t\"nombre\": \"%s\",\n",task_child->comm);
	seq_printf(archivo, "\t\"usuario\": \"root\",\n");
	seq_printf(archivo, "\t\"estado\": %ld,\n",task_child->state);
	//seq_printf(archivo, "\"ram\": %d\n",task_child->ram);
	seq_printf(archivo, "\t},\n");

	}
	seq_printf(archivo, "\t]\n");
	seq_printf(archivo, "},\n");
 
		printk("*******");	
	}



	return 0;
}



static int proc_abrir_archivo(struct inode *inode, struct  file *file) {
  return single_open(file, proc_llenar_archivo, NULL);
}



static struct file_operations myops ={
        .owner = THIS_MODULE,
        .open = proc_abrir_archivo,
        .read = seq_read,
        .llseek = seq_lseek,
        .release = single_release,
};



static int inicializando(void){
    proc_create(FileProc,077,NULL,&myops);  
    printk(KERN_INFO "INICIAR LISTA PROCESOS");
    return 0;
}

static void finalizando(void){

    printk(KERN_INFO "FINALIZAR LISTA PROCESOS");
    remove_proc_entry(FileProc,NULL);
}



module_init(inicializando);
module_exit(finalizando);
MODULE_LICENSE("GPL");
MODULE_AUTHOR("Proyecto 1 Sopes");
MODULE_DESCRIPTION("Procesos");