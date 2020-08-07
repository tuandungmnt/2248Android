using System;
using UnityEngine;

namespace Domain
{
    public class BlockCreator : MonoBehaviour
    {
        public GameObject[] cubePrefab;
        public GameObject[] numberPrefab;
        public GameObject smallCubePrefab;
        
        public GameObject CreateBlock(int i)
        {    
            Debug.Log("Create: " + i);
            var x = Instantiate(cubePrefab[i / 10]);
            var y = Instantiate(numberPrefab[i % 10], x.transform, true);
            return x;
        }

        public GameObject CreateSmallBlock()
        {
            return Instantiate(smallCubePrefab);
        }
    }
}
