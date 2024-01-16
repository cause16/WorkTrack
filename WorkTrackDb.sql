CREATE DATABASE WorkTrackDb
ON
(
   NAME = 'WorkTrackDb',
   FILENAME = 'D:\WorkTrackDb.mdf',
   SIZE = 100MB,
   MAXSIZE = 1000MB,
   FILEGROWTH = 100MB
)
LOG ON
(
   NAME = 'LogWorkTrackDb',
   FILENAME = 'D:\WorkTrackDb.ldf',
   SIZE = 50MB,
   MAXSIZE = 500MB,
   FILEGROWTH = 50MB
)
COLLATE Cyrillic_General_CI_AS;
GO

USE WorkTrackDb;
GO

CREATE TABLE [Addresses] (
    [AddressId] int NOT NULL IDENTITY,
    [Country] nvarchar(25) NOT NULL,
    [City] nvarchar(25) NOT NULL,
    [Street] nvarchar(25) NOT NULL,
    [HouseNumber] nvarchar(8) NOT NULL,
    [ApartmentNumber] smallint NULL,
    [PostIndex] nvarchar(5) NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([AddressId]),
    CONSTRAINT [CK_Addresses_ApartmentNumber] CHECK ([ApartmentNumber] > 0)
);
GO

CREATE TABLE [Departments] (
    [DepartmentId] int NOT NULL IDENTITY,
    [DepartmentName] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([DepartmentId])
);
GO

CREATE TABLE [Positions] (
    [PositionId] int NOT NULL IDENTITY,
    [PositionName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Positions] PRIMARY KEY ([PositionId])
);
GO

CREATE TABLE [CompanyInfo] (
    [CompanyId] int NOT NULL IDENTITY,
    [CompanyName] nvarchar(25) NOT NULL,
    [AddressId] int NOT NULL,
    [PhoneNumber] nvarchar(12) NOT NULL,
    CONSTRAINT [PK_CompanyInfo] PRIMARY KEY ([CompanyId]),
    CONSTRAINT [CK_CompanyInfo_PhoneNumber] CHECK ([PhoneNumber] LIKE '([0-9][0-9][0-9])[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT [FK_CompanyInfo_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Employees] (
    [EmployeeId] int NOT NULL IDENTITY,
    [PositionId] int NOT NULL,
    [DepartmentId] int NOT NULL,
    [FirstName] nvarchar(20) NOT NULL,
    [LastName] nvarchar(20) NOT NULL,
    [MiddleName] nvarchar(20) NOT NULL,
    [AddressId] int NOT NULL,
    [PhoneNumber] nvarchar(12) NOT NULL,
    [BirthDate] date NOT NULL,
    [HireDate] date NOT NULL,
    [Salary] money NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeId]),
    CONSTRAINT [CK_Employees_PhoneNumber] CHECK ([PhoneNumber] LIKE '([0-9][0-9][0-9])[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT [CK_Employees_Salary] CHECK ([Salary] > 0),
    CONSTRAINT [FK_Employees_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([DepartmentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Positions_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [Positions] ([PositionId]) ON DELETE CASCADE
);
GO

INSERT INTO Addresses (Country, City, Street, HouseNumber, ApartmentNumber, PostIndex)
VALUES 
    ('Україна', 'Київ', 'Хрещатик', '1', 10, '01001'),
    ('Україна', 'Львів', 'площа Ринок', '5', 20, '79008'),
    ('Україна', 'Одеса', 'Дерибасівська', '10', 30, '65000'),
    ('Україна', 'Харків', 'проспект Свободи', '15', 40, '61000'),
    ('Україна', 'Дніпро', 'проспект Карла Маркса', '20', 50, '49000'),
    ('Україна', 'Івано-Франківськ', 'вулиця Незалежності', '25', 60, '76000');
GO

INSERT INTO Departments (DepartmentName)
VALUES 
    ('Продажі'),
    ('Маркетинг'),
    ('Кадрові ресурси'),
    ('IT'),
    ('Фінанси');
GO

INSERT INTO Positions (PositionName)
VALUES 
    ('Менеджер з продажів'),
    ('Спеціаліст з маркетингу'),
    ('Координатор з кадрів'),
    ('IT Спеціаліст'),
    ('Фінансовий аналітик'),
    ('Торговий представник'),
    ('Ключовий клієнт-менеджер'),
    ('Продажі в корпоративному секторі'),
    ('Телемаркетолог'),
    ('Спеціаліст з розвитку бізнесу');
GO

INSERT INTO CompanyInfo (CompanyName, AddressId, PhoneNumber)
VALUES ('Укрпошта', 1, '(044)1234567');
GO

INSERT INTO Employees (DepartmentId, PositionId, FirstName, LastName, MiddleName, AddressId, PhoneNumber, BirthDate, HireDate, Salary)
VALUES 
    (2, 5, 'Іван', 'Петров', 'Миколайович', 4, '(066)1111111', '1990-01-01', '2020-01-01', 50000),
    (5, 2, 'Марія', 'Іванова', 'Олександрівна', 1, '(067)2222222', '1991-02-02', '2021-02-02', 60000),
    (4, 4, 'Олег', 'Сидоров', 'Володимирович', 5, '(068)3333333', '1992-03-03', '2022-03-03', 70000),
    (3, 1, 'Наталія', 'Григоренко', 'Ігорівна', 2, '(095)4444444', '1993-04-04', '2023-04-04', 80000),
    (1, 3, 'Петро', 'Коваль', 'Васильович', 3, '(099)5555555', '1994-05-05', '2024-05-05', 90000);
GO