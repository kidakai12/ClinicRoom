using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class SelectObjectController : MonoBehaviour
{
    [SerializeField]
    private AudioSource HOVER_SOUND;


    [SerializeField]
    private UnityEvent onEnter;

    [SerializeField]
    public UnityEvent onExit;


    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    public GameObject API;
    public string Content;
    public TextMeshProUGUI text;
    public RawImage img;
    public Texture2D texture;
    public int i;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Va chạm nè");
        TrigExit.instance.currentCollider4 = GetComponent<SelectObjectController>();
        onEnter.Invoke();
    }
    private void Start()
    {
    }
    public void HoverEnter()
    {
        HOVER_SOUND.Play();
     
    }
    public void HoverExit()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ObjectInfo();
        }
    }
    public void TriggerPanel()
    {
        panel.SetActive(true);
        ObjectInfo();
    }

    public void ObjectInfo()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;
        float spawnDistance = 1.5f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        panel.transform.position = new Vector3(spawnPos.x, 1.8f,spawnPos.z); ;
        panel.transform.rotation = new Quaternion(0, playerRotation.y, 0, playerRotation.w);
        texture.anisoLevel = 16;
        RectTransform rectRaw = img.GetComponent<RectTransform>();
        double fraction = (float)texture.width / (float)texture.height;
        Debug.Log("Height: " + fraction);
        float height = 1920 / (float)fraction;
        Debug.Log("Height: " + height);
        rectRaw.sizeDelta = new Vector2(0.8f, 0.4f);
        img.texture = texture;
        SizeToParent(img, 0);

        text.text = Content;
    }

    public static Vector2 SizeToParent(RawImage image, float padding)
    {
        float w = 0, h = 0;
        var parent = image.GetComponentInParent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();

        // check if there is something to do
        if (image.texture != null)
        {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding)
            { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }


}
