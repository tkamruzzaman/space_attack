using UnityEngine;

public class Obstacle : Foe
{
    private void Start()
    {
        currentHealth = 25;
        speed = 5;

        foeSpawner = FindObjectOfType<FoeSpawner>();
        if (foeSpawner == null)
        {
            Debug.LogError("FoeSpawner is NULL");
        }
    }

    private void Update()
    {
        Move();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void DoDamage(int damage)
    {
        base.DoDamage(damage);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
    }
}
