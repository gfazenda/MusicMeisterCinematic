using UnityEngine;

public enum GUN { SHOOT, EXPLOSIVE, MAGNETIC, FREEZING };

public class PlayerScript : Entity
{
    lerpColor lerp;
    float _velocidade;

    public GameObject bullet, gun, explosive, magnetic, freezing, AoE;
    public GUN currentGun;
    GUN tempGun;
    bool validChoice = true, gunChanged = false;
    public int Mana = 100, maxMana = 0;
    GameObject lastPlaced = null;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float mousesensitivityX = 15F;
    public float mousesensitivityY = 15F;

    public float joysensitivityX = 3F;
    public float joysensitivityY = 3F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    private void OnCollisionEnter (Collision collision)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (collision.gameObject.tag == Tags.Enemy)
        {
            base.HP -= collision.gameObject.GetComponent<Entity>().damage;
           // EventManager.TriggerEvent(Events.UIUpdate);
            lerp.targetColor = Color.red;
            lerp.Lerp(1f);

            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == Tags.Consumable)
        {
            lerp.targetColor = Color.blue;
            lerp.Lerp(1f);
           //restoreMana(collision.gameObject.GetComponent<ManaConsumable>().restoreAmount);
            collision.gameObject.SetActive(false);
        }
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    new void Start()
    {
        base.Start();
        lerp = this.GetComponent<lerpColor>();
        _velocidade = 10.0F;
        currentGun = GUN.SHOOT;
    }

    private void OnEnable()
    {
        base.Start();
        maxMana = Mana;
    }

    void OnParticleCollision(GameObject other)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (other.tag == Tags.Ring)
        {
            base.HP -= 20;    
            lerp.targetColor = Color.green;
            lerp.Lerp(2f);
        }
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void restoreMana(int amount)
    {
        if (maxMana >= (Mana + amount))
        {
            Mana += amount;
        }
        else
        {
            Mana = maxMana;
        }
       // updateAOE(true);
     //   EventManager.TriggerEvent(Events.UIUpdate);
    }

    private void FixedUpdate()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 testX, testZ;
        testX = Input.GetAxis("Horizontal") * cameraTransform.right;
        testZ = Input.GetAxis("Vertical") * cameraTransform.up;

        Vector3 translate = (testX + testZ) * _velocidade * Time.deltaTime;
        translate.y = 0;

        transform.position += translate;
    }

    public void updateAOE(GUN spell)//bool activate)
    {
        AoE.gameObject.SetActive(true);
        Color _newColor = Color.clear;
        switch (currentGun)
        {
            case GUN.SHOOT:
                AoE.gameObject.SetActive(false);
                break;
            case GUN.EXPLOSIVE:
                _newColor = Color.red;
                break;
            case GUN.MAGNETIC:
                _newColor = Color.yellow;
                break;
            case GUN.FREEZING:
                _newColor = Color.cyan;
                break;
        }
        _newColor.a = 0.7f;
        AoE.GetComponent<Renderer>().material.color = _newColor;
        //AoE.GetComponent<checkValidSpot>().canPlant = activate;
        //AoE.GetComponent<checkValidSpot>().setColor();
    }

    void Update()
    {
        //só verifica se não estiver pausado o jogo
        //if (GameManager.Instance.isPaused)
        //{
        //    return;
        //}
            //mouse controller
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;
            float angle = (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        }


            /*JOY TEST*/
            float Xon = Mathf.Abs(Input.GetAxis("Joy X"));
            
            if (Xon > 0.1)
                this.transform.Rotate(0, Input.GetAxis("Joy X") * joysensitivityX, 0);


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonUp("Fire"))
            {
                //if (currentGun == GUN.SHOOT)
                //{
                //    Shoot();
                //}
                //else
                //{
                //    if (AoE.GetComponent<checkValidSpot>().canPlant)
                //    {
                //        PutTrap();
                //    }
                //    else
                //    {
                //    //    UIManager.Instance.showFeedback("Not Enough Mana");
                //    }
                //}
            } else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(1) || Input.GetButtonUp("WeaponPlus"))
            {
                gunChanged = true;
                validChoice = false;
                
                tempGun = currentGun;
                if ((int)tempGun == 3)
                {
                    tempGun = GUN.SHOOT;
                }
                else
                {               
                    tempGun++;
                }
            }else if (Input.GetButtonUp("WeaponLess"))
            {
                gunChanged = true;
                validChoice = false;

                tempGun = currentGun;
                if ((int)tempGun == 0)
                {
                    tempGun = GUN.FREEZING;
                }
                else
                {
                    tempGun--;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || (tempGun == GUN.SHOOT && gunChanged))
            {
                tempGun = GUN.SHOOT;
              //  EventManager.TriggerEvent(Events.usingGun);
                validChoice = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) || (tempGun == GUN.EXPLOSIVE && gunChanged))
            {
                tempGun = GUN.EXPLOSIVE;
            //    EventManager.TriggerEvent(Events.usingBomb);
                validChoice = true;

            }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3) || (tempGun == GUN.MAGNETIC && gunChanged))
            {
                tempGun = GUN.MAGNETIC;
              //  EventManager.TriggerEvent(Events.usingBomb);
                validChoice = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4) || (tempGun == GUN.FREEZING && gunChanged))
            {
                tempGun = GUN.FREEZING;
             //   EventManager.TriggerEvent(Events.usingBomb);
                validChoice = true;
            }

            if (validChoice && gunChanged)
            {
             //   EventManager.TriggerEvent(Events.UIUpdate);
                currentGun = tempGun;
                updateAOE(currentGun);
                gunChanged = false;
                validChoice = false;
            }
    }

    void Shoot()
    {
      //  GameObject _bullet = ObjectPooler.SharedInstance.GetPooledObject(Tags.Bullet);
        //if (_bullet != null && castSpell(_bullet))
        //{
        //    _bullet.transform.position = gun.GetComponent<Transform>().position + gun.GetComponent<Transform>().forward;
        //    _bullet.transform.rotation = gun.GetComponent<Transform>().rotation;
        //    _bullet.SetActive(true);
        //}
    }

    bool castSpell(GameObject spell, bool usingSpell = true)
    {

        int cost = 0;
        //if (spell.tag == Tags.Bomb)
        //  //  cost = spell.GetComponent<BombScript>().manaCost;
        //else
        //{
        //   // cost = spell.GetComponent<BulletBehaviour>().manaCost;
        //}
       
        if (Mana >= cost)
        {
            if (usingSpell)
            {
                Mana -= cost;
                //EventManager.TriggerEvent(Events.UIUpdate);
            }
            // updateAOE(currentGun);
            return true;
        }
        else
        {
           // UIManager.Instance.showFeedback("Not Enough Mana");
           // updateAOE(false);
            return false;
        }

    }

    void PutTrap()
    {
        Quaternion rot = gun.GetComponent<Transform>().rotation;
        rot.z = 1.0f;
        GameObject bomb = null;
        switch (currentGun)
        {
            case GUN.EXPLOSIVE:
                bomb = explosive;
                break;
            case GUN.MAGNETIC:
                bomb = magnetic;
                break;
            case GUN.FREEZING:
                bomb = freezing;
                break;
        }
        if (castSpell(bomb))
        {
            lastPlaced = Instantiate(bomb, gun.GetComponent<Transform>().position + gun.GetComponent<Transform>().forward, rot);
        }
    }
}
