namespace JuanApp.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }

        public PaginatedList(List<T> _items, int _pageCount, int _page)
        {
            this.AddRange(_items);
            PageCount = _pageCount;
            CurrentPage = _page;
            HasNext = CurrentPage < PageCount;
            HasPrev = CurrentPage > 1;
        }
        public static PaginatedList<T> Create(IQueryable<T> query, int page, int take)
        {
            int count = query.Count();
            var items = query.Skip((page - 1) * take).Take(take).ToList();
            var pageCount = (int)Math.Ceiling((decimal)count / take);

            return new PaginatedList<T>(items, pageCount, page);
        }
    }
}
