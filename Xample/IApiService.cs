using Refit;
using System.Threading.Tasks;

public interface IApiService
{
    [Get("/")]
    Task<string> GetAsync([AliasAs("url")] string url);
}
