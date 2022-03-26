using Domain.Static;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract {
	internal interface IPaginatedRequest<out TResponse> : IRequest<TResponse> {
		public int Page { get; set; }
		public int PageSize { get; set; }
	}

	public abstract class PaginatedRequest<TResponse> : IRequest<TResponse>, IPaginatedRequest<TResponse> {
		public int Page { get; set; } = ApiConfig.DefaultPage;
		public int PageSize { get; set; } = ApiConfig.DefaultPageSize;
		public int Skip => (Page * PageSize) - PageSize;
		public int Take => PageSize;
	}
}
