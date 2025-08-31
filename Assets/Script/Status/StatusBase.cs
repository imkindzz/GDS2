using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _currentHealth = 100f; //the amount of health the status have
    [SerializeField, Min(1)] private float _maxHealth = 100f; //the maximum amount of health
    [SerializeField] private bool showHealthMeter = true; //whether or not the health meter is shown
    [SerializeField] private Slider healthMeter; //the visual slider that shows the amount of health

    [Header("Death")]
    [SerializeField] private float deathDelay = 0.25f; //the time taken before the gameObject completely disappears

    #region Properties
    public float currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float maxHealth { get => _maxHealth; }
    public bool noHealth { get => currentHealth == 0; } //when there is no health
    #endregion

    #region Unity methods
    void Awake()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        else if (currentHealth < 0) currentHealth = 0;

        SetupHealthMeter(showHealthMeter);
    }

    private void LateUpdate()
    {
        if (noHealth) OnDeathState();
    }
    #endregion

    #region Health meter methods
    //sets the healthMeter slider values
    private void SetHealthMeterValue(float value) { healthMeter.value = value; }

    //sets the healthMeter slider max
    private void SetHealthMeterMaxValue(float maxValue) { healthMeter.maxValue = maxValue; }

    //sets up the healthMeter slider before gameplay
    private void SetupHealthMeter(bool active) {
        if (healthMeter) {
            SetHealthMeterValue(currentHealth);
            SetHealthMeterMaxValue(maxHealth);

            healthMeter.gameObject.SetActive(active);
        }
    }

    //updates the healthMeter slider
    private void UpdateHealthMeter() {
        if (healthMeter) SetHealthMeterValue(currentHealth);
    }
    #endregion

    #region Health methods
    //increases the current health, which takes in account of the maximum
    private bool IncreaseCurrentHealth(float amount)
    {
        if (currentHealth == maxHealth) return false;

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        return true;
    }

    //decreases the current health, which takes in account of the minimum
    private bool DecreaseCurrentHealth(float amount)
    {
        if (currentHealth == 0) return false;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        return true;
    }

    //takes health to increase the current health
    public virtual void TakeHealth(float amount)
    {
        IncreaseCurrentHealth(amount);
        UpdateHealthMeter();
    }

    //takes damage to decrease the current health
    public virtual void TakeDamage(float amount)
    {
        DecreaseCurrentHealth(amount);
        UpdateHealthMeter();
    }
    #endregion

    #region State methods
    //when the status have no health, and is therefore dead
    public virtual void OnDeathState()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        Collider2D col = GetComponent<Collider2D>(); 
        if (col) col.enabled = false;
        
        Destroy(this.gameObject, deathDelay);
    }
    #endregion
}
