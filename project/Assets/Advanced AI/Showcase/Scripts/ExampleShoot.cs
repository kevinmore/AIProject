using UnityEngine;

public class ExampleShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 15.0f;
    public float damageAmount = 10.0f;
    public float decalsDestructionTime = 10.0f;
    public float impactFxDestructionTime = 3.0f;
    private GameObject player;
    private float playerHealth;
    public GameObject projecitleImpactFx;
    public Texture2D projectileDecal;
    public AudioClip projectileImpactSfx;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        if (!player) return;

        playerHealth = player.GetComponent<PlayerHealth>().health;
        if (playerHealth > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;


                if (!bullet.GetComponent<Projectile>())
                    bullet.AddComponent("Projectile");

                if (!bullet.rigidbody)
                    bullet.AddComponent("Rigidbody");

                Physics.IgnoreCollision(bullet.collider, player.collider);

                bullet.rigidbody.useGravity = false;
                bullet.GetComponent<Projectile>().damage = damageAmount;
                if (projectileDecal)
                {
                    bullet.GetComponent<Projectile>().projectileDecal = projectileDecal;
                    bullet.GetComponent<Projectile>().decalsDestructionTime = decalsDestructionTime;
                }
                if (projecitleImpactFx)
                {
                    bullet.GetComponent<Projectile>().projectileImpactFx = projecitleImpactFx;
                    bullet.GetComponent<Projectile>().ImpactFxDestructionTime = impactFxDestructionTime;
                }
                if (projectileImpactSfx)
                {
                    bullet.GetComponent<Projectile>().projectileImpactSfx = projectileImpactSfx;
                }
                bullet.rigidbody.velocity = transform.forward*bulletSpeed;
            }
        }
    }
}