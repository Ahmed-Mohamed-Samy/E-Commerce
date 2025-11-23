using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Exceptions;
using E_Commerce.Services.Specifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            
            var Brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
            
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            var specifications = new ProductWithTypeAndBrandSpecification(queryParams);
            var Products = await ProductRepo.GetAllAsync(specifications);
            var Data = _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CountSpec = new ProductCountSpecifications(queryParams);
            var CountOfAllProducts = await ProductRepo.CountAsync(CountSpec);
            var CountOfReturnedData = Data.Count();

            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfReturnedData, CountOfAllProducts, Data);

        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithTypeAndBrandSpecification(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            if (Product is null)
                throw new ProductNotFoundException(id);
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
