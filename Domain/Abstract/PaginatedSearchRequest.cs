using Domain.Static;
using MediatR;

namespace Domain.Abstract {
	internal interface IPaginatedSearchRequest<out TResponse> : IRequest<TResponse> {
		public string? SearchPropertyName { get; set; }
		public object? SearchValue { get; set; }

		public int Page { get; set; }
		public int PageSize { get; set; }
		public int Skip { get; }
		public int Take { get; }
	}

	public abstract class PaginatedSearchRequest<TResponse> : IRequest<TResponse>, IPaginatedSearchRequest<TResponse> {
		public string? SearchPropertyName { get; set; } = null;
		public object? SearchValue { get; set; } = null;

		public int Page { get; set; } = ApiConfig.DefaultPage;
		public int PageSize { get; set; } = ApiConfig.DefaultPageSize;
		public int Skip => (Page * PageSize) - PageSize;
		public int Take => PageSize;
	}


}
