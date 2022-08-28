using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImagineController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "ImagineDestroy")
        {
            Destroy(gameObject);
        }
    }
}
