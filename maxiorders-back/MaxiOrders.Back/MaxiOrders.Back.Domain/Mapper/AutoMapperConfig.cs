using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxiOrders.Back.Domain.Mapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.ValidateInlineMaps = false;
                cfg.AllowNullCollections = true;
                //cfg.AddProfile(new DomainToViewModelMappingProfile());
                //cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}
