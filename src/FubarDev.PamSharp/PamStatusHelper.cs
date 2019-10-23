// <copyright file="PamStatusHelper.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    internal static class PamStatusHelper
    {
        /// <summary>
        /// Transforms status to its description from PAM header (_pam_types.h).
        /// </summary>
        /// <param name="status">Status to convert.</param>
        /// <param name="invokedMethod">Name of invoked PAM action.</param>
        /// <returns>Formatted message with info about status.</returns>
        public static string TransformStatusToString(PamStatus status, string invokedMethod)
        {
            return status switch
            {
                PamStatus.PAM_SUCCESS => $"{status.ToString()}. Successful function return.",

                PamStatus.PAM_OPEN_ERR => $"{status.ToString()}. dlopen() failure when dynamically loading a service module.",

                PamStatus.PAM_SYMBOL_ERR => $"{status.ToString()}. Symbol not found.",

                PamStatus.PAM_SERVICE_ERR => $"{status.ToString()}. Error in service module.",

                PamStatus.PAM_SYSTEM_ERR => $"{status.ToString()}. System error.",

                PamStatus.PAM_BUF_ERR => $"{status.ToString()}. Memory buffer error.",

                PamStatus.PAM_PERM_DENIED => $"{status.ToString()}. Permission denied.",

                PamStatus.PAM_AUTH_ERR => $"{status.ToString()}. Authentication failure.",

                PamStatus.PAM_CRED_INSUFFICIENT => $"{status.ToString()}. Can not access authentication data due to insufficient credentials.",

                PamStatus.PAM_AUTHINFO_UNAVAIL => $"{status.ToString()}. Underlying authentication service can not retrieve authentication information.",

                PamStatus.PAM_USER_UNKNOWN => $"{status.ToString()}. User not known to the underlying authenticaiton module.",

                PamStatus.PAM_MAXTRIES => $"{status.ToString()}. An authentication service has maintained a retry count which has been reached.  No further retries should be attempted.",

                PamStatus.PAM_NEW_AUTHTOK_REQD => $"{status.ToString()}. New authentication token required. This is normally returned if the machine security policies require that the password should be changed beccause the password is NULL or it has aged.",

                PamStatus.PAM_ACCT_EXPIRED => $"{status.ToString()}. User account has expired.",

                PamStatus.PAM_SESSION_ERR => $"{status.ToString()}. Can not make/remove an entry for the specified session.",

                PamStatus.PAM_CRED_UNAVAIL => $"{status.ToString()}. Underlying authentication service can not retrieve user credentials unavailable.",

                PamStatus.PAM_CRED_EXPIRED => $"{status.ToString()}. User credentials expired",

                PamStatus.PAM_CRED_ERR => $"{status.ToString()}. Failure setting user credentials.",

                PamStatus.PAM_NO_MODULE_DATA => $"{status.ToString()}. No module specific data is present.",

                PamStatus.PAM_CONV_ERR => $"{status.ToString()}. Conversation error.",

                PamStatus.PAM_AUTHTOK_ERR => $"{status.ToString()}. Authentication token manipulation error.",

                PamStatus.PAM_AUTHTOK_RECOVERY_ERR => $"{status.ToString()}. Authentication information cannot be recovered.",

                PamStatus.PAM_AUTHTOK_LOCK_BUSY => $"{status.ToString()}. Authentication token lock busy.",

                PamStatus.PAM_AUTHTOK_DISABLE_AGING => $"{status.ToString()}. Authentication token aging disabled.",

                PamStatus.PAM_TRY_AGAIN => $"{status.ToString()}. Preliminary check by password service.",

                PamStatus.PAM_IGNORE => $"{status.ToString()}. Ignore underlying account module regardless of whether the control flag is required, optional, or sufficient.",

                PamStatus.PAM_ABORT => $"{status.ToString()}. Critical error (?module fail now request).",

                PamStatus.PAM_AUTHTOK_EXPIRED => $"{status.ToString()}. User's authentication token has expired.",

                PamStatus.PAM_MODULE_UNKNOWN => $"{status.ToString()}. Module is not known.",

                PamStatus.PAM_BAD_ITEM => $"{status.ToString()}. Bad item passed to pam_*_item().",

                PamStatus.PAM_CONV_AGAIN => $"{status.ToString()}. Conversation function is event driven and data is not available yet.",

                PamStatus.PAM_INCOMPLETE => $"{status.ToString()}. Please call {invokedMethod} function again to complete authentication stack. Before calling again, verify that conversation is completed.",

                _ => $"{status.ToString()}. Unknown PAM status code.",
            };
        }
    }
}
