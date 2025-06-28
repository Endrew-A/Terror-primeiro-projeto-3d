using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papel : MonoBehaviour
{

    GameObject player_obj;

    float distance_player;

    // Start is called before the first frame update
    void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance_player = Vector3.Distance(this.gameObject.transform.position, player_obj.transform.position);
        CollectPaper();
    }

    private void OnMouseOver()
    {
        if (distance_player < 5)
        {
            HUDmanager.Instance.press_e_obj.SetActive(true);
        }
        else
        {
            HUDmanager.Instance.press_e_obj.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        HUDmanager.Instance.press_e_obj.SetActive(false);
    }

    void CollectPaper()
    {
        if(distance_player <5 && Input.GetKeyDown(KeyCode.E))
        {
            HUDmanager.Instance.press_e_obj.SetActive(false);
            HUDmanager.Instance.Addpaper();

            Destroy(this.gameObject);
        }

    }
}
