using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{

    [SerializeField] float speed = 0.4f;
    [SerializeField] float rotation_cushion = 4f;
    [SerializeField] Camera cam;

    private bool isWandering = false;
    private bool isMovingUp = false;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    // Update is called once per frame
    void Update()
    {
        //UFO Looks at the AR-Camera
        var rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotation_cushion);


        if(isWandering == false) { StartCoroutine(randomizeMovement()); }   //Starts Routine for randomized Movement
        if(isMovingRight == true) { transform.position = transform.position + Vector3.right * speed * Time.deltaTime; }   // Moves to the right
        if(isMovingLeft == true) { transform.position = transform.position + Vector3.left * speed * Time.deltaTime; }   // Moves to the left
        if(isMovingUp == true) { transform.position = transform.position + Vector3.up * speed * Time.deltaTime; }   // Moves straight up
    }

    IEnumerator randomizeMovement()
    {
        //Random Variables for the Movement
        int movWait = Random.Range(0, 1);
        int movTime = Random.Range(0, 2);
        int movSide = Random.Range(1, 3);
        int movRight = Random.Range(0, 2);
        int movLeft = Random.Range(0, 2);

        isWandering = true;

        yield return new WaitForSeconds(movWait);

        isMovingUp = true;

        yield return new WaitForSeconds(movTime);

        isMovingUp = false;

        if(movSide == 1)
        {
            isMovingRight = true;
            yield return new WaitForSeconds(movRight);
            isMovingRight = false;
        }

        if(movSide == 2)
        {
            isMovingLeft = true;
            yield return new WaitForSeconds(movLeft);
            isMovingLeft = false;
        }

        isWandering = false;
    }

    public void destroyInstance()
    {
        Destroy(this.gameObject);
    }
}
