using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyController : MonoBehaviour
{
    public GameObject[] grabObject;
    public List<Vector3> oldpos;
    public List<Quaternion> oldrot;
    public GameObject parent;
    public Animator AssemblyAnimation;
    public GameObject CameraView;
    public GameObject player;
    public GameObject UIHelper;
    public Transform ObjectParent;
    public Vector3 parentOldPot;
    public Quaternion parentOldRot;
    public bool TriggerAssemble;
    private int i;
    public void Start()
    {
        /*player.SetActive(false);
        CameraView.SetActive(true);
        StartCoroutine(IntroView());*/
        i = 0;
        parentOldPot = parent.transform.position;
        parentOldRot = parent.transform.rotation;
        TriggerAssemble = true;
        foreach (GameObject g in grabObject)
        {
            oldpos.Add(g.transform.position);
            oldrot.Add(g.transform.rotation);
        }
    }
    public void Update()
    {
        /*if (i == 0)
        {
            player.SetActive(false);
            CameraView.SetActive(true);
            IntroView();
            i++;
        }*/
        if (Input.GetKeyDown(KeyCode.A))
        {
            parent.GetComponent<Animator>().enabled = true;
            StartCoroutine(AssemblyObject());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            parent.GetComponent<Animator>().enabled = true;
            StartCoroutine(DisassemblyObject());
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (TriggerAssemble)
            {
                parent.GetComponent<Animator>().enabled = true;
                StartCoroutine(DisassemblyObject());
                TriggerAssemble = false;
            }
            else
            {
                parent.GetComponent<Animator>().enabled = true;
                StartCoroutine(AssemblyObject());
                TriggerAssemble = true;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            player.SetActive(false);
            CameraView.SetActive(true);
            StartCoroutine(IntroView());
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            UIHelper.SetActive(!UIHelper.activeSelf);
            if (!UIHelper.activeSelf)
            {
                foreach (GameObject g in grabObject)
                {
                    g.layer = 0;
                }
            }
            else
            {
                foreach (GameObject g in grabObject)
                {
                    g.layer = 6;
                }
            }
        }
    }

    IEnumerator IntroView()
    {
        CameraView.GetComponent<Animator>().SetBool("CameraStart", true);
        yield return new WaitForSeconds(24.30f);
        player.SetActive(true);
        CameraView.SetActive(false);
        CameraView.GetComponent<Animator>().SetBool("CameraStart", false);
    }
    IEnumerator AssemblyObject()
    {
        foreach (GameObject g in grabObject)
        {
            g.transform.parent = parent.transform;
        }
        AssemblyAnimation.SetBool("Assembly",true);
        yield return new WaitForSeconds(3.0f);
        parent.GetComponent<Animator>().enabled = false;
        parent.GetComponent<Collider>().enabled = true;
        parent.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        int i = 0;
        foreach (GameObject g in grabObject)
        {
            g.GetComponent<Collider>().enabled = false;
            g.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            g.transform.position = oldpos[i];
            g.transform.rotation = oldrot[i];
            i++;
        }
        parent.transform.parent = ObjectParent;
        parent.transform.position = parentOldPot;
        parent.transform.rotation = parentOldRot;
    }

    IEnumerator DisassemblyObject()
    {
        foreach (GameObject g in grabObject)
        {
            g.transform.parent = parent.transform;
        }
        AssemblyAnimation.SetBool("Assembly", false);
        yield return new WaitForSeconds(3.0f);
        parent.GetComponent<Animator>().enabled = false;
        parent.GetComponent<Collider>().enabled = false;
        parent.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        foreach (GameObject g in grabObject)
        {
            g.GetComponent<Collider>().enabled = true;
            g.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
    }
}
