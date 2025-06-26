using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDmanager : MonoBehaviour
{

    public static HUDmanager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public Slider stamina_bar;
    public Image stamina_color;
    //public CanvasGroup slidergroup;

    // Start is called before the first frame update
    void Start()
    {
        //slidergroup.alpha = 0f;
        //slidergroup.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public IEnumerator FadeInStamina()
    {
        float transition = 0;

        while(transition < 1f)
        {
            transition += Time.deltaTime;
            slidergroup.alpha = Mathf.Clamp01(transition / 1f);
            yield return null;
        }
        
    }

    public IEnumerator FadeOutStamina()
    {
        float transition = 0;

        while(transition < 1f)
        {
            transition += Time.deltaTime;
            slidergroup.alpha = Mathf.Clamp01( 1 - (transition / 1f));
            yield return null;
        }
    }*/
}
