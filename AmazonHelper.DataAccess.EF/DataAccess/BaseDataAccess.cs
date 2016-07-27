namespace AmazonHelper.DataAccess.Services
{
    using AutoMapper;
    using Models;
    using EF.Models;

    public class BaseDataAccess
    {
        public BaseDataAccess()
        {
            Mapper = new DaMapper();
        }
        public DaMapper Mapper { get; set; }
    }

    public class DaMapper
    {
        public static void RegisterMappings()
        {
            //Product
            Mapper.CreateMap<Product, ProductDto>();
            Mapper.CreateMap<ProductDto, Product>();

            //User
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>();

            //CommonSettings
            Mapper.CreateMap<CommonSettings, CommonSettingsDto>();
            Mapper.CreateMap<CommonSettingsDto, CommonSettings>();

            //CommonSettings
            Mapper.CreateMap<ProductScanningProcess, ProductScanningProcessDto>();
            Mapper.CreateMap<ProductScanningProcessDto, ProductScanningProcess>();
        }

        public Product MapProduct(ProductDto productDto)
        {
            return Mapper.Map<Product>(productDto);
        }

        public ProductDto MapProduct(Product product)
        {
            return Mapper.Map<ProductDto>(product);
        }

        public User MapUser(UserDto userDto)
        {
            return Mapper.Map<User>(userDto);
        }
        public UserDto MapUser(User user)
        {
            return Mapper.Map<UserDto>(user);
        }

        public CommonSettings MapCommonSettings(CommonSettingsDto CommonSettingsDto)
        {
            return Mapper.Map<CommonSettings>(CommonSettingsDto);
        }
        public CommonSettingsDto MapCommonSettings(CommonSettings commonSettings)
        {
            return Mapper.Map<CommonSettingsDto>(commonSettings);
        }
    }
}
