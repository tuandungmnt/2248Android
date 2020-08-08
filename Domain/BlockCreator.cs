using UnityEngine;

namespace Domain
{
    public class BlockCreator : MonoBehaviour
    {
        public GameObject[] cubePrefab;
        public GameObject[] numberPrefab;
        public GameObject smallCubePrefab;

        private int _counter;
        
        public GameObject CreateBlock(int i)
        {    
            var x = Instantiate(cubePrefab[i / 10]);
            var y = Instantiate(numberPrefab[i % 10], x.transform, true);
            x.name = _counter.ToString();
            _counter++;
            return x;
        }

        public GameObject CreateSmallBlock()
        {
            return Instantiate(smallCubePrefab);
        }
    }
}
