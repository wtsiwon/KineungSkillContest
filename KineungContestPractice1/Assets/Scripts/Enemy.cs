using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICanDamaged
{
    public bool isBoss;

    public float moveSpd;

    public float maxHp;

    [SerializeField]
    private float hp;
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0) OnDie();
            else if (value >= maxHp) hp = maxHp;
            else
            {
                hp = value;
                StartCoroutine(IOnDamaged());
            }
        }
    }

    public bool isMove = true;

    protected SpriteRenderer spriteRenderer;

    protected Color ondmgColor = new Color(255, 255, 255, 150);
    protected Color originColor = Color.white;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isMove == true)
        {
            transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
        }
    }

    public void OnDamaged(float dmg)
    {
        Hp -= dmg;
    }

    private IEnumerator IOnDamaged()
    {
        spriteRenderer.color = ondmgColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originColor;
    }

    protected virtual void OnDie()
    {
        GameObject obj = Instantiate(GameManager.Instance.destroyEffect, transform.position, Quaternion.identity);
        obj.transform.localScale = new Vector3(20, 20, 1);
        obj.GetComponent<SpriteRenderer>().color = Color.green;
        if (isBoss == false)
        {
            Destroy(gameObject);
        }
    }
}

public interface ICanDamaged
{

}