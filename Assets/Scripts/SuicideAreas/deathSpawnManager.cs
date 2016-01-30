using UnityEngine;
using System.Collections;

public class deathSpawnManager : MonoBehaviour
{

    void startSuicideLoop()
    {
        //while true

    }

    public Vector3 getSpawnLoc()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-30.0F, 30.0F), transform.position.y, transform.position.z + Random.Range(-30.0f, 30.0f));
        return pos;
    }
}
