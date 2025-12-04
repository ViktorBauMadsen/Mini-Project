using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    // The enemy this bar belongs to
    public EnemyHealth enemy;
    // UI image used as the fill
    public Image fillImage;

    // Offset from the enemy position (world space)
    public Vector3 offset = new Vector3(0, 0.1f, 0);

    // Optional boss label (assign BOSSText here)
    public GameObject bossLabel; // Assign BOSSText here

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBarUI has no enemy assigned!");
            Destroy(gameObject);
            return;
        }

        // Enable boss label only when the enemy is a boss
        if (bossLabel != null)
            bossLabel.SetActive(enemy.isBoss);
    }

    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Scale vertical offset by enemy size so bar sits correctly
        Vector3 scaledOffset = new Vector3(
            offset.x,
            offset.y * enemy.transform.localScale.y,
            offset.z
        );

        // Position above the enemy and face the camera
        transform.position = enemy.transform.position + scaledOffset;
        transform.LookAt(transform.position + cam.transform.forward);
    }

    // Update the fill amount based on current health
    public void UpdateFill()
    {
        fillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;
    }
}
