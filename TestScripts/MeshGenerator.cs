using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;
    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        _vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
            /*new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1)*/
        };

        _triangles = new int[]
        {
            0,2,1,
            2,3,1
            /*2,6,3,
            3,6,7,
            6,4,7,
            7,4,5,
            4,0,1,
            5,4,1,
            1,3,5,
            5,3,7,
            0,4,6,
            0,6,2*/
        };

        _uvs = new Vector2[4];
        _uvs[0] = new Vector2(0,0);
        _uvs[1] = new Vector2(1,0);
        _uvs[2] = new Vector2(0,1);
        _uvs[3] = new Vector2(1,1);


        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
    }
}
