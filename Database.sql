create database cloth_bazarDB

use cloth_bazarDB

create table Categories(
cat_id int primary key identity,
cat_name nvarchar(50) not null,
cat_description nvarchar(max) not null,
cat_image nvarchar(max),
cat_isFeatured bit not null
)

create table Products(
pro_id int primary key identity,
pro_name nvarchar(50) not null,
pro_price int not null,
pro_description nvarchar(max) not null,
pro_image nvarchar(max),
pro_fk_cat int foreign key references Categories(cat_id)
)

create table [Configurations]
(
[key] nvarchar(50) primary key,
[value] nvarchar(max) not null
)

create table [User](
u_id int primary key identity,
u_firstname nvarchar(50) not null,
u_lastname nvarchar(50) not null,
u_gender nvarchar(50) not null,
u_image nvarchar(max) not null,
u_email nvarchar(50) not null,
u_username nvarchar(50) not null,
u_password nvarchar(50) not null,
)


create table [Order](
o_id int primary key identity,
o_fk_user int foreign key references [User](u_id),
o_ordered_At datetime,
o_status nvarchar(50) not null,
o_totalAmount int not null
)

create table [OrderItem](
oi_id int primary key identity,
oi_fk_o_id int foreign key references [Order](o_id),
oi_quantity int not null,
oi_fk_pro int foreign key references Products(pro_id),
)

select * from Categories
select * from Products
select * from [Configurations]
select * from [User]
select * from [Order]
select * from [OrderItem]
