using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject gun, gun_aimed, flashlight, shoot_effect;
    Vector3 initial_pos, aimed_pos;
    Quaternion inition_rot, aimed_rot;
    bool can_shoot = true;
    float cooldown_shoot =0.5f, cooldown_shoot_timer = 0;

    //Animation control
    bool is_aiming = false, is_reloading = false;
    public Animator gun_anim;

    //cooldown
    float cooldown_time = 1.4f, cooldown = 0;

    //Munition control
    public int current_ammo, max_ammo, inv_ammo;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = gun.transform.localPosition;
        inition_rot = gun.transform.localRotation;
        aimed_pos = gun_aimed.transform.localPosition;
        aimed_rot = gun_aimed.transform.localRotation;

        HUDmanager.Instance.RefreshMunition(current_ammo, max_ammo);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        FlashOnOff();
        Reload();
        Cooldown_Shoot();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            if (!is_aiming && !is_reloading)
            {
                //mira
                is_aiming = true;
                
                gun_anim.SetBool("is_aiming", true);
                //gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, aimed_pos, 5 * Time.deltaTime);
                //gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, aimed_rot, 5 * Time.deltaTime);
            }
            else
            {
                if (!is_reloading && is_aiming&& can_shoot)
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
                gun_anim.SetBool("is_aiming", false);
                //gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, initial_pos, 5 * Time.deltaTime);
                //gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, inition_rot, 5 * Time.deltaTime);
            }
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && current_ammo > 0 && can_shoot)
        {
            can_shoot = false;

            RaycastHit bullet_hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bullet_hit);
            if(bullet_hit.collider.tag == "Enemy")
            {
                Destroy(bullet_hit.collider.gameObject); 
            }

            GameObject particle = Instantiate(shoot_effect, gun.transform);
            Destroy(particle, 0.25f);
            gun.transform.localPosition += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);

            current_ammo--;
            HUDmanager.Instance.RefreshMunition(current_ammo, max_ammo);
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
            gun_anim.SetBool("is_reloading", true);
            cooldown = 0;

            if(current_ammo<max_ammo && inv_ammo > 0)
            {
                inv_ammo -= max_ammo - current_ammo;
                current_ammo = max_ammo;
                HUDmanager.Instance.RefreshMunition(current_ammo, max_ammo);
            }
        }
        else
        {
            cooldown += Time.deltaTime;
            if (cooldown > cooldown_time)
            {
                is_reloading = false;
                gun_anim.SetBool("is_reloading", false);
            }
        }
        
    }
}
