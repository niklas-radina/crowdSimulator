using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    public GameObject[] exitLocations;
    public GameObject[] roamingLocations;
    public Material evacuatingMaterial;
    public Material panicMaterial;
    public Material calmMaterial;
    public GameObject agentPrefab;

    private void Start()
    {
        roamingLocations = GameObject.FindGameObjectsWithTag("roamingTarget");
        exitLocations = GameObject.FindGameObjectsWithTag("exit");
    }
}