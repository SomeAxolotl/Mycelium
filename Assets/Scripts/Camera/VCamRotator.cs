using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VCamRotator : MonoBehaviour
{
    private GameObject currentPlayer;
    private GameObject camTracker;
    private GameObject levelGoal;

    [SerializeField] private CinemachineBrain cineBrain;
    [SerializeField] private CinemachineVirtualCamera virtualResetCamera;
    [SerializeField] private CinemachineVirtualCamera virtualNavigationCamera;

    void Start()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer");
        camTracker = GameObject.FindWithTag("Camtracker");

        //Finding the Goal of the level
        switch(SceneManager.GetActiveScene().name)
        {
            case "The Carcass":
                levelGoal = GameObject.Find("==StartRun==");
                break;

            case "Daybreak Arboretum":
                levelGoal = GameObject.Find("BackToHub");
                break;

            case "Delta Crag":
                levelGoal = GameObject.Find("BackToHub");
                break;

            case "Impact Barrens":
                levelGoal = GameObject.Find("Rival Colony Leader");
                break;

            default:
                levelGoal = null;
                break;
        }
    }

    void Update()
    {
        transform.position = camTracker.transform.position;
    }

    private void OnResetCamera()
    {
        if (currentPlayer.tag != "currentPlayer")
        {
            currentPlayer = GameObject.FindWithTag("currentPlayer");
        }
        transform.eulerAngles = new Vector3(0, currentPlayer.transform.eulerAngles.y, 0);

        if (cineBrain.IsBlending == false)
        {
            StartCoroutine(SetCameraPosition(virtualResetCamera));
        }
    }

    private void OnNavigateCamera()
    {
        if (levelGoal == null)
        {
            return;
        }

        Vector3 viewDir = (levelGoal.transform.position - transform.position).normalized;
        Quaternion newRotation = Quaternion.LookRotation(viewDir, Vector3.up);
        newRotation.eulerAngles = new Vector3(0, newRotation.eulerAngles.y, 0);

        transform.rotation = newRotation;

        if (cineBrain.IsBlending == false)
        {
            StartCoroutine(SetCameraPosition(virtualNavigationCamera));
        }
    }

    IEnumerator SetCameraPosition(CinemachineVirtualCamera vCam)
    {
        float elapsedTime = 0f;

        float oldBlendTime = cineBrain.m_DefaultBlend.m_Time;
        float newBlendTime = 0.1f;

        vCam.enabled = true;
        cineBrain.m_DefaultBlend.m_Time = newBlendTime;

        while (elapsedTime < cineBrain.m_DefaultBlend.m_Time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vCam.enabled = false;
        yield return new WaitForSeconds(newBlendTime);
        cineBrain.m_DefaultBlend.m_Time = oldBlendTime;
    }
}
