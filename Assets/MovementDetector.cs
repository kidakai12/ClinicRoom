using UnityEngine;
using static UnityEngine.ParticleSystem;
using TMPro;
using System.Threading.Tasks;

public class MovementDetector : MonoBehaviour
{
    private bool isMoving = false;
    public TextMeshPro textShow;
    public GameObject meshl;
    public GameObject meshContainer;
    public GameObject anchorObj;
    public GameObject drawMaterial;
    public GameObject tempObj;
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
        tempObj = Instantiate(drawMaterial, anchorObj.transform.position, Quaternion.identity, transform); 
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
        numParticlesAlive = tempObj.GetComponent<ParticleSystem>().GetParticles(listobject);
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            tempObj.transform.localScale += Vector3.one;
        }
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            tempObj.transform.localScale -= Vector3.one;
        }

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

}
