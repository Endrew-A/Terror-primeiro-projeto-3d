using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    GameObject player_obj;

    public GameObject gameover_obj, gameover_ui;

    //HP
    public int monster_hp = 50;

    // Start is called before the first frame update
    void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player_obj.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameover_obj.SetActive(true);
            gameover_ui.SetActive(true);

            player_obj.GetComponent<CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;

            Destroy(this.gameObject);
        }
    }

    public void RemoveHP(int hp)
    {
        monster_hp -= hp;
        if (monster_hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
