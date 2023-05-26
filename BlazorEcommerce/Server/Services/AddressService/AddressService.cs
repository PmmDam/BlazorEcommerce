﻿namespace BlazorEcommerce.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public AddressService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }


        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
           var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;
            if(dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _context.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;

                dbAddress.City = address.City;
                dbAddress.Country = address.Country;
                
                dbAddress.Zip = address.Zip;
                dbAddress.Street = address.Street;
            }
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetUserId();
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);
            return new ServiceResponse<Address> { Data = address };
        }
    }
}
