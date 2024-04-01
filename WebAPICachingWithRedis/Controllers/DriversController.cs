using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebAPICachingWithRedis.Models;
using WebAPICachingWithRedis.Service;

namespace WebAPICachingWithRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _db;
        public DriversController(ICacheService cacheService, AppDbContext appDbContext)
        {
            _cacheService = cacheService;   
            _db = appDbContext;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers");

            if (cacheData!=null)
            {
                return Ok(cacheData);
            }
            cacheData =await _db.Drivers.ToListAsync();

            var expireTime = DateTimeOffset.Now.AddSeconds(30);
            _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expireTime);

            return Ok(cacheData);   
        }
    }
}
