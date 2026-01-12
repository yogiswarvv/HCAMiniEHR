USE EHR2; -- Ensure this is the correct database name
GO

CREATE TRIGGER trg_PreventAuditLogDeletion
ON [Healthcare].[AuditLog]
FOR DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Raise an error and roll back the transaction to prevent deletion
    RAISERROR ('Deletion from the AuditLog table is not allowed. Audit records must be immutable to maintain integrity.', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO
