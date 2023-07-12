CREATE OR ALTER PROCEDURE practica1.TR1
    @firstname nvarchar(100), @lastname nvarchar(100), @email nvarchar(100), @password nvarchar(100), @credits int
AS BEGIN
    DECLARE @usuarioid uniqueidentifier, @roleid uniqueidentifier;
    IF exists(select 1 from Usuarios where Email = @email) begin
        insert into HistoryLog (Date, Description) values (getdate(), 'Ya existe un email asociado');
        print N'Ya existe un email asociado';
    end
    else begin
        insert into Usuarios (Id, Firstname, Lastname, Email, DateOfBirth, Password, LastChanges, EmailConfirmed) values
        (NEWID(), @firstname, @lastname, @email, GETDATE(), @password, GETDATE(), 0);
        /*OBTENEMOS LOS IDS PARA ASIGNAR EN LAS SIGUIENTES TABLAS*/
        select @usuarioid = (select Id from Usuarios where Email = @email);
        select @roleid = (select Id from Roles where RoleName = 'Student');

        insert into UsuarioRole (RoleId, UserId, IsLatestVersion) values (@roleid, @usuarioid, 1);
        insert into ProfileStudent (UserId, Credits) values (@usuarioid, @credits);
        insert into TFA (UserId, Status, LastUpdate) values (@usuarioid, 0, GETDATE());
        insert into Notification (UserId, Message, Date) values (@usuarioid, 'El usuario ha sido registrado', getdate());
        insert into HistoryLog (Date, Description) VALUES (getdate(), 'Usuario registrado');
    end
END

create or alter procedure practica1.TR2
    @email nvarchar(100), @codCourse int
as begin
    declare @usuarioid uniqueidentifier, @roleid uniqueidentifier;
    if exists(select * from Usuarios where Email = @email and EmailConfirmed = 1) begin
        select @usuarioid = (select Id from Usuarios where Email = @email);
        select @roleid = (select Id from Roles where RoleName = 'Tutor');

        insert into UsuarioRole (RoleId, UserId, IsLatestVersion) values (@roleid, @usuarioid, 1);
        insert into TutorProfile (UserId, TutorCode) values (@usuarioid, @usuarioid);
        insert into CourseTutor (TutorId, CourseCodCourse) values (@usuarioid, @codCourse);
        insert into Notification (UserId, Message, Date) values (@usuarioid, 'Ha sido asignado como tutor', getdate());
        insert into HistoryLog (Date, Description) values (getdate(), 'Usuario asignado como tutor');
    end
    else begin
        insert into HistoryLog (Date, Description) values (getdate(), 'El usuario no ha confirmado su email');
        print N'El usuario no ha confirmado su email';
    end
end

create or alter procedure practica1.TR3
    @email nvarchar(100), @codCourse int
as begin
    declare @usuarioid uniqueidentifier, @tutorid uniqueidentifier, @creditos int, @creditosUsuario int;
    if exists(select * from Usuarios where Email = @email and EmailConfirmed = 1) begin
        select @usuarioid = (select Id from Usuarios where Email = @email);
        select @tutorid = (select TutorId from CourseTutor where CourseCodCourse = @codCourse);
        select @creditos = (select CreditsRequired from Course where CodCourse = @codCourse);
        select @creditosUsuario = (select Credits from ProfileStudent where UserId = @usuarioid);
        if @creditosUsuario >= @creditos begin
            insert into CourseAssignment (StudentId, CourseCodCourse) values (@usuarioid, @codCourse);
            insert into Notification (UserId, Message, Date) values (@tutorid, 'Un nuevo estudiante fue asignado a tu curso', GETDATE());
            insert into Notification (userid, message, date) values (@usuarioid, 'Has asignado al curso', getdate());
            insert into HistoryLog (Date, Description) values (getdate(), 'Estudiante asignado con exito');
        end
        else begin
            insert into HistoryLog (date, description) values (getdate(), 'Creditos insuficientes para asignar al estudiante')
            print N'Creditos insuficientes'
        end
    end
    else begin
        insert into HistoryLog (Date, Description) values (getdate(), 'El usuario no ha confirmando su email');
        print N'El usuario no ha confirmado su email';
    end
end

/*
execute practica1.TR1 @firstname = 'Fernando', @lastname = 'Alonso', @email = 'falonso@gmail.com', @password = 'totito', @credits = 10;

execute practica1.TR2 @email = 'falonso@gmail.com', @codCourse = 283;

execute practica1.TR3 @email = 'falonso@gmail.com', @codCourse = 970;
*/