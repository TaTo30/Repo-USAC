create table Usuario (
	id serial,
	nombre varchar,
	email varchar,
	password varchar,
	foto varchar,
	primary key (id)
);

create table Archivo (
	id serial,
	nombre varchar,
	extension varchar,
	public boolean default false,
	fecha timestamp,
	primary key (id)
);

create table Amigos (
	usuario1 integer,
	usuario2 integer,
	foreign key (usuario1) references Usuario(id),
	foreign key (usuario2) references Usuario(id),
	primary key (usuario1, usuario2)
);

create table Carpeta (
	usuario integer,
	archivo integer,
	foreign key (usuario) references Usuario(id),
	foreign key (archivo) references Archivo(id),
	primary key (usuario, archivo)
);