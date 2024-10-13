using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //control movement
    public Rigidbody rb;

    //control speed
    public float speed = 15f;

    //check if ball is moving and direction of movement
    private bool isTraveling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 1000; //tutorial set to 500
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;





    private void FixedUpdate()
    {
        if (isTraveling)
        {
            rb.velocity = speed * travelDirection;
        }

        if(nextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, nextCollisionPosition )< 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        //control swipe mechanism code if code is not travelling
        if (isTraveling)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                //if swipe was  up or down
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    //go up/down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward: Vector3.back);

                }

                //if swipe was  up or down
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    //go left/right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);

                }
            }

            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //touch is done
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }


    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;


        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point; // direction where we hit something. //stored in the next collision position

        }

        isTraveling = true;
    }

}
