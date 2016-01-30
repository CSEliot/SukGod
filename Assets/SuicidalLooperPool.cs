using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuicidalLooperPool : MonoBehaviour {

    [System.Serializable]
    public class Suicidee
    {
        //Doesnt execute clone until it's a success!
        public bool Pending = true;

        public bool AlreadyMade;
        public Vector3 SpawnLocation;
        public Vector3 DeathLocation;
        //When Clone finishes summoning
        public bool isSummoned;
        //When Clone gets to dying
        public bool isDone;
        public float betweenSpawnTimer = 0;

        //wait for clones to materialize then move them
        public float materializeTimer = 0;

        public GameObject Instance;
    }


    public Suicidee DefaultItem;
    public int currentCloneNum;
    public bool AddMore;
    public bool LoopPending;

//    public Suicidee[] ClonePool = new Suicidee[0];
    public List<Suicidee> ClonePool = new List<Suicidee>();
    public GameObject Prefab;
    public float TimeBetweenLoops = 3;
    public float SummonTime = 3;

    public void AddToList()
    {
        Debug.Log(ClonePool.Count);

        DefaultItem.AlreadyMade = false;
        DefaultItem.isSummoned = false;
        DefaultItem.isDone = false;
        DefaultItem.Instance = null;

        DefaultItem.betweenSpawnTimer = 0;
        DefaultItem.materializeTimer = 0;

        //ClonePool.Add(DefaultItem);
        Suicidee temp = new Suicidee();
        ClonePool.Add(temp);
        Debug.Log(ClonePool.Count);
        LoopPending = true;
    }



    public void AddClone (Vector3 SpawnLoc, Vector3 DeathLoc) {

        //ClonePool.Add(DefaultItem);
       // ClonePool.Capacity ++;
            ClonePool[currentCloneNum].SpawnLocation = SpawnLoc;
            ClonePool[currentCloneNum].DeathLocation = DeathLoc;
            ClonePool[currentCloneNum].Pending = false;

            AddMore = true;
            currentCloneNum++;
       // }
        
        
        LoopPending = false;
        
    }

	
	// Update is called once per frame
	void Update () {



        if (ClonePool.Capacity > 0)
        {
            if (AddMore == true)
            {

                for (int i = 0; i < ClonePool.Count; i++)
                {
                    if (ClonePool[i].AlreadyMade == false)
                        ClonePool[i].Instance = Instantiate(Prefab, ClonePool[i].SpawnLocation, transform.rotation) as GameObject;
                         ClonePool[i].AlreadyMade = true;

                }
                AddMore = false;
            }
            //if (ClonePool[currentCloneNum - 1].Pending == true)
            if (LoopPending == true)
            {
                for (int i = 0; i < Mathf.Clamp(ClonePool.Count - 1, 0, ClonePool.Count); i++)
                {
                    MoveClone(i);
                    CheckCloneCycle(i);
                    BetweenCycleTimer(i);
                }


            }
            else
            {
                for (int i = 0; i < ClonePool.Count; i++)
                {
                    SummonTimer(i);
                    MoveClone(i);
                    CheckCloneCycle(i);
                    BetweenCycleTimer(i);
                }

            }
        }
	}

    void SummonTimer(int number)
    {
        if(ClonePool[number].isDone == false)

        if (ClonePool[number].materializeTimer < SummonTime)
        {
            ClonePool[number].materializeTimer += Time.deltaTime;
        }
        else
        {
            ClonePool[number].isSummoned = true;
        }

    }


    void MoveClone(int number)
    {
        //Check to see if clone is active (between loops)
        if (ClonePool[number].isSummoned == true)

            ClonePool[number].Instance.transform.position = Vector3.Lerp(ClonePool[number].Instance.transform.position, ClonePool[number].DeathLocation, Time.deltaTime);
    }

    void CheckCloneCycle(int number)
    {
        float distanceTillDead = (ClonePool[number].Instance.transform.position - ClonePool[number].DeathLocation).sqrMagnitude;

        //Debug.Log(distanceTillDead + number);

        if(distanceTillDead < 5)
        {
            ClonePool[number].Instance.SetActive(false);
            ClonePool[number].isDone = true;
            ClonePool[number].isSummoned = false;
            ClonePool[number].materializeTimer = 0;
            ClonePool[number].Instance.transform.position = ClonePool[number].SpawnLocation;
            ClonePool[number].betweenSpawnTimer = 0;
        }
    }

    void BetweenCycleTimer(int number)
    {
        if (ClonePool[number].isDone == true)
        {
            ClonePool[number].betweenSpawnTimer += Time.deltaTime;

            if(ClonePool[number].betweenSpawnTimer >= TimeBetweenLoops)
            {
                ClonePool[number].isDone = false;
                
                ClonePool[number].Instance.SetActive(true);
            }
        }
            

        
    }
}
