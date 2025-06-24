CREATE PROCEDURE usp_GetUsersByRole
    @RoleId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get paginated users
    SELECT 
        u.Id,
        u.Username,
        u.Email,
        u.CreatedAt,
        u.UpdatedAt,
        u.LastLogin,
        u.IsActive
    FROM Users u
    WHERE u.RoleId = @RoleId
    ORDER BY u.Username
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
    
    -- Get total count for pagination
    SELECT COUNT(*) AS TotalCount
    FROM Users
    WHERE RoleId = @RoleId;
END