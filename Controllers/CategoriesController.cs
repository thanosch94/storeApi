using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data.Dto;
using StoreApi.Data;
using Microsoft.EntityFrameworkCore;
using StoreApi.Processors;
using Newtonsoft.Json;
using StoreApi.Data.Enums;
using StoreApi.Services;

namespace StoreApi.Controllers
{
    public class CategoriesController : BaseController
    {
        private ApplicationDbContext _context;
        private CategoriesProcessor _categoriesProcessor;
        private IMapper _mapper;
        private static readonly object _lockObject = new object();

        public CategoriesController(CategoriesProcessor categoriesProcessor, ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _categoriesProcessor = categoriesProcessor;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoriesProcessor.GetAll();
            return Ok(data);
        }

    }
}
