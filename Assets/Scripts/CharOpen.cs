using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    
    public float mouseSens = 5f;
    public Transform playerbody;
    private CharacterController cc;
    float xRotation = 0f;
    public float speed= 5f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens + Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens + Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //float zInput = Input.GetAxis("Vertical"); //W and S keys
    	//float xInput = Input.GetAxis("Horizontal"); //A and D keys

    	//Vector3 movement = new Vector3(xInput, 0, zInput);
        //Vector3 playerDirection = new Vector3(playerbody.x, 0, playerbody.z);
    	//movement = movement.normalized * speed * Time.deltaTime * playerbody.transform.forward;


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerbody.Rotate(Vector3.up * mouseX);
        //playerbody.transform.forward += movement;

        float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 forward = transform.forward * v * speed * Time.deltaTime;
		Vector3 right = transform.right * h * speed * Time.deltaTime;

		cc.Move(forward + right);
        
    }
}
