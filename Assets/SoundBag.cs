using UnityEngine;
using System.Collections;




public class SoundBag : MonoBehaviour {


    public AudioSource[] Grunts;

    public AudioSource Winner;
    public AudioSource Loser;
    public AudioSource Points;
    public AudioSource Intro;

    public AudioSource[] Death;

    void Start()
    {
        //Intro.Play();
    }

    void Grunt ()
    {
        int rando = Random.Range(0, Grunts.Length - 1);
        if (Grunts[rando].isPlaying == false)
        {
            Grunts[rando].Play();
        }
       

	}

	void Win()
    {
        Winner.Play();

	}

    void Loss()
    {
        Loser.Play();
    }

    void Dying()
    {
        int rando = Random.Range(0, Death.Length - 1);
        if (Death[rando].isPlaying == false)
        {
            Death[rando].Play();
        }

    }
}
