#include <linux/module.h>
#include <linux/init.h>
#include <linux/proc_fs.h>
#include <linux/sched.h>
#include <linux/uaccess.h>
#include <linux/fs.h>
#include <linux/sysinfo.h>
#include <linux/seq_file.h>
#include <linux/slab.h>
#include <linux/mm.h>
#include <linux/swap.h>
#include <linux/timekeeping.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Aldo Hernandez - Sopes 2 Grupo 6");
MODULE_DESCRIPTION("Monitor de memoria RAM por el grupo 6");
MODULE_VERSION("0.01");

static int my_proc_show(struct seq_file *m, void *v)
{
    struct sysinfo i;
    si_meminfo(&i);


    seq_printf(m, "{\"total\": %lu, \"free\": %lu, \"use\": %lu}", i.totalram * 4, i.freeram * 4, i.totalram * 4 - i.freeram * 4);
    return 0;
}

static ssize_t my_proc_write(struct file *file, const char __user *buffer, size_t count, loff_t *f_pos)
{
    return 0;
}

static int my_proc_open(struct inode *inode, struct file *file)
{
    return single_open(file, my_proc_show, NULL);
}

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
    entry = proc_create("ram_mem_g6", 0777, NULL, &my_fops);
    if (!entry)
    {
        return -1;
    }
    else
    {
        printk(KERN_INFO "El grupo 6 ha instalado el monitor de memoria\n");
    }
    return 0;
}

static void __exit kernel_module_exit_event(void)
{
    remove_proc_entry("ram_mem_g6", NULL);
    printk(KERN_INFO "El grupo 6 ha removido el monitor de memoria\n");
}

module_init(kernel_module_init_event);
module_exit(kernel_module_exit_event);