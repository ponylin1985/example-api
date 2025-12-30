using Example.Api.Models;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository interface for managing patients.
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Retrieves patients created within the specified time range with pagination.
    /// </summary>
    /// <param name="startTime">The start time of the range.</param>
    /// <param name="endTime">The end time of the range.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the list of patients and the total count.</returns>
    Task<(IEnumerable<Patient> Data, long TotalCount)> GetPatientsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Checks if a patient exists by their id.
    /// </summary>
    /// <param name="id">The id of the patient.</param>
    /// <returns>True if the patient exists, otherwise false.</returns>
    Task<bool> IsExistPatientAsync(long id);

    /// <summary>
    /// Retrieves a patient by their id.
    /// </summary>
    /// <param name="id">The id of the patient.</param>
    /// <returns>The patient with the specified id, or null if not found.</returns>
    Task<Patient?> GetPatientAsync(long id);

    /// <summary>
    /// Retrieves a patient by their name.
    /// </summary>
    /// <param name="name">The name of the patient.</param>
    /// <returns>The patient with the specified name, or null if not found.</returns>
    Task<Patient?> GetPatientByNameAsync(string name);

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="patient">The patient entity to create.</param>
    /// <returns>The created patient entity.</returns>
    Task<Patient> AddAsync(Patient patient);
}
