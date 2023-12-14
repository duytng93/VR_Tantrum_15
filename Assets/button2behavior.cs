using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class button2behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Youseemcalmrightnow;
    public AudioClip Youseemupsetaboutnotbeingabletoplay;
    public AudioClip Youarecryingbecauseyouarereallyupsetaboutnotplaying;
    public AudioClip Iamgoingtostayoverhereuntilyoucalmdown;
    public AudioClip Iamgoingtokeepawayfromyouuntilyoustartcalmingdown;

    public AudioClip Enestemomentoparecesestartranquilo;
    public AudioClip Parecesestarenojadopornopoderjugar;
    public AudioClip Estásllorandoporquetesientesenojadopornopoderjugar;
    //public AudioClip Manténgasetranquiloyfísicamentealejadodelniño;
    //public AudioClip Aléjesefísicamentedelniño;
    public AudioClip Mevoyaquedaraquíhastaquetecalmes;
    public AudioClip Voyamantenermealejadodetihastaquecomiencesacalmarte;
    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;

    private ChildState childState;
    public TextMeshProUGUI tmpText;

    public int previousTantrumLevel; // to keep track of change in tantrumLevel
    public float tantrumLevelInV2;
    public int tantrumLevel;
    public float amount;
    private bool atStart;
    private float introTimer;
    private float introLength;
    void Start()
    {
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();

        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

        switch (userPrefs.GetChildAvatar())
        {
            case Enums.ChildAvatars.Hispanic:
                childState = GameObject.Find("TKBoyA").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Black:
                childState = GameObject.Find("TKBoyB").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.White:
                childState = GameObject.Find("TKBoyC").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Asian:
                childState = GameObject.Find("TKBoyD").GetComponent<ChildController>().childState;
                break;
        }

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);
        UpdateTextAndAudioClip(tantrumLevel); // Initial setup of text and audio clip

        yourButton.onClick.AddListener(OnButtonClick);
        introTimer = 0f;
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoice.length : StartVoiceSpanish.length;
        atStart = true;
    }

    void Update()
    {
        if (atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;
        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);

        

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || childState.tantrumLevel == 0 || tatrumchildbehavior.gameLost)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
        }
        else if (!button1behavior.adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change

            UpdateTextAndAudioClip(tantrumLevel);
            yourButton.interactable = true;
            atStart = false;
        }

        /*if (audioSource.isPlaying || childState.tantrumLevel == 0 || tatrumchildbehavior.gameLost)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
        }
        else if (!audioSource.isPlaying)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change

            UpdateTextAndAudioClip(tantrumLevel);
            yourButton.interactable = true;
            
        }*/
    }

    void UpdateTextAndAudioClip(int tantrumLevel)
    {
        //switch (tantrumLevel)
        //{
        //    case 0:
        //        tmpText.text = "En este momento pareces estar tranquilo";
        //        audioSource.clip = Enestemomentoparecesestartranquilo;
        //        amount = -5.0f;
        //        break;
        //    case 1:
        //        tmpText.text = "Pareces estar enojado por no poder jugar";
        //        audioSource.clip = Parecesestarenojadopornopoderjugar;
        //        amount = -5.0f;
        //        break;
        //    case 2:
        //        tmpText.text = "Pareces estar enojado por no poder jugar";
        //        audioSource.clip = Parecesestarenojadopornopoderjugar;
        //        amount = -5.0f;
        //        break;
        //    case 3:
        //        tmpText.text = "Estás llorando porque te sientes enojado por no poder jugar";
        //        audioSource.clip = Enestemomentoparecesestartranquilo;
        //        amount = 0.0f;
        //        break;
        //    case 4:
        //        tmpText.text = "Manténgase tranquilo y físicamente alejado del niño";
        //        audioSource.clip = Manténgasetranquiloyfísicamentealejadodelniño;
        //        amount = 0.0f;
        //        break;
        //    case 5:
        //        tmpText.text = "Aléjese físicamente del niño";
        //        audioSource.clip = Aléjesefísicamentedelniño;
        //        amount = 0.0f;
        //        break;
        //    default:
        //        tmpText.text = "";
        //        audioSource.clip = null;
        //        amount = 0.0f;
        //        break;
        //}


        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You seem calm right now" : "En este momento pareces estar tranquilo";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youseemcalmrightnow : Enestemomentoparecesestartranquilo;
                amount = -5.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You seem upset about not being able to play" : "Pareces estar enojado por no poder jugar";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youseemupsetaboutnotbeingabletoplay : Parecesestarenojadopornopoderjugar;
                amount = -5.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You seem upset about not being able to play" : "Pareces estar enojado por no poder jugar";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youseemupsetaboutnotbeingabletoplay : Parecesestarenojadopornopoderjugar;
                amount = -5.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are crying because you are really upset about not playing" : "Estás llorando porque te sientes enojado por no poder jugar";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youarecryingbecauseyouarereallyupsetaboutnotplaying : Parecesestarenojadopornopoderjugar;
                amount = -5.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am going to stay over here until you calm down" : "Me voy a quedar aquí hasta que te calmes";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamgoingtostayoverhereuntilyoucalmdown : Mevoyaquedaraquíhastaquetecalmes;
                amount = -5.0f;
                break; ;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am going to keep away from you until you start calming down" : "Voy a mantenerme alejado de ti hasta que comiences a calmarte";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamgoingtokeepawayfromyouuntilyoustartcalmingdown : Voyamantenermealejadodetihastaquecomiencesacalmarte;
                amount = -5.0f;
                break;
            default:
                tmpText.text = "";
                audioSource.clip = null;
                amount = 0.0f;
                break;
        }
    }

    void OnButtonClick()
    {
        button1behavior.adultIsSpeaking = true;
        StartCoroutine(PlayAudioAndChangeTantrumLevel());
    }

    System.Collections.IEnumerator PlayAudioAndChangeTantrumLevel()
    {

        if (audioSource.clip != null)
        {
            if (!tatrumchildbehavior.childIsTalking)
                childState.ChangeTantrumLevel(amount);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            button1behavior.adultIsSpeaking = false;
            /*if (!tatrumchildbehavior.childIsTalking)
            {
                childState.ChangeTantrumLevel(amount);
            }*/
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }
        
    }

}
