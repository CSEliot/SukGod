using UnityEngine;
using System.Collections;

public class CustomCamera : MonoBehaviour {

    public Transform LookTarget;
    public Transform Target;
    public Vector3 TargetPosAdjust;
    public Vector3 PlayerFacing;
    public float height;
    public float distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        TargetPosAdjust = Target.position;

        PlayerFacing = Target.forward * distance;
        

        TargetPosAdjust -= PlayerFacing;
        TargetPosAdjust.y += height;

        transform.position = Vector3.Lerp(transform.position, TargetPosAdjust, Time.deltaTime * 8);

        transform.LookAt(LookTarget);

	}
}
