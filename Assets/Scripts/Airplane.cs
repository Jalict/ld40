using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    public float speed;

    private Transform sprite;
    private Fuelmeter fuelmeter;

    private Rigidbody2D body;

    // Idle
    public bool isControllable;
    public bool isControlled;
    public float randomIdleSteer;

	// Use this for initialization
	void Start () {
        sprite = transform.Find("Airplane Sprite");
        fuelmeter = transform.Find("Fuelmeter").GetComponent<Fuelmeter>();

        body = GetComponent<Rigidbody2D>();

        randomIdleSteer = Random.Range(0.1f, 0.3f) * (Random.value >= 0.5f ? -1 : 1);

        isControllable = false;
        isControlled = true;
	}
	
	// Update is called once per frame
	void Update () {
        body.velocity = sprite.up * speed * Time.deltaTime;

        if (!isControlled)
            Idle();

        if(fuelmeter.fuel <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void Steer(Vector3 vec)
    {
        sprite.Rotate(sprite.forward * vec.x);
    }

    private void Idle()
    {
        Steer(new Vector3(randomIdleSteer, 0, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Runway")
        {
            isControllable = true;
            isControlled = false;
        }
    }
}
