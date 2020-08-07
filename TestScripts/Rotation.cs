using UnityEngine;
using DG.Tweening;

public class Rotation : MonoBehaviour
{
    public GameObject[] cubes;
    public Ray ray;
    public RaycastHit hit;
    public GameObject currentCube;
    public LineRenderer lineRenderer;
    public GameObject particle;
    public GameObject[] cubePrefab;

    public Vector3 realSize;
    public Vector3 bigSize;

    private void Start()
    {
        realSize = new Vector3(80, 80, 80);
        bigSize = new Vector3(90, 90, 90);
        
        /*cubes = new GameObject[35];
        for (var i = 0; i < 5; ++i)
        for (var j = 0; j < 7; ++j)
        {
            var x = i * 7 + j;
            cubes[x] = Instantiate(cubePrefab[x%2]);
            cubes[x].name = x.ToString();
            cubes[x].transform.position = new Vector3(80 + i * 140, 100 + j * 140);
        }*/
    }

    private void Update()
    {
        var y = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        particle.transform.position = new Vector3(y.x, y.y, 0);
        
        if (Physics.Raycast(ray, out hit))
        {
            var n = hit.collider.name;
            //Debug.Log(n);
            foreach (var cube in cubes)
            {
                if (cube.name != n) continue;
                    
                if (currentCube == null)
                {
                    cube.transform.DOScale(bigSize, 0.3f);
                    currentCube = cube;
                    var position = cube.transform.position;
                    lineRenderer.SetPosition(1, position);
                    lineRenderer.SetPosition(0, position);
                }
                else
                {
                    if (currentCube.name == n) continue;
                    currentCube.transform.DOScale(realSize, 0.3f);
                    cube.transform.DOScale(bigSize, 0.3f);
                    
                    lineRenderer.SetPosition(1, cube.transform.position);
                    lineRenderer.SetPosition(0, currentCube.transform.position);
                    currentCube = cube;
                }
            }
        }

        if (currentCube == null) return;
        var z = y;
        z.z = 0;
        lineRenderer.SetPosition(2, z);
        
        var x = currentCube.transform.position;

        var a = (y.x - x.x) / 7f; 
        var b = (y.y - x.y) / 7f;
            
        currentCube.transform.DORotate(new Vector3(b, -a, -135), 0.8f);
    }
}
