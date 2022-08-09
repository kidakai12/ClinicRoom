using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FollowMouse : MonoBehaviour
{

    Vector3 mousePos;
    public Particle[] partices = new Particle[10000];
    public GameObject prefabs;
    public Renderer rend;
    public GameObject parti;
    public GameObject MeshContainer;
    public int sum;
    public float speed = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        sum = 0;
        rend = prefabs.GetComponent<Renderer>();
        rend.enabled = true;
    }
    public void MouseControl()
    {
        mousePos = Input.mousePosition;
        mousePos.z = speed;
        float posY = transform.position.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z);
        //transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            MouseControl();
            sum = parti.GetComponent<ParticleSystem>().GetParticles(partices);
        }
    }

    private void LateUpdate()
    {
        for(int i = 0; i < sum; i++)
        {
            Instantiate(prefabs,partices[i].position,Quaternion.identity,MeshContainer.transform);
            MeshContainer.GetComponent<MeshCombiner>().CombineMesh(rend.sharedMaterial);
        }
        partices.Initialize();
    }
}
