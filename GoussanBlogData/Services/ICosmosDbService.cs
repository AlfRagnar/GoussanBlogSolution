﻿using GoussanBlogData.Models.MediaModels;
using Microsoft.Azure.Cosmos;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    // USER API
    Task<ItemResponse<Models.UserModels.User>> AddUser(Models.UserModels.User user);
    Task<Models.UserModels.User> GetUserAsync(string id);
    Task<IEnumerable<Models.UserModels.User>> GetUsersAsync(string queryString);
    Task<IEnumerable<Models.UserModels.User>> GetUserByName(string Username);
    Task<IEnumerable<Models.UserModels.User>> GetUserByMail(string Email);
    Task<IEnumerable<Models.UserModels.User>> CheckUser(string Username, string Email);

    // VIDEO API
    Task AddVideo(UploadVideo video);
    Task DeleteVideoAsync(string id);
    Task<IEnumerable<UploadVideo>> GetMultipleVideosAsync(string queryString);
    Task<UploadVideo> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, UploadVideo video);
    Task<IEnumerable<UploadVideo>> GetVideoList();
}
