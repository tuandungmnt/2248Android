using System;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

namespace Domain
{
    public class FacebookManager : MonoBehaviour
    {
        public static string userId;
        public static string userName;
        
        
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
            FB.LogInWithReadPermissions(permissions);
        }

        public void LogOut()
        {
            FB.LogOut();
        }

        public void Share()
        {
            FB.ShareLink(new Uri("https://www.youtube.com/watch?v=-op5cj6uknE"), 
                "Facebook Share",
                "Share Success", 
                new Uri("https://candyrat.azureedge.net/ImageGen.ashx?image=/media/10501/Don-Ross.jpg&width=468&height=468"));
        }
        
        public bool IsLoggedIn()
        {
            return FB.IsLoggedIn;
        }

        public void UpdateAccountInfo()
        {
            if (!FB.IsLoggedIn) return;
            var aToken = AccessToken.CurrentAccessToken;
            userId = aToken.UserId;
            FB.API("/me?fields=first_name", HttpMethod.GET, CallBack);
        }

        private void CallBack(IGraphResult result)
        {
            userName = result.ResultDictionary["name"].ToString();
            Debug.Log("fbName: " + userName);
        }
    }
}
