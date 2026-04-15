using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data.Dto;
using StoreApi.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreApi.Data.Enums;
using StoreApi.Data.Models;
using StoreApi.Services;

namespace StoreApi.Controllers
{
    public class ImportsSettingsController : BaseController
    {


        private ApplicationDbContext _context;
        private IMapper _mapper;
        private static readonly object _lockObject = new object();

        public ImportsSettingsController(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ImportSettings.ToListAsync();

            var dataToReturn = _mapper.Map<ImportSettingsDto[]>(data);

            return Ok(dataToReturn);
        }


        [HttpPost("insertdto")]
        public async Task<IActionResult> InsertDto([FromBody] ImportSettingsDto dto)
        {
            var actionUser = await GetActionUser();

            var data = new ImportSetting();
            data.Title = dto.Title;
            data.Folder = dto.Folder;
            data.GetUrl = dto.GetUrl;
            data.Name = dto.Name;
            data.ImportType = dto.ImportType;
            data.UpdateExistingEntities = dto.UpdateExistingEntities;
            data.DbMatchProperty = dto.DbMatchProperty;;
            data.FileMatchProperty = dto.FileMatchProperty;;


            lock (_lockObject)
            {
                var maxNumber = _context.ImportSettings.Max(x => (x.SerialNumber)) ?? 0;
                data.SerialNumber = maxNumber + 1;
                data.Code = data.SerialNumber.ToString().PadLeft(7, '0');

                try
                {
                    _context.Add(data);
                    _context.SaveChanges();
                    LogService.CreateLog($"Data \"{data.Name}\" inserted by \"{actionUser.UserName}\". Data: {JsonConvert.SerializeObject(data)}", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);
                }
                catch (Exception ex)
                {
                    LogService.CreateLog($"Data \"{data.Name}\" could not be inserted by \"{actionUser.UserName}\". Data: {JsonConvert.SerializeObject(data)} Error: {ex.Message}", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);
                    throw;
                }
            }
            ;
            var dataToReturn = _mapper.Map<ImportSettingsDto>(data);

            return Ok(dataToReturn);
        }



        [HttpPut("updatedto")]
        public async Task<IActionResult> UpdateDto([FromBody] ImportSettingsDto dto)
        {
            var actionUser = await GetActionUser();

            var data = await _context.ImportSettings.FirstOrDefaultAsync(x => x.Id == dto.Id);
            data.Title = dto.Title;
            data.Folder = dto.Folder;
            data.GetUrl = dto.GetUrl;
            data.Name = dto.Name;
            data.ImportType = dto.ImportType;
            data.UpdateExistingEntities = dto.UpdateExistingEntities;
            data.DbMatchProperty = dto.DbMatchProperty;
            data.FileMatchProperty = dto.FileMatchProperty;


            try
            {
                await _context.SaveChangesAsync();
                LogService.CreateLog($"Data \"{data.Name}\" updated by \"{actionUser.UserName}\". Data: {JsonConvert.SerializeObject(data)}", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            catch (Exception ex)
            {
                LogService.CreateLog($"Data could not be updated by \"{actionUser.UserName}\". Data: {JsonConvert.SerializeObject(data)} Error: {ex.Message}", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }

            var dataToReturn = _mapper.Map<ImportSettingsDto>(data);

            return Ok(dataToReturn);
        }


        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var actionUser = await GetActionUser();

            var data = await _context.ImportSettings.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.ImportSettings.Remove(data);
                await _context.SaveChangesAsync();
                LogService.CreateLog($"Data \"{data.Name}\" deleted by \"{actionUser.UserName}\"  Data: {JsonConvert.SerializeObject(data)}.", LogTypeEnum.Information, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            catch (Exception ex)
            {
                LogService.CreateLog($"Data \"{data.Name}\" could not be deleted by \"{actionUser.UserName}\"  Data: {JsonConvert.SerializeObject(data)} Error:{ex.Message}.", LogTypeEnum.Error, LogOriginEnum.StoreApp, actionUser.Id, _context);

            }
            return Ok(data);
        }
    }
}
