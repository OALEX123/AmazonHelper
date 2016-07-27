namespace AmazonHelper.Business.Services
{
    using AutoMapper;
    using BL = Models;
    using DA = DataAccess.Models;

    public class BaseService
    {
        public BaseService()
        {
            Mapper = new BlMapper();
        }
        public BlMapper Mapper { get; set; }
    }

    public class BlMapper
    {
        public static void RegisterMappings()
        {
            //Product
            Mapper.CreateMap<BL.Product, DA.Product>();
            Mapper.CreateMap<DA.Product, BL.Product>();

            //Product Group
            Mapper.CreateMap<BL.ProductGroup, DA.ProductGroup>();
            Mapper.CreateMap<DA.ProductGroup, BL.ProductGroup>();

            //User
            Mapper.CreateMap<BL.User, DA.User>();
            Mapper.CreateMap<DA.User, BL.User>();

            //CommonSettings
            Mapper.CreateMap<BL.CommonSettings, DA.CommonSettings>();
            Mapper.CreateMap<DA.CommonSettings, BL.CommonSettings>();

            //CommonSettings
            Mapper.CreateMap<BL.StatsEntry, DA.StatsEntry>();
            Mapper.CreateMap<DA.StatsEntry, BL.StatsEntry>();
        }

        public BL.Product MapProduct(DA.Product productDto)
        {
            return Mapper.Map<BL.Product>(productDto);
        }

        public DA.Product MapProduct(BL.Product product)
        {
            return Mapper.Map<DA.Product>(product);
        }

        public DA.ProductGroup MapProductGroup(BL.ProductGroup productGroup)
        {
            return Mapper.Map<DA.ProductGroup>(productGroup);
        }

        public BL.ProductGroup MapProductGroup(DA.ProductGroup productGroup)
        {
            return Mapper.Map<BL.ProductGroup>(productGroup);
        }

        public BL.User MapUser(DA.User userDto)
        {
            return Mapper.Map<BL.User>(userDto);
        }
        public DA.User MapUser(BL.User user)
        {
            return Mapper.Map<DA.User>(user);
        }

        public BL.CommonSettings MapCommonSettings(DA.CommonSettings commonSettings)
        {
            return Mapper.Map<BL.CommonSettings>(commonSettings);
        }
        public DA.CommonSettings MapCommonSettings(BL.CommonSettings commonSettings)
        {
            return Mapper.Map<DA.CommonSettings>(commonSettings);
        }

        public BL.StatsEntry MapStatEntry(DA.StatsEntry statsEntry)
        {
            return Mapper.Map<BL.StatsEntry>(statsEntry);
        }
        public DA.StatsEntry MapStatEntry(BL.StatsEntry statsEntry)
        {
            return Mapper.Map<DA.StatsEntry>(statsEntry);
        }
    }
}
