using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    //Unity Object References
    [SerializeField] Camera playerCamera = null;
    
    //Camera controls
    [Header("Mouse Camera")]
    [SerializeField] bool lockCursor = true;
    [SerializeField] float mouseSens = 3.5f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    //[SerializeField] float playerMass = 10.0f;
    [Header("Player Movement")]
    [SerializeField] float playerSpeed = 6.0f;
    [SerializeField] float gravity = 13.0f;
    [SerializeField] float jumpVel = 7.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    [Header("Inventory")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] float itemPickupTime = 1.0f;
    [SerializeField] float reachDist = 5.0f;
      

    //Movement variables
    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    CharacterController controller = null;
    
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private Vector3 currentDir = Vector3.zero;
    Vector3 currentDirVelocity = Vector3.zero;

    //Inventory variables
    private KeyItem itemBeingPickedUp = null;
    private float currentPickupTime = 0f;
    List<KeyItem> playerInventory = null;

    private void Start() {
        controller = GetComponent<CharacterController>();

        if(lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update() {
        //Call all update functions
        UpdateMouseLook();
        UpdatePlayerMovement();
        UpdateInventory();
    }

    private void OnTriggerEnter(Collider col) {
        Debug.Log("Trigger!");

        if(col.gameObject.CompareTag("Respawn")) {
            Debug.Log("Respawning!");
            controller.enabled = false;
            gameObject.transform.position = new Vector3(0, 0, 0);
            controller.enabled = true;
        }
    }


    //Functions

    private void UpdateMouseLook() {
        //Mouse look
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSens;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSens);
    }

    private void UpdatePlayerMovement() {
        //Player movement
        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            //Since diagonal input, ie. (1,0,1) has a larger length, normalize to equal vector magnitudes

        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        
        if(controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            velocityY = jumpVel;
        }
        
        velocityY -= gravity * Time.deltaTime;

        velocityY = Mathf.Clamp(velocityY, -100.0f, 20.0f);
        //Debug.Log("VelocityY: " + velocityY);

        Vector3 velocity = (transform.forward * currentDir.z + transform.right * currentDir.x) * playerSpeed + Vector3.up*velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateInventory() {
        //Inventory tracking

        //do stuff with playerInventory;

        //Detect if hovering over item
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction*reachDist, Color.red);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, reachDist, layerMask)) {       //Raycast to see if hit gameObject
            KeyItem hitItem = hitInfo.collider.GetComponent<KeyItem>();     //Does gameObject have KeyItem Component?
            if(itemBeingPickedUp != null && hitItem == null) {              //If previously hovered over item and now not, delete highlighting
                itemBeingPickedUp.ItemDeselectHighlight();
                itemBeingPickedUp = null;                                   
            } else if(hitItem != null && hitItem != itemBeingPickedUp) {    //If hovering over item, highlight for first time
                itemBeingPickedUp = hitItem;
                itemBeingPickedUp.ItemSelectHighlight();
            } else if(hitItem == null) {                                    //If not hovering over KeyItem, null
                itemBeingPickedUp = null;
            }
        } else if(itemBeingPickedUp != null) {              //If previously hovered over item and now not, delete highlighting
            itemBeingPickedUp.ItemDeselectHighlight();
            itemBeingPickedUp = null; 
        } else {
            itemBeingPickedUp = null;                                       //If not hovering over anything, null
        }
        //Debug.Log(itemBeingPickedUp);


        //If hovering over item, and holding e to interact, hold for itemPickupTime to add to inventory
        if(itemBeingPickedUp != null) {
            if(Input.GetKey(KeyCode.E)) {
                currentPickupTime += Time.deltaTime;
                if(currentPickupTime >= itemPickupTime) {
                    //Add item to inventory
                    Debug.Log("Picking up item!");
                    Destroy(itemBeingPickedUp.gameObject);
                    itemBeingPickedUp = null;
                }
            }
        } else {
            currentPickupTime = 0;
        }

    }
}