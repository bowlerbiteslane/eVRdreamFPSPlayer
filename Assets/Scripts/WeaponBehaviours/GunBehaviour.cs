using UnityEngine;
using System.Collections;

public class GunBehaviour : WeaponBehaviour
{



    [SerializeField]
    private int clipSize = 10;
    [SerializeField]
    private int maxSpareClips = 1;
    [SerializeField]
    private int currentSpareClips = 1;
    [SerializeField]
    private int ammoInCurrentClip = 10;
    [SerializeField]
    private float gunFlashLength = 0.1f;

    public float reloadTime = 5f;

    public bool DEBUGMODE;

    public AudioClip dryFireClip = null;
    private AudioSource gunshotAudio = null;
    private AudioClip fireClip = null;
    private bool pointLightOn = false;
    private bool reloading = false;
    private float timeCnt;
    private float rldTimeCnt;

    private Light pointLight;

    private string debugString;


    public int AmmoInClip
    {
        get { return ammoInCurrentClip; }
    }

    public int CurrentClips
    {
        get { return currentSpareClips; }
    }

    public int ClipSize
    {
        get { return clipSize; }
    }

    public int MaxClips
    {
        get { return maxSpareClips; }
    }

    public int TotalAmmo
    {
        get { return ammoInCurrentClip + (currentSpareClips * clipSize);  }
    }


    // Use this for initialization
    void Awake()
    {
        ammoInCurrentClip = (int)Mathf.Clamp((float)ammoInCurrentClip, 0f, (float)clipSize);
        currentSpareClips = (int)Mathf.Clamp((float)currentSpareClips, 0f, (float)maxSpareClips);
        timeCnt = 0f;
        rldTimeCnt = 0f;
        pointLight = GetComponentInChildren<Light>();
        gunshotAudio = gameObject.GetComponent<AudioSource>();
        fireClip = gunshotAudio.clip;
        if (pointLight == null)
        {
            Debug.Log("Cannot find reference to Point Light. Script Disabled");
            this.enabled = false;
        }
        if (gunshotAudio == null)
        {
            Debug.Log("Cannot find reference to AudioSource. Script Disabled");
            this.enabled = false;
        }
        debugString = "[Ammo In Clip: " + ammoInCurrentClip + "] [Spare Clips: " + currentSpareClips + "] [Total Ammo: " + (ammoInCurrentClip + (clipSize * currentSpareClips) + "]");
    }

    // Update is called once per frame
    void Update()
    {
        timeCnt += Time.deltaTime;
        if (pointLightOn && timeCnt > gunFlashLength)
        {
            //disable point light
            pointLightOn = false;
            pointLight.enabled = false;
            timeCnt = 0f;
        }
        rldTimeCnt += Time.deltaTime;
        if (reloading && rldTimeCnt > reloadTime)
        {
            // reload
            Reload();
            reloading = false;
            rldTimeCnt = 0f;
            debugString = "Reloaded.";
        }
        if(DEBUGMODE)
            Debug.Log(debugString);
    }


    public override bool TriggerAttack()
    {
        // update Ammo
        if (ammoInCurrentClip > 0)
        {
            ammoInCurrentClip--;
            debugString = "[Ammo In Clip: " + ammoInCurrentClip + "] [Spare Clips: " + currentSpareClips + "] [Total Ammo: " + (ammoInCurrentClip + (clipSize * currentSpareClips)+"]");
            // other gun effects
            pointLightOn = true;
            pointLight.enabled = true;
            timeCnt = 0f;
            gunshotAudio.Play();
            return true;
        }
        else
        {
            if (TotalAmmo > 0)
            {
                if (!reloading)
                    rldTimeCnt = 0f;
                reloading = true;
                debugString = "Reloading...";
            }
            else
            {
                DryShot();
            }
            return false;
        }
    }


    public void Reload()
    {
        if (currentSpareClips > 0)
        {
            currentSpareClips--;
            ammoInCurrentClip = clipSize;
        }
        else
        {
            debugString = "There are no spare clips to reload.";
        }
    }

    private void DryShot()
    {
        debugString = "No clips left.";
        gunshotAudio.clip = dryFireClip;
        gunshotAudio.Play();
    }


    public void PickUpAmmoClip()
    {
        if (currentSpareClips < maxSpareClips)
        {
            currentSpareClips++;
            debugString = "Ammo Picked Up. Now have " + currentSpareClips + " spare clips.";
            gunshotAudio.clip = fireClip;
        }
        else
        {
            debugString = "Already at max clip capacity.";
        }
    }



}
