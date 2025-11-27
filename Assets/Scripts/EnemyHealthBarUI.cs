using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public EnemyHealth enemy;
    public Image fillImage;
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

        transform.position = enemy.transform.position + offset;

        transform.LookAt(transform.position + cam.transform.forward);
    }

    public void UpdateFill()
    {
        fillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;
    }
}
