using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class FirstPersonController : MonoBehaviour {

    public float rotUpDown;// = 0;
    //public Vector3 speed;
    public float verticalSpeed;
    public float rotLeftRight;
    public float maxVelocityChange;


    PhotonView m_PhotonView;
    private PhotonTransformView m_PhotonTransform;

    public float jumpHeight;
    public float gravity = 9.81f;
    public float upDownRange;
    public float upDownSpeed;
    public float RotateSpeed;
    public float SprintSpeedAddon;
    private bool canSprint;
    public float SprintLengthTime;
    public float ReEnableSprintTime;
    private Vector3 playerPos;
    private Ray	ray;
    private RaycastHit rayHitDown;
    private bool isGrounded = true;
    public float moveSpeed;    
    public float totalJumpsAllowed;
    public float totalJumpsMade;
    private float floorInclineThreshold = 0.3f;


    public bool canCheckForJump;

    private bool isDead;
    private int messageEdited; //1 = not caring, 2 = not finished, 3 = finished

    //ACTION STRINGS
    //==================================================================
    private string Haim_str = "_Look Rotation";
    private string Vaim_str = "_Look UpDown";
    private string Strf_str = "_Strafe";
    private string FWmv_str = "_Forward";
    private string Fire_str = "_Fire";
    private string Jump_str = "_Jump";
    private string Dash_str = "_Run";
    private string Zoom_str = "_Zoom";
    private string Drop_str = "_Drop";
    private string Bury_str = "_Bury";
    private string Help_str = "_Help";
    //==================================================================

    //PERSONAL CHARACTER MODIFIERS
    public float walkSpeedModifier;
    public float jumpHeightModifier;
    private float normalSpeed;

    public bool isZoomed = false;

    public GameObject PanelHelpText;
    private bool ActiveText = true;
	
    //cliff control
    Ray rayOrigin;
    RaycastHit hitInfo;
    Vector3 rayOriginStart;
    bool ableToInteract;
    bool canMove;

    //For looking, we are assigning rotations, but we need original values
    //that aren't getting modified, so we can re-assign them.
    private Vector3 startingCameraRotation;
    private Vector3 newRotationAngle;

    //Anim Controller
    private Animator animController;

    //carrying object
    private bool isCarrying;
    private bool dropped;

    private bool paused;

    void Awake () {
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }
    
    
    
    // Use this for initialization
    void Start () {
        normalSpeed = moveSpeed;
        dropped = true;
		
        setControlStrings();
        //animController = gameObject.transform.GetComponent<Animator> ();
        canCheckForJump = true;
        newRotationAngle = new Vector3();
        totalJumpsMade = 0;
        rotLeftRight = 0.0f;
        isDead = false;
        isCarrying = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        maxVelocityChange = moveSpeed;
        canMove = true;
        paused = false;
        canSprint = true;

        m_PhotonView = GetComponent<PhotonView>();
        m_PhotonTransform = GetComponent<PhotonTransformView>();
        if (m_PhotonView.isMine)
        {
            startingCameraRotation = transform.GetChild(0).transform.localRotation.eulerAngles;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void Update ()
    {
        if (Input.GetAxis(Fire_str) == 1)
        {
            Debug.Log(GetComponent<Rigidbody>().velocity);
        }

        if (Input.GetButtonDown(Help_str))
        {
            ToggleHelpText();
        }
        if (Input.GetButtonDown(Dash_str))
        {
            if (canSprint)
            {
                StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        moveSpeed += SprintSpeedAddon;
        canSprint = false;
        yield return new WaitForSeconds(SprintLengthTime);
        moveSpeed -= SprintSpeedAddon;
        StartCoroutine(ReallowSprinting());
    }

    IEnumerator ReallowSprinting()
    {
        yield return new WaitForSeconds(ReEnableSprintTime);
        canSprint = true; 
    }

    void FixedUpdate () {

        if (!m_PhotonView.isMine)
            return;

        rayOrigin = new Ray(transform.position, transform.up*-1);

        //if you are able to reach something, anything important or not
        if (Physics.Raycast(rayOrigin, out hitInfo)) {
            //if (Time.time % 2f > 1.8f) { Debug.Log("Below me is: " + hitInfo.normal.y); }
            if (hitInfo.normal.y <= 0.4f)
            {
                canMove = false;
            }
            else
            {
                canMove = true;
            }
        }

        if (!paused) {		
            //player rotation
            //left and right
            rotLeftRight = Input.GetAxis (Haim_str) * RotateSpeed;
            transform.Rotate (0, rotLeftRight, 0);
            //up and down (with camera)
            rotUpDown -= Input.GetAxis (Vaim_str) * upDownSpeed;
            rotUpDown = Mathf.Clamp (rotUpDown, -upDownRange, upDownRange);
            newRotationAngle.x = rotUpDown;
            newRotationAngle.y = startingCameraRotation.y;
            newRotationAngle.z = startingCameraRotation.z;
            transform.GetChild (0).transform.localRotation = Quaternion.Euler (newRotationAngle);

            //Jumping!!
            if (totalJumpsMade < totalJumpsAllowed && Input.GetButtonDown (Jump_str)) {
                totalJumpsMade += 1;
                isGrounded = false;
                canCheckForJump = false;

                GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, CalculateJumpVerticalSpeed (), GetComponent<Rigidbody>().velocity.z);

                Invoke ("AllowJumpCheck", 0.1f);
            }
            if (canMove) {
                Vector3 targetVelocity;
                targetVelocity = new Vector3 (Input.GetAxis (Strf_str), 0, Input.GetAxis (FWmv_str));
                targetVelocity = transform.TransformDirection (targetVelocity);
                targetVelocity *= moveSpeed;
                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = GetComponent<Rigidbody>().velocity;
                Vector3 velocityChange = (targetVelocity - velocity);

                velocityChange.x = Mathf.Clamp (velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp (velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                GetComponent<Rigidbody>().AddForce (velocityChange, ForceMode.VelocityChange);
                
                // Jump
                //Manager.say("Jumping action go. Jumps Made: " + totalJumpsMade + " Jumps Allowed: " + totalJumpsAllowed, "eliot");
            }

            GetComponent<Rigidbody>().AddForce (new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));
            // We apply gravity manually for more tuning control
        }
    }  

    private float CalculateJumpVerticalSpeed () {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * (jumpHeight+jumpHeightModifier) * gravity);
    }

    public bool getIsCarrying(){
        return isCarrying;
    }

    private void setControlStrings(){
        string pName = gameObject.name;

        if(pName.Contains("1")){
            Fire_str = "p1" + Fire_str;
            FWmv_str = "p1" + FWmv_str;
            Strf_str = "p1" + Strf_str;
            Haim_str = "p1" + Haim_str;
            Vaim_str = "p1" + Vaim_str;
            Jump_str = "p1" + Jump_str;
            Dash_str = "p1" + Dash_str;
            Zoom_str = "p1" + Zoom_str;
            Drop_str = "p1" + Drop_str;
            Bury_str = "p1" + Bury_str;
            Help_str = "p1" + Help_str;
        }
        if (pName.Contains("4"))
        {
            Fire_str = "p4" + Fire_str;
            FWmv_str = "p4" + FWmv_str;
            Strf_str = "p4" + Strf_str;
            Haim_str = "p4" + Haim_str;
            Vaim_str = "p4" + Vaim_str;
            Jump_str = "p4" + Jump_str;
            Dash_str = "p4" + Dash_str;
            Zoom_str = "p4" + Zoom_str;
            Drop_str = "p4" + Drop_str;
            Bury_str = "p4" + Bury_str;
            Help_str = "p4" + Help_str;
        }
        if (pName.Contains("2"))
        {
            Fire_str = "p2" + Fire_str;
            FWmv_str = "p2" + FWmv_str;
            Strf_str = "p2" + Strf_str;
            Haim_str = "p2" + Haim_str;
            Vaim_str = "p2" + Vaim_str;
            Jump_str = "p2" + Jump_str;
            Dash_str = "p2" + Dash_str;
            Zoom_str = "p2" + Zoom_str;
            Drop_str = "p2" + Drop_str;
            Bury_str = "p2" + Bury_str;
            Help_str = "p2" + Help_str;
        }
        if (pName.Contains("3"))
        {
            Fire_str = "p3" + Fire_str;
            FWmv_str = "p3" + FWmv_str;
            Strf_str = "p3" + Strf_str;
            Haim_str = "p3" + Haim_str;
            Vaim_str = "p3" + Vaim_str;
            Jump_str = "p3" + Jump_str;
            Dash_str = "p3" + Dash_str;
            Zoom_str = "p3" + Zoom_str;
            Drop_str = "p3" + Drop_str;
            Bury_str = "p3" + Bury_str;
            Help_str = "p3" + Help_str;
        }
    }

    public string GetFire_Str(){
        return Fire_str;
    }

    // piece of delays OnCollisionStay's ground check so we can't jump for 2 seconds after
    public void AllowJumpCheck()
    {
        //Manager.say("CAN CJECK JUMP", "eliot");
        canCheckForJump = true;
    }

    void OnCollisionStay(Collision floor){
        Vector3 tempVect;
        // we want to prevent isGrounded from being true and totalJumpsMade = 0 until 2 seconds later
        if(isGrounded == false && canCheckForJump){
            for(int i = 0; i < floor.contacts.Length; i++){
                tempVect = floor.contacts[i].normal;
                if( tempVect.y > floorInclineThreshold){
                    isGrounded = true;
                    totalJumpsMade = 0;
                    return;
                    //Manager.say("Collision normal is: " + tempVect);
                }
            }
        }
    }

    void OnTriggerEnter(Collider colObj)
    {
    }

    void OnTriggerStay(Collider colObj)
    {
    }

    void OnTriggerExit(Collider colObj)
    {
    }

    IEnumerator DelayCarrying()
    {
        yield return new WaitForSeconds(1f);
        isCarrying = false;
    }

    private void ToggleHelpText(){
        ActiveText = !ActiveText;
        PanelHelpText.SetActive(ActiveText);
    }

    public void PauseCharacter()
    {
        paused = true;
    }

    public void UnpauseCharacter()
    {
        paused = false;
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
    
    public void SetIsDead(bool DeadState)
    {
        isDead = DeadState;
    }
}