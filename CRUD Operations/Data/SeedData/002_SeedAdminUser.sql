-- Seed admin user with password 'Admin@123'
BEGIN TRANSACTION;

DECLARE @AdminRoleId INT;
SELECT @AdminRoleId = Id FROM Roles WHERE JSON_VALUE(Name, '$.en') = 'Admin';

IF @AdminRoleId IS NOT NULL
BEGIN
    DECLARE @Salt VARBINARY(64) = CRYPT_GEN_RANDOM(64);
    DECLARE @PasswordHash VARBINARY(64);
    
    -- Generate hash for 'Admin@123'
    SET @PasswordHash = HASHBYTES('SHA2_512', 'Admin@123' + CAST(@Salt AS NVARCHAR(128)));
    
    INSERT INTO Users (
        Username, 
        Email, 
        PasswordHash, 
        PasswordSalt, 
        RoleId, 
        CreatedAt, 
        UpdatedAt
    )
    VALUES (
        'admin',
        'admin@example.com',
        @PasswordHash,
        @Salt,
        @AdminRoleId,
        GETUTCDATE(),
        GETUTCDATE()
    );
END

COMMIT TRANSACTION;