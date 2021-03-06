using System.ComponentModel.DataAnnotations;
using Jericho.Validations;
using Jericho.Validations.Interfaces;

namespace Jericho.Models.v1.Entities
{
    using System;
    using System.Collections.Generic;

    using Jericho.Models.v1.Entities.Enums;

    using Newtonsoft.Json;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;
    using Newtonsoft.Json.Converters;
    using Providers.ServiceResultProvider;

    public class CommentEntity : IValidatableEntity
    {
        #region Properties

        [BsonId]
        [JsonProperty(PropertyName = "id")]
        public ObjectId Id { get; set; }

        [BsonElement("postid")]
        [JsonProperty(PropertyName = "postid")]
        public ObjectId PostId { get; set; }

        [BsonElement("parentid")]
        [JsonProperty(PropertyName = "parentid")]
        public ObjectId ParentId { get; set; }

        [JsonProperty(PropertyName = "url")]
        [BsonElement("url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("type")]
        public CommentTypeEnum Type { get; set; }

        [JsonProperty(PropertyName = "text")]
        [BsonElement("text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "upvotes")]
        [BsonElement("upvotes")]
        public long UpVotes { get; set; }

        [JsonProperty(PropertyName = "downvotes")]
        [BsonElement("downvotes")]
        public long DownVotes { get; set; }

        [JsonProperty(PropertyName = "postedby")]
        [BsonElement("postedby")]
        public string PostedBy { get; set; }

        [JsonProperty(PropertyName = "createdon")]
        [BsonElement("createdon")]
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Id.ToString();
        }

        public IEnumerable<Error> Validate()
        {
            var validationErrors = new List<Error>();

            this.RequiredValidationRule(nameof(PostId), validationErrors);
            this.RequiredValidationRule(nameof(ParentId), validationErrors);
            this.RequiredValidationRule(nameof(Type), validationErrors);

            switch (Type)
            {
                case CommentTypeEnum.None:
                    this.RequiredValidationRule(nameof(Text), validationErrors);
                    break;
                case CommentTypeEnum.Image:
                case CommentTypeEnum.Gif:
                case CommentTypeEnum.Video:
                    this.RequiredValidationRule(nameof(Url), validationErrors);
                break;
                default:
                    break;
            }
            return validationErrors;
        }

        #endregion
    }
}