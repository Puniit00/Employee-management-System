-- =============================================
-- 01_create_tables.sql
-- =============================================

-- Employees Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
BEGIN
    CREATE TABLE Employees (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
        Email VARCHAR(100) NOT NULL,
        Department VARCHAR(100) NOT NULL,
        DateOfJoining DATETIME NOT NULL
    );
END
GO

-- Goals Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Goals' AND xtype='U')
BEGIN
    CREATE TABLE Goals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL,
        Title VARCHAR(100) NOT NULL,
        Description VARCHAR(2000) NULL,
        TargetDate DATETIME NOT NULL,
        IsCompleted BIT NOT NULL DEFAULT 0,
        FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE
    );
END
GO

-- PerformanceReviews Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PerformanceReviews' AND xtype='U')
BEGIN
    CREATE TABLE PerformanceReviews (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL,
        ReviewerName VARCHAR(100) NOT NULL,
        ReviewDate DATETIME NOT NULL,
        Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
        Comments VARCHAR(2000) NULL,
        FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE
    );
END
GO
