using UnityEngine;
using System.Collections;


//Script references waypoints that are used to
//spawn the blue clone in random places
public class deathSpawnManager : MonoBehaviour
{

    //public GameObject blueAI;
    public Transform[] wayPoints;

    void Start()
    {
    }
     
        //Get random location around the spawn area near the cliff
        public Vector3 getSpawnLoc()
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-30.0F, 30.0F), transform.position.y, transform.position.z + Random.Range(-30.0f, 30.0f));
            return pos;
        } 
        
        //Get random location using waypoints objects
        public Vector3 getWayPointLoc()
        {
           return wayPoints[Random.Range(0, wayPoints.Length-1)].position;
        }
    }
