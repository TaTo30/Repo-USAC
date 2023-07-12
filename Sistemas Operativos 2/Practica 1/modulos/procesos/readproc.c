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


#include <asm/processor.h>
#include <asm/mmu_context.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Grupo 6 Sistemas Operativos 2");
MODULE_DESCRIPTION("Procesos");
MODULE_VERSION("0.01");

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

static ssize_t my_proc_write(struct file *file, const char __user *buffer, size_t count, loff_t *f_pos){
    return 0;
}

static int my_proc_open(struct inode *inode, struct file *file){
    return single_open(file, my_proc_show, NULL);
}

static struct file_operations my_fops = {
    .open = my_proc_open,
    .release = single_release,
    .read = seq_read,
    .llseek = seq_lseek,
    .write = my_proc_write
};

static int __init init_p(void){
        struct proc_dir_entry *entry;
        entry = proc_create("procs_grupo6", 0777, NULL, &my_fops);
        if(!entry) {
                return -1;
        } else {
                printk(KERN_INFO "Grupo6, monitor de procesos \n");
        }
        return 0;
}

static void __exit exit_p(void){
        remove_proc_entry("procs_grupo6",NULL);
        printk(KERN_INFO "Grupo 6, monitor de procesos \n");
}

module_init(init_p);
module_exit(exit_p);