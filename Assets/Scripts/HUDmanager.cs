using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public GameObject press_e_obj;

    public Text paper_count;
    public int papers=0;

    public GameObject monster_obj;
    GameObject player_obj;

    public GameObject victory_screen;


    // Start is called before the first frame update
    void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");
        Papercount();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Papercount()
    {
        paper_count.text = papers.ToString() + "/5";
    }

    public void Addpaper()
    {
        papers++;
        Papercount();

        monster_obj.SetActive(true);

        if(papers == 5)
        {
            monster_obj.SetActive(false);
            victory_screen.SetActive(true);
            player_obj.GetComponent<CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
