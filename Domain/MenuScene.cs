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
        public static bool FinishUpdate;
        public static bool FinishLoadHighScore;
        
        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _audioManager = FindObjectOfType<AudioManager>();
            _platformManager = FindObjectOfType<PlatformManager>();
            
            AddButtonListener();
            StartCoroutine(UpdateScene());
            FindObjectOfType<PopUpController>().CreatePopUp("Hello");
        }

        private void AddButtonListener()
        {
            playButton.onClick.AddListener(() => {
                _audioManager.Play("Click");
                FinishLoadHighScore = false;
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
            yield return new WaitForSeconds(0.7f);
            while (!FinishLoadHighScore) yield return new WaitForSeconds(0.1f);
                SceneManager.LoadScene(1);
        }

        private IEnumerator UpdateScene()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _newStatus = _platformManager.IsLoggedIn();
                //if (_counter % 200 == 0) Debug.Log("Menu scene update: " + _newStatus);
                //_counter++;
                if (_newStatus == _oldStatus) continue;

                if (_newStatus) 
                {    
                    FinishUpdate = false;
                    _platformManager.UpdateUserData();
                    while(!FinishUpdate) yield return  new WaitForSeconds(0.1f);
                    
                    userNameText.text = UserData.userName;
                    _gameUiChanger.ChangePosition(facebookLogInButton, new Vector2(-1000, 300), 0.4f);
                    _gameUiChanger.ChangePosition(googleLogInButton, new Vector2(1000, 200), 0.4f);

                    
                    _gameUiChanger.ChangePosition(playButton, new Vector2(0, 100), 0.4f);
                    _gameUiChanger.ChangePosition(logOutButton, new Vector2(0, 300), 0.4f);
                }
                else
                {
                    userNameText.text = "";
                    _gameUiChanger.ChangePosition(facebookLogInButton, new Vector2(0, 300), 0.4f);
                    _gameUiChanger.ChangePosition(googleLogInButton, new Vector2(0, 200), 0.4f);
                    
                    _gameUiChanger.ChangePosition(playButton, new Vector2(0, -200), 0.4f);
                    _gameUiChanger.ChangePosition(logOutButton, new Vector2(0, -200), 0.4f);
                }

                _oldStatus = _newStatus;
            }
        }
    }
}
