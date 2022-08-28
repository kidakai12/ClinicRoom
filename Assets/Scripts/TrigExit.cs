using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigExit : MonoBehaviour
{
    public static TrigExit instance;

  

    [HideInInspector]
    public SelectObjectController currentCollider4;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        if (currentCollider4 != null)
            currentCollider4.onExit.Invoke();
    }    
}
