FULL BACKUP: mysqldump -u root -p "BD2P2">./FBacks/FB2.sql
RESTORE BACKUP: mysql -u root -p "BD2P2"<./FBacks/FB1.sql

mysqlbinlog /var/lib/mysql-bin.000016 | mysql –u root –p

INCREMENTAL 1: binlog.00002 - binlog.00003
INCREMENTAL 2: binlog.00007 - binlog.00008
INCREMENTAL 3: binlog.00010 - binlog.00011
INCREMENTAL 4: binlog.00013 - binlog.00014
INCREMENTAL 5: binlog.00015 - binlog.00016


