using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint; // Transform to specify the spawn location
    public Button playPauseButton;
    private bool isPlaying = false;
    private Coroutine currentCoroutine;
    private List<WorkoutDetail> currentWorkoutDetails;

    private void Start()
    {
        playPauseButton.onClick.AddListener(TogglePlayPause);
    }

    public void SetWorkoutDetails(List<WorkoutDetail> workoutDetails)
    {
        currentWorkoutDetails = workoutDetails;
        ResetSpawner();
    }

    private void ResetSpawner()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        isPlaying = false;
        playPauseButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Play"; // Using TMP for button text
    }

    public void TogglePlayPause()
    {
        if (isPlaying)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            playPauseButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Play"; // Using TMP for button text
        }
        else
        {
            if (currentWorkoutDetails != null)
            {
                currentCoroutine = StartCoroutine(SpawnBalls());
            }
            playPauseButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Pause"; // Using TMP for button text
        }
        isPlaying = !isPlaying;
    }

    private IEnumerator SpawnBalls()
    {
        // List to hold all spawned balls
        List<GameObject> spawnedBalls = new List<GameObject>();

        // Instantiate all balls at once
        foreach (var detail in currentWorkoutDetails)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity); // Spawn at spawnPoint
            ball.transform.position += new Vector3(detail.ballDirection, 0, 0);
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, detail.speed);
            spawnedBalls.Add(ball);
        }

        // Optionally, you can do something with spawnedBalls after all have been instantiated

        currentCoroutine = null;
        isPlaying = false;
        playPauseButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Play"; // Using TMP for button text

        yield return null; // Explicitly return null to satisfy IEnumerator
    }
}
