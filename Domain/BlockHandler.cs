using System;
using System.Collections;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Domain
{
    public class BlockHandler : MonoBehaviour
    {
        public GameObject parent;

        private BlockCreator _blockCreator;

        private void Start()
        {
            _blockCreator = FindObjectOfType<BlockCreator>();
        }

        private Vector3 TranslateTablePositionToVector3(int x, int y)
        {
            return new Vector3(100 + x * 100, 200 + y * 100, 0);
        }

        public BlockData CreateBlock(int number, int x, int y)
        {
            var block = parent.AddComponent<BlockData>();
            block.block = _blockCreator.CreateBlock(number);
            block.number = number;
            block.value = (int) Math.Pow(2, number);
            block.isClicked = false;
            SetPosition(block, x, y);
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
            StartCoroutine(CoDelete(x.block, y.block));
        }

        private IEnumerator CoDelete(GameObject x, GameObject y)
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
            StartCoroutine(CoChangeNumber(block, number));
        }

        private IEnumerator CoChangeNumber(BlockData block, int number)
        {
            var x = block.block;
            var y = _blockCreator.CreateBlock(number);
            block.block = y;

            y.transform.position = x.transform.position;
            y.transform.DORotate(new Vector3(0, 180, -135), 0f);
            
            x.transform.DORotate(new Vector3(0, -90, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 90, -135), 0.4f);
            yield return new WaitForSeconds(0.3f);

            x.transform.DORotate(new Vector3(0, 180, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 0, -135), 0.4f);
            
            Destroy(x, 0.4f);
        }

        public void RecreateBlock(BlockData block, int number, int x, int  y) 
        {
            block.number = number;
            block.value = (int) Math.Pow(2, number);
            block.isClicked = false;
            StartCoroutine(CoRecreateBlock(block, number, x, y));
        }

        private IEnumerator CoRecreateBlock(BlockData block, int number, int x, int y)
        {
            block.block = _blockCreator.CreateBlock(number);
            block.block.transform.DOScale(new Vector3(1, 1, 1), 0f);
            yield return new WaitForSeconds(0.5f);
            SetPosition(block, x, y);
            block.block.transform.DOScale(new Vector3(80, 80, 80), 0.5f);
        }
    }
}
