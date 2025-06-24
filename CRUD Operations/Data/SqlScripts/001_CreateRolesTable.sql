-- Create Roles table with multilingual support
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,          -- Stores JSON for multilingual names
    Description NVARCHAR(500),             -- Stores JSON for multilingual descriptions
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);

-- Create index for performance
CREATE INDEX IX_Roles_Name ON Roles(Name);