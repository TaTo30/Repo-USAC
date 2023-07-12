#include <linux/proc_fs.h>
#include <linux/seq_file.h>
#include <linux/module.h>
#include <linux/init.h>
#include <linux/kernel.h>
#include <linux/sched/signal.h>
#include <linux/sched.h>
#include <linux/fs.h>
#include <linux/version.h> 
struct task_struct *task; //estructura definida en sched.h para tareas/procesos
struct task_struct *childtask; //estructura necesaria para iterar a travez de procesos secundarios
struct task_struct *memtask; 
struct list_head *list; // estructura necesaria para recorrer cada lista de tareas tarea->estructura de hijos

static int escribir_archivo(struct seq_file * archivo, void *v){
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
			childtask= list_entry( list, struct task_struct, sibling );
	//printk(KERN_INFO "HIJO DE %s[%d]PID: %d PROCESO: %s ESTADO: %ld",task->comm,task->pid,childtask->pid, childtask->comm, childtask->state);
	seq_printf(archivo, "\t\"pid\": %d,\n",childtask->pid);
	seq_printf(archivo, "\t\"nombre\": \"%s\",\n",childtask->comm);
	seq_printf(archivo, "\t\"usuario\": \"root\",\n");
	seq_printf(archivo, "\t\"estado\": %ld,\n",childtask->state);
	//seq_printf(archivo, "\"ram\": %d\n",childtask->ram);
	seq_printf(archivo, "\t},\n");

	}
	seq_printf(archivo, "\t]\n");
	seq_printf(archivo, "},\n");
 
		printk("*****************");	
	}



	return 0;
}

#if LINUX_VERSION_CODE >= KERNEL_VERSION(4,18,0)
#define HAVE_PROC_CREATE_SINGLE
#endif



#ifndef HAVE_PROC_CREATE_SINGLE
static int skynet_open(struct inode *inode, struct  file *file) {
  return single_open(file, escribir_archivo, NULL);
}

static const struct file_operations skynet_fops = {
  .owner = THIS_MODULE,
  .open = skynet_open,
  .read = seq_read,
  .llseek = seq_lseek,
  .release = single_release,
};
#endif


static int __init skynet_init(void) {
#ifdef HAVE_PROC_CREATE_SINGLE
  proc_create_single("skynet", 0, NULL, skynet_show);
#else
  proc_create("skynet", 0, NULL, &skynet_fops);
#endif
  printk(KERN_INFO "Skynet in control\n");

  return 0;
}

static void __exit skynet_cleanup(void) {
  remove_proc_entry("skynet", NULL);
  printk(KERN_INFO "I'll be back!\n");
}

module_init(skynet_init);
module_exit(skynet_cleanup);
