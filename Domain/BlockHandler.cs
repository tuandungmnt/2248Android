using System;
using System.Collections;
using Data;
using DG.Tweening;
using Presentation;
using UnityEngine;

namespace Domain
{
    public class BlockHandler : MonoBehaviour
    {
        public GameObject parent;

        private GameUiChanger _gameUiChanger;
        private BlockCreator _blockCreator;
        private int _counter;

        private void Start()
        {
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            _blockCreator = FindObjectOfType<BlockCreator>();
        }

        private Vector3 TranslateTablePositionToVector3(int x, int y)
        {
            return new Vector3(100 + x * 100, 400 + y * 100, 0);
        }

        public BlockData CreateBlock(int x)
        {
            var block = parent.AddComponent<BlockData>();
            block.block = _blockCreator.CreateBlock(x);
            block.block.name = _counter.ToString();
            _counter++;
            block.number = x;
            block.value = (int) Math.Pow(2, x);
            block.isClicked = false;
            return block;
        }

        public GameObject CreateSmallBlock()
        {
            return _blockCreator.CreateSmallBlock();
        }

        public void SetPosition(BlockData block, int x, int y)
        {
            var z = TranslateTablePositionToVector3(x, y);
            block.block.transform.position = z;
        }

        public void SetPosition(GameObject go, int x, int y)
        {
            var z = TranslateTablePositionToVector3(x, y);
            go.transform.position = z;
        }

        public void MovePosition(BlockData block, int x, int y, float time)
        {
            var z = TranslateTablePositionToVector3(x, y);
            block.block.transform.DOMove(z, time);
        }

        public void MovePosition(GameObject go, int x, int y, float time)
        {
            var z = TranslateTablePositionToVector3(x, y);
            go.transform.DOMove(z, time);
        }

        public void Rotate(BlockData block, Vector3 x, float time)
        {
            block.block.transform.DORotate(x, time);
        }

        public void ShakePosition(BlockData block, float time)
        {
            block.block.transform.DOShakePosition(time, new Vector3(6f, 3f, 3f), 5, 20);
        }

        public void Delete(BlockData x, BlockData y)
        {
            StartCoroutine(Delete1(x.block, y.block));
        }

        private IEnumerator Delete1(GameObject x, GameObject y)
        {
            var z = CreateSmallBlock();
            z.transform.position = x.transform.position;

            x.transform.DOScale(new Vector3(10, 10, 10), 0.5f);
            Destroy(x, 0.4f);

            z.transform.DOScale(new Vector3(30, 30, 30), 0.2f);
            yield return new WaitForSeconds(0.5f);
            z.transform.DOMove(y.transform.position, 0.4f);
            Destroy(z, 0.4f);
        }

        public void ChangeNumber(BlockData block, int number)
        {
            block.number = number;
            block.value = (int) Math.Pow(2, number);
            StartCoroutine(ChangeNumber1(block, number));
        }

        private IEnumerator ChangeNumber1(BlockData block, int number)
        {
            var x = block.block;
            var y = _blockCreator.CreateBlock(number);

            y.transform.position = x.transform.position;
            y.transform.DORotate(new Vector3(0, 180, -135), 0f);
            
            x.transform.DORotate(new Vector3(0, -90, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 90, -135), 0.4f);
            yield return new WaitForSeconds(0.3f);

            x.transform.DORotate(new Vector3(0, 180, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 0, -135), 0.4f);
            
            Destroy(x, 0.4f);
            block.block = y;
        }
    }
}
