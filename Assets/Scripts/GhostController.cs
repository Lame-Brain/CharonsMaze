using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    /*
     * Ghost moves toward player location until it is within <seekRange> units, then it drops a waypoint.
     * Ghost then moves toward waypoint at half speed, looking for the player.
     * If the ghost spots the player, it pauses for <aggression> frames.
     * If the player is still in LOS after <aggression> frames, it darts toward the player at <speed x2>
     * if the ghost reaches the waypoint, drops a random waypoint and moves toward that.
     * Once it is beyond <seekRange> of the first waypoint, it clears it, and resumes normal speed.
     * once it reaches the second waypoint, it clears it and then starts over.
    */
    public float speed, seekRange, aggression;
    private Vector3 target, waypoint1, waypoint2;
    public enum State { waiting, moving, searching, preparing, attacking, hunting, wandering};
    public State ghost, saved;
    private float playerX, playerZ, aggressonCounter;
    private GameObject player;
    public bool canSeePlayer, playerIsInFront;
    public float fov;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;

        if (ghost == State.waiting)
        { 
            ghost = State.moving;
            Debug.Log("IM GONNA GET CHA!");
        }

        if(ghost == State.moving || ghost == State.preparing) target = new Vector3(playerX, 0, playerZ); //if moving, set target to player

        if (ghost == State.moving && Vector3.Distance(transform.position, target) < seekRange)
        {
            ghost = State.searching;
            waypoint1 = new Vector3(playerX, 0, playerZ);
            Debug.Log("You are nearby");
        }

        if((ghost == State.searching || ghost == State.hunting) && transform.position == target)
        {
            Debug.Log("Moving on...");
            ghost = State.wandering;
            target = new Vector3(Random.Range(transform.position.x - 30, transform.position.x + 30), 0, Random.Range(transform.position.z - 30, transform.position.z + 30));
        }

        if (ghost == State.wandering && transform.position == target)
        {
            ghost = State.waiting;
        }

        if (ghost == State.searching) target = waypoint1;
    }

    public void FixedUpdate()
    {
        transform.LookAt(target); //point the ghost

        if (ghost != State.waiting && ghost != State.preparing) //doesn't move if he is waiting or preparing
        {
            float adjustedSpeed = speed;
            if (ghost == State.searching) adjustedSpeed = speed * 0.5f; //speed is different when searching and attacking
            if (ghost == State.attacking || ghost == State.hunting) adjustedSpeed = speed * 3;            
            transform.position = Vector3.MoveTowards(transform.position, target, adjustedSpeed * Time.deltaTime); //move the sucker
        }
        
        RaycastHit hit; //Test to see if the ghosty can see our crafty player
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, direction);
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.transform.tag == "Player") canSeePlayer = true;
            if (Mathf.Abs(angle) <= 45) playerIsInFront = true;
            if (hit.transform.tag == "Player" && Mathf.Abs(angle) <= fov) //YUP!
            {
                if (ghost != State.preparing && ghost != State.attacking) { saved = ghost; ghost = State.preparing; aggressonCounter = 0; } //First see me? go to preparing state
                if (ghost == State.preparing) { aggressonCounter++; waypoint1 = new Vector3(playerX, 0, playerZ); Debug.Log(aggressonCounter + " of " + aggression); if (aggressonCounter > aggression) { ghost = State.attacking; Debug.Log("I WILL END YOU"); } } //in preparing and can still see me? count up to aggression (then attack)
                if (ghost == State.attacking)
                {
                    //.PLAY SOUND
                    target = new Vector3(playerX, 0, playerZ);
                }
            }
            if (hit.transform.tag != "Player") canSeePlayer = false;
            if (Mathf.Abs(angle) > 45) playerIsInFront = false;
            if (hit.transform.tag != "Player" || Mathf.Abs(angle) > fov) //NOPE! Can't see ya!
            {
                if(ghost == State.preparing) { ghost = saved; Debug.Log("DAMN! LOST YOU!"); }
                if(ghost == State.attacking) //Lose the ghost
                {
                    //.PLAY SOUND
                    ghost = State.hunting;
                }
            }
        }
    }
}
