using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Represents a base request for paged queries.
/// </summary>
public record PagedRequest
{
    /// <summary>
    /// Gets the page number to retrieve. Defaults to 1.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the number of items per page. Defaults to 10.
    /// </summary>
    [Required]
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; init; } = 10;
}
