using Data;
using Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class EndScene : MonoBehaviour
    {
        public Button replayButton;
        public Button scoreBoardButton;
        public Button closeScoreBoardButton;
        public Button shareButton;
        public Button facebookButton;
        public Text scoreTagText;
        public Text scoreText;

        private ScoreBoardPresenter _scoreBoardPresenter;
        private AudioManager _audioManager;

        private void Start()
        {
            _scoreBoardPresenter = FindObjectOfType<ScoreBoardPresenter>();
            _audioManager = FindObjectOfType<AudioManager>();
            
            AddButtonListener();
            UpdateScene();
        
            FindObjectOfType<AdsManager>().PlayInitializeAds();
            _audioManager.Play("End");
        }

        private void AddButtonListener()
        {
            replayButton.onClick.AddListener(() => 
            {
                _audioManager.Play("Click");
                SceneManager.LoadScene(1);
            });  
        
            scoreBoardButton.onClick.AddListener(async () =>
            {
                _audioManager.Play("Click");
                await FindObjectOfType<FirebaseManager>().UpdateScoreBoard();
                Debug.Log("hello");
                _scoreBoardPresenter.UpdateScoreBoard(ScoreData.yourPosition.ToString());
                for (var i = 0; i < 5; ++i)
                    _scoreBoardPresenter.UpdateScoreBoard(i, ScoreData.scoreBoardName[i], ScoreData.scoreBoardScore[i]);
                FindObjectOfType<ScoreBoardPresenter>().ShowScoreBoard();
            });
        
            closeScoreBoardButton.onClick.AddListener(() =>
            {    
                _audioManager.Play("Click");
                FindObjectOfType<ScoreBoardPresenter>().HideScoreBoard();
            });
            
            shareButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                FindObjectOfType<ShareManager>().Share();
            });
            
            facebookButton.onClick.AddListener(() =>
            {
                _audioManager.Play("Click");
                FindObjectOfType<FacebookManager>().Share();
            });
        }

        private void UpdateScene()
        {
            scoreText.text = ScoreData.currentScore.ToString();

            if (ScoreData.currentScore <= ScoreData.bestScore) return;
            ScoreData.bestScore = ScoreData.currentScore;
            scoreTagText.text = "New Best Score!!";
            FindObjectOfType<FirebaseManager>().SaveScore();
        }
    }
}
