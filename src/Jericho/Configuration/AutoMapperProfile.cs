﻿namespace Jericho.Configuration
{
    using System;

    using AutoMapper;

    using Extensions;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.DTOs.Favorite;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Jericho.Models.v1.Entities.Enums;
    using Jericho.Providers.ServiceResultProvider;

    using Microsoft.AspNetCore.Identity;

    using Models.v1.DTOs.Post;

    using MongoDB.Bson;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            this.ConfigureUserMappers();
            this.ConfigurePostMappers();
            this.ConfigureFavoriteMappers();
            this.ConfigureCommentMappers();
        }

        private void ConfigureUserMappers()
        {
            this.CreateMap<IdentityError, Error>();

            this.CreateMap<SaveUserRequestDto, ApplicationUser>()
                .ConstructUsing(dto => new ApplicationUser(dto.UserName, dto.EMail))
                .ForMember(user => user.Email, opt => opt.Ignore());

            this.CreateMap<ApplicationUser, GetUserResponseDto>()
                .ForMember(userDto => userDto.EMail, opt => opt.MapFrom(appUser => appUser.Email.NormalizedValue.ToLower()));
        }

        private void ConfigurePostMappers()
        {
            this.CreateMap<CreatePostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.MapFrom(postDto => ObjectId.Empty))
                .ForMember(postEntity => postEntity.Type, opt => opt.MapFrom(postDto => GetPostType(postDto.Type)))
                .ForMember(postEntity => postEntity.Url, opt=>opt.MapFrom(postDto => $"{postDto.Title.Trim().Replace(' ', '_').ToLower()}_{DateTime.UtcNow.ToTimeStamp()}"))
                .ForMember(postEntity => postEntity.UpVotes, opt => opt.MapFrom(postDto => 0))
                .ForMember(postEntity => postEntity.DownVotes, opt => opt.MapFrom(postDto => 0))
                .ForMember(postEntity => postEntity.CreatedOn, opt => opt.MapFrom(postDto => DateTime.Now));

            this.CreateMap<UpdatePostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.MapFrom(postDto => string.IsNullOrEmpty(postDto.Id) ? ObjectId.Empty : ObjectId.Parse(postDto.Id)));

            this.CreateMap<PostEntity, UpdatePostDto>()
                .ForMember(postDto => postDto.Id, opt => opt.MapFrom(postEntity => postEntity.Id.ToString()));
        }

        private PostTypeEnum GetPostType(string postType)
        {
            PostTypeEnum type;
            Enum.TryParse(postType, out type);
            return type;
        }

        private void ConfigureFavoriteMappers()
        {
            this.CreateMap<SaveFavoriteDirectoryDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.CreatedOn, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Directory));

            this.CreateMap<SaveFavoritePostDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.CreatedOn, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Post));

            this.CreateMap<FavoriteEntity, GetFavoriteResponseDto>()
                .ForMember(getFavoriteDirectoryDto => getFavoriteDirectoryDto.FavoriteType, opt => opt.MapFrom(favoriteEntity => favoriteEntity.FavoriteType.ToString()));                
        }
        
        private void ConfigureCommentMappers()
        {
            this.CreateMap<CommentDto, CommentEntity>()
                .ForMember(commentEntity => commentEntity.Type, opt => opt.MapFrom(commentDTO => GetCommentType(commentDTO.Type)))
                .ForMember(commentEntity => commentEntity.Id, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.Id) ? ObjectId.Empty : ObjectId.Parse(commentDto.Id)))
                .ForMember(commentEntity => commentEntity.PostId, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.PostId) ? ObjectId.Empty : ObjectId.Parse(commentDto.PostId)))
                .ForMember(commentEntity => commentEntity.ParentId, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.ParentId) ? ObjectId.Empty : ObjectId.Parse(commentDto.ParentId)));


            this.CreateMap<CommentEntity, CommentDto>()
                .ForMember(commentDto => commentDto.Type, opt => opt.MapFrom(commentEntity => commentEntity.Type.ToString()))
                .ForMember(commentDto => commentDto.Id, opt => opt.MapFrom(commentEntity => commentEntity.Id.ToString()))
                .ForMember(commentDto => commentDto.PostId, opt => opt.MapFrom(commentEntity => commentEntity.PostId.ToString()))
                .ForMember(commentDto => commentDto.ParentId, opt => opt.MapFrom(commentEntity => commentEntity.ParentId.ToString()));
        }

        
        private CommentTypeEnum GetCommentType(string commentDTOType)
        {
            CommentTypeEnum type;
            Enum.TryParse(commentDTOType, out type);
            return type;
        }
    }
}