using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    
    Renderer renderer;
    Color startColor;
    // Start is called before the first frame update
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        startColor = renderer.material.color;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ItemSelectHighlight() {
        //renderer.material.color = Color.yellow;
        var outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    public void ItemDeselectHighlight() {
        //renderer.material.color = startColor;
        var outline = gameObject.GetComponent<Outline>();
        Destroy(outline);
    }
}
