 using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecifications;
using Talabt.APIs.DTOs;
using Talabt.APIs.Errors;
using Talabt.APIs.Helpers;

namespace Talabt.APIs.Controllers
{
    public class ProductsController : BaseAPIController
    {
        //private readonly IGenaricRepository<Product> _productsRepo;
        //private readonly IGenaricRepository<ProductBrand> _brandsRepo;
        //private readonly IGenaricRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper,IProductService productService/*IGenaricRepository<ProductBrand> brandsRepo,IGenaricRepository<ProductCategory> categoryRepo*/)
        {
            _mapper = mapper;
            _productService = productService;
            //_productsRepo = productsRepo;
            //_brandsRepo = brandsRepo;
            //_categoryRepo = categoryRepo;
        }


        // /api/Products
        //[Authorize/*(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)*/]
        [CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams /*string? sort , int?CategoryId , int?BrandId*/)
         {
            var products = await _productService.GetProductsAsync(specParams);
            var count =await _productService.GetCountAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count,data));
        }


        // /api/Products/1
        [CachedAttribute(600)]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));

        }

        // /api/Products/brands
        [CachedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands= await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        // /api/Products/categories
        [CachedAttribute(600)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
