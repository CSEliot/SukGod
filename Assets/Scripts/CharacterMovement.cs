using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public float Speed;
    public bool ListAdded;
    public bool CurrentlyChanting;

    public Vector3 ChantStart;
    public GameObject ChantObj;

    public GameObject CloneManager;

    public Rigidbody rb;
    public float VerticalVector;
    public float GravityVector;
    public float JumpVector;
    public Vector3 transVector;
    public Vector3 sideVector;
    public Vector3 MovementVector;

    public Vector3 CamRotation;

    public float isGroundedRayLength = 0.1f;
    public LayerMask layerMaskForGrounded;
    public bool touchingGround;
    

    public bool isGrounded  {
        get
        {
            Vector3 position = transform.position;
            position.y = GetComponent<Collider>().bounds.min.y + 0.1f;
            float length = isGroundedRayLength + 0.1f;
            Debug.DrawRay(position, Vector3.down * length);
            bool grounded = Physics.Raycast(position, Vector3.down, length, layerMaskForGrounded.value);

            return grounded;
        }
    }

	// Use this for initialization
	void Start () {

        rb = this.GetComponent<Rigidbody>();


    }
	
	// Update is called once per frame
	void Update () {


        CalculateFalling();
        CalculateCamPlayerRotation();
        CalculatePlayerMovement();

        rb.velocity = Vector3.Lerp(rb.velocity, MovementVector, Time.deltaTime * 100);

        if (Input.GetButton("Fire2")) {
            StartChant();
        }
        if (Input.GetButton("Fire1") && CurrentlyChanting == true)
        {

            EndChant();
        }
        
    }


    void CalculateFalling()

    {
        if (isGrounded)
        {
            GravityVector = 0;
            if (Input.GetButton("Jump"))
            {
                Jump();
            }
            VerticalVector = rb.velocity.y - GravityVector + JumpVector;
            if (JumpVector < .9)
            {
                JumpVector = 0;
            }

        }
        else
        {
            VerticalVector = rb.velocity.y - GravityVector + JumpVector;
            GravityVector += Time.deltaTime * 3;

            JumpVector = Mathf.Lerp(JumpVector, 0, Time.deltaTime * 1.5f/JumpVector);
        }
    }

    void CalculateCamPlayerRotation()
    {
        CamRotation = transform.localEulerAngles;

        CamRotation.y += Input.GetAxis("Mouse_X") * 1.5f;

        transform.localEulerAngles = CamRotation;

    }

    void Jump()
    {
        JumpVector = 1f;
    }

    void CalculatePlayerMovement()
    {
        transVector = this.transform.TransformDirection(0, 0, 1);
        sideVector = this.transform.TransformDirection(1, 0, 0);

        if (isGrounded)
        {

            MovementVector = (transVector * Input.GetAxis("Vertical") * Speed) + (sideVector * Input.GetAxis("Horizontal") * Speed * .8f);
        }
        MovementVector.y = VerticalVector;
        

    }

    void StartChant()
    {
        ChantObj.SetActive(true);
        CurrentlyChanting = true;
        if (ListAdded == false)
        {
            ChantStart = transform.position;
            CloneManager.GetComponent<SuicidalLooperPool>().AddToList();
            ListAdded = true;
        }
     
    }

    void EndChant()
    {
        ChantObj.SetActive(false);
        CurrentlyChanting = false;
        
        CloneManager.GetComponent<SuicidalLooperPool>().AddClone(ChantStart, transform.position);
        ListAdded = false;
    } 
}

