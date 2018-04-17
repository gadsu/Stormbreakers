using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    Rigidbody2D rb;

    private int damage; //Damage on hit
    private int chipDamage; //Damage on block
    private int hitstun;
    private Vector2 pushBack;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        //setVelocity(10);
        //setDamage(10); //This currently does nothing! Right now, damage values are hard coded in the CharacterState file. Needs to be fixed later.
    }

    public void setHorizontalVelocity(int vel)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(vel, 0);
        if (vel < 0)
        {
            Vector3 flip = gameObject.transform.localScale;
            flip.x = flip.x * -1;
            gameObject.transform.localScale = flip;
        }
    }

    public void setVerticalVelocity(int vel)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, vel);
        Debug.Log("SetVel!");
        if (vel < 0)
        {
            Vector3 flip = gameObject.transform.localScale;
            flip.x = flip.x * -1;
            gameObject.transform.localScale = flip;
        }
    }



    public void setDamage(int dmg)
    {
        damage = dmg;
    }

    public void setChipDamage(int cd)
    {
        chipDamage = cd;
    }

    public void setHitstun(int hts)
    {
        hitstun = hts;
    }

    public void setPushback(Vector2 psh)
    {
        pushBack = psh;
    }

    public int getDamage()
    {
        return damage;
    }

    public int getChipDamage()
    {
        return chipDamage;
    }

    public int getHitstun()
    {
        return hitstun;
    }

    public Vector2 getPushback()
    {
        return pushBack;
    }

    // Update is called once per frame
    void Update () {

    }
}
