using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public GameObject[] cubes;
    public GameObject smallCube;
    private void Start()
    {
        StartCoroutine(MakeSmaller());
        StartCoroutine(Change());
    }

    private IEnumerator MakeSmaller()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var x = Instantiate(cubes[0]);
            x.transform.position = new Vector3(200, 1100,0);
            yield return new WaitForSeconds(1f);
            
            x.transform.DOScale(new Vector3(10,10,10), 0.7f);
            Destroy(x,0.6f);
            
            var y = Instantiate(smallCube);
            y.transform.position = new Vector3(200, 1100,0);
            y.transform.DOScale(new Vector3(30, 30, 30), 0.2f);

            yield return new WaitForSeconds(1f);
            Destroy(y);
        }
    }

    private IEnumerator Change()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var x = Instantiate(cubes[0]);
            x.transform.position = new Vector3(550, 1100,0);
            x.transform.DORotate(new Vector3(0, 0, -135), 0f);
            
            yield return new WaitForSeconds(0.5f);
    
            var y = Instantiate(cubes[1]);
            y.transform.position = new Vector3(550, 1100,0);
            y.transform.DORotate(new Vector3(0, 180, -135), 0f);
            
            
            x.transform.DORotate(new Vector3(0, -90, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 90, -135), 0.4f);
            yield return new WaitForSeconds(0.3f);

            x.transform.DORotate(new Vector3(0, 180, -135), 0.4f);
            y.transform.DORotate(new Vector3(0, 0, -135), 0.4f);
            yield return new WaitForSeconds(2f);
            Destroy(x);
            Destroy(y);
        }
    }
}
