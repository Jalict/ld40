using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    public GameObject explosion;

    public float speed;
    public float height = 1f;

    private Transform sprite;
    private Fuelmeter fuelmeter;
    private ParticleSystem particle;

    private Rigidbody2D body;

    public bool onRunway;

    // Idle
    public bool isControllable;
    public bool isControlled;
    public float randomIdleSteer;

    public bool airplaneDisabled = false;

    public bool foundAirport = false;

    public AudioClip landingClip;

	// Use this for initialization
	void Start () {
        sprite = transform.Find("Airplane Sprite");
        fuelmeter = transform.Find("Fuelmeter").GetComponent<Fuelmeter>();
        particle = sprite.transform.Find("Particle").GetComponent<ParticleSystem>();

        body = GetComponent<Rigidbody2D>();

        randomIdleSteer = Random.Range(0.3f, 0.6f) * (Random.value >= 0.5f ? -1 : 1);

        isControllable = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (airplaneDisabled) return;

        body.velocity = sprite.up * speed * Time.deltaTime;

        if (!isControlled && foundAirport)
            Idle();

        if (height <= 0.5f && !onRunway)
            Explosion();
        else if (height <= 0.5f && onRunway)
            Landed();

        if (fuelmeter.fuel <= 0)
        {
            Slowdown();
        }

	}

    void Landed()
    {
        airplaneDisabled = true;

        speed = 0f;
        Destroy(fuelmeter.gameObject);
        Destroy(body);
        Destroy(particle);

        AudioSource.PlayClipAtPoint(landingClip, transform.position, 0.5f);

        StartCoroutine(Score());
    }

    IEnumerator Score()
    {
        yield return null;
    }

    void Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        airplaneDisabled = true;
        speed = 0f;
        Destroy(fuelmeter.gameObject);
        Destroy(sprite.gameObject);
        Destroy(body);
    }

    public void Steer(Vector3 vec, bool dethrusting)
    {
        if (airplaneDisabled) return;

        if(!dethrusting)
            sprite.Rotate(sprite.forward * vec.x);
        else
        {
            Slowdown();
        }
    }

    public void Slowdown()
    {
        height -= Time.deltaTime * 0.5f;
        transform.localScale = Vector3.one * remap(height, 0f, 1f, 0.5f, 1f);
        speed -= Time.deltaTime * 1f;
    }

    public static float remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    private void Idle()
    {
        Steer(new Vector3(randomIdleSteer, 0, 0), false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Runway")
            onRunway = true;

        if (foundAirport) return;
        if (other.tag == "RunwaySphere") { 
            foundAirport = true;
            isControllable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Runway")
            onRunway = false;
    }
}
