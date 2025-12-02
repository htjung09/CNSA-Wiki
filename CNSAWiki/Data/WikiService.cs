using System.Net.Http.Json;
using CNSAWiki.Models;

public class WikiService
{
    private readonly HttpClient _http;

    public WikiService(HttpClient http)
    {
        _http = http;
    }

    public Task<List<WikiPage>> GetPagesAsync()
        => _http.GetFromJsonAsync<List<WikiPage>>("api/wiki") ?? Task.FromResult(new List<WikiPage>());

    public Task<WikiPage?> GetPageAsync(long id)
        => _http.GetFromJsonAsync<WikiPage>($"api/wiki/{id}");

    public Task CreatePageAsync(WikiPage page)
        => _http.PostAsJsonAsync("api/wiki", page);

    public Task UpdatePageAsync(WikiPage page)
        => _http.PutAsJsonAsync($"api/wiki/{page.PageId}", page);

    public Task DeletePageAsync(long id)
        => _http.DeleteAsync($"api/wiki/{id}");
}
