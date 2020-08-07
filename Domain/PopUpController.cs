using System;
using System.Collections;
using Presentation;
using UnityEngine;
using UnityEngine.UI;


namespace Domain
{
    public class PopUpController : MonoBehaviour
    {
        private GameUiChanger _gameUiChanger;
        private GameObject _go;

        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
        }

        public void CreatePopUp(string message)
        {
            var canvas = FindObjectOfType<Canvas>();
            _go = FindObjectOfType<GameUiCreator>().CreatePopUp(canvas.transform);
            _gameUiChanger.SetPosition(_go, new Vector2(0, 150));
            var x = _go.transform.position;
            x.z = 1;
            _go.transform.position = x;
            _gameUiChanger.ChangePosition(_go, new Vector2(0, -25), 0.5f);

            _go.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Hay The");
                _gameUiChanger.ChangePosition(_go, new Vector2(0, -25), 0.5f);
                Destroy(_go, 0.6f);
                //StartCoroutine(SwipePopUp(x));
            });
            _go.GetComponentInChildren<Text>().text = message;
        }

        private IEnumerator SwipePopUp(GameObject go)
        {
            var goPosition = go.transform.position;
            var mousePosition = Input.mousePosition;
            var difference = 0f;
            
            while (GameInput.isPressed)
            {
                difference = Input.mousePosition.x - mousePosition.x;
                go.transform.position = goPosition + new Vector3(difference, 0, 0);
            }

            if (Math.Abs(difference) < GameInput.RealWidth * 0.3f)
            {
                _gameUiChanger.ChangePosition(go, new Vector2(0, -25), 0.2f);
                yield break;
            }

            if (difference < 0)
            {
                _gameUiChanger.ChangePosition(go, new Vector2(-720, -25), 0.2f);
                Destroy(go, 0.3f);
            }
            else
            {
                _gameUiChanger.ChangePosition(go, new Vector2(720, -25), 0.2f);
                Destroy(go, 0.3f);
            }
        }
    }

}
