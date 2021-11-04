using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeyItem : MonoBehaviour
{
    
    
    [SerializeField][Range(0.0f, 100.0f)] float grabSpeed = 15.0f;

    private Renderer rend;
    private Rigidbody rb;
    private bool held;
    

    // Start is called before the first frame update
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        held = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if(held) {
            // Reset position and rotation
            var currentPos = transform.localPosition;
            var targetPos = Vector3.zero + new Vector3(0, 0, 3f);
            transform.localPosition = Vector3.Lerp(currentPos, targetPos, grabSpeed*Time.deltaTime);
            transform.localEulerAngles = Vector3.zero;
        }
    }
    public void SetHeld(bool beingHeld) {
        held = beingHeld;
    }

    public bool GetHeld() {
        return held;
    }

    public void ItemSelectHighlight() {
        //renderer.material.color = Color.yellow;
        if(gameObject.GetComponent<Outline>() == null) {   
            var outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
        }
    }

    public void ItemDeselectHighlight() {
        //renderer.material.color = startColor;
        var outline = gameObject.GetComponent<Outline>();
        Destroy(outline);
    }

    public void PickItem(Transform grabSlot) {
        held = true;

        // Disable rigidbody and reset velocities
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Set Slot as a parent
        transform.SetParent(grabSlot);        
    }

    public void DropItem() {
        // Remove reference
        held = false;

        // Remove parent
        transform.SetParent(null);

        // Enable rigidbody
        rb.isKinematic = false;

        // Add force to throw a little bit
        rb.AddForce(transform.forward * 2, ForceMode.VelocityChange);
    }
}
