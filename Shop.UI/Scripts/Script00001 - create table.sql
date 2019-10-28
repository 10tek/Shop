create table [Item]
(
	[Id] uniqueidentifier primary key not null,
	[CreationDate] datetime not null,
	[DeletedDate] datetime null,
	[Name] nvarchar(max) not null,
	[ImagePath] nvarchar(max) not null,
	[Description] nvarchar(max) not null,
	[Price] int not null,
	[CategoryId] uniqueidentifier not null
)

create table [User]
(
	[Id] uniqueidentifier primary key not null,
	[CreationDate] datetime not null,
	[DeletedDate] datetime null,
	[PhoneNumber] nvarchar(max) not null,
	[Password] nvarchar(max) not null,
)

create table [Category]
(
	[Id] uniqueidentifier primary key not null,
	[CreationDate] datetime not null,
	[DeletedDate] datetime null,
	[Name] nvarchar(max) not null,
	[ImagePath] nvarchar(max) not null,
)