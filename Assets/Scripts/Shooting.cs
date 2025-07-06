using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject flashlight, shoot_effect;
    public GameObject[] gun, particle_pos;
    public int selected_gun;
    bool can_shoot = true, is_auto = false;
    float cooldown_shoot =0.5f, cooldown_shoot_timer = 0;

    //Animation control
    bool is_aiming = false, is_reloading = false;
    public Animator[] gun_anim;

    //cooldown
    float cooldown_time = 1.4f, cooldown = 0, cooldown_coice= 0.13f;

    //Munition control
    public int[] current_ammo, max_ammo, inv_ammo;

    // Start is called before the first frame update
    void Start()
    {

        HUDmanager.Instance.RefreshMunition(current_ammo[selected_gun], max_ammo[selected_gun]);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        FlashOnOff();
        Reload();
        Cooldown_Shoot();
        ChangeGun();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            if (!is_aiming && !is_reloading)
            {
                //mira
                is_aiming = true;
                
                gun_anim[selected_gun].SetBool("is_aiming", true);
                //gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, aimed_pos, 5 * Time.deltaTime);
                //gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, aimed_rot, 5 * Time.deltaTime);
            }
            else
            {
                if (!is_reloading && is_aiming)
                {
                    Shoot();
                }
            }
        }
        else
        {
            if (is_aiming)
            {
                //volta pro idle
                is_aiming = false;
                gun_anim[selected_gun].SetBool("is_aiming", false);
                //gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, initial_pos, 5 * Time.deltaTime);
                //gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, inition_rot, 5 * Time.deltaTime);
            }
        }
    }

    void Shoot()
    {
        if (((Input.GetMouseButtonDown(0) && !is_auto) || (Input.GetMouseButton(0) && is_auto)) && current_ammo[selected_gun] > 0 && can_shoot)
        {
            can_shoot = false;
            gun_anim[selected_gun].SetBool("coice", true);

            RaycastHit bullet_hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bullet_hit);
            if(bullet_hit.collider.tag == "Enemy")
            {
                if(selected_gun == 0)
                {
                    bullet_hit.collider.GetComponent<Monster>().RemoveHP(20);
                }
                else
                {
                    bullet_hit.collider.GetComponent<Monster>().RemoveHP(5);
                }
            }
            else if(bullet_hit.collider.tag == "EnemyHead")
            {
                if (selected_gun == 0)
                {
                    bullet_hit.collider.GetComponentInParent<Monster>().RemoveHP(50);
                }
                else
                {
                    bullet_hit.collider.GetComponentInParent<Monster>().RemoveHP(15);
                }
            }

            if (selected_gun == 1)
            {
                GameObject particle = Instantiate(shoot_effect, particle_pos[selected_gun].transform);
                Destroy(particle, 0.25f);
            }
            else
            {
                GameObject particle = Instantiate(shoot_effect, particle_pos[selected_gun].transform);
                Destroy(particle, 0.25f);
            }
            //gun.transform.localPosition += new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), 0);

            current_ammo[selected_gun]--;
            HUDmanager.Instance.RefreshMunition(current_ammo[selected_gun], max_ammo[selected_gun]);
        }
    }

    void Cooldown_Shoot()
    {
        if(!can_shoot && cooldown_shoot_timer > cooldown_shoot)
        {
            can_shoot = true;
            cooldown_shoot_timer = 0;
        }
        else if(!can_shoot && cooldown_shoot_timer <= cooldown_shoot)
        {
            cooldown_shoot_timer += Time.deltaTime;
            if (cooldown_shoot_timer > cooldown_coice)
            {
                gun_anim[selected_gun].SetBool("coice", false);
            }
        }
    }

    void FlashOnOff()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(flashlight.activeSelf)
            {
                flashlight.SetActive(false);
            }
            else
            {
                flashlight.SetActive(true);
            }
        }
    }

    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && cooldown>cooldown_time)
        {
            is_reloading = true;
            gun_anim[selected_gun].SetBool("is_reloading", true);
            cooldown = 0;

            if(current_ammo[selected_gun] < max_ammo[selected_gun] && inv_ammo[selected_gun] > 0)
            {
                inv_ammo[selected_gun] -= max_ammo[selected_gun] - current_ammo[selected_gun];
                current_ammo[selected_gun] = max_ammo[selected_gun];
                HUDmanager.Instance.RefreshMunition(current_ammo[selected_gun], max_ammo[selected_gun]);
            }
        }
        else
        {
            cooldown += Time.deltaTime;
            if (cooldown > cooldown_time)
            {
                is_reloading = false;
                gun_anim[selected_gun].SetBool("is_reloading", false);
            }
        }
    }

    void ChangeGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !is_aiming && selected_gun!=0)
        {
            StartCoroutine(ChangeAnim1());
            is_auto = false;
            cooldown_shoot = 0.5f;
            cooldown_coice = 0.13f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !is_aiming && selected_gun!=1)
        {
            StartCoroutine(ChangeAnim2());
            is_auto = true;
            cooldown_shoot = 0.1f;
            cooldown_coice = 0.05f;
        }
    }

    IEnumerator ChangeAnim1()
    {
        gun_anim[selected_gun].SetBool("entry", false);
        yield return new WaitForSeconds(0.3f);
        
        gun[1].SetActive(false);
        gun[0].SetActive(true);
        selected_gun = 0;

        HUDmanager.Instance.RefreshMunition(current_ammo[selected_gun], max_ammo[selected_gun]);
    }

    IEnumerator ChangeAnim2()
    {
        gun_anim[selected_gun].SetBool("entry", false);
        yield return new WaitForSeconds(0.3f);

        gun[0].SetActive(false);
        gun[1].SetActive(true);
        selected_gun = 1;

        HUDmanager.Instance.RefreshMunition(current_ammo[selected_gun], max_ammo[selected_gun]);
    }
}
