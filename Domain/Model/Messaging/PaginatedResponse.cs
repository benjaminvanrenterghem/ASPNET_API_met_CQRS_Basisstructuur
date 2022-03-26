namespace Domain.Model.Messaging {
	public class PaginatedResponse<T> : Response<T> { 
		public int CurrentPage { get; set; }
		public int AmountPerPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalRecords { get; set; }

        public PaginatedResponse(T response, int page, int pageSize, int totalRecords) : base(response) {
            CurrentPage = page;
		    AmountPerPage = pageSize;
            TotalPages = (int)Math.Ceiling(totalRecords / ((decimal)pageSize));
            TotalRecords = totalRecords;
        }
    }
}
