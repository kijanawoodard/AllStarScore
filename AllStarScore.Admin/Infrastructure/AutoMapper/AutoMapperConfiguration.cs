using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using AutoMapper;

namespace AllStarScore.Admin.Infrastructure.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<CompetitionCreateCommand, Competition>()
                .ForMember(x => x.Id, o => o.Ignore())
                ;
        }
    }
}