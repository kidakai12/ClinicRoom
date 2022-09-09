using UnityEngine;
using static UnityEngine.ParticleSystem;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MovementDetector : MonoBehaviour
{
    #region SETUP
    private bool isMoving = false;
    public float newPositionThresholdDistance = 0.001f;
    public float inputThreshold = 0.1f;
    public List<Vector3> positionList = new List<Vector3>();
    public GameObject anchorObj;
    public GameObject drawObject1;
    public GameObject drawObject;

    public GameObject prevObject;
    private GameObject nextObject;
    #endregion


    private void Start()
    {
    }
    public void StartDrawing()
    {
        isMoving = true;

        Mesh anchorMesh = anchorObj.GetComponent<MeshFilter>().mesh;
        prevObject = Instantiate(drawObject1, anchorObj.transform.position, Quaternion.identity, null);
        nextObject = Instantiate(drawObject, anchorObj.transform.position, Quaternion.identity, null);

        positionList.Clear();
        positionList.Add(prevObject.transform.position);

        nextObject.GetComponent<LookAtScript>().target = prevObject.transform.position;
        nextObject.GetComponent<LookAtScript>().enabled = true;
        //nextObject.GetComponent<LookAtScript>().target = prevObject.transform;
        /*
        prevObject = Instantiate(drawMaterial, anchorObj.transform.position, Quaternion.identity, null) as GameObject;
        nextObject = Instantiate(drawMaterial, anchorObj.transform.position, Quaternion.identity, null) as GameObject;



        prevObject.GetComponent<DodecagonCreate>().outerDodecagon = nextObject;
        prevObject.GetComponent<DodecagonCreate>().enabled = true;

        MapPosition(nextObject.transform, anchorObj.transform);*/
    }
    List<Vector3> CaculateDistance(List<Vector3> inner, List<Vector3> outer)
    {
        List<Vector3> listDistance = new List<Vector3>();
        for (int i = 0; i < 12; i++)
        {
            listDistance.Add(inner[i] + (nextObject.transform.TransformPoint(outer[i]) - prevObject.transform.TransformPoint(inner[i])));
        }
        return listDistance;
    }
    public void UpdateDrawing()
    {
        MapPosition(nextObject.transform,anchorObj.transform);
        nextObject.GetComponent<LookAtScript>().target = positionList[positionList.Count - 1];
        /*if (Vector3.Distance(anchorObj.transform.position, positionList[positionList.Count-1]) > newPositionThresholdDistance)
        {*/
            Vector3 newPoint = anchorObj.transform.position;
            positionList.Add(newPoint);
            nextObject.GetComponent<LookAtScript>().target = positionList[positionList.Count - 2];

            Vector3[] newVertices = new List<Vector3>(nextObject.GetComponent<MeshFilter>().mesh.vertices).GetRange(0, 12).ToArray();
            Vector3[] prevVertices = prevObject.GetComponent<MeshFilter>().mesh.vertices;
            prevObject.GetComponent<MeshLineContainer>().RefreshMesh(CaculateDistance(new List<Vector3>(prevVertices).GetRange(prevVertices.Length-12,12),new List<Vector3>(newVertices)).ToArray());
            

            //nextObject.GetComponent<LookAtScript>().target = prevObject.transform;

        /*}
        else
        {
            nextObject.GetComponent<LookAtScript>().target = positionList[positionList.Count - 1];
            //anchorObj.GetComponent<LookAtScript>().target = prevObject.transform;
            Vector3[] newVertices = new List<Vector3>(nextObject.GetComponent<MeshFilter>().mesh.vertices).GetRange(0, 12).ToArray();
            Vector3[] prevVertices = prevObject.GetComponent<MeshFilter>().mesh.vertices;
            prevObject.GetComponent<MeshLineContainer>().RefreshMesh2(CaculateDistance(new List<Vector3>(prevVertices).GetRange(prevVertices.Length-12,12),new List<Vector3>(newVertices)).ToArray());
        }*/
    }

    void MapPosition(Transform target, Transform host)
    {
        target.position = host.position;
        //target.rotation = host.rotation;
    }

    public void EndDrawing()
    {
        isMoving = false;
        Vector3[] newVertices = new List<Vector3>(nextObject.GetComponent<MeshFilter>().mesh.vertices).GetRange(12, 12).ToArray();
        Vector3[] prevVertices = prevObject.GetComponent<MeshFilter>().mesh.vertices;
        prevObject.GetComponent<MeshLineContainer>().RefreshMesh(CaculateDistance(new List<Vector3>(prevVertices).GetRange(prevVertices.Length - 12, 12), new List<Vector3>(newVertices)).ToArray());
        prevObject.GetComponent<MeshCollider>().sharedMesh = prevObject.GetComponent<MeshFilter>().mesh;
        Destroy(nextObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving && OVRInput.Get(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.Z) )
        {
            StartDrawing();
        }
        else if (isMoving && !OVRInput.Get(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.X))
        {
            EndDrawing();
        }
        else if (isMoving && OVRInput.Get(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.C))
        {
            UpdateDrawing();
        }

        //if (/*!isMoving && OVRInput.Get(OVRInput.Button.Two) ||*/ Input.GetKeyDown(KeyCode.Z))
        //{
        //    StartDrawing();
        //}
        //else if (/*isMoving && !OVRInput.Get(OVRInput.Button.Two) ||*/ Input.GetKeyDown(KeyCode.X))
        //{
        //    EndDrawing();
        //}
        //else if (/*isMoving && OVRInput.Get(OVRInput.Button.Two) ||*/ Input.GetKeyDown(KeyCode.C))
        //{
        //    UpdateDrawing();
        //}
    }

}
