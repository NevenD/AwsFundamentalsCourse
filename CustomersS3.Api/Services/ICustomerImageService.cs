﻿using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace CustomersS3.Api.Services
{
    public interface ICustomerImageService
    {
        Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);
        Task<GetObjectResponse> GetImageAsync(Guid id);
        Task<DeleteObjectResponse> DeleteImageAsync(Guid id);
    }
}
