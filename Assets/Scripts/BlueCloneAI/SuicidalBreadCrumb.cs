using UnityEngine;
using System.Collections.Generic;

public class SuicidalBreadCrumb : MonoBehaviour {

    public Queue<Vector3> AIbreadCrumbs;


	// Use this for initialization
	void Start ()
    {
        AIbreadCrumbs = new Queue<Vector3>();   
	}
	// Update is called once per frame
	void Update () {
	
	}
}
