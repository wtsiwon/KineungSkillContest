using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    public bool isBoss;

    public float moveSpd;

    public int score;//������ �ִ� ����

    public float atkSpd;//���ݼӵ�

    public bool isAttack;

    public Bullet bullet;

    public float maxHp;

    [SerializeField]
    private float hp;
    public virtual float Hp
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

    [SerializeField]
    protected GameObject onDmgObj;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(IAttack));

    }

    private void Update()
    {
        if (isMove == true)
        {
            transform.Translate(Vector3.down * moveSpd * Time.deltaTime);
        }
    }

    public void OnDamaged(float dmg)
    {
        Hp -= dmg;
    }

    private IEnumerator IOnDamaged()
    {
        onDmgObj.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        onDmgObj.SetActive(false);
    }

    protected virtual void OnDie()
    {
        GameManager.Instance.GetDestroyEffect(transform.position, 15);
        GameManager.Instance.Score += score;
        if (isBoss == false)
        {
            GameManager.Instance.SpawnRandomItem(transform.position);
            Destroy(gameObject);
        }
    }

    protected virtual IEnumerator IAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isAttack == true)
            {
                yield return new WaitForSeconds(atkSpd);
                Attack();
            }
        }
    }

    protected abstract void Attack();
}