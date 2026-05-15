using UnityEngine;

public class DropFlightController : MonoBehaviour
{
    [Header("Flight Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public float flightSpeed = 50f;
    
    [Header("Player Settings")]
    public GameObject playerPrefab;
    private bool hasEjected = false;
    private float flightProgress = 0f;

    void Update()
    {
        if (startPoint == null || endPoint == null) return;

        // Move the plane from startPoint to endPoint across the map
        if (flightProgress < 1f)
        {
            flightProgress += (flightSpeed / Vector3.Distance(startPoint.position, endPoint.position)) * Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, flightProgress);
            transform.rotation = Quaternion.LookRotation(endPoint.position - startPoint.position);
        }
        else
        {
            // Auto-eject player if they reach the end of the map without jumping
            if (!hasEjected) EjectPlayer();
        }

        // Press 'F' to Jump/Eject from the plane
        if (Input.GetKeyDown(KeyCode.F) && !hasEjected)
        {
            EjectPlayer();
        }
    }

    void EjectPlayer()
    {
        hasEjected = true;
        Debug.Log("Player Ejected from the plane!");

        // Spawn player at the plane's current position
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        
        // Add the Parachute component to the spawned player dynamically
        PlayerParachute parachuteLogic = player.AddComponent<PlayerParachute>();
        
        // Disable regular player movement while free-falling
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false; 
    }
}
