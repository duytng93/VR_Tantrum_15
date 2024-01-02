
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tatrumchildbehavior : MonoBehaviour
{
    public ChildState childState;
    private UserPrefs userPrefs;
    #region Objects
    Animator animator;
    AudioSource audioSource;
    private GameObject mainCamera;
    #endregion
     
    public int tantrumLevel;     // simplified version of tantrumCoefficient only has 6 levels
    string move;    // to hold the child behavior(cry, stamp, walk etc...) at any moment  
    private float breakTimer; // timer for 9s calm between tantrum behaviors

    #region Audio Clips
    public AudioClip startVoice;
    public AudioClip startVoiceSpanish;
    public AudioClip level2Cry1;
    public AudioClip level2Cry2;
    public AudioClip level2Cry3;
    public AudioClip level3Cry1;
    public AudioClip level3Cry2;
    public AudioClip level3Cry3;
    public AudioClip level4Cry1;
    public AudioClip level4Cry2;
    public AudioClip level4Cry3;
    public AudioClip level5Cry1;
    public AudioClip level5Cry2;
    public AudioClip level5Cry3;

    public AudioClip Butwhy;
    public AudioClip Ireallywanttoplay;
    public AudioClip AllIwanttodoisplaywiththetoys;
    public AudioClip Youaretheworst;
    public AudioClip Idontwanttobehereanymore;
    public AudioClip IfIcantplayIamleaving;
    public AudioClip Youbettergivemethetoysorelse;

    public AudioClip spanish_butwhy;
    public AudioClip spanish_ireallywanttoplay;
    public AudioClip spanish_allIwanttodoisplaywiththetoys;
    public AudioClip spanish_youaretheworst;
    public AudioClip spanish_idontwanttobehereanymore;
    public AudioClip spanish_ificantplayiamleaving;
    public AudioClip spanish_youbettergivethetoyorelse;

    #endregion


    #region static varibles
    public static bool childIsTalking = false; // used in other scripts to check if the child is showing bad behaviors
    public static bool simluationOnGoing; //used in other scripts to check if the simulation is ongoing
    public static bool negativeStatementSelected; //used in other scripts. when a negative statement is selected, this flag will be true and
                                                  // the child keep showing bad behaviors with out calm break

    public static bool gameLost; //used in simulationcontroller script. when gameLost is true, child audio source is mute so that simulationController can play ending audio
    #endregion

    #region variables related to walk and where to walk
    private bool isWalkingOrRunning;
    public List<GameObject> spots;
    private GameObject nextSpot;
    private bool walkedOrRan;
    private bool isIdle;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        mainCamera = GameObject.Find("Main Camera");

        tantrumLevel = 0;

        nextSpot = spots[0];
        isWalkingOrRunning = false;
        walkedOrRan = false;

        if (userPrefs.IsEnglishSpeaker())
            PlayAudioClip(startVoice);
        else
            PlayAudioClip(startVoiceSpanish);

        move = "firstTalk";
        StartCoroutine(setMoveatStartVoice());
        StartCoroutine(setStartTantrum());
        simluationOnGoing = false;
        gameLost = false;
        isIdle = false;
    }

    

    void Update()
    {
        
        if (gameLost)
        {
            audioSource.volume = 0f; // mute this to play ending kid voice in simulationcontrol script
        }
        else
            audioSource.volume = 1f;
        
        //if the child stop saying. start counting the time
        if (!audioSource.isPlaying)
        {
            breakTimer += Time.deltaTime;
            if (!isWalkingOrRunning && !walkedOrRan)
            {
                // randomly let the child walk or run or not
                int randValue = Random.Range(0, 5);
                if (randValue == 1 || randValue == 0)
                {
                    StartCoroutine(addWalkOrRuntoMove("walk"));
                }
                else if(randValue == 2 || randValue == 3)
                {
                    StartCoroutine(addWalkOrRuntoMove("run"));
                }
                else walkedOrRan = true; // skip walking this time
            }
        }

        childIsTalking = audioSource.isPlaying; // this is a static variable to use in other script

        // if the time reach 9 seconds. the child talk again
        if (!audioSource.isPlaying && breakTimer > 9f || !audioSource.isPlaying && negativeStatementSelected && !isWalkingOrRunning)
        {
            Debug.Log("about to update movement");
            UpdateMovement();
            walkedOrRan = false; // reset walkedOrRan
            breakTimer = 0f; //reset
            isIdle = false;
            negativeStatementSelected = false;
        }
        else if (!audioSource.isPlaying && childState.tantrumLevel > 0 && !isWalkingOrRunning)
        {
            if (!isIdle)
            {
                if (tantrumLevel < 4)
                {
                    if (Random.Range(0, 2) == 1)
                        move = "SadIdle";
                    else move = "embarrased";
                }
                else
                {
                    if (Random.Range(0, 2) == 1)
                        move = "idleDeepBreath";
                    else move = "SadIdle";
                }
                isIdle = true;
            }
        }
        else if (!audioSource.isPlaying && childState.tantrumLevel == 0)
        {
            move ="idle" ;
            
            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 pointBelowCamera = new Vector3(cameraPosition.x, spots[0].transform.position.y, cameraPosition.z);
            Quaternion lookRotation = Quaternion.LookRotation((pointBelowCamera - transform.position).normalized);
            //over time
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }

        // Convert tantrum coefficient to tantrum level
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);    // tantrum coefficient is 0-100 and we want 6 levels 0-5
                                                                  

        if (move==("SadIdle") && !animator.IsInTransition(0))
        {
            animator.CrossFade("sad", 0.1f);

        }

        if (move == ("embarrased") && !animator.IsInTransition(0))
        {
            animator.CrossFade("idleEmbarrased", 0.1f);

        }

        if (move == ("idleDeepBreath") && !animator.IsInTransition(0))
        {
            animator.CrossFade("idleDeepBreath", 0.1f);

        }

        //
        if (move==("sad2") && !animator.IsInTransition(0))
        {
            animator.CrossFade("sad2", 0.1f);
        }

        //
        if (move==("idle") && !animator.IsInTransition(0))
        {
            animator.CrossFade("idle", 0.1f);
        }

        //
        if (move==("cry1") && !animator.IsInTransition(0))
        {
            animator.CrossFade("cry1", 0.1f);

        }

        //
        if (move==("cry2") && !animator.IsInTransition(0))
        {
            animator.CrossFade("cry2", 0.1f);
        }

        //
        if (move==("stamp") && !animator.IsInTransition(0))
        {
            animator.CrossFade("stamp", 0.1f);
        }

        //
        if (move==("telloff") && !animator.IsInTransition(0))
        {
            animator.CrossFade("telloff", 0.05f);
        }

        //
        else if (move==("shorttalk") && !animator.IsInTransition(0))
        {
            animator.CrossFade("shorttalk", 0.05f);
        }

        //
        if (move == ("firstTalk") && !animator.IsInTransition(0)) {
            animator.CrossFade("firstTalk", 0.05f);
        }

        if (move == "sadwalk")
        {
            animator.Play("sadWalk");
            walkOrRun("walk");
        }

        if (move == "sadrun") {
            animator.Play("sadRun");
            walkOrRun("run");
        }

    }

    IEnumerator addWalkOrRuntoMove(string type)
    {

        isWalkingOrRunning = true;
        move = "SadIdle"; // switch tantrum animation to sad idle animation first because the kid will be idle for a second before he start to walk
        yield return new WaitForSeconds(1f);
        if (type == "walk")
            move = "sadwalk";
        else if (type == "run")
            move = "sadrun";
    }

    void walkOrRun(string type)
    {
        Vector3 direction = nextSpot.transform.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (transform.position != nextSpot.transform.position) {
            if(type == "walk")
                transform.position = Vector3.MoveTowards(transform.position, nextSpot.transform.position, 0.8f * Time.deltaTime);
            else transform.position = Vector3.MoveTowards(transform.position, nextSpot.transform.position, 1.3f * Time.deltaTime);
        }
        else
        {
            walkedOrRan = true;
            isWalkingOrRunning = false;
            int curIndex = spots.IndexOf(nextSpot);
            int nextIndex = Random.Range(0, spots.Count);
            while (curIndex == nextIndex) { nextIndex = Random.Range(0, spots.Count); }
            nextSpot = spots[nextIndex];
        }
    }


    void UpdateMovement()
    {

        if (tantrumLevel == 0)
        {

            int moveType = Random.Range(0, 2);
            switch (moveType)
            {
                case 0:
                    move = "idle";
                    break;
                case 1:
                    move = "idle";
                    break;
            }
        }
        else if (tantrumLevel == 1)
        {
            int moveType = Random.Range(0, 5);
            AudioClip clip;
            switch (moveType)
            {
                case 0:
                    move = "cry1" ;
                    PlayAudioClip(level2Cry3);
                    break;
                case 1:
                    move =  "shorttalk";
                    clip = userPrefs.IsEnglishSpeaker() ? Ireallywanttoplay : spanish_ireallywanttoplay;
                    PlayAudioClip(clip);
                    break;
                case 2:
                    move =  "shorttalk";
                    clip = userPrefs.IsEnglishSpeaker() ? Butwhy : spanish_butwhy;
                    PlayAudioClip(clip);
                    break;
                case 3:
                    move = "shorttalk" ;
                    clip = userPrefs.IsEnglishSpeaker() ? AllIwanttodoisplaywiththetoys : spanish_allIwanttodoisplaywiththetoys;
                    PlayAudioClip(clip);
                    break;
                case 4:
                    move =  "cry1";
                    PlayAudioClip(level2Cry1);
                    break;
            }
        }
        else if (tantrumLevel == 2)
        {
            int moveType = Random.Range(0, 3);
            switch (moveType)
            {
                case 0:
                    move = "cry1";
                    PlayAudioClip(level2Cry1);
                    break;
                case 1:
                    move = "cry2";
                    PlayAudioClip(level2Cry2);
                    break;
                case 2:
                    move = "cry1";
                    PlayAudioClip(level2Cry3);
                    break;

            }
        }
        else if (tantrumLevel == 3)
        {
            int moveType = Random.Range(0, 4);
            switch (moveType)
            {
                case 0:
                    move = "stamp";
                    PlayAudioClip(level3Cry1);
                    break;
                case 1:
                    move =  "stamp";
                    PlayAudioClip(level3Cry2);
                    break;
                case 2:
                    move = "stamp";
                    PlayAudioClip(level3Cry3);
                    break;
                case 3:
                    move =  "cry2";
                    PlayAudioClip(level2Cry2);
                    break;

            }

        }
        else if (tantrumLevel == 4)
        {

            int moveType = Random.Range(0, 7);
            AudioClip clip;
            switch (moveType)
            {
                case 0:  //"WandC"
                    move = "cry2";
                    PlayAudioClip(level4Cry1);
                    break;
                case 1:
                    move = "stamp";
                    PlayAudioClip(level4Cry2);
                    break;
                case 2:
                    move = "stamp";
                    PlayAudioClip(level4Cry3);
                    break;
                case 3:
                    move ="telloff";
                    clip = userPrefs.IsEnglishSpeaker() ? Youaretheworst : spanish_youaretheworst;
                    PlayAudioClip(clip);
                    break;
                case 4:
                    move = "telloff";
                    clip = userPrefs.IsEnglishSpeaker() ? Idontwanttobehereanymore : spanish_idontwanttobehereanymore;
                    PlayAudioClip(clip);
                    break;
                case 5:
                    move = "telloff";
                    clip = userPrefs.IsEnglishSpeaker() ? IfIcantplayIamleaving : spanish_ificantplayiamleaving;
                    PlayAudioClip(clip);
                    break;
                case 6:
                    move = "telloff";
                    clip = userPrefs.IsEnglishSpeaker() ? Youbettergivemethetoysorelse : spanish_youbettergivethetoyorelse;
                    PlayAudioClip(clip);
                    break;
            }
        }
        else if (tantrumLevel == 5)
        {
            int moveType = Random.Range(0, 3);
            switch (moveType)
            {
                case 0:
                    move = "cry2";
                    PlayAudioClip(level5Cry3);
                    break;
                case 1:
                    move = "stamp";
                    PlayAudioClip(level5Cry2);
                    break;
                case 2:
                    move = "stamp";
                    PlayAudioClip(level5Cry1);
                    break;

            }

        }

    }



    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    IEnumerator setStartTantrum()
    {
        if (userPrefs.IsEnglishSpeaker())
            yield return new WaitForSeconds(startVoice.length);
        else
            yield return new WaitForSeconds(startVoiceSpanish.length);
        childState.setStartTantrumLevel(30);
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
        UpdateMovement(); // first tantrum
        simluationOnGoing = true;

    }

    IEnumerator setMoveatStartVoice()
    {
        if (userPrefs.IsEnglishSpeaker())
        {
            yield return new WaitForSeconds(7.5f); // introduction talk
            move = "shorttalk";
            yield return new WaitForSeconds(1.5f); // Can I play with the toy
            move = "SadIdle";
            yield return new WaitForSeconds(5f); // adult talk
            move ="telloff";
            yield return new WaitForSeconds(1.5f); // I really want to play
            move =  "sad2";
        }
        else
        {
            move =  "shorttalk";
            yield return new WaitForSeconds(1.6f); // Can I play with the toy
            move = "SadIdle";
            yield return new WaitForSeconds(5.4f); // adult talk
            move = "telloff";
            yield return new WaitForSeconds(1.3f); // I really want to play
            move = "sad2";
        }

    }
}
