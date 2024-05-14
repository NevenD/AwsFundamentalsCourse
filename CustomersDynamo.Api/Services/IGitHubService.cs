namespace CustomersDynamo.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
