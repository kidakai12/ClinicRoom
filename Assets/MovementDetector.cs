using UnityEngine;
using static UnityEngine.ParticleSystem;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MovementDetector : MonoBehaviour
{
    private bool isMoving = false;
    public TextMeshPro textShow;
    public GameObject meshl;
    public GameObject meshContainer;
    public GameObject drawMaterial;
    public GameObject tempObj;
    public GameObject brush;
    public float newPositionThresholdDistance = 0.05f;
    public float inputThreshold = 0.1f;
    public Transform movementSource;
    public List<Vector3> positionList = new List<Vector3>();
    Renderer rend;
    public Particle[] listobject = new Particle[1000000];
    [SerializeField]
    private int numParticlesAlive;
    // Update is called once per frame
    private void Start()
    {
        rend = drawMaterial.GetComponent<Renderer>();
        rend.enabled = true;
    }
    void Update()
    {
        textShow.text = movementSource.position.ToString();
        RotateBrush();
        if (!isMoving && OVRInput.Get(OVRInput.Button.Two))
        {
            StartMovement();
        }
        else if (isMoving && !OVRInput.Get(OVRInput.Button.Two))
        {
            EndMovement();
        }
        else if (isMoving && OVRInput.Get(OVRInput.Button.Two))
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        isMoving = true;

        //Old way
        //tempObj = Instantiate(drawMaterial, anchorObj.transform.position, Quaternion.identity, transform); 


        //New way
        positionList.Clear();
        positionList.Add(movementSource.position);
    }

    void EndMovement()
    {
        isMoving = false;
        //textmesh.text += tempObj.GetComponent<ParticleSystem>().randomSeed.ToString() + System.Environment.NewLine;
        tempObj.transform.SetParent(null);
        tempObj = null;
    }

    void UpdateMovement()
    {
        //Old way
        /*numParticlesAlive = tempObj.GetComponent<ParticleSystem>().GetParticles(listobject);
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            tempObj.transform.localScale += Vector3.one;
        }
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            tempObj.transform.localScale -= Vector3.one;
        }*/

        //New way
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if(Vector3.Distance(movementSource.position,lastPosition) > newPositionThresholdDistance)
            positionList.Add(movementSource.position);
    }


    private void LateUpdate()
    {
        //textShow.text = "No " + linenum + " : " + numParticlesAlive + Environment.NewLine ;
        //linenum++;
        //createMesh();
    }

    private async void createMesh()
    {
        var result = await Task.Run(()=>
        {
            for (int i = 0; i < numParticlesAlive; i++)
            {
                Instantiate(meshl, listobject[i].position, Quaternion.identity, meshContainer.transform);
                //meshContainer.GetComponent<MeshCombiner>().CombineMesh(rend.sharedMaterial);
            }

            return 0;
        }
        );
    }

    private void RotateBrush()
    {
        Quaternion toRotation = Quaternion.LookRotation(meshl.transform.position, Vector3.up);
        brush.transform.rotation = Quaternion.RotateTowards(brush.transform.rotation,toRotation, 5000f);
    }

}
