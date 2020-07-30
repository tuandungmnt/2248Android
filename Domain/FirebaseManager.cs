using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using Google;

namespace Domain
{
    public class FirebaseManager : MonoBehaviour
    {
        private DatabaseReference _reference;
        private FirebaseAuth _auth;
        private const string WebClientId = "653330373773-7t5sf0ib9neb5n7pup1mm75pc8s6amj7.apps.googleusercontent.com";

        private async void Start()
        {
            // Initialization Firebase;
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hwaiting-df83d.firebaseio.com/");
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
            _auth = FirebaseAuth.DefaultInstance;
           
        }

        public void LogIn()
        {
            Debug.Log("Firebase Login");

            GoogleSignIn.Configuration = new GoogleSignInConfiguration 
            {
                WebClientId = WebClientId, 
                RequestIdToken = true, 
                UseGameSignIn = false, 
            };

            var signIn = GoogleSignIn.DefaultInstance.SignIn ();

            var signInCompleted = new TaskCompletionSource<FirebaseUser> ();
            signIn.ContinueWith (task => {
                if (task.IsCanceled) {
                    signInCompleted.SetCanceled ();
                } else if (task.IsFaulted) {
                    signInCompleted.SetException (task.Exception);
                } else {

                    var credential = GoogleAuthProvider.GetCredential ((task).Result.IdToken, null);
                    _auth.SignInWithCredentialAsync (credential).ContinueWith (authTask => {
                        if (authTask.IsCanceled) {
                            signInCompleted.SetCanceled();
                        } else if (authTask.IsFaulted) {
                            signInCompleted.SetException(authTask.Exception);
                        } else {
                            signInCompleted.SetResult((authTask).Result);
                        }
                    });
                }
            });
        }

        public void LogOut()
        {
            Debug.Log("Firebase Logout");
            var user = _auth.CurrentUser;
            if (user == null) return;
            _auth.SignOut();
            GoogleSignIn.DefaultInstance.SignOut();
        }

        public bool IsLoggedIn()
        {
            var user = _auth.CurrentUser;
            return user != null;
        }

        public void UpdateUserData()
        {
            Debug.Log("Firebase Update");
            var user = _auth.CurrentUser;
            if (user == null) return;
            UserData.userId = user.UserId;
            UserData.userName = user.DisplayName;
            MenuScene.finishUpdate = true;
        }

        public async void GetHighScore()
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    if (!child.Key.Equals(UserData.userId)) continue;
                    Debug.Log("Yet");
                    foreach (var kid in child.Children)
                    {
                        if (kid.Key.Equals("score")) ScoreData.bestScore = int.Parse(kid.Value.ToString());
                    }
                    return;
                }

                var userId = new Dictionary<string, object> {[UserData.userId] = "0"};
                var bestScore = new Dictionary<string, object> {["score"] = ScoreData.bestScore};
                var userName = new Dictionary<string, object> {["name"] = UserData.userName};
                _reference.Child("users").UpdateChildrenAsync(userId);
                _reference.Child("users").Child(UserData.userId).UpdateChildrenAsync(userName);
                _reference.Child("users").Child(UserData.userId).UpdateChildrenAsync(bestScore);
                Debug.Log("Not yet");
            });
            Debug.Log("End signin " + ScoreData.bestScore);
            MenuScene.finishLoadHighScore = true;
        }

        public async void SaveScore()
        {
            await _reference.Child("users").Child(UserData.userId).Child("score").SetValueAsync(ScoreData.bestScore);
        }

        public async Task UpdateScoreBoard()
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;
                var childNum = (int) snapshot.ChildrenCount;
                var counter = childNum;

                foreach (var child in snapshot.Children.OrderBy(ch => ch.Child("score").Value))
                {
                    Debug.Log("scoreboard: " + child.Key + " " + child.Value);
                    if (child.Key.Equals(UserData.userId))
                    {
                        ScoreData.yourPosition = counter;
                        Debug.Log("write counter: " + counter);
                    }
                    
                    counter--;
                    if (counter >= 5) continue;
                    foreach (var n in child.Children)
                    {
                        if (n.Key.Equals("name")) ScoreData.scoreBoardName[counter] = n.Value.ToString();
                        if (n.Key.Equals("score")) ScoreData.scoreBoardScore[counter] = n.Value.ToString();
                    }
                }
            });
        }
    }
}
