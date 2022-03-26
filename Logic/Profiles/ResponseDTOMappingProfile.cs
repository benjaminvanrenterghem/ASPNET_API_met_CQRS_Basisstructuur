using AutoMapper;
using Domain.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Profiles {
	public class ResponseDTOMappingProfile : AutoMapper.Profile {
		public ResponseDTOMappingProfile() {
			CreateMap<Domain.Model.Profile, ProfileResponseDTO>();
		}
	}
}
