using AutoMapper;
using Domain.Abstract;
using Domain.Model;
using Domain.Model.DTO;
using Domain.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Profiles {
	public class ResponseDTOMappingProfile : Profile {
		public ResponseDTOMappingProfile() {
			CreateMap<Entity, ResponseDTO>();

			CreateMap<Domain.Model.StageProfile, StageProfileResponseDTO>()
					 .IncludeBase<Entity, ResponseDTO>();

			CreateMap<User, UserResponseDTO>()
					 .IncludeBase<Entity, ResponseDTO>()
					 // We retourneren nooit het geencrypteerde wachtwoord als response
					 .ForMember(target => target.Password, 
								 opt => opt.MapFrom(src => ""))
					 .ForMember(target => target.ClearanceLevels,
								 opt => opt.MapFrom(src => src.ClearanceLevels.ConvertAll(cl => cl.ToString())));
		}
	}
}
