using System;
using System.Collections.Generic;
using Data;
using Facebook.Unity;
using UnityEngine;

namespace Domain
{
    public class FacebookManager : MonoBehaviour
    {
        private void Start()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(() =>
                {
                    if (FB.IsInitialized) 
                        FB.ActivateApp();
                    else 
                        Debug.Log("FB initialize error");
                }, 
                    isGameShown => { Time.timeScale = !isGameShown ? 0 : 1; });
            } 
            else FB.ActivateApp();
        }

        public void LogIn()
        {
            var permissions = new List<string>() {"public_profile"};
            FB.LogInWithPublishPermissions(permissions, AuthCallBack);
        }

        private void AuthCallBack(ILoginResult result)
        {
            if (FB.IsLoggedIn) {
                var aToken = AccessToken.CurrentAccessToken;
                Debug.Log(aToken.UserId);
                foreach (var perm in aToken.Permissions) {
                    Debug.Log("permissions: " + perm);
                }
            } else {
                Debug.Log("User cancelled login");
            }
        }

        public void LogOut()
        {
            FB.LogOut();
        }

        public void Share()
        {
            if (!FB.IsLoggedIn) return;
            FB.ShareLink(new Uri("https://www.youtube.com/watch?v=-op5cj6uknE"), 
                "Facebook Share",
                "Share Success", 
                new Uri("https://candyrat.azureedge.net/ImageGen.ashx?image=/media/10501/Don-Ross.jpg&width=468&height=468"));
        }

        public bool IsLoggedIn()
        {
            return FB.IsLoggedIn;
        }

        public void UpdateUserData()
        {
            if (!FB.IsLoggedIn) return;
            Debug.Log("In update account info");
            var aToken = AccessToken.CurrentAccessToken;
            UserData.userId = aToken.UserId;
            Debug.Log("userID: " + UserData.userId);
            FB.API("/me?fields=name", HttpMethod.GET, CallBack);
        }

        private void CallBack(IGraphResult result)
        {
            Debug.Log("Update account info call back");
            UserData.userName = result.ResultDictionary["name"].ToString();
            Debug.Log("fbName: " + UserData.userName);
        }
    }
}
