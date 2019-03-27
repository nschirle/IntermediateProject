using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogservice;

        public CatalogController(CatalogService service)
        {
            _catalogservice = service;
        }


        public async Task<IActionResult> Index(int? brand, int? type, int? page)
        {
            var itemsOnPage = 10;
             var catalog = await _catalogservice.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brand, type);

            var vm = new CatalogIndexViewModel
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogservice.GetBrandsAsync(),
                Types = await _catalogservice.GetTypesAsync(),
                BrandFilterApplied = brand ?? 0,
                TypesFilterApplied = type ?? 0,
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                }
            };
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            return View(vm);
        }
    }
}