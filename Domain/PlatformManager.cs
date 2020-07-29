using Data;
using UnityEngine;

namespace Domain
{
    public class PlatformManager : MonoBehaviour
    {
        private FacebookManager _facebookManager;
        private FirebaseManager _firebaseManager;

        private void Start()
        {
            _facebookManager = FindObjectOfType<FacebookManager>();
            _firebaseManager = FindObjectOfType<FirebaseManager>();
        }

        public void LogIn(string platformName)
        {
            if (platformName.Equals("facebook")) _facebookManager.LogIn();
            if (platformName.Equals("google")) _firebaseManager.LogIn();
        }
        public void LogOut()
        {
            if (_facebookManager.IsLoggedIn()) _facebookManager.LogOut();
            if (_firebaseManager.IsLoggedIn()) _firebaseManager.LogOut();
        }

        public bool IsLoggedIn()
        {
            if (_facebookManager.IsLoggedIn())
            {
                UserData.platform = "facebook";
                return true;
            }
            if (_firebaseManager.IsLoggedIn())
            {
                UserData.platform = "google";
                return true;
            }
            return false;
        }

        public void UpdateUserData()
        {
            if (UserData.platform.Equals("facebook")) _facebookManager.UpdateUserData();
            if (UserData.platform.Equals("google")) _firebaseManager.UpdateUserData();
        } 
    }
}
