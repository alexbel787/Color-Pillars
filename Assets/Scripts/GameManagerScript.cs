using System.Collections;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public bool randomFixersStartColors;
    [SerializeField] private float nextRoundDelay;
    public bool disableInput;
    public FixerScript[] FSs;
    private int currentFixer = 1;
    public PillarScript[] PSs;
    public Material[] meterials;
    private string[] materialNames = { "Red", "Green", "Blue"};
    [HideInInspector] public AudioSource AS;
    public AudioClip[] soundClips;
    [SerializeField] private GameObject nextRoundTextObj;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
        SetRandomPillarColor();
        if (randomFixersStartColors) SetRandomFixersColors();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !disableInput)
        {
            FSs[currentFixer - 1].isActive = false;
            FSs[currentFixer - 1].DisableFixer();
            currentFixer++;
            if (currentFixer > FSs.Length) currentFixer = 1;
            FSs[currentFixer - 1].GetComponent<MeshRenderer>().material = meterials[3];
            FSs[currentFixer - 1].enabled = true;
            FSs[currentFixer - 1].isActive = true;
        }
    }

    private void SetRandomPillarColor()
    {
        foreach (PillarScript ps in PSs)
        {
            ps.pillarColor = "None";
            //ps.tag = "Untagged";
        }
        int rnd = Random.Range(0, PSs.Length);
        var rndPillar = PSs[rnd];
        rndPillar.pillarColor = materialNames[rnd];
        rndPillar.GetComponent<MeshRenderer>().material = meterials[rnd];
        //rndPillar.tag = "ActivePillar";
    }

    private void SetRandomFixersColors()
    {
        string[] canFixColors = (string[])materialNames.Clone();
        int rnd = Random.Range(0, canFixColors.Length);
        FSs[0].canFixColor = (FixerScript.Colors)rnd;
        canFixColors[rnd] = "";
        while (true)
        {
            rnd = Random.Range(0, canFixColors.Length);
            if (canFixColors[rnd] != "")
            {
                FSs[1].canFixColor = (FixerScript.Colors)rnd;
                break;
            }
        }
        foreach (FixerScript fs in FSs) fs.SetFlagsColors();
    }



    public IEnumerator ChangeColorCoroutine()
    {
        disableInput = true;
        AS.clip = soundClips[0];
        AS.Play();
        foreach (FixerScript fs in FSs) fs.transform.position = fs.startPos;
        nextRoundTextObj.SetActive(true);
        yield return new WaitForSeconds(nextRoundDelay);
        SetRandomPillarColor();
        SetRandomFixersColors();
        FSs[0].GetComponent<MeshRenderer>().material = meterials[3];
        FSs[0].enabled = true;
        FSs[0].isActive = true;
        nextRoundTextObj.SetActive(false);
        disableInput = false;
    }

}
