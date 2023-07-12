create table Course
(
    CodCourse       int           not null
        constraint PK_Course
            primary key,
    Name            nvarchar(max) not null,
    CreditsRequired int           not null
)
go

create table HistoryLog
(
    Id          int identity
        constraint PK_HistoryLog
            primary key,
    Date        datetime2     not null,
    Description nvarchar(max) not null
)
go

create table Roles
(
    Id       uniqueidentifier not null
        constraint PK_Roles
            primary key,
    RoleName nvarchar(max)    not null
)
go

create table Usuarios
(
    Id             uniqueidentifier not null
        constraint PK_Usuarios
            primary key,
    Firstname      nvarchar(max)    not null,
    Lastname       nvarchar(max)    not null,
    Email          nvarchar(max)    not null,
    DateOfBirth    datetime2        not null,
    Password       nvarchar(max)    not null,
    LastChanges    datetime2        not null,
    EmailConfirmed bit              not null
)
go

create table CourseAssignment
(
    Id              int identity
        constraint PK_CourseAssignment
            primary key,
    StudentId       uniqueidentifier not null
        constraint FK_CourseAssignment_Usuarios_StudentId
            references Usuarios
            on delete cascade,
    CourseCodCourse int              not null
        constraint FK_CourseAssignment_Course_CourseCodCourse
            references Course
            on delete cascade
)
go

create index IX_CourseAssignment_CourseCodCourse
    on CourseAssignment (CourseCodCourse)
go

create index IX_CourseAssignment_StudentId
    on CourseAssignment (StudentId)
go

create table CourseTutor
(
    Id              int identity
        constraint PK_CourseTutor
            primary key,
    TutorId         uniqueidentifier not null
        constraint FK_CourseTutor_Usuarios_TutorId
            references Usuarios
            on delete cascade,
    CourseCodCourse int              not null
        constraint FK_CourseTutor_Course_CourseCodCourse
            references Course
            on delete cascade
)
go

create index IX_CourseTutor_CourseCodCourse
    on CourseTutor (CourseCodCourse)
go

create index IX_CourseTutor_TutorId
    on CourseTutor (TutorId)
go

create table Notification
(
    Id      int identity
        constraint PK_Notification
            primary key,
    UserId  uniqueidentifier not null
        constraint FK_Notification_Usuarios_UserId
            references Usuarios
            on delete cascade,
    Message nvarchar(max)    not null,
    Date    datetime2        not null
)
go

create index IX_Notification_UserId
    on Notification (UserId)
go

create table ProfileStudent
(
    Id      int identity
        constraint PK_ProfileStudent
            primary key,
    UserId  uniqueidentifier not null
        constraint FK_ProfileStudent_Usuarios_UserId
            references Usuarios
            on delete cascade,
    Credits int              not null
)
go

create index IX_ProfileStudent_UserId
    on ProfileStudent (UserId)
go

create table TFA
(
    Id         int identity
        constraint PK_TFA
            primary key,
    UserId     uniqueidentifier not null
        constraint FK_TFA_Usuarios_UserId
            references Usuarios
            on delete cascade,
    Status     bit              not null,
    LastUpdate datetime2        not null
)
go

create index IX_TFA_UserId
    on TFA (UserId)
go

create table TutorProfile
(
    Id        int identity
        constraint PK_TutorProfile
            primary key,
    UserId    uniqueidentifier not null
        constraint FK_TutorProfile_Usuarios_UserId
            references Usuarios
            on delete cascade,
    TutorCode nvarchar(max)    not null
)
go

create index IX_TutorProfile_UserId
    on TutorProfile (UserId)
go

create table UsuarioRole
(
    Id              int identity
        constraint PK_UsuarioRole
            primary key,
    RoleId          uniqueidentifier not null
        constraint FK_UsuarioRole_Roles_RoleId
            references Roles
            on delete cascade,
    UserId          uniqueidentifier not null
        constraint FK_UsuarioRole_Usuarios_UserId
            references Usuarios
            on delete cascade,
    IsLatestVersion bit              not null
)
go

create index IX_UsuarioRole_RoleId
    on UsuarioRole (RoleId)
go

create index IX_UsuarioRole_UserId
    on UsuarioRole (UserId)
go

CREATE   PROCEDURE practica1.TR1
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
go

create   procedure practica1.TR2
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
go

create   procedure practica1.TR3
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
go


