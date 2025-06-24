-- Seed initial roles with English/Arabic support
BEGIN TRANSACTION;

INSERT INTO Roles (Name, Description, CreatedAt, UpdatedAt)
VALUES 
(
    '{"en": "Admin", "ar": "مدير"}',
    '{"en": "Administrator with full access", "ar": "مدير مع صلاحيات كاملة"}',
    GETUTCDATE(),
    GETUTCDATE()
),
(
    '{"en": "User", "ar": "مستخدم"}',
    '{"en": "Standard user with limited access", "ar": "مستخدم عادي بصلاحيات محدودة"}',
    GETUTCDATE(),
    GETUTCDATE()
);

COMMIT TRANSACTION;