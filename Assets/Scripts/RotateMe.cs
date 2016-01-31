using UnityEngine;
using System.Collections;

public class RotateMe : MonoBehaviour {

    private float currentRotation;
    public float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        currentRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = ((currentRotation + rotationSpeed) * Time.deltaTime) % 360;

        //Manager.say("Current Rotation: " + currentRotation + "Actual Rotation: " + transform.rotation.y);

        //transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, currentRotation, this.transform.eulerAngles.z);
        transform.Rotate(0, currentRotation, 0);
    }
}
