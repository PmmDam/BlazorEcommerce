namespace BlazorEcommerce.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Devuelve todas las categorias de la base de datos
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    }
}
