using Data;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Domain
{
    public class BlockHandler : MonoBehaviour
    {
        private readonly Color _clickedColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        private readonly Color _notClickedColor = new Color(0.9f, 0.9f, 0.9f, 0.8f);
        private readonly GameObject[] _line = new GameObject[70];
        private GameObject _parent;

        private GameUiChanger _gameUiChanger;
        private GameUiCreator _gameUiCreator;

        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _gameUiCreator = FindObjectOfType<GameUiCreator>();
        }

        public void Initialize(GameObject parent)
        {
            _parent = parent;
        }
 
        private Vector2 TranslateTablePositionToVector2(int x, int y)
        {
            return new Vector2(-250 + x * 125, 85 + y * 125);
        }

        public BlockData CreateBlock()
        {
            var block = _parent.AddComponent<BlockData>();
            block.block = _gameUiCreator.CreateBlock(_parent);
            block.text = block.block.transform.Find("Text").gameObject.GetComponent<Text>();
            block.rectTransform = block.block.GetComponent<RectTransform>();
            block.image = block.block.GetComponent<Image>();
            
            _gameUiChanger.SetColor(block.image, _notClickedColor);
            return block;
        }

        public void SetNumber(BlockData block, int number)
        {
            block.number = number;
            _gameUiChanger.SetText(block.text, number.ToString());
        }

        public void SetPosition(BlockData block, int x, int y)
        {
            var z = TranslateTablePositionToVector2(x, y);
            _gameUiChanger.SetPosition(block.rectTransform, z);
        }

        public void Click(BlockData block)
        {
            if (block.isClicked) return;
            block.isClicked = true;
            _gameUiChanger.SetColor(block.image, _clickedColor);
        }

        public void UndoClick(BlockData block)
        {
            if (!block.isClicked) return;
            block.isClicked = false;
            _gameUiChanger.SetColor(block.image, _notClickedColor);
        }

        public void Match(int n, int block1, int block2, float time)
        {
            _line[n] = _gameUiCreator.CreateLine(_parent);
            
            var p1 = TranslateTablePositionToVector2(block1 / 7, block1 % 7);
            var p2 = TranslateTablePositionToVector2(block2 / 7, block2 % 7);
            var p3 = new Vector2 {x = (p1.x + p2.x) / 2, y = (p1.y + p2.y) / 2};
            var p4 = new Vector2 {x = (p3.x + p1.x) / 2, y = (p3.y + p1.y) / 2};

            _gameUiChanger.SetPosition(_line[n], p4);
            _gameUiChanger.ChangePosition(_line[n], p3, time);
        }
        
        public void UndoMatch(int n, int block1, int block2, float time)
        {
            var p1 = TranslateTablePositionToVector2(block1 / 7, block1 % 7);
            var p2 = TranslateTablePositionToVector2(block2 / 7, block2 % 7);
            var p3 = new Vector2 {x = (3 * p1.x + p2.x) / 4, y = (3 * p1.y + p2.y) / 4};

            _gameUiChanger.ChangePosition(_line[n], p3, time);
            Destroy(_line[n], 0.1f);
        }

        public void DestroyLine(int n)
        {
            Destroy(_line[n]);
        }
        public void Move(BlockData block, int x, int y, float time)
        {
            var z = TranslateTablePositionToVector2(x, y);
            _gameUiChanger.ChangePosition(block.rectTransform, z, time);
        }

        public void ChangeNumber(BlockData block, int newNum)
        {
            StartCoroutine(_gameUiChanger.CoChangeNumber(block.text, block.number, newNum));
            block.number = newNum;
        }

        public void ShakePosition(BlockData block, float time)
        {
            _gameUiChanger.ShakePosition(block.block, time);
        }
    }
}
