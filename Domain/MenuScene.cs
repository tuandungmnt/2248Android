using System.Collections;
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
        public Text inputText;
        public Text logInText;
        public Button playButton;
        public Button logInButton;

        private FacebookManager _facebookManager;
        private GameUiChanger _gameUiChanger;
        private AudioManager _audioManager;

        private bool _oldStatus;
        private bool _newStatus;
        private int _counter;
        
        private void Start()
        {
            _facebookManager = FindObjectOfType<FacebookManager>();
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _audioManager = FindObjectOfType<AudioManager>();
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
            
            logInButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                if (_facebookManager.IsLoggedIn()) _facebookManager.LogOut();
                    else _facebookManager.LogIn();
            });
        }

        private IEnumerator ChangeScene() 
        {
            _gameUiChanger.ChangePosition(playButton.GetComponent<RectTransform>(), new Vector2(1000, 38), 0.7f);
            _gameUiChanger.ChangePosition(welcomeText.GetComponent<RectTransform>(), new Vector2(-1000, 65), 0.7f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

        private IEnumerator UpdateScene()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _newStatus = _facebookManager.IsLoggedIn();
                if (_counter % 500 == 0) Debug.Log("Menu scene update: " + _newStatus);
                _counter++;
                if (_newStatus == _oldStatus) continue;

                if (_newStatus)
                {
                    _facebookManager.UpdateAccountInfo();
                    yield return new WaitForSeconds(2f);
                    logInText.text = "Log Out";
                    userNameText.text = FacebookManager.userName;
                    Debug.Log("In menu, username: " + FacebookManager.userName);
                    _gameUiChanger.ChangePosition(playButton.GetComponent<RectTransform>(),
                        new Vector2(0, 100), 0.4f);
                }
                else
                {
                    logInText.text = "Log In";
                    userNameText.text = "";
                    _gameUiChanger.ChangePosition(playButton.GetComponent<RectTransform>(),
                        new Vector2(0, -200), 0.4f);
                }

                _oldStatus = _newStatus;
            }
        }
    }
}
