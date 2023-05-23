namespace BlazorEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        /// <summary>
        /// Obtiene los productos de la orden almacenados en la db, calculamos el precio total y crea los OrderItems y lo amacena en la base de datos
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<bool>> PlaceOrder();

        Task<ServiceResponse<List<OrderOverviewResponseDTO>>>
    }
}
