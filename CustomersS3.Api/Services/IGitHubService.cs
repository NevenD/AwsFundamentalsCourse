namespace CustomersS3.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
