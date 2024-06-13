namespace IceCreamMAUI.Shared.Dtos
{
    public record OrderItemDto(long Id, int IcecreamId, string Name, int Quantity, double Price, string Flavor, string Topping)
    {
        public double TotalPrice = Quantity * Price;
    }
    public record OrderDto(long Id, DateTime OrderedAt, double TotalPrice);
    public record OrderPlaceDto(OrderDto Order, OrderItemDto[] Items);

}
