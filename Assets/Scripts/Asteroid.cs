using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : MonoBehaviour
{

    [SerializeField] float speed = 0.2f;
    [SerializeField] int random;

    [SerializeField] private GameObject blueprint;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        // Randomize the Trajectory of the Asteroid
        switch(random)
        {
            case 0:
                transform.position = transform.position + Vector3.up * speed * Time.deltaTime;  // Moves straight up
                break;
            case 1:
                transform.position = transform.position + Vector3.right * speed * Time.deltaTime;   // Moves to the right
                break;
            case 2:
                transform.position = transform.position + Vector3.left * speed * Time.deltaTime;    // Moves to the left
                break;
        }

        transform.position = transform.position + Vector3.up * speed * Time.deltaTime;  // Adds a Movement upwards so the Asteroids moves top-right and top-left in Case 1/2
    }

    public void destroyInstance()
    {
        Destroy(this.gameObject);
        spawnInstance();
    }

    public void spawnInstance()
    {
        Instantiate(blueprint, this.gameObject.transform.position, Quaternion.identity);
    }
}
