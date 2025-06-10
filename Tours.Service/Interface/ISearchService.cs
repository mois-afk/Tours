namespace Tours.Service.Interface
{
    using Tours.Models;

    public interface ISearchService
    {
        public Task<SearchModel> SearchResultAsync(SearchModel model);
    }
}
