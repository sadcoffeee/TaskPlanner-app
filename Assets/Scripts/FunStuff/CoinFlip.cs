using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CoinFlip : MonoBehaviour
{
    public GameObject coinFlipObject;
    public Rigidbody coinRigidbody;
    public float forceMultiplier = 5f; // Scale for swipe force
    public float torqueMultiplier = 10f; // Scale for spin torque
    public GameObject UIComponents;
    bool timeLock;

    private Vector2 touchStart, touchEnd;

    void Update()
    {
        if (!timeLock)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEnd = touch.position;
                    ProcessSwipe();
                }
            }
        }
    }
    private void ProcessSwipe()
    {
        Vector2 swipeDirection = touchEnd - touchStart;
        float swipeMagnitude = swipeDirection.magnitude;

        if (swipeMagnitude > 50f) // Minimum swipe distance threshold
        {
            Vector3 force = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(3.0f, 5.0f), Random.Range(0.5f, 3.0f)).normalized * forceMultiplier;
            Vector3 torque = new Vector3(Random.Range(0.8f, 1.2f), 0, 0) * torqueMultiplier;

            coinRigidbody.AddForce(force, ForceMode.Impulse);
            coinRigidbody.AddTorque(torque, ForceMode.Impulse);
            coinRigidbody.useGravity = true;
            StartCoroutine(flippingCoroutine());
        }
    }
    public void resetCoin()
    {
        UIComponents.SetActive(false);
        coinRigidbody.useGravity = false;
        transform.position = new Vector3(0, -0.1f, -9.5f);
        transform.rotation = Quaternion.Euler(-20, 0, 0);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.transform.rotation = Quaternion.identity;
        touchStart = Vector3.zero;
        touchEnd = Vector3.zero;
        timeLock = false;
    }
    IEnumerator flippingCoroutine()
    {
        timeLock = true;
        float timer = 0;
        while (timer < 0.5)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // Wait until the coin's velocity is almost zero
        while (coinRigidbody.linearVelocity.magnitude > 0.01f || coinRigidbody.angularVelocity.magnitude > 0.01f)
        {
            Debug.Log("Velocity: " + coinRigidbody.linearVelocity + " | Angular: " + coinRigidbody.angularVelocity);
            Camera.main.transform.LookAt(transform);
            yield return null; // Wait for the next frame
        }
        Debug.Log("We're stationary :D");

        // Once the coin is stationary, adjust the camera
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Camera.main.transform.LookAt(transform);

        // Determine which side is facing up
        bool isHeads = transform.up.y > 0;

        // Update the UI
        if (UIComponents != null)
        {
            UIComponents.SetActive(true);
            UIComponents.GetComponentInChildren<TextMeshProUGUI>().text = isHeads ? "Heads" : "Tails";
        }
        else Debug.Log("UI Components not found");

    }

}