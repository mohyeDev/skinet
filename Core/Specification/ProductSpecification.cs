using System;
using Core.Entities;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProcutSpecParams specParams) : base(x => 
        (!specParams.brands.Any()|| specParams.brands.Contains(x.Brand)) &&
        (!specParams.Types.Any()||specParams.Types.Contains(x.Type))
    )
    {
    ApplyPaging(specParams.PageSize * (specParams.PageIndex -1 ) , specParams.PageSize);

        switch (specParams.sort)
        {
            case "priceAsc":
            AddOrderBy(x => x.Price);
            break ; 

            case "priceDesc":
            AddOrderByDescending(x => x.Price);
            break ; 

            default:
            AddOrderBy(x => x.Name);
            break; 
        }
    }

}
