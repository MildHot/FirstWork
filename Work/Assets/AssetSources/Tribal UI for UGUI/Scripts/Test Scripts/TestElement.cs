using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TribalUI
{
	public class TestElement : MonoBehaviour {
		
		public enum Type
		{
			Progress,
			Fade,
		}
		
		[SerializeField] private Graphic m_TargetGraphic;
		[SerializeField] private Type m_Type = Type.Progress;
		[SerializeField] private float duration = 1f;
		[SerializeField] private float startDelay = 0f;
		[SerializeField] private float reverseDelay = 0f;
		private bool ascending = false;
		
		[SerializeField] private Text progressLabel;
		[SerializeField] private string appendBefore = "";
		[SerializeField] private string appendAfter = "%";
		
		[SerializeField] private Color[] colors;
		[SerializeField] private float colorLerp = 4f;
		
		protected void Start()
		{
			if (this.startDelay > 0f)
			{
				this.StartCoroutine("DelayedStart");
			}
			else
			{
				this.StartEffect();
			}
		}
		
		protected void Update()
		{
			if (this.m_TargetGraphic == null || (this.m_TargetGraphic is Image) == false || this.m_Type != Type.Progress)
				return;
			
			// Change the bar color based on the percentage
			if (this.colors.Length > 1)
			{
				Image image = this.m_TargetGraphic as Image;
				
				if (image != null)
				{
					// As default
					int colorIndex = 0;
					
					// If we're over or at 100 percent
					if (image.fillAmount >= 1f)
					{
						colorIndex = this.colors.Length - 1;
					}
					else if (image.fillAmount > 0f)
					{
						colorIndex = Mathf.FloorToInt(this.colors.Length * image.fillAmount);
					}
					
					image.canvasRenderer.SetColor(Color.Lerp(image.canvasRenderer.GetColor(), this.colors[colorIndex], Time.deltaTime * this.colorLerp));
				}
			}
		}
		
		public void StartEffect()
		{
			if (this.m_TargetGraphic == null)
				return;
			
			if (this.m_TargetGraphic is Image && this.m_Type == Type.Progress)
			{
				Image image = (this.m_TargetGraphic as Image);
				
				if (image.type != Image.Type.Filled)
					image.type = Image.Type.Filled;
				
				image.fillAmount = 1f;
				this.StopCoroutine("FillProgress");
				this.StartCoroutine("FillProgress");
			}
			else if (this.m_Type == Type.Fade)
			{
				this.m_TargetGraphic.canvasRenderer.SetAlpha(1f);
				this.StopCoroutine("Fade");
				this.StartCoroutine("Fade");
			}
		}
		
		private void UpdateProgressLabel()
		{
			if (this.m_TargetGraphic == null || (this.m_TargetGraphic is Image) == false || this.m_Type != Type.Progress)
				return;
			
			Image image = this.m_TargetGraphic as Image;
			
			if (image != null && this.progressLabel != null)
				this.progressLabel.text = this.appendBefore + (image.fillAmount * 100).ToString("0") + this.appendAfter;
		}
		
		private IEnumerator DelayedStart()
		{
			if (this.startDelay > 0f)
				yield return new WaitForSeconds(this.startDelay);
			
			this.StartEffect();
		}
		
		private IEnumerator FillProgress()
		{
			if (this.m_TargetGraphic == null || (this.m_TargetGraphic is Image) == false || this.m_Type != Type.Progress)
				yield break;
				
			Image image = this.m_TargetGraphic as Image;
			float startTime = Time.time;
			
			while (Time.time <= (startTime + this.duration))
			{
				float RemainingTime = ((startTime + this.duration) - Time.time);
				float ElapsedTime = (this.duration - RemainingTime);
				
				if (this.ascending)
					image.fillAmount = (ElapsedTime / this.duration);
				else
					image.fillAmount = (RemainingTime / this.duration);
				
				this.UpdateProgressLabel();
				
				yield return 0;
			}
			
			if (this.reverseDelay > 0f)
				yield return new WaitForSeconds(this.reverseDelay);
			
			this.ascending = !this.ascending;
			this.StartCoroutine("FillProgress");
		}
		
		private IEnumerator Fade()
		{
			if (this.m_TargetGraphic == null || this.m_Type != Type.Fade)
				yield break;
			
			float startTime = Time.time;
			
			while (Time.time <= (startTime + this.duration))
			{
				float RemainingTime = ((startTime + this.duration) - Time.time);
				float ElapsedTime = (this.duration - RemainingTime);
				
				if (this.ascending)
					this.m_TargetGraphic.canvasRenderer.SetAlpha(ElapsedTime / this.duration);
				else
					this.m_TargetGraphic.canvasRenderer.SetAlpha(RemainingTime / this.duration);
				
				yield return 0;
			}
			
			if (this.reverseDelay > 0f)
				yield return new WaitForSeconds(this.reverseDelay);
			
			this.ascending = !this.ascending;
			this.StartCoroutine("Fade");
		}
	}
}