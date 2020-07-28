using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

namespace Domain
{
    public class FirebaseManager : MonoBehaviour
    {
        private DatabaseReference _reference;

        private async void Start()
        {
            // Initialization Firebase;
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hwaiting-df83d.firebaseio.com/");
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async void GetHighScore()
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    if (!child.Key.Equals(FacebookManager.userId)) continue;
                    Debug.Log("Yet");
                    foreach (var kid in child.Children)
                    {
                        if (kid.Key.Equals("score")) ScoreData.bestScore = int.Parse(kid.Value.ToString());
                    }
                    return;
                }

                var userId = new Dictionary<string, object> {[FacebookManager.userId] = "0"};
                var bestScore = new Dictionary<string, object> {["score"] = ScoreData.bestScore};
                var userName = new Dictionary<string, object> {["name"] = FacebookManager.userName};
                _reference.Child("users").UpdateChildrenAsync(userId);
                _reference.Child("users").Child(FacebookManager.userId).UpdateChildrenAsync(userName);
                _reference.Child("users").Child(FacebookManager.userId).UpdateChildrenAsync(bestScore);
                Debug.Log("Not yet");
            });
            Debug.Log("End signin " + ScoreData.bestScore);
        }

        public async void SaveScore()
        {
            //var childUpdates = new Dictionary<string, object> {[FacebookManager.userId] = ScoreData.bestScore};
            await _reference.Child("users").Child(FacebookManager.userId).Child("score").SetValueAsync(ScoreData.bestScore);
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
                    if (child.Key.Equals(FacebookManager.userId))
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
