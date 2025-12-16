using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper for Order related objects.
/// </summary>
public static class OrderMapper
{
    /// <summary>
    /// Maps an Order entity to an OrderDto.
    /// </summary>
    /// <param name="order">The order entity.</param>
    /// <returns>The order DTO.</returns>
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Message = order.Message,
            PatientId = order.PatientId,
            CreatedAt = order.CreatedAt,
        };
    }

    /// <summary>
    /// Maps a collection of Order entities to a collection of OrderDtos.
    /// </summary>
    /// <param name="orders">The collection of order entities.</param>
    /// <returns>The collection of order DTOs.</returns>
    public static IEnumerable<OrderDto> ToDtos(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.ToDto());
    }
}
