using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class PauseOnSteamOverlay : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    protected Callback<GameOverlayActivated_t> cb_gameOverlayActivated;

    private PauseMenu pauseMenuScript;
    private bool overlayIsActive = false;

    private void OnEnable()
    {
        pauseMenuScript = GetComponent<PauseMenu>();

        if (SteamManager.Initialized == true)
        {
            cb_gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }
    }

    private void OnDisable()
    {
        if (SteamManager.Initialized == true)
        {
            if (cb_gameOverlayActivated != null) { cb_gameOverlayActivated.Dispose(); }
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t cb)
    {
        if (cb.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
            overlayIsActive = true;

            StartCoroutine(AttemptToPause());
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
            overlayIsActive = false;

            StopAllCoroutines();
        }
    }

    private IEnumerator AttemptToPause()
    {
        while(overlayIsActive == true)
        {
            if(GlobalData.isGamePaused == false)
            {
                pauseMenuScript.OnPause();
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
#endif
}
