﻿namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Providers.ServiceResultProvider;
    using Jericho.Models;
    using Jericho.Models.v1.Entities;

    using Microsoft.Azure.Documents;
    using Microsoft.AspNetCore.Http;

    public interface IPostService
    {
        Task<ServiceResult<PostEntity>> CreatePostAsync(PostEntity post);

        Task<PostEntity> GetPostAsync(string id);
        
        Task<IEnumerable<PostEntity>> GetPostsAsync(IQueryCollection query, int page, int limit);

        Task<bool> UpdatePostAsync(PostEntity postEntity);

        Task<bool> DeletePostAsync(string id);
        
    }
}
