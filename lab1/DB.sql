--    данный скрипт создает базу данных toplivo с тремя таблицами и генерирует тестовые записи:
-- 1. виды топлива (Fuels) - 1000 штук
-- 2. список емкостей (Tanks) - 100 штук
-- 3. факты совершения операций прихода, расхода топлива (Operations) - 300000 штук

--Создание базы данных
USE master
CREATE DATABASE TaxiGomel

GO
ALTER DATABASE TaxiGomel SET RECOVERY SIMPLE
GO

USE TaxiGomel
--DROP TABLE Fuels, Tanks, Operations
--DROP VIEW View_AllOperations

-- Создание таблиц
CREATE TABLE dbo.CarModels (ModelID int IDENTITY(1,1) NOT NULL PRIMARY KEY, 
ModelName nvarchar(50), 
TechStats nvarchar(50), 
Price decimal(18,2), 
Specifications nvarchar(50)) -- марки машин

CREATE TABLE dbo.Rates (RateID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
RateDescription nvarchar(50),
RatePrice decimal(18,2)) -- тарифы

CREATE TABLE dbo.Cars (CarID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
RegistrationNumber nvarchar(20),
ModelId int,
CarcaseNumber nvarchar(17),
EngineNumber nvarchar(17),
ReleaseYear date,
Mileage int,
DriverId int,
LastTI date,
MechanicId int,
SpecialMarks nvarchar(50)) -- автомобили

CREATE TABLE dbo.Positions (PositionID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
PositionName nvarchar(50),
Salary decimal(18,2)) -- должности

CREATE TABLE dbo.Employees (EmployeeID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
FirstName nvarchar(50),
LastName nvarchar(50),
Age int,
PositionID int,
Experience int) -- сотрудники

CREATE TABLE dbo.Calls (CallID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
CallTime smalldatetime,
Telephone varchar(12),
StartPosition nvarchar(20),
EndPosition nvarchar(20),
RateId int,
CarId int,
DispatcherId int) -- Вызовы


-- Добавление связей между таблицами
ALTER TABLE dbo.Cars  WITH CHECK ADD  CONSTRAINT FK_Cars_CarModels FOREIGN KEY(ModelID)
REFERENCES dbo.CarModels (ModelID) ON DELETE CASCADE
GO
ALTER TABLE dbo.Cars  WITH CHECK ADD  CONSTRAINT FK_Cars_EmployeesD FOREIGN KEY(DriverID)
REFERENCES dbo.Employees (EmployeeID) ON DELETE CASCADE
GO
ALTER TABLE dbo.Cars  WITH CHECK ADD  CONSTRAINT FK_Cars_EmployeesM FOREIGN KEY(MechanicID)
REFERENCES dbo.Employees (EmployeeID) ON DELETE NO ACTION
GO
ALTER TABLE dbo.Employees  WITH CHECK ADD  CONSTRAINT FK_Employees_Positions FOREIGN KEY(PositionID)
REFERENCES dbo.Positions (PositionID) ON DELETE CASCADE
GO
ALTER TABLE dbo.Calls  WITH CHECK ADD  CONSTRAINT FK_Calls_Cars FOREIGN KEY(CarID)
REFERENCES dbo.Cars (CarID) ON DELETE CASCADE
GO
ALTER TABLE dbo.Calls  WITH CHECK ADD  CONSTRAINT FK_Calls_Employees FOREIGN KEY(DispatcherID)
REFERENCES dbo.Employees (EmployeeID) ON DELETE NO ACTION
GO
ALTER TABLE dbo.Calls  WITH CHECK ADD  CONSTRAINT FK_Calls_Rates FOREIGN KEY(RateID)
REFERENCES dbo.Rates (RateID) ON DELETE NO ACTION
GO

SET NOCOUNT ON


DECLARE @Symbol CHAR(52)= 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz',
		@Position int,
		@PositionName nvarchar(50),
		@Salary decimal(18,2),
		@FirstName nvarchar(50),
		@LastName nvarchar(50),
		@Age int,
		@PositionID int,
		@Experience int,
		@ModelName nvarchar(50),
		@TechStats nvarchar(50),
		@Price decimal(18,2),
		@Specifications nvarchar(50),
		@RegistrationNumber nvarchar(20),
		@ModelId int,
		@CarcaseNumber nvarchar(17),
		@EngineNumber nvarchar(17),
		@ReleaseYear date,
		@Mileage int,
		@DriverId int,
		@LastTI date,
		@MechanicId int,
		@SpecialMarks nvarchar(50),
		@i int,
		@NameLimit int,
		@odate date,
		@Inc_Exp real,
		@RowCount INT,
		@NumberPositions int,
		@NumberEmployees int,
		@NumberCarModels int,
		@NumberCars int,
		@MinNumberSymbols int,
		@MaxNumberSymbols int



SET @NumberPositions = 500
SET @NumberCarModels = 500
SET @NumberCars = 20000
SET @NumberEmployees = 20000

BEGIN TRAN

SELECT @i=0 FROM dbo.Positions WITH (TABLOCKX) WHERE 1=0


-- Заполнение видов топлива 
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberPositions
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @PositionName=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @PositionName = @PositionName + SUBSTRING(@Symbol, @Position, 1)
			SET @Salary = ROUND(RAND(CHECKSUM(NEWID())) * (100), 2)
			SET @i=@i+1
		END

		INSERT INTO dbo.Positions (PositionName, Salary) SELECT @PositionName, @Salary
		

		SET @RowCount +=1
	END
SELECT @i=0 FROM dbo.Employees WITH (TABLOCKX) WHERE 1=0


-- Заполнение видов топлива 
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberEmployees
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @FirstName = ''
		SET @LastName = ''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @FirstName = @FirstName + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @LastName = @LastName + SUBSTRING(@Symbol, @Position, 1)
			SET @Age = CAST(RAND()*52 as int)
			SET @PositionID = CAST( (1+RAND()*(@NumberPositions-1)) as int)
			SET @Experience = CAST(RAND()*52 as int)
			SET @i=@i+1
		END

		INSERT INTO dbo.Employees (FirstName, LastName, Age, PositionID, Experience) SELECT @FirstName, @LastName, @Age, @PositionID, @Experience
		

		SET @RowCount +=1
	END

SELECT @i=0 FROM dbo.CarModels WITH (TABLOCKX) WHERE 1=0


-- Заполнение видов топлива 
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberCarModels
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @ModelName = ''
		SET @TechStats = ''
		SET @Specifications = ''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @ModelName = @ModelName + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @TechStats = @TechStats + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @Specifications = @Specifications + SUBSTRING(@Symbol, @Position, 1)
			SET @Price = ROUND(RAND(CHECKSUM(NEWID())) * (100), 2)
			SET @i=@i+1
		END

		INSERT INTO dbo.CarModels (ModelName, TechStats, Price, Specifications) SELECT @ModelName, @TechStats, @Price, @Specifications
		

		SET @RowCount +=1
	END

SELECT @i=0 FROM dbo.Cars WITH (TABLOCKX) WHERE 1=0
-- Заполнение видов топлива 
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberCars
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
		SET @RegistrationNumber = ''
		SET @CarcaseNumber  = ''
		SET @EngineNumber  = ''
		SET @SpecialMarks = ''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @RegistrationNumber = @RegistrationNumber + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @CarcaseNumber = @CarcaseNumber + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @EngineNumber = @EngineNumber + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @SpecialMarks = @SpecialMarks + SUBSTRING(@Symbol, @Position, 1)
			SET @ModelId = CAST( (1+RAND()*(@NumberCarModels-1)) as int)
			SET @DriverId = CAST( (1+RAND()*(@NumberEmployees-1)) as int)
			SET @MechanicId = CAST( (1+RAND()*(@NumberEmployees-1)) as int)
			SET @Mileage=RAND()*52
			SET @LastTI=dateadd(day,-RAND()*15000,GETDATE())
			SET @ReleaseYear=dateadd(day,-RAND()*15000,GETDATE())
			SET @i=@i+1
		END

		INSERT INTO dbo.Cars (RegistrationNumber, ModelId, CarcaseNumber, EngineNumber, ReleaseYear, Mileage, DriverId, LastTI, MechanicId, SpecialMarks) SELECT @RegistrationNumber, @ModelId, @CarcaseNumber, @EngineNumber, @ReleaseYear, @Mileage, @DriverId, @LastTI, @MechanicId, @SpecialMarks
		

		SET @RowCount +=1
	END
COMMIT TRAN
GO
-- ================================================
-- создание представления для отбора данных всех операций
CREATE VIEW [dbo].[View_EmployeeAndPositions]
AS
SELECT        dbo.Employees.EmployeeID, dbo.Employees.FirstName, dbo.Employees.LastName, dbo.Employees.Age, dbo.Employees.Experience, dbo.Employees.PositionID, 
				dbo.Positions.PositionName, dbo.Positions.Salary 
FROM            dbo.Positions INNER JOIN
                         dbo.Employees ON dbo.Positions.PositionID = dbo.Employees.PositionID
GO
CREATE VIEW [dbo].[View_CarsAndDrivers]
AS
SELECT        dbo.Cars.CarID, dbo.Cars.RegistrationNumber, dbo.Cars.ModelID, dbo.Cars.DriverId, (dbo.Employees.FirstName + ' ' + dbo.Employees.LastName) AS Driver, dbo.Cars.SpecialMarks
FROM            dbo.Employees INNER JOIN
                         dbo.Cars ON dbo.Employees.EmployeeID = dbo.Cars.DriverId
GO

CREATE PROCEDURE ChangeLastTI (@CarId int, @NewTI date)
        AS UPDATE dbo.Cars
        SET LastTi = @NewTI
		WHERE(
		dbo.Cars.CarID = @CarId
		);
GO
CREATE PROCEDURE AddCar (@RegistrationNumber nvarchar(20), @ModelId int, @CarcaseNumber nvarchar(17), @EngineNumber nvarchar(17), @ReleaseYear date, @Mileage int, @DriverId int, @LastTI date, @MechanicId int, @SpecialMarks nvarchar(50))
        AS INSERT INTO dbo.Cars(
		RegistrationNumber,
		ModelId,
		CarcaseNumber,
		EngineNumber,
		ReleaseYear, 
		Mileage, 
		DriverId,
		LastTI,
		MechanicId,
		SpecialMarks) 
		VALUES
		(@RegistrationNumber, 
		@ModelId , 
		@CarcaseNumber, 
		@EngineNumber, 
		@ReleaseYear, 
		@Mileage, 
		@DriverId, 
		@LastTI, 
		@MechanicId, 
		@SpecialMarks)
GO
CREATE PROCEDURE AddEmployee (@FirstName nvarchar(50), @LastName nvarchar(50), @Age int, @PositionID int, @Experience int)
        AS INSERT INTO dbo.Employees(
		FirstName,
		LastName,
		Age,
		PositionID,
		Experience) 
		VALUES
		(@FirstName, 
		@LastName , 
		@Age, 
		@PositionID, 
		@Experience)