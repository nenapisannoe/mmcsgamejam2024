using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
public class ShowButtonHint : MonoBehaviour
{
    [SerializeField] private GameObject imageToShow;
    [SerializeField] private Transform imageTransform;

    private bool changePos = false;

    void Start()
    {
        imageToShow.SetActive(false);    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 14)
            return; 
        changePos = true;
        imageToShow.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 14)
            return; 
        imageToShow.SetActive(false);
        changePos = false;
    }

    void Update()
    {
        if (changePos)
            imageToShow.transform.position = Camera.main.WorldToScreenPoint(imageTransform.position);
    }
}
