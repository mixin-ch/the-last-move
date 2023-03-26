#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using Mixin.Utils;

namespace Mixin.TheLastMove.Social
{
    public class GooglePlayGamesManager : MonoBehaviour
    {
        public void Start()
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            $"Google Play Games Status: {status}".Log();

            if (status == SignInStatus.Success)
            {
                // Continue with Play Games Services
            }
            else
            {
                // Disable your integration with Play Games Services or show a login button
                // to ask users to sign-in. Clicking it should call
                // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            }
        }
    }
}
#endif
