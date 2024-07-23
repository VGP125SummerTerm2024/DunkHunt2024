using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))] //can switch to box if needed

public class DogAI : MonoBehaviour
{
    // add reference to where the score is saved/generated
    //add reference to ammo count/ end game


    //gameobj
    public GameObject dog; // for location/ movement

    //comps
    public SpriteRenderer sr;
    public Animator anim;

    //anims
    public AnimationClip Laugh;
    public AnimationClip Jump;
    public AnimationClip walk;
    public AnimationClip HoldDuck;

    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        // could make dog state 1,2,3 to determine or integrate to other scripts to update the functions below 
    }

    private void DogLaugh()
    {
        //anim plays when out of ammo/miss/endgame?
    }

    private void DogJump()
    {
        //moves from foreground to background layer
    }

    private void DogMove()
    {
        //walks out at start
        //move up/down holding duck when duck is hit after duck has hit the ground
    }
}
