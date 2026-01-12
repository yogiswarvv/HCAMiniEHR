SELECT
    P.PatientId,
    P.FirstName,
    P.LastName,
    P.DateOfBirth,
    P.Gender,
    P.Phone,
    P.Email,
    A.AppointmentId,
    A.AppointmentDate,
    A.AppointmentTime,
    A.Reason,
    A.Status
FROM
    Healthcare.Patient AS P
INNER JOIN
    Healthcare.Appointment AS A ON P.PatientId = A.PatientId
WHERE
    A.AppointmentDate >= GETDATE() -- This condition filters for current and future appointments
ORDER BY
    P.LastName, P.FirstName, A.AppointmentDate, A.AppointmentTime;