using AmazonHelper.WebApp.Models;
using BL = AmazonHelper.Business.Models;
using AutoMapper;

namespace AmazonHelper.WebApp.App_Start
{
    public class WebAppMapper
    {
        public static void RegisterMappings()
        {
            //Product
            Mapper.CreateMap<BL.Product, ProductModel>();
            Mapper.CreateMap<BL.Product, ProductViewModel>();
            Mapper.CreateMap<ProductModel, BL.Product>();
        }

        public ProductModel MapProduct(BL.Product product)
        {
            return Mapper.Map<ProductModel>(product);
        }

        public ProductViewModel MapProductViewModel(BL.Product product)
        {
            return Mapper.Map<ProductViewModel>(product);
        }

        public BL.Product MapProduct(ProductModel product)
        {
            return Mapper.Map<BL.Product>(product);
        }
    }
}