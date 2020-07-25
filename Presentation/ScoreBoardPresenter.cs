using Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class ScoreBoardPresenter : MonoBehaviour
    {
        public GameObject scoreBoardPanel;
        public Text yourPositionText;
        private static GameObject[] _nameText;
        private static GameObject[] _scoreText;

        private GameUiChanger _gameUiChanger;

        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _nameText = new GameObject[5];
            _scoreText = new GameObject[5];
        
            for (var i = 0; i < 5; ++i)
            {
                _nameText[i] = FindObjectOfType<GameUiCreator>().CreateNameText(scoreBoardPanel);
                _gameUiChanger.SetPosition(_nameText[i], new Vector2(0, 260 - 130 * i));
            
                _scoreText[i] = FindObjectOfType<GameUiCreator>().CreateScoreText(scoreBoardPanel);
                _gameUiChanger.SetPosition(_scoreText[i], new Vector2(0, 260 - 130 * i));
            }
        }

        public void UpdateScoreBoard(int n, string username, string score)
        {
            _gameUiChanger.SetText(_nameText[n], username);
            _gameUiChanger.SetText(_scoreText[n], score);
        }

        public void UpdateScoreBoard(string n)
        {
            yourPositionText.text = "Your position: " + n;
        }

        public void ShowScoreBoard()
        {
            _gameUiChanger.ChangePosition(scoreBoardPanel, new Vector2(0,0), 1f);
        }

        public void HideScoreBoard()
        {
            _gameUiChanger.ChangePosition(scoreBoardPanel, new Vector2(720,0), 1f);
        }
    }
}
