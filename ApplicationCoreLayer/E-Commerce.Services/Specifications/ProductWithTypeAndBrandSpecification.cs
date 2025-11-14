using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product,int>
    {
        // Get All Products
        public ProductWithTypeAndBrandSpecification(ProductQueryParams queryParams) 
            : base(ProductSpecificationsHelper.GetProductCriteria(queryParams))
        {
            AddInclude(P => P.ProductType);
            AddInclude(P => P.ProductBrand);

            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc: 
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc: 
                    AddOrderByDesc(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        // get product by Id
        public ProductWithTypeAndBrandSpecification( int id ) : base(P => P.Id == id)
        {
            AddInclude(P => P.ProductType);
            AddInclude(P => P.ProductBrand);
        }
    }
}
