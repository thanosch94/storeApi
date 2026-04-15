using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreApi.Data;
using StoreApi.Data.Dto;
using StoreApi.Data.Enums;
using StoreApi.Data.Models;
using StoreApi.Services;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace StoreApi.Controllers
{
    public class ProductsController : BaseController
    {


        private ApplicationDbContext _context;
        private IMapper _mapper;
        private static readonly object _lockObject = new object();

        public ProductsController(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Products.Include(x => x.Brand).Select(x => new ProductDto()
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                AffiliateId = x.AffiliateId,
                Name = x.Name,
                Sku = x.Sku,
                Description = x.Description,
                AffiliateUrl = x.AffiliateUrl,
                AffiliateProgramId = x.AffiliateProgramId,
                FeatureImageUrl = x.FeatureImageUrl,
                Price = x.Price,
                DiscountPrice = x.DiscountPrice,
                BrandId = x.BrandId,
                Barcode = x.Barcode,
            }).Take(30).ToListAsync();

            return Ok(data);
        }

        [HttpGet("getallwithoptions")]
        public async Task<IActionResult> GetAllWithOptions(ListOptionsDto options)
        {
            var query = _context.Products.AsQueryable();
            // Search
            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                var s = options.SearchText;
                query = query.Where(x =>
                    x.Description.Contains(s) ||
                    x.Name.Contains(s) ||
                    x.AffiliateUrl.Contains(s) ||
                    x.Sku.Contains(s));
            }

            // Total count (optional – if you need pagination info)
            var totalCount = await query.CountAsync();

            // Paging
            if (options.PagingEnabled)
            {
                int page = options.PageNumber ?? 1;
                int size = options.ItemsPerPage ?? 20;

                query = query
                    .Skip((page - 1) * size)
                    .Take(size);
            }

            // Projection (Select) – only executed once
            var data = await query.Select(x => new ProductDto
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                AffiliateId = x.AffiliateId,
                Name = x.Name,
                Sku = x.Sku,
                Description = x.Description,
                AffiliateUrl = x.AffiliateUrl,
                AffiliateProgramId = x.AffiliateProgramId,
                FeatureImageUrl = x.FeatureImageUrl,
                Price = x.Price,
                DiscountPrice = x.DiscountPrice,
                BrandId = x.BrandId,
                Barcode = x.Barcode
            }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _context.Products.Include(x => x.Brand).Where(x => x.Id == id).Select(x => new ProductDto()
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                AffiliateId = x.AffiliateId,
                Name = x.Name,
                Sku = x.Sku,
                Description = x.Description,
                AffiliateUrl = x.AffiliateUrl,
                AffiliateProgramId = x.AffiliateProgramId,
                FeatureImageUrl = x.FeatureImageUrl,
                Price = x.Price,
                DiscountPrice = x.DiscountPrice,
                BrandId = x.BrandId,
                Barcode = x.Barcode,
            }).FirstOrDefaultAsync();

            var dto = _mapper.Map<ProductDto>(data);


            return Ok(dto);
        }

        [HttpPost("insertdto")]
        public async Task<IActionResult> InsertDto([FromBody] ProductDto dto)
        {
            var actionUser = await GetActionUser();

            var data = new Product();
            data.SerialNumber = dto.SerialNumber;
            data.AffiliateId = dto.AffiliateId;
            data.Name = dto.Name;
            data.Sku = dto.Sku;
            data.Description = dto.Description;
            data.AffiliateUrl = dto.AffiliateUrl;
            data.AffiliateProgramId = dto.AffiliateProgramId;
            data.FeatureImageUrl = dto.FeatureImageUrl;
            data.Price = dto.Price;
            data.IsInStock = dto.IsInStock;
            data.DiscountPrice = dto.DiscountPrice;
            data.BrandId = dto.BrandId;
            data.Barcode = dto.Barcode;

            lock (_lockObject)
            {
                var maxNumber = _context.Products.Max(x => (x.SerialNumber)) ?? 0;
                data.SerialNumber = maxNumber + 1;
                data.Code = data.SerialNumber.ToString().PadLeft(7, '0');

                try
                {
                    _context.Products.Add(data);
                    _context.SaveChanges();
                    LogService.CreateLog($"Product \"{data.Name}\" inserted by \"{actionUser.UserName}\". Product: {JsonConvert.SerializeObject(data)}", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);
                }
                catch (Exception ex)
                {
                    LogService.CreateLog($"Product \"{data.Name}\" could not be inserted by \"{actionUser.UserName}\". Product: {JsonConvert.SerializeObject(data)} Error: {ex.Message}", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);
                    throw;
                }
            }
            ;
            var dataToReturn = _mapper.Map<ProductDto>(data);

            return Ok(dataToReturn);
        }


        [HttpPut("updatedto")]
        public async Task<IActionResult> UpdateDto([FromBody] ProductDto dto)
        {
            var actionUser = await GetActionUser();

            var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
            data.SerialNumber = dto.SerialNumber;
            data.AffiliateId = dto.AffiliateId;
            data.Name = dto.Name;
            data.Sku = dto.Sku;
            data.Description = dto.Description;
            data.AffiliateUrl = dto.AffiliateUrl;
            data.AffiliateProgramId = dto.AffiliateProgramId;
            data.FeatureImageUrl = dto.FeatureImageUrl;
            data.Price = dto.Price;
            data.IsInStock = dto.IsInStock;
            data.DiscountPrice = dto.DiscountPrice;
            data.BrandId = dto.BrandId;
            data.Barcode = dto.Barcode;
            data.IsActive = dto.IsActive;

            try
            {
                await _context.SaveChangesAsync();
                LogService.CreateLog($"Product \"{data.Name}\" updated by \"{actionUser.UserName}\". Product: {JsonConvert.SerializeObject(data)}", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            catch (Exception ex)
            {
                LogService.CreateLog($"Product could not be updated by \"{actionUser.UserName}\". Product: {JsonConvert.SerializeObject(data)} Error: {ex.Message}", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }

            var dataToReturn = _mapper.Map<ProductDto>(data);

            return Ok(dataToReturn);
        }


        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var actionUser = await GetActionUser();

            var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Products.Remove(data);
                await _context.SaveChangesAsync();
                LogService.CreateLog($"Product \"{data.Name}\" deleted by \"{actionUser.UserName}\"  Product: {JsonConvert.SerializeObject(data)}.", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            catch (Exception ex)
            {
                LogService.CreateLog($"Product \"{data.Name}\" could not be deleted by \"{actionUser.UserName}\"  Product: {JsonConvert.SerializeObject(data)} Error:{ex.Message}.", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            return Ok(data);
        }


        [HttpPost("execute_import")]
        public async Task<IActionResult> ExecuteImport([FromBody] ImportSettingsDto dto)
        {
            if (dto.ImportType == ImportTypeEnum.Products)
            {
                if (!String.IsNullOrEmpty(dto.GetUrl))
                {
                    await FilesHandler.GetFileFromUrl(dto);
                }

                var products = FilesHandler.GetDataFromCSV(AppContext.BaseDirectory + "Downloads/" + dto.Name);

                if (products?.Count() > 0)
                {
                    await AddOrUpdateProducts(products, dto);
                    return Ok();
                }
                else
                {
                    return BadRequest();

                }

            }
            return Ok();

        }

        private async Task AddOrUpdateProducts(List<dynamic> dataLines, ImportSettingsDto dto)
        {
            foreach (var dataLine in dataLines)
            {
                {
                    var dict = (IDictionary<string, object>)dataLine;

                    var value = dict[dto.FileMatchProperty];
                    var dbRows = _context.Products.ToList(); // must be in memory

                    var dBProductExists = dbRows.Any(p =>
                    {
                        var prop = p.GetType().GetProperty(dto.DbMatchProperty);
                        if (prop == null) return false;

                        var val = prop.GetValue(p);
                        return val != null && val.ToString() == value.ToString();
                    });


                    if (!dBProductExists)
                    {
                        var productToAdd = new Product();
                        productToAdd.Name = dataLine.product_name;
                        productToAdd.Sku = dataLine.sku;
                        productToAdd.Description = dataLine.description;
                        productToAdd.AffiliateId = dataLine.lw_product_id;
                        productToAdd.AffiliateUrl = dataLine.tracking_url;
                        productToAdd.FeatureImageUrl = dataLine.image_url;

                        if (!String.IsNullOrEmpty(dataLine.full_price))
                        {
                            decimal fullPrice = decimal.Parse(dataLine.full_price, CultureInfo.CurrentCulture);
                            productToAdd.Price = fullPrice;

                        }

                        if (!String.IsNullOrEmpty(dataLine.price))
                        {
                            decimal discountPrice = decimal.Parse(dataLine.price, CultureInfo.CurrentCulture);

                            productToAdd.DiscountPrice = discountPrice;

                            if (productToAdd.Price == null)
                            {
                                productToAdd.Price = discountPrice;
                            }

                        }
                        //productToAdd.Pr = dataLine.program_name;


                        try
                        {
                            _context.Products.Add(productToAdd);
                        }
                        catch (Exception ex)
                        {

                        }



                    }
                    else
                    {
                        if (dto.UpdateExistingEntities)
                        {

                            var productToUpdate = await _context.Products.Where(x => x.AffiliateId == value).FirstOrDefaultAsync();

                            productToUpdate.Name = dataLine.product_name;
                            productToUpdate.Sku = dataLine.sku;
                            productToUpdate.Description = dataLine.description;
                            productToUpdate.AffiliateId = dataLine.lw_product_id;
                            productToUpdate.AffiliateUrl = dataLine.tracking_url;
                            productToUpdate.FeatureImageUrl = dataLine.image_url;
                            productToUpdate.Price = dataLine.full_price;
                            productToUpdate.DiscountPrice = dataLine.price;



                        }
                    }


                }


            }
            await _context.SaveChangesAsync();
        }

    }
}
