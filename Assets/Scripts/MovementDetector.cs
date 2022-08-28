using UnityEngine;
using static UnityEngine.ParticleSystem;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MovementDetector : MonoBehaviour
{

    public Transform movementSource;

    private bool isMoving = false;
    public TextMeshPro textShow;
    public float newPositionThresholdDistance = 0.05f;
    public float inputThreshold = 0.1f;
    public List<Vector3> positionList = new List<Vector3>();
    public GameObject meshl;
    public GameObject meshContainer;
    public GameObject anchorObj;
    public GameObject drawMaterial;
    public GameObject tempObj;
    Renderer rend;
    public Particle[] listobject = new Particle[1000000];
    private void Start()
    {
        rend = drawMaterial.GetComponent<Renderer>();
        rend.enabled = true;
    }
    public void StartDrawing()
    {
        isMoving = true;

        tempObj = Instantiate(drawMaterial, anchorObj.transform.position, Quaternion.identity, transform);


    }

    public void UpdateDrawing()
    {
       
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);
        }
    }

    public void EndDrawing()
    {
        isMoving = false;
        tempObj.transform.SetParent(null);
        tempObj = null;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving && OVRInput.Get(OVRInput.Button.Two) )
        {
            StartDrawing();
        }
        else if (isMoving && !OVRInput.Get(OVRInput.Button.Two))
        {
            EndDrawing();
        }
        else if (isMoving && OVRInput.Get(OVRInput.Button.Two) )
        {
            UpdateDrawing();
        }
    }

}
