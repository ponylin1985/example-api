using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Represents a base request for paged queries.
/// </summary>
public record PagedRequest
{
    /// <summary>
    /// Page number to retrieve. Defaults to 1.
    /// </summary>
    [Required]
    [DefaultValue(1)]
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    [FromQuery(Name = "pageNumber")]
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Number of items per page. Defaults to 10.
    /// </summary>
    [Required]
    [DefaultValue(10)]
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    [FromQuery(Name = "pageSize")]
    public int PageSize { get; init; } = 10;
}
