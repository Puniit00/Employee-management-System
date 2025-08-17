CREATE
OR
ALTER PROCEDURE AddEmployee(@name varchar(100), @email varchar(100), @department varchar(100), @dateOfJoining DATETIME) AS BEGIN
INSERT INTO Employees
VALUES (@name,
        @email,
        @department,
        @dateOfJoining) END 
        
GO
Create
OR
ALTER PROCEDURE GetAllEmployees AS BEGIN
select * from Employees
 END
GO

Create
OR
ALTER PROCEDURE UpdateEmployee(@name varchar(100), @email varchar(100), @department varchar(100), @dateOfJoining DATETIME, @id Int) AS BEGIN
Update Employees set Name = @name, Email = @email, DateOfJoining = @dateOfJoining
where id = @id END

GO
Create
OR
ALTER PROCEDURE DeleteEmployee(@id Int) AS BEGIN
Delete from Employees
where id = @id END

GO

Create
or Alter PROCEDURE GetEmployeeById(@id int) AS BEGIN
select * from Employees where id = @id
END
GO

-- Goals Sp's
Create Or ALter Procedure AddEmployeeGoals(@employeeId int, @title VARCHAR(100), @description Varchar(2000), @targetDate DateTime, @isComplete BIT)
AS
BEGIN
Insert into Goals select id, @title, @description, @targetDate, @isComplete from Employees where id = @employeeId
END
GO

Create Or ALter Procedure GetEmployeeGoals(@employeeId int)
AS
BEGIN
Select * from goals where EmployeeId =@employeeId
END
GO

Create Or ALter Procedure UpdateEmployeeGoals(@id int, @title VARCHAR(100), @description Varchar(2000), @targetDate DateTime, @isComplete BIT)
AS
BEGIN
Update goals set Title = @title, Description = @description, TargetDate = @targetDate, IsCompleted = @isComplete
where id = @id
END
GO

-- Performance reviews Sp's
Create Or ALter Procedure AddPerformanceReview(@employeeId int, @reviewerName VARCHAR(100), @reviewDate DateTime, @rating int, @Comments VARCHAR(2000))
AS
BEGIN
Insert into PerformanceReviews select id, @reviewerName, @reviewDate, @rating, @Comments from Employees where id = @employeeId
END
GO

Create Or ALter Procedure GetPerformanceReviews(@employeeId int)
AS
BEGIN
Select * from PerformanceReviews where EmployeeId =@employeeId
END
GO

--Authentiction SP's

Create Or ALter Procedure GetUserByUsernameAndPassword(@username Varchar(100), @password VARCHAR(2000))
AS
BEGIN
Select top 1 * from Users where name = @username and Password = @password
END
GO

CREATE OR ALTER PROCEDURE CreateUser
    @badgeNo INT,
    @name VARCHAR(100),
    @password VARCHAR(2000)
AS
BEGIN
    INSERT INTO Users (badgeNo, Name, CreateDate, Password)
    VALUES (@badgeNo, @name, GETDATE(), @password);

    SELECT CAST(SCOPE_IDENTITY() AS INT); -- returns the new User Id
END
GO
