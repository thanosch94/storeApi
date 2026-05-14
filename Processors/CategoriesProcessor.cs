using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Data.Dto;

namespace StoreApi.Processors
{
    public class CategoriesProcessor
    {
        private ApplicationDbContext _context;
        public CategoriesProcessor(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<List<ProductDto>> GetAll()
        {
            var data = await _context.Categories.Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToListAsync();

            return data;
        }
    }
}
