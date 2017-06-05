using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TribalUI
{
	public class TestStepBar : MonoBehaviour {
		
		[SerializeField] private UIStepBar bar;
		[SerializeField] private float stepDelay = 0.5f;
		
		protected void Start()
		{
			if (this.bar == null)
				this.bar = this.GetComponent<UIStepBar>();
			
			if (this.bar == null)
			{
				this.enabled = false;
				return;
			}
			
			this.StartCoroutine("FillProgress");
		}
		
		private IEnumerator FillProgress()
		{
			this.bar.GoToStep(5);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(4);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(3);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(2);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(1);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(0);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(1);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(2);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(3);
			yield return new WaitForSeconds(this.stepDelay);
			this.bar.GoToStep(4);
			yield return new WaitForSeconds(this.stepDelay);
			
			this.StartCoroutine("FillProgress");
		}
	}
}