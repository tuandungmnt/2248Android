using System.Collections;
using Data;
using Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class MenuScene : MonoBehaviour
    {
        public Text welcomeText;
        public Text userNameText;
        public Text logInText;
        public Button playButton;
        public Button logOutButton;
        public Button facebookLogInButton;
        public Button googleLogInButton;

        private GameUiChanger _gameUiChanger;
        private AudioManager _audioManager;
        private PlatformManager _platformManager;

        private bool _oldStatus;
        private bool _newStatus;
        private int _counter;
        
        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _audioManager = FindObjectOfType<AudioManager>();
            _platformManager = FindObjectOfType<PlatformManager>();
            
            AddButtonListener();
            StartCoroutine(UpdateScene());
        }

        private void AddButtonListener()
        {
            playButton.onClick.AddListener(() => {
                _audioManager.Play("Click");
                FindObjectOfType<FirebaseManager>().GetHighScore();
                StartCoroutine(ChangeScene());
            });
            
            logOutButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                _platformManager.LogOut();
            });
            
            facebookLogInButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                _platformManager.LogIn("facebook");
            });
            
            googleLogInButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                _platformManager.LogIn("google");
            });
        }

        private IEnumerator ChangeScene() 
        {
            _gameUiChanger.ChangePosition(playButton, new Vector2(1000, 38), 0.7f);
            _gameUiChanger.ChangePosition(welcomeText, new Vector2(-1000, 65), 0.7f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

        private IEnumerator UpdateScene()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _newStatus = _platformManager.IsLoggedIn();
                if (_counter % 500 == 0) Debug.Log("Menu scene update: " + _newStatus);
                _counter++;
                if (_newStatus == _oldStatus) continue;

                if (_newStatus) 
                {
                    _platformManager.UpdateUserData();
                    yield return new WaitForSeconds(1.4f);
                    
                    userNameText.text = UserData.userName;
                    _gameUiChanger.ChangePosition(facebookLogInButton, new Vector2(-500, 250), 0.4f);
                    _gameUiChanger.ChangePosition(googleLogInButton, new Vector2(500, 250), 0.4f);

                    
                    _gameUiChanger.ChangePosition(playButton, new Vector2(0, 100), 0.4f);
                    _gameUiChanger.ChangePosition(logOutButton, new Vector2(0, 300), 0.4f);
                }
                else
                {
                    userNameText.text = "";
                    _gameUiChanger.ChangePosition(facebookLogInButton, new Vector2(-120, 250), 0.4f);
                    _gameUiChanger.ChangePosition(googleLogInButton, new Vector2(120, 250), 0.4f);
                    
                    _gameUiChanger.ChangePosition(playButton, new Vector2(0, -200), 0.4f);
                    _gameUiChanger.ChangePosition(logOutButton, new Vector2(0, -200), 0.4f);
                }

                _oldStatus = _newStatus;
            }
        }
    }
}
