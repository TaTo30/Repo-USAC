#include <linux/module.h> 
#include <linux/kernel.h>
#include <linux/init.h>
#include <linux/list.h>
#include <linux/types.h>
#include <linux/slab.h>
#include <linux/sched.h>
#include <linux/string.h>
#include <linux/fs.h>
#include <linux/seq_file.h>
#include <linux/proc_fs.h>
#include <asm/uaccess.h> 
#include <linux/hugetlb.h>
#include <linux/mm.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Grupo 6 -  Sistemas Operativos 2");
MODULE_DESCRIPTION("Kill Process");
MODULE_VERSION("0.01");

static int my_proc_show(struct seq_file *m, void *v){
    //Kill Proc

    return 0;
}

static ssize_t my_proc_write(struct file *file, const char __user *buffer, size_t count, loff_t *f_pos){
    return 0;
}

static int my_proc_open(struct inode *inode, struct file *file){
    return single_open(file, my_proc_show, NULL);
}

static struct proc_ops my_fops = {
    .proc_open = my_proc_open,
    .proc_release = single_release,
    .proc_read = seq_read,
    .proc_lseek = seq_lseek,
    .proc_write = my_proc_write
};

static int __init init_p(void){
        struct proc_dir_entry *entry;
        entry = proc_create("mem_grupo6", 0777, NULL, &my_fops);
        if(!entry) {
                return -1;
        } else {
                printk(KERN_INFO "Grupo 6 - killprocess \n");
        }
        return 0;
}

static void __exit exit_p(void){
        remove_proc_entry("mem_grupo6",NULL);
        printk(KERN_INFO "Grupo 6 - killprocess \n");
}

module_init(init_p);
module_exit(exit_p);