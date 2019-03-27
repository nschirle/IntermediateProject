using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService
    {
        private IHttpClient _client;
        private readonly string _remoteServiceBaseUrl;
        public CatalogService(IHttpClient client, IConfiguration configuration)
        {
            _client = client;
            _remoteServiceBaseUrl = $"{configuration["CatalogUrl"]}/api/catalog/";
        }

        public async Task<Catalog> GetCatalogItemsAsync( int page, int size, int? brand, int? type)
        {
            var allcatalogitemsuri = ApiPaths.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, size, page, brand, type);

            var dataString =  await _client.GetStringAsync(allcatalogitemsuri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
        {
           var brandsUri =  ApiPaths.Catalog.GetAllBrands(_remoteServiceBaseUrl);

           var dataString = await _client.GetStringAsync(brandsUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected=true
                }
            };

            var brands=JArray.Parse(dataString);
            foreach (var brand in brands)
            {
                items.Add(new SelectListItem
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("brand"),

                });
            }

            return items;
        }


        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typesUri = ApiPaths.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var dataString = await _client.GetStringAsync(typesUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected=true
                }
            };

            var types = JArray.Parse(dataString);
            foreach (var type in types)
            {
                items.Add(new SelectListItem
                {
                    Value = type.Value<string>("id"),
                    Text = type.Value<string>("type"),

                });
            }

            return items;
        }
    }
}
