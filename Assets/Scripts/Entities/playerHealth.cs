using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class playerHealth : MonoBehaviour
{
    public int health;
    public int numHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    // Start is called before the first frame update
    void Start()
    {
		InitializeHealth();
    }
	public void Heal(int healDone) {
		health = health + healDone;
		HealthUpdate();
	}
    public void Damage(int damageDone) {
		health = health - damageDone;
		HealthUpdate();
	}
    // Update is called on change.
    void HealthUpdate() {
		 if (health > numHearts)
        {
            health = numHearts;
        }
		if (health <= 0) {
			Die();
		}
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
	}
	// Initiazes, full heal. Called at beginning of each level or on special healing item effect.
	public void InitializeHealth() {
		health = 3;
		numHearts = 3;
	}
	// Doesn't need to be called. Health only needs to be updated on damage or heal event, not checked each frame. Frees up some CPU.
	void Update()
    {
       
    }
	// Called on death. Don't think we have a death screen/protocol yet, need to be discussed with team.
	void Die() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
