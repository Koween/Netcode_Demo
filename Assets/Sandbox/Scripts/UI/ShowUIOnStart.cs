using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIOnStart : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
