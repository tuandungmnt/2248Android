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
                    ScoreData.bestScore = int.Parse(child.Value.ToString());
                    return;
                }

                var childUpdates = new Dictionary<string, object> {[FacebookManager.userId] = ScoreData.bestScore};
                _reference.Child("users").UpdateChildrenAsync(childUpdates);
                Debug.Log("Not yet");
            });
            Debug.Log("End signin " + ScoreData.bestScore);
        }

        public async void SaveScore()
        {
            var childUpdates = new Dictionary<string, object> {[FacebookManager.userId] = ScoreData.bestScore};
            await _reference.Child("users").UpdateChildrenAsync(childUpdates);
        }

        public async Task UpdateScoreBoard()
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;
                var childNum = (int) snapshot.ChildrenCount;
                var counter = childNum;

                foreach (var child in snapshot.Children.OrderBy(ch => ch.Value))
                {
                    Debug.Log("scoreboard: " + child.Key + " " + child.Value);
                    if (child.Key.Equals(FacebookManager.userId))
                    {
                        ScoreData.yourPosition = counter;
                        Debug.Log("write counter: " + counter);
                    }
                    
                    counter--;
                    if (counter >= 5) continue;
                    ScoreData.scoreBoardName[counter] = child.Key;
                    ScoreData.scoreBoardScore[counter] = child.Value.ToString();
                }
            });
        }
    }
}
