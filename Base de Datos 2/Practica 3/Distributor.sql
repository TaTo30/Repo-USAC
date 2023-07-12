SELECT @@version AS version
SELECT @@servername AS server

/*Le indicamos al server que es el distribuidor*/

EXEC sp_adddistributor @distributor = 'distributor', @password = 'Pa55w0rd!';

/*Creamos la base de datos de distribuicion*/

EXEC sp_adddistributiondb @database = 'DIST';

/*Le indicamos al distribuidor quien es el publicador*/
EXEC sp_adddistpublisher @publisher = 'publisher', @distribution_db = 'DIST';

USE DIST;
GO

/*Verificamos la configuracion*/
SELECT * FROM [dbo].[MSrepl_commands];

SELECT name, date_modified 
FROM msdb.dbo.sysjobs 
ORDER by date_modified desc
