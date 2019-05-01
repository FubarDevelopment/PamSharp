// <copyright file="RootUserInfo.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Linq;

using Mono.Unix;

namespace LoginSharp
{
    /// <summary>
    /// Some information around the user being root or a <c>sudoer</c>.
    /// </summary>
    public class RootUserInfo
    {
        public RootUserInfo()
        {
            var currentUser = UnixUserInfo.GetRealUser();
            Info = currentUser;
            IsUserRoot = currentUser.UserId == 0;
            var rootGroup = new UnixGroupInfo(0);
            IsGroupRoot = rootGroup.GetMembers().Any(x => x.UserId == currentUser.UserId);
            var groups = UnixGroupInfo.GetLocalGroups();
            var sudoGroup = groups.FirstOrDefault(x => x.GroupName == "sudo");
            IsSudo = sudoGroup != null && sudoGroup.GetMembers().Any(x => x.UserId == currentUser.UserId);
        }

        /// <summary>
        /// Gets a value indicating whether the user is root or in the root group.
        /// </summary>
        public bool IsRoot => IsUserRoot || IsGroupRoot;

        /// <summary>
        /// Gets a value indicating whether the user is the root user.
        /// </summary>
        public bool IsUserRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the user is in the root group.
        /// </summary>
        public bool IsGroupRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the user is in the <c>sudo</c> group.
        /// </summary>
        public bool IsSudo { get; }

        /// <summary>
        /// Gets information about the real user.
        /// </summary>
        public UnixUserInfo Info { get; }
    }
}
