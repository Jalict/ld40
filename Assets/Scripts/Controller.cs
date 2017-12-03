using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Inspector
    [Header("Settings")]
    public bool isKeyboard;
    public float yawSpeed;

    [Header("Other")]
    public Airplane[] airplanes;
    public int selectedAirplaneIndex = 0;

    // Public (Hidden)

    // Private
    private Vector3 steer;

    void Start()
    {
        steer = Vector3.zero;

        airplanes[selectedAirplaneIndex].isControlled = true;
    }

    void Update()
    {
        // Controller Switch
        int previousSelectedAirplaneIndex = selectedAirplaneIndex;
        if (Input.GetKeyDown(KeyCode.Q))
            selectedAirplaneIndex--;
        if (Input.GetKeyDown(KeyCode.E))
            selectedAirplaneIndex++;

        if (selectedAirplaneIndex >= airplanes.Length) selectedAirplaneIndex = 0;
        if (selectedAirplaneIndex < 0) selectedAirplaneIndex = airplanes.Length - 1;

        // Camera
        if (previousSelectedAirplaneIndex != selectedAirplaneIndex)
            ActivateAirplane(previousSelectedAirplaneIndex);

        // Steering
        float yawAxis = -Input.GetAxis("Horizontal");
        steer.x = yawAxis * yawSpeed;

        airplanes[selectedAirplaneIndex].Steer(steer);

    }

    void ActivateAirplane(int previousIndex)
    {
        airplanes[selectedAirplaneIndex].isControlled = true;
        airplanes[selectedAirplaneIndex].transform.Find("Main Camera").gameObject.SetActive(true);

        airplanes[previousIndex].isControlled = false;
        airplanes[previousIndex].transform.Find("Main Camera").gameObject.SetActive(false);
    }
}
