using AutoMapper;
using Domain.Model.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Profiles {
	public class RequestDTOMappingProfile : AutoMapper.Profile {
		public RequestDTOMappingProfile() {
			CreateMap<StageProfileRequestDTO, Domain.Model.StageProfile>();
		}
	}
}
