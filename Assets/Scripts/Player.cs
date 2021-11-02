using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    //Unity Object References
    [SerializeField] Transform playerCamera = null;
    
    //Public Serial Fields
    [SerializeField] bool lockCursor = true;
    [SerializeField] float mouseSens = 3.5f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;


    [SerializeField] float playerSpeed = 6f;
    [SerializeField] float gravity = 13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    

    //Player variables
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;
    
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    Vector3 currentDir = Vector3.zero;
    Vector3 currentDirVelocity = Vector3.zero;

    void Start() {
        controller = GetComponent<CharacterController>();

        if(lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update() {
        //Call all update functions
        UpdateMouseLook();
        UpdatePlayerMovement();
    }

    void UpdateMouseLook() {
        //Mouse look
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSens;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSens);
    }

    void UpdatePlayerMovement() {
        //Player movement
        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            //Since diagonal input, ie. (1,0,1) has a larger length, normalize to equal vector magnitudes

        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        
        if(controller.isGrounded) {
            velocityY = 0.0f;
            if(Input.GetKeyDown(KeyCode.Space)) {
            velocityY += 10.0f;
        }
        }
        
        velocityY -= gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.z + transform.right * currentDir.x) * playerSpeed + Vector3.up*velocityY;
        controller.Move(velocity * Time.deltaTime);
    }
}