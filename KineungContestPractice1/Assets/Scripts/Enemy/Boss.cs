using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Boss : Enemy
{
    [SerializeField]
    private Transform shootingPos;

    public bool isBossMove;

    public int patternNum;


    public override float Hp
    {
        get => base.Hp;
        set
        {
            base.Hp = value;
            if (value <= 0 && IsDie == false)
            {
                OnDie();
            }
        }
    }


    protected override void Start()
    {
        base.Start();
        StartCoroutine(IBossMove());
        isBossMove = true;
        IsDie = false;
    }

    void Update()
    {
        if (IsDie == true)
        {
            transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        BossDie();
    }

    private void BossDie()
    {
        isDie = true;
        EnemySpawner.Instance.isBossSpawned = false;
        StartCoroutine(IBossDieEffect());
    }

    private IEnumerator IBossDieEffect()
    {
        print("DieEffect");
        int count = 0;
        while (count < 30)
        {
            yield return new WaitForSeconds(0.1f);
            count++;
            GameObject obj = Instantiate(GameManager.Instance.destroyEffect, RandomPositon(transform.position, 4, 3), Quaternion.identity);
            obj.transform.localScale = new Vector3(15, 15, 1);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private Vector3 RandomPositon(Vector3 originPos, float xRange, float yRange)
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, 0);
        Vector3 pos = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

        return pos;
    }

    private IEnumerator IBossMove()
    {
        Vector3 pos1 = new Vector3(0, 7, 0);
        Vector3 pos2 = new Vector3(0, 13, 0);

        float duration = 2;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos2, pos1, timer / duration);
            yield return null;
        }

        isBossMove = false;
    }



    protected override void Attack()
    {
        RandomBossPattern();
    }

    private void RandomBossPattern()
    {


        int randPattern = Random.Range(1, 2);

        StartCoroutine($"IBossPattern{1}");
    }

    private IEnumerator IBossPattern1()
    {
        int angle = 360 / 12;

        for (int j = 0; j < 10; j++)
        {
            int count = 0;
            for (int i = 0; i < 360; i += angle)
            {
                Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
                bullet1.SetBullet(transform.position, new Vector3(0, 0, i + count * 15), bulletSpd, 1, 1, true);
            }
            count++;
            yield return new WaitForSeconds(0.3f);
        }
    }
}