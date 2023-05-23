﻿using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context,ICartService cartService,IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders()
        {
            //Inicializamos la respuesta del servidor
            var response = new ServiceResponse<List<OrderOverviewResponseDTO>>();

            //Obtenemos todas los pedidos del usuario incluyendo los orderItems y los productos
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            //Inicializamos la lista de overviews de pedidos
            var orderResponse = new List<OrderOverviewResponseDTO>();

            //Recorremos los pedidos obtenidos anteriormente e instanciamos el dto correspondiente por cada elemento
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponseDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more..." : o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));

            //Añadimos la lista de DTOs a la respuesta del servidor
            response.Data = orderResponse;

            //Devolvemos la respuesta
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            //Obtiene los productos del carrito
            var products = (await _cartService.GetDbCartProducts()).Data;

            //Precio total del pedido
            decimal totalPrice = 0;

            //Recorre los productos para calcular el precio total
            products.ForEach(products => totalPrice += products.Price * products.Quantity);

            //Crea la lista de OrderItems que perteneceran al pedido
            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price*product.Quantity
            }));

            //Creamos el pedido
            var order = new Order
            {
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            //Guardamos el pedido en la base de datos
            _context.Orders.Add(order);

            //Vaciamos el carrito
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()));

            //Guardamos los cambios
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
