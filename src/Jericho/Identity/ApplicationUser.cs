﻿namespace Jericho.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using Jericho.Identity.Models;

    using Microsoft.AspNet.Identity;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ApplicationUser : IUser
    {
        #region Fields

        private readonly List<MongoUserClaim> _claims;

        private readonly List<MongoUserLogin> _logins;

        #endregion

        #region Constructor

        public ApplicationUser(string userName, string email) : this(userName)
        {
            if (email != null)
            {
                Email = new MongoUserEmail(email);
            }
        }

        public ApplicationUser(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
            CreatedOn = new Occurrence();
            _claims = new List<MongoUserClaim>();
            _logins = new List<MongoUserLogin>();
        }

        #endregion

        #region Properties

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NormalizedUserName { get; private set; }

        public MongoUserEmail Email { get; private set; }

        public string PasswordHash { get; private set; }

        public string SecurityStamp { get; private set; }

        public bool IsTwoFactorEnabled { get; private set; }

        [BsonIgnoreIfNull]
        public IEnumerable<MongoUserClaim> Claims
        {
            get
            {
                return _claims;
            }

            private set
            {
                if (value != null && value.Any())
                {
                    _claims.AddRange(value);
                }
            }
        }

        public IEnumerable<MongoUserLogin> Logins
        {
            get
            {
                return _logins;
            }

            private set
            {
                if (value != null && value.Any())
                {
                    _logins.AddRange(value);
                }
            }
        }

        public int AccessFailedCount { get; private set; }

        public bool IsLockoutEnabled { get; private set; }

        public FutureOccurrence LockoutEndDate { get; private set; }

        public Occurrence CreatedOn { get; private set; }

        public Occurrence DeletedOn { get; private set; }

        #endregion

        #region Methods
        public virtual void EnableTwoFactorAuthentication()
        {
            IsTwoFactorEnabled = true;
        }

        public virtual void DisableTwoFactorAuthentication()
        {
            IsTwoFactorEnabled = false;
        }

        public virtual void EnableLockout()
        {
            IsLockoutEnabled = true;
        }

        public virtual void DisableLockout()
        {
            IsLockoutEnabled = false;
        }

        public virtual void SetEmail(string email)
        {
            var mongoUserEmail = new MongoUserEmail(email);
            SetEmail(mongoUserEmail);
        }

        public virtual void SetEmail(MongoUserEmail mongoUserEmail)
        {
            Email = mongoUserEmail;
        }

        public virtual void SetNormalizedUserName(string normalizedUserName)
        {
            if (normalizedUserName == null)
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }

            NormalizedUserName = normalizedUserName;
        }

        public virtual void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public virtual void SetSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
        }

        public virtual void SetAccessFailedCount(int accessFailedCount)
        {
            AccessFailedCount = accessFailedCount;
        }

        public virtual void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
        }

        public virtual void LockUntil(DateTime lockoutEndDate)
        {
            LockoutEndDate = new FutureOccurrence(lockoutEndDate);
        }

        public virtual void AddClaim(Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            AddClaim(new MongoUserClaim(claim));
        }


        public virtual void AddClaim(MongoUserClaim mongoUserClaim)
        {
            if (mongoUserClaim == null)
            {
                throw new ArgumentNullException(nameof(mongoUserClaim));
            }

            _claims.Add(mongoUserClaim);
        }


        public virtual void RemoveClaim(MongoUserClaim mongoUserClaim)
        {
            if (mongoUserClaim == null)
            {
                throw new ArgumentNullException(nameof(mongoUserClaim));
            }

            _claims.Remove(mongoUserClaim);
        }


        public virtual void AddLogin(MongoUserLogin mongoUserLogin)
        {
            if (mongoUserLogin == null)
            {
                throw new ArgumentNullException(nameof(mongoUserLogin));
            }

            _logins.Add(mongoUserLogin);
        }

        public virtual void RemoveLogin(MongoUserLogin mongoUserLogin)
        {
            if (mongoUserLogin == null)
            {
                throw new ArgumentNullException(nameof(mongoUserLogin));
            }

            _logins.Remove(mongoUserLogin);
        }


        public void Delete()
        {
            if (DeletedOn != null)
            {
                throw new InvalidOperationException($"User '{Id}' has already been deleted.");
            }

            DeletedOn = new Occurrence();
        }

        private static string GenerateId(string userName)
        {
            return userName.ToLower();
        }

        #endregion
    }
}