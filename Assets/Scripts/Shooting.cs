using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject gun, gun_aimed, flashlight, shoot_effect;
    Vector3 initial_pos, aimed_pos;
    Quaternion inition_rot, aimed_rot;
    bool is_aiming = false, is_reloading = false;
    public Animator gun_anim;

    //cooldown
    float cooldown_time = 1.4f, cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = gun.transform.localPosition;
        inition_rot = gun.transform.localRotation;
        aimed_pos = gun_aimed.transform.localPosition;
        aimed_rot = gun_aimed.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        FlashOnOff();
        Reload();
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
                if (!is_reloading)
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
        if (Input.GetMouseButton(0))
        {
            RaycastHit bullet_hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bullet_hit);
            if(bullet_hit.collider.tag == "Enemy")
            {
                Destroy(bullet_hit.collider.gameObject); 
            }

            GameObject particle = Instantiate(shoot_effect, gun.transform);
            Destroy(particle, 0.5f);
            gun.transform.localPosition += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
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
