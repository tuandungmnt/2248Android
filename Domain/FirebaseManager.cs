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

namespace Domain
{
    public class FirebaseManager : MonoBehaviour
    {
        private DatabaseReference _reference;
        private FirebaseAuth _auth;

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
 
        }

        public void LogOut()
        {
            Debug.Log("Firebase Logout");
        }

        public bool IsLoggedIn()
        {
            Debug.Log("Firebase Is");
            return false;
        }

        public void UpdateUserData()
        {
            Debug.Log("Firebase Update");
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
        }

        public async void SaveScore()
        {
            //var childUpdates = new Dictionary<string, object> {[FacebookManager.userId] = ScoreData.bestScore};
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
