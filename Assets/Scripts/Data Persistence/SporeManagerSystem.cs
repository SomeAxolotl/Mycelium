using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SporeManagerSystem : MonoBehaviour
{
    [Tooltip("What is saved to json. A custom class that is a list of sporeData objects")] protected SporeDataList sporeDataList;
    [Tooltip("Where we are saving our json.")] protected string filePath;

    protected SwapCharacter swapCharacterScript;
    protected SkillManager skillManagerScript;
    protected PlayerHealth playerHealthScript;
    protected PlayerController playerControllerScript;
    protected GameObject currentPlayerSpore;

    [SerializeField] protected List<Texture2D> MouthTextures;
    [SerializeField] protected List<Texture2D> EyeTextures;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        swapCharacterScript = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();
        skillManagerScript = GameObject.Find("PlayerParent").GetComponent<SkillManager>();
        playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        playerControllerScript = GameObject.Find("PlayerParent").GetComponent<PlayerController>();
        currentPlayerSpore = GameObject.Find("Spore");

        swapCharacterScript.characters.RemoveAll(item => item == null);
        swapCharacterScript.currentCharacterIndex = swapCharacterScript.characters.IndexOf(GameObject.FindWithTag("currentPlayer"));
    }

    protected IEnumerator StaggerSkillSets(SporeData sporeData, GameObject Spore)
    {
        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot0, 0, Spore);
        }
        catch
        {

        }

        yield return new WaitForSecondsRealtime(0.1f);

        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot1, 1, Spore);
        }
        catch
        {

        }

        yield return new WaitForSecondsRealtime(0.1f);

        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot2, 2, Spore);
        }
        catch
        {

        }
    }

    protected IEnumerator RunSporeSetup(SporeData sporeData, CharacterStats stats, DesignTracker design)
    {
        //Delay by one frame
        yield return null;

        stats.UpdateSporeName();
        stats.StartCalculateAttributes();
        design.UpdateBlendshape(sporeData.lvlSentience, sporeData.lvlPrimal, sporeData.lvlVitality, sporeData.lvlSpeed);
        design.UpdateColorsAndTexture();
        playerHealthScript.ResetHealth();
        playerControllerScript.GetStats();
        stats.GetComponent<Animator>().speed = stats.animatorSpeed;
    }
}
