SELECT @@version AS version
SELECT @@servername AS server

select * from dbo.Region;

-- Le indicamos al server quien es el distribuidor

EXEC sp_adddistributor @distributor = 'distributor', @password = 'Pa55w0rd!';

-- Habilitamos la base para replicacion
USE BD2Practica3;
EXEC sp_replicationdboption @dbname = N'BD2Practica3', @optname = N'publish', @value = N'true';

-- Creamos el agente de replicacion snapshot
EXEC sp_addpublication @publication = N'BD2Practica3DB',
                       @description = N'',
                       @retention = 0,
                       @allow_push = N'true',
                       @repl_freq = N'continuous',
                       @status = N'active',
                       @independent_agent = N'true';

-- Creamos los articulos a replicar
USE BD2Practica3;
EXEC sp_addarticle @publication = N'BD2Practica3DB',
                   @article = N'Region',
                   @source_owner = N'dbo',
                   @source_object = N'Region',
                   @type = N'logbased',
                   @description = NULL,
                   @creation_script = NULL,
                   @pre_creation_cmd = N'drop',
                   @schema_option = 0x000000000803509D,
                   @identityrangemanagementoption = N'manual',
                   @destination_table = N'Region',
                   @destination_owner = N'dbo',
                   @vertical_partition = N'false';

USE BD2Practica3;
EXEC sp_addarticle @publication = N'BD2Practica3DB',
                   @article = N'Shippers',
                   @source_owner = N'dbo',
                   @source_object = N'Shippers',
                   @type = N'logbased',
                   @description = NULL,
                   @creation_script = NULL,
                   @pre_creation_cmd = N'drop',
                   @schema_option = 0x000000000803509D,
                   @identityrangemanagementoption = N'manual',
                   @destination_table = N'Shippers',
                   @destination_owner = N'dbo',
                   @vertical_partition = N'false';

-- Agregamos el subscriptor
use BD2Practica3
exec sp_addsubscription 
@publication = N'BD2Practica3DB', 
@subscriber = 'subscriber1',
@destination_db = 'BD2Practica3', 
@subscription_type = N'Push', 
@sync_type = N'none', 
@article = N'all', 
@update_mode = N'read only', 
@subscriber_type = 0

-- Pusheamso el agente
exec sp_addpushsubscription_agent 
@publication = N'BD2Practica3DB', 
@subscriber = 'subscriber1',
@subscriber_db = 'BD2Practica3', 
@subscriber_security_mode = 0, 
@subscriber_login =  'sa',
@subscriber_password =  'Pa55w0rd!',
@frequency_type = 64,
@frequency_interval = 0, 
@frequency_relative_interval = 0, 
@frequency_recurrence_factor = 0, 
@frequency_subday = 0, 
@frequency_subday_interval = 0, 
@active_start_time_of_day = 0, 
@active_end_time_of_day = 0, 
@active_start_date = 0, 
@active_end_date = 19950101
GO
-- by default it sets up the log reader agent with a default account that won�t work, you need to change that to something that will.
EXEC sp_changelogreader_agent @publisher_security_mode = 0,
                              @publisher_login = 'sa',
                              @publisher_password = 'Pa55w0rd!';



select * from dbo.Region;

insert into dbo.Region (RegionID, RegionDescription) values (8, 'SouthWEstern');

select * from dbo.Shippers;

insert into dbo.Shippers (CompanyName, Phone) values ('Grand Company 2', '(503) 555-0032 2');
