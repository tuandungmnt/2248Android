using System.Collections;
using Presentation;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class MenuScene : MonoBehaviour
    {
        public Text welcomeText;
        public Text userNameText;
        public Text inputText;
        public Text logInText;
        public Button playButton;
        public Button logInButton;

        private FacebookManager _facebookManager;

        private bool _oldStatus;
        private bool _newStatus;
        private int _counter;
        
        private void Start()
        {
            _facebookManager = FindObjectOfType<FacebookManager>();
            AddButtonListener();
        }

        private void AddButtonListener()
        {
            playButton.onClick.AddListener(() => {
                FindObjectOfType<AudioManager>().Play("Click");
                FindObjectOfType<FirebaseManager>().GetHighScore();
                StartCoroutine(ChangeScene());
            });
            
            logInButton.onClick.AddListener(() =>
            {
                FindObjectOfType<AudioManager>().Play("Click");
                if (_facebookManager.IsLoggedIn()) _facebookManager.LogOut();
                    else _facebookManager.LogIn();
            });
        }

        private IEnumerator ChangeScene() 
        {
            FindObjectOfType<GameUiChanger>().ChangePosition(playButton.GetComponent<RectTransform>(), new Vector2(1000, 38), 0.7f);
            FindObjectOfType<GameUiChanger>().ChangePosition(welcomeText.GetComponent<RectTransform>(), new Vector2(-1000, 65), 0.7f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

        private void Update()
        {
            _newStatus = _facebookManager.IsLoggedIn();
            _counter++;
            if (_counter % 30 == 0) Debug.Log("Menu scene update: " + _newStatus);
            if (_newStatus == _oldStatus) return;
            
            if (_newStatus)
            {    
                _facebookManager.UpdateAccountInfo();
                logInText.text = "Log Out";
                userNameText.text = FacebookManager.userName;
                FindObjectOfType<GameUiChanger>().ChangePosition(playButton.GetComponent<RectTransform>(),
                    new Vector2(0, 100), 0.4f);
            }
            else
            {
                logInText.text = "Log In";
                userNameText.text = "";
                FindObjectOfType<GameUiChanger>().ChangePosition(playButton.GetComponent<RectTransform>(),
                    new Vector2(0, -200), 0.4f);
            }
            _oldStatus = _newStatus;
        }
    }
}
