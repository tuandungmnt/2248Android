using System;
using System.Collections;
using Data;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Domain
{
    public class GameController : MonoBehaviour
    {
        public BlockData[] block;
        public Text scoreText;
        public LineRenderer line;
        public LineRenderer line2;
        
        private readonly System.Random _rand = new System.Random();
        private int _stackN;
        private int[] _stack;
        private BlockData _tmp;
        private int _currentBlock;

        private AudioManager _audioManager;
        private BlockHandler _blockHandler;
        private GameUiChanger _gameUiChanger;

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _blockHandler = FindObjectOfType<BlockHandler>();
            _gameUiChanger = FindObjectOfType<GameUiChanger>();
            
            block = new BlockData[70];
            _stack = new int[70];
            _stackN = 0;
            ScoreData.currentScore = 0;

            for (var i = 0; i < 7; ++i)
            for (var j = 0; j < 9; ++j)
            {
                if ((i + j) % 2 != 0) continue;
                var x = i * 9 + j;
                block[x] = _blockHandler.CreateBlock(Rand(), i,  j + 4);
                _blockHandler.MovePosition(block[x], i, j, 0.5f);
            }

            StartCoroutine(Play());
        }

        private void Update()
        {
            if (_stackN == 0) line2.positionCount = 0;
            else
            {
                var x = block[_stack[_stackN - 1]];
                var y = x.block.transform.position;
                var z = GameInput.MouseWorldPosition();
                line2.positionCount = 2;
                line2.SetPosition(0, y);
                line2.SetPosition(1, z);

                var a = (z.x - y.x) / 7f; 
                var b = (z.y - y.y) / 7f;
            
                _blockHandler.Rotate(x,new Vector3(b, -a, -135), 0.8f);
            }
        }

        private IEnumerator Play()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (GameInput.isPressed)
                {
                    yield return FindMouseOnBlock();
                    Debug.Log("Block: " + _currentBlock);
                    if (_currentBlock == -1) continue;

                    if (RemoveBlockFromList()) continue;
                    if (block[_currentBlock].isClicked) continue;
                    AddBlockToList();
                }
                else
                {
                    if (_stackN == 1) block[_stack[0]].isClicked = false;
                    if (_stackN > 1) yield return SolveList();
                    
                    _stackN = 0;
                    line.positionCount = 0;
                    if (!CheckEndGame()) continue;

                    yield return new WaitForSeconds(1.5f);

                    for (var i = 0; i < 35; ++i)
                        _blockHandler.ShakePosition(block[i], 1.5f);
                    yield return new WaitForSeconds(2f);

                    FindObjectOfType<GameScene>().ChangeScene();
                    break;
                }
            }
        }

        private IEnumerator FindMouseOnBlock()
        {
            _currentBlock = -1;
            var n = GameInput.MouseOnBlockName();
            if (n == null) yield break;

            for (var i = 0; i < 7; ++i)
            for (var j = 0; j < 9; ++j)
            {
                if ((i + j) % 2 != 0) continue;
                var x = i * 9 + j;
                if (block[x].block.name == n) _currentBlock = x;
            }
            yield return null;
        }

        private bool RemoveBlockFromList()
        {
            if (_stackN <= 1 || _stack[_stackN - 2] != _currentBlock) return false;
            _audioManager.Play("Turn");
            block[_stack[_stackN - 1]].isClicked = false;
            line.positionCount--;
            _stackN--;
            return true;
        }

        private void AddBlockToList()
        {
            var check = false;
            if (_stackN == 0) check = true;
            else
            {
                var topNumber = block[_stack[_stackN - 1]].number;
                var currentNumber = block[_currentBlock].number;
                if (_stackN == 1 && topNumber == currentNumber) check = true;
                if (_stackN > 1 && (topNumber == currentNumber || topNumber + 1 == currentNumber)) check = true;
                if (!CheckAdjacent(_currentBlock, _stack[_stackN - 1])) check = false;
            }

            if (!check) return;
            _audioManager.Play("Turn");
            _stack[_stackN] = _currentBlock;
            _stackN++;
            block[_currentBlock].isClicked = true;
            line.positionCount++;
            line.SetPosition(_stackN - 1, block[_currentBlock].block.transform.position);
        }

        private IEnumerator SolveList()
        {
            _audioManager.Play("Delete");

            var sum = 0;
            for (var i = 0; i < _stackN; ++i)
                sum += block[_stack[i]].value;

            var s = 0;
            while ((int) Math.Pow(2, s) <= sum) s++;
            s--;
            
            var lastBlock = block[_stack[_stackN - 1]];
            line.positionCount = 0;
            for (var i = 0; i < _stackN - 1; ++i)
                _blockHandler.Delete(block[_stack[i]], lastBlock);
            _stackN = 0;
            
            yield return new WaitForSeconds(0.9f);
            _blockHandler.ChangeNumber(lastBlock, s);
            lastBlock.isClicked = false;
            AddScore(sum);
            
            yield return new WaitForSeconds(0.8f);

            for (var i = 0; i < 7; ++i)
            {
                var p = i % 2;
                for (var j = i % 2; j < 9; j += 2)
                {
                    var x = i * 9 + j;
                    var y = i * 9 + p;

                    if (!block[x].isClicked)
                    {
                        _tmp = block[x];
                        block[x] = block[y];
                        block[y] = _tmp;
                        _blockHandler.MovePosition(block[y], i, p, 0.5f);

                        p += 2;
                    } 
                }

                for (var j = p; j < 9; j += 2)
                {
                    var x = i * 9 + j;
                    _blockHandler.RecreateBlock(block[x], Rand(), i, j);
                }
            }


            /*for (var i = 0; i < 5; ++i)
            {
                var p = 0;
                var c = 0;

                for (var j = 0; j < 7; ++j)
                {
                    var x = i * 7 + j;
                    var y = i * 7 + p;

                    if (!block[x].isClicked) 
                    {
                        _tmp = block[x];
                        block[x] = block[y];
                        block[y] = _tmp;

                        _down[y] = c;
                        p++;
                        if (x == _stack[_stackN - 1]) k = y;
                    }
                    else
                    {
                        //_blockHandler.UndoClick(block[x]);
                        c++;
                    }
                }

                for (var j = p; j < 7; ++j)
                {
                    var x = i * 7 + j;
                    //_blockHandler.SetNumber(block[x], Rand());
                    _blockHandler.SetPosition(block[x], i, j + c);
                    _down[x] = c;
                }
            }

            //_blockHandler.ChangeNumber(block[k], s);
            for (var i = 0; i < 35; ++i)
                _blockHandler.MovePosition(block[i], i / 7, i % 7, 0.6f);
            yield return null;
            */
        }
        
        private bool CheckAdjacent(int x,int y) {
            return Math.Abs(x / 9 - y / 9) + Math.Abs(x % 9 - y % 9) == 2;
        }

        private void AddScore(int score)
        {
            StartCoroutine( _gameUiChanger.CoChangeNumber(scoreText, ScoreData.currentScore,
                ScoreData.currentScore + score));
            ScoreData.currentScore += score;
        }

        private bool CheckEndGame()
        {
            int[] s1 = {-1, 1, 1, -1, 0, 0, 2, -2};
            int[] s2 = {1, -1, 1, -1, 2, -2, 0, 0};
            
            for (var i = 0; i < 7; ++i)
            for (var j = i % 2; j < 9; j += 2)
            {
                var b = i * 9 + j;
                for (var k = 0; k < 8; ++k)
                {
                    var x = i + s1[k];
                    var y = j + s2[k];
                    var z = x * 9 + y;
                    if (x < 0 || x >= 7) continue;
                    if (y < 0 || y >= 9) continue;
                    if (block[b].number == block[z].number) return false;
                }
            }

            return true;
        }
        private int Rand()
        {
            return _rand.Next(1, 5); //isToggleOn ? _rand.Next(4, 11) : _rand.Next(1, 5);
        }
    }
}
