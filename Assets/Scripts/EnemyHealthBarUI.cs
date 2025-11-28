using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public EnemyHealth enemy;
    public Image fillImage;

    // Default offset works for normal zombies
    public Vector3 offset = new Vector3(0, 0.1f, 0);

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBarUI has no enemy assigned!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Automatically scale health bar height with enemy size
        Vector3 scaledOffset = new Vector3(
            offset.x,
            offset.y * enemy.transform.localScale.y,
            offset.z
        );

        transform.position = enemy.transform.position + scaledOffset;

        transform.LookAt(transform.position + cam.transform.forward);
    }

    public void UpdateFill()
    {
        fillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;
    }
}
