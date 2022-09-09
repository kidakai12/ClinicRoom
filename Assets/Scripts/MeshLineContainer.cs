using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshLineContainer : MonoBehaviour
{

    private Vector3[] vertices;
    private Mesh mesh;
    private List<int> newTriangles = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        vertices = this.GetComponent<MeshFilter>().mesh.vertices;
        mesh = this.GetComponent<MeshFilter>().mesh;

        //We have 12 point in this Object right !?
    }

    //we Input 12 more point in to this func
    public void RefreshMesh(Vector3[] newVertices)
    {
        //Add 12 point in to this list
        //0  1  2  3  4  5  6  7  8  9  10 11
        //12 13 14 15 16 17 18 19 20 21 22 23

        List<Vector3> listVertices = new List<Vector3>(vertices);
        listVertices.AddRange(newVertices);
        vertices = listVertices.ToArray();
        Debug.Log("Points Added : "+ listVertices.Count);
        int i = 0;
        //after addding 12 point to the list we now have 24 point with point start at 0-23
        Dictionary<int, int> tempdic = new Dictionary<int, int>();
        for (int z = listVertices.Count - 24; z < listVertices.Count; z++)
        {
            //z is the real position in list
            //i is the fake one
            tempdic.Add(i,z);
            i++;
        }
        int[] polygonTriangles;
        for (int y = 0; y < 12; y++)
        {
            int outerIndex = y;
            int innerIndex = y + 12;

            //first triangle starting at outer edge i
            newTriangles.Add(tempdic[outerIndex]);
            newTriangles.Add(tempdic[innerIndex]);
            newTriangles.Add(tempdic[(y + 1) % 12]);

            //second triangle starting at outer edge i
            newTriangles.Add(tempdic[outerIndex]);
            newTriangles.Add(tempdic[12 + ((12 + y - 1) % 12)]);
            newTriangles.Add(tempdic[outerIndex + 12]);
        }
        Debug.Log("Triangle Added : " + newTriangles.Count);
        polygonTriangles = newTriangles.ToArray();
        mesh.Clear();
        mesh.vertices = listVertices.ToArray();
        mesh.triangles = polygonTriangles;

        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void RefreshMesh2(Vector3[] modifyVertices)
    {
        //replace last 12 point
        //0  1  2  3  4  5  6  7  8  9  10 11
        //12 13 14 15 16 17 18 19 20 21 22 23
        int x = 0;
        List<Vector3> listVertices = new List<Vector3>(vertices);
        for (int h = listVertices.Count - 12;h<listVertices.Count;h++)
        {
            listVertices[h] = modifyVertices[x];
            x++;
        }
        int i = 0;
        //after addding 12 point to the list we now have 24 point with point start at 0-23
        Dictionary<int, int> tempdic = new Dictionary<int, int>();
        for (int z = listVertices.Count - 24; z < listVertices.Count; z++)
        {
            //z is the real position in list
            //i is the fake one
            tempdic.Add(i, z);
            i++;
        }
        int[] polygonTriangles;

        polygonTriangles = newTriangles.ToArray();
        mesh.Clear();
        mesh.vertices = listVertices.ToArray();
        mesh.triangles = polygonTriangles;

        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ImagineDestroy")
        {
            Destroy(gameObject);
        }
    }
}
