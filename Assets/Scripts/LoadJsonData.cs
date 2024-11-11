using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this for UI components like Button
using TMPro; // Include this for TextMeshPro
using System.IO;

public class LoadJsonData : MonoBehaviour
{
    public TextMeshProUGUI titleText; // Using TextMeshProUGUI for title text
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public TextMeshProUGUI descriptionText; // Using TextMeshProUGUI for description
    public Button playPauseButton; // Play/Pause button for ball spawning
    private List<WorkoutInfo> workouts;
    private BallSpawner ballSpawner; // Reference to the BallSpawner
    private WorkoutInfo currentWorkout; // Store the current selected workout

    private void Start()
    {
        // Load the JSON file
       string jsonPath = Path.Combine(Application.streamingAssetsPath, "workoutData.json");
string jsonString = File.ReadAllText(jsonPath);
Debug.Log("Loaded JSON: " + jsonString); // Log the JSON content
WorkoutData data = JsonUtility.FromJson<WorkoutData>(jsonString);
Debug.Log("Project Name: " + data.ProjectName); // Log the Project Name


        // Set the title from the JSON
        titleText.text = data.ProjectName;
        workouts = data.workoutInfo;

        // Setup Ball Spawner
        ballSpawner = GetComponent<BallSpawner>();

        // Generate buttons dynamically based on the workout info
        foreach (var workout in workouts)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = workout.workoutName;
            button.GetComponent<Button>().onClick.AddListener(() => ShowWorkoutDetails(workout));
        }

        // Set initial description
        descriptionText.text = "Workout Description here";

        // Setup play/pause button
        playPauseButton.onClick.AddListener(() => TogglePlayPause());
    }

    private void ShowWorkoutDetails(WorkoutInfo workout)
    {
        // Display the description for the selected workout
        descriptionText.text = workout.description;

        // Set the current workout
        currentWorkout = workout;

        // Pass workout details to the BallSpawner
        ballSpawner.SetWorkoutDetails(workout.workoutDetails);
    }

    private void TogglePlayPause()
    {
        if (ballSpawner != null)
        {
            ballSpawner.TogglePlayPause(); // Call the method in BallSpawner to handle play/pause
        }
    }
}

// JSON Data Structures
[System.Serializable]
public class WorkoutData
{
    public string ProjectName;
    public List<WorkoutInfo> workoutInfo;
}

[System.Serializable]
public class WorkoutInfo
{
    public int workoutID;
    public string workoutName;
    public string description;
    public string ballType;
    public List<WorkoutDetail> workoutDetails;
}

[System.Serializable]
public class WorkoutDetail
{
    public int ballId;
    public float speed;
    public float ballDirection;
}
