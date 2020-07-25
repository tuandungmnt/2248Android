using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Presentation
{
    public class GameUiChanger : MonoBehaviour
    {
        public void SetPosition(RectTransform rectTransform, Vector2 x)
        {
            rectTransform.anchoredPosition = x;
        }

        public void SetPosition(GameObject gameObject, Vector2 x)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = x;
        }

        public void SetText(Text text, string t)
        {
            text.text = t;
        }

        public void SetText(GameObject gameObject, string t)
        {
            gameObject.GetComponent<Text>().text = t;
        }

        public void SetColor(Image image, Color color)
        {
            image.color = color;
        }

        public void ChangePosition(RectTransform rectTransform, Vector2 x, float time)
        {
            rectTransform.DOAnchorPos(x, time);
        }
        
        public void ChangePosition(GameObject gameObject, Vector2 x, float time)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(x, time);
        }

        public void ShakePosition(GameObject gameObject, float time)
        {
            gameObject.transform.DOShakePosition(time, new Vector3(6f, 3f, 3f), 5, 20);
        }

        public IEnumerator CoChangeNumber(Text text, int oldNum, int newNum) {
            if (newNum - oldNum < 10) {
                for (var i = oldNum + 1; i <= newNum; ++i) { 
                    text.text = i.ToString();
                    yield return new WaitForSeconds(0.07f);
                }
            }
            else {
                var xxx = (float) (newNum - oldNum) / 10;
                float yyy = oldNum;
                for (var i = 0; i < 10; ++i) {
                    yyy += xxx;
                    var zzz = (int) yyy;
                    text.text = zzz.ToString();
                    yield return new WaitForSeconds(0.07f);
                }
                text.text = newNum.ToString();
            }
        }
    }
}
