using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Application.Interface;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Transversal.Common;
using System.Text;
using System.Text.Json;

namespace Pacagroup.Ecommerce.Application.Main
{
    public class CategoriesApplication : ICategoriesApplication
    {
        private readonly ICategoriesDomain _categoriesDomain;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public CategoriesApplication(ICategoriesDomain categoriesDomain, IMapper mapper, IDistributedCache distributedCache)
        {
            _categoriesDomain = categoriesDomain;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<Response<IEnumerable<CategoriesDto>>> GetAll()
        {
            var response = new Response<IEnumerable<CategoriesDto>>();
            var cacheKey = "categoriesList";

            try
            {
                var redisCategories = await _distributedCache.GetAsync(cacheKey);
                if (redisCategories != null)
                {
                    response.Data = JsonSerializer.Deserialize<IEnumerable<CategoriesDto>>(redisCategories);
                }
                else
                {
                    var categories = await _categoriesDomain.GetAll();
                    response.Data = _mapper.Map<IEnumerable<CategoriesDto>>(categories);
                    if (response.Data != null)
                    {
                        var serializedCategories = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response.Data));
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(8))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                        await _distributedCache.SetAsync(cacheKey, serializedCategories, options);
                    }
                }

                if (response.Data != null)
                {
                    response.IsSuccess = true;
                    response.Message = "Consulta Exitosa!!!";
                }

            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }
    }
}
