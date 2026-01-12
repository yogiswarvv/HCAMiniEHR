CREATE PROCEDURE [Healthcare].[usp_CreateAppointment]
    @PatientId INT,
    @AppointmentDate DATE,
    @AppointmentTime TIME,
    @DoctorId INT,
    @Reason NVARCHAR(500),
    @NewAppointmentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate patient
    IF NOT EXISTS (SELECT 1 FROM [Healthcare].[Patient] WHERE PatientId = @PatientId)
    BEGIN
        RAISERROR('Patient does not exist.', 16, 1);
        RETURN;
    END

    -- Validate doctor
    IF NOT EXISTS (SELECT 1 FROM [Healthcare].[Doctor] WHERE DoctorId = @DoctorId)
    BEGIN
        RAISERROR('Doctor does not exist.', 16, 1);
        RETURN;
    END

    -- Validate doctor availability
    IF NOT EXISTS (SELECT 1 FROM [Healthcare].[Doctor] WHERE DoctorId = @DoctorId AND IsAvailable = 1)
    BEGIN
        RAISERROR('Doctor is not available.', 16, 1);
        RETURN;
    END

    -- Validate that the appointment is not in the past
    IF (CAST(@AppointmentDate AS DATETIME) + CAST(@AppointmentTime AS DATETIME)) < GETDATE()
    BEGIN
        RAISERROR('Appointment date and time cannot be in the past.', 16, 1);
        RETURN;
    END

    -- Validate for double booking
    IF EXISTS (
        SELECT 1
        FROM [Healthcare].[Appointment]
        WHERE DoctorId = @DoctorId
          AND AppointmentDate = @AppointmentDate
          AND AppointmentTime = @AppointmentTime
    )
    BEGIN
        RAISERROR('Doctor is already booked for this date and time.', 16, 1);
        RETURN;
    END

    DECLARE @DoctorName NVARCHAR(100);

    SELECT @DoctorName = CONCAT('Dr. ', FirstName, ' ', LastName)
    FROM [Healthcare].[Doctor]
    WHERE DoctorId = @DoctorId;

    INSERT INTO [Healthcare].[Appointment]
    (
        PatientId,
        AppointmentDate,
        AppointmentTime,
        DoctorId,
        DoctorName,
        Reason,
        Status,
        CreatedDate
    )
    VALUES
    (
        @PatientId,
        @AppointmentDate,
        @AppointmentTime,
        @DoctorId,
        @DoctorName,
        @Reason,
        'Scheduled',
        GETDATE()
    );

    SET @NewAppointmentId = SCOPE_IDENTITY();
END;
GO
