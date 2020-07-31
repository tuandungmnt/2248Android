using UnityEngine;

namespace Domain
{
    public class GameUiCreator : MonoBehaviour
    {
        public GameObject blockPrefab;
        public GameObject linePrefab;
        public GameObject nameTextPrefab;
        public GameObject scoreTextPrefab;
        public GameObject popUpPrefab;

        public GameObject CreateBlock(GameObject parent) 
        {
            var go = Instantiate(blockPrefab, parent.transform, false);
            return go;
        }
        
        public GameObject CreateLine(GameObject parent) 
        {
            var go = Instantiate(linePrefab, parent.transform, false);
            return go;
        }
        
        public GameObject CreateNameText(GameObject parent) 
        {
            var go = Instantiate(nameTextPrefab, parent.transform, false);
            return go;
        }
        
        public GameObject CreateScoreText(GameObject parent) 
        {
            var go = Instantiate(scoreTextPrefab, parent.transform, false);
            return go;
        }

        public GameObject CreatePopUp(Transform parent)
        {
            var go = Instantiate(popUpPrefab, parent, false);
            return go;
        }
    }
}
