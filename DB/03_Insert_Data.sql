-- Insert Employees if less than 20 exist
IF (SELECT COUNT(*) FROM Employees) < 20
BEGIN
    INSERT INTO Employees (Name, Email, Department, DateOfJoining)
    SELECT TOP (20 - (SELECT COUNT(*) FROM Employees))
           CONCAT('Employee ', ROW_NUMBER() OVER (ORDER BY (SELECT NULL))),
           CONCAT('employee', ROW_NUMBER() OVER (ORDER BY (SELECT NULL)), '@company.com'),
           CASE (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) % 5)
                WHEN 0 THEN 'HR'
                WHEN 1 THEN 'IT'
                WHEN 2 THEN 'Finance'
                WHEN 3 THEN 'Sales'
                ELSE 'Operations'
           END,
           DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 3650), GETDATE())
    FROM sys.objects s1
    CROSS JOIN sys.objects s2;
END


-- Insert Goals if less than 20 exist
IF (SELECT COUNT(*) FROM Goals) < 20
BEGIN
    INSERT INTO Goals (EmployeeId, Title, Description, TargetDate, IsCompleted)
    SELECT TOP (20 - (SELECT COUNT(*) FROM Goals))
           e.Id,
           CONCAT('Goal ', ROW_NUMBER() OVER (ORDER BY (SELECT NULL))),
           'Description for the goal',
           DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 365), GETDATE()),
           CASE WHEN (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) % 2) = 0 THEN 1 ELSE 0 END
    FROM Employees e
    CROSS JOIN sys.objects
    WHERE e.Id IS NOT NULL;
END


-- Insert Performance Reviews if less than 20 exist
IF (SELECT COUNT(*) FROM PerformanceReviews) < 20
BEGIN
    INSERT INTO PerformanceReviews (EmployeeId, ReviewerName, ReviewDate, Rating, Comments)
    SELECT TOP (20 - (SELECT COUNT(*) FROM PerformanceReviews))
           e.Id,
           CONCAT('Reviewer ', ROW_NUMBER() OVER (ORDER BY (SELECT NULL))),
           DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),
           (ABS(CHECKSUM(NEWID())) % 5) + 1,
           'Performance review comments'
    FROM Employees e
    CROSS JOIN sys.objects
    WHERE e.Id IS NOT NULL;
END

-- Insert dummy data into Users table
INSERT INTO Users (badgeNo, Name, CreateDate, Password)
VALUES
(1001, 'User One', GETDATE(), 'password1'),
(1002, 'User Two', GETDATE(), 'password2');
GO

