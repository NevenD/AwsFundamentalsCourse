using CustomersS3.Api.Contracts.Data;
using CustomersS3.Api.Domain;

namespace CustomersS3.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = Guid.Parse(customerDto.Id),
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
}
