using UnityEngine;

namespace HealthSysteme.SaverioScript.HealthSysteme.Core
{
	public class HealthComponent : MonoBehaviour
	{
		[SerializeField] private HealthData healthData;
		[field: SerializeField] public float CurrentHealth { get; private set; }
		private float MaxHealth => healthData.maxHealth;

		private void Start()
		{
			CurrentHealth = healthData.maxHealth;
		}
	
		public void AddOrRemoveHealth(float amount)
		{
			CurrentHealth += amount;
		
			if (CurrentHealth > healthData.maxHealth)
				CurrentHealth = healthData.maxHealth;
		
			if (CurrentHealth <= 0)
				CurrentHealth = 100;
		
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, healthData.maxHealth);
		}
	}
}