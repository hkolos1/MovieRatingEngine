namespace MovieRatingEngine.Dtos
{
    public class PaginationNumbers
    {
        //pagination
        const int maxPageSize = 13;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;


        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public PaginationNumbers()
        {
            PageNumber = 1;
            PageSize = 10;
        }



    }
}
