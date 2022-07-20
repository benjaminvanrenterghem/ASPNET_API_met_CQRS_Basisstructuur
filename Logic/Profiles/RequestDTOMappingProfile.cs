using AutoMapper;
using Domain.Abstract;
using Domain.Model;
using Domain.Model.DTO;
using Domain.Model.DTO.Converted;
using Domain.Model.DTO.Request;
using Micro2Go.Extensions;
using Micro2Go.Model;

namespace Logic.Profiles {
	public class RequestDTOMappingProfile : Profile {
		public RequestDTOMappingProfile() {
			CreateMap<RequestDTO, Entity>()
					.ForMember(target => target.Deleted, 
								opt => opt.MapFrom(src => false)
					).ForMember(target => target.CreatedDate,
								opt => opt.MapFrom<DateTime?>(src => null)
					).ForMember(target => target.UpdatedDate,
								opt => opt.MapFrom<DateTime?>(src => null)
					).ForMember(target => target.RowVersion,
								opt => opt.MapFrom<byte[]?>(src => null)
					);

			CreateMap<StageProfileRequestDTO, Domain.Model.StageProfile>()
					.IncludeBase<RequestDTO, Entity>()
					.ForMember(target => target.OwnerId,
								opt => opt.MapFrom(src => src.OwnerUserId)
					).ForMember(target => target.Owner,
								opt => opt.MapFrom<object?>(src => null)
					);

			CreateMap<UserRequestDTO, User>()
					.IncludeBase<RequestDTO, Entity>()
					.ForMember(target => target.ClearanceLevels,
								opt => opt.MapFrom(src =>
									src.ClearanceLevels.ConvertAll(cl => Enum.Parse<ClearanceLevel>(cl)))
					).ForMember(target => target.Profiles,
								 opt => opt.MapFrom(src => new List<Domain.Model.StageProfile>())
					).ForMember(target => target.Password,
								 opt => opt.MapFrom(src => src.Password.GetSHA256String())
					);

			CreateMap<LoginRequestDTO, ConvertedLoginRequestDTO>()
					.ForMember(target => target.SHA256Password,
							    opt => opt.MapFrom(src => src.Password.GetSHA256String()));
		}
	}
}
