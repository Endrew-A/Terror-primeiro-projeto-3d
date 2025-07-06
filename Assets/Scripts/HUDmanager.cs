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
    bool spawned = false;
    GameObject player_obj;

    public GameObject victory_screen, gameover_screen, gameover_anim;

    public Text Munition_count;

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

        if(papers == 1 && !spawned)
        {
            GameObject monster_instance = Instantiate(monster_obj, new Vector3(38.18f, 0.029f, 20f), Quaternion.identity);
            monster_instance.GetComponent<Monster>().gameover_obj = gameover_anim;
            monster_instance.GetComponent<Monster>().gameover_ui = gameover_screen;
            spawned = true;
        }
        

        if(papers == 5)
        {
            monster_obj.SetActive(false);
            victory_screen.SetActive(true);
            player_obj.GetComponent<CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void RefreshMunition(int current, int max)
    {
        Munition_count.text = current.ToString() + "/" + max.ToString();
    }
}
