create database TAHCoffee;

use TAHCoffee;

create table Unit
(
	Id int primary key auto_increment,
	DisplayName nvarchar(10000)
);

create table Category
(
	Id int primary key auto_increment,
	DisplayName nvarchar(10000)
);

create table Item
(
	Id char(5) primary key,
	DisplayName varchar(100) not null,
	UnitId int not null, foreign key (UnitId) references Unit(Id),
	CategoryId int not null, foreign key (CategoryId) references Category(Id),
    Price int not null,
    constraint CHK_Price check(Price>0)
);

create table Account
(
	Id char(10) primary key,
    DisplayName varchar(20) not null,
    Email varchar(50) not null,
    PhoneNumber varchar(20) not null,
    Password varchar(1000) not null,
    AccountType varchar(10),
    constraint CHK_AccountType check (AccountType in ('admin', 'staff'))
);

create table Promo
(
	Id varchar(20) primary key,
    DisplayName varchar(30),
    Script varchar(300),
    StartTime datetime,
    EndTime datetime
);

create table Bill
(
	IdNumber int primary key auto_increment,
    ExportTime datetime not null,
    CustomerId char(10), foreign key (CustomerId) references Account(Id),
    PromoId varchar(20), foreign key (PromoId) references Promo(Id)
);

create table BillInfor
(
	IdNumber int not null, foreign key (IdNumber) references Bill(IdNumber),
	ItemId char(5), foreign key (ItemId) references Item(Id),
    Quantity int not null,
    Price int not null,
    constraint CHK_Quantity check(Quantity>0),
    primary key(IdNumber, ItemId)
);

alter table Account add ManagedBy char(10);
alter table Account add foreign key(ManagedBy) references Account(Id);


drop database TAHCoffee;
