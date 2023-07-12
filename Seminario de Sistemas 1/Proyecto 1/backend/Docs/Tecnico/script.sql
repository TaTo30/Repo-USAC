create table usuario (
	id integer auto_increment,
	nombre varchar(250),
	email varchar(250),
	password varchar(250),
	foto varchar(250),
	primary key (id)
);

create table archivo (
	id integer auto_increment,
	nombre varchar(250),
	extension varchar(250),
	public boolean default false,
	fecha bigint,
	datos varchar(250),
	primary key (id)
);

create table amigos (
	usuario1 integer,
	usuario2 integer,
	foreign key (usuario1) references usuario (id),
	foreign key (usuario2) references usuario (id),
	primary key (usuario1, usuario2)
);

create table carpeta (
	usuario integer,
	archivo integer,
	foreign key (usuario) references usuario (id),
	foreign key (archivo) references archivo (id),
	primary key (usuario, archivo)
);