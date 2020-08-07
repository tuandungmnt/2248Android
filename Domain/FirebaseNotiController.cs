using System;
using Firebase.Messaging;
using UnityEngine;

namespace Domain
{
    public class FirebaseNotiController : MonoBehaviour
    {
        public void Initialize()
        {
	    Debug.Log("Init Firebase Noti");
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }

        private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
        {
            Debug.Log("Received Registration Token: " + token.Token);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log("Received Message From:" + e.Message.From);
            //FindObjectOfType<PopUpController>().CreatePopUp(e.Message.Data.Values.ToString());
        }
    }
}
