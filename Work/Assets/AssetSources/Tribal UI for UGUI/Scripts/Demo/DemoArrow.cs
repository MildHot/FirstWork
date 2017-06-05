using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Tweens;
using System.Collections;

namespace TribalUI
{
	public class DemoArrow : MonoBehaviour {
		
		public ScrollRect scrollView;
		public float disableOnOffsetY = 0f;
		public bool disableOnHide = true;
		private Image m_Image;
		private CanvasGroup m_CanvasGroup;
		
		[System.NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
		
		// Called by Unity prior to deserialization, 
		// should not be called by users
		protected DemoArrow()
		{
			if (this.m_FloatTweenRunner == null)
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			
			this.m_FloatTweenRunner.Init(this);
		}
		
		protected void Start ()
		{
			this.m_Image = this.gameObject.GetComponent<Image>();
			
			if (this.scrollView == null || this.m_Image == null)
			{
				this.enabled = false;
				return;
			}
			
			this.m_CanvasGroup = this.m_Image.gameObject.AddComponent<CanvasGroup>();
		}
		
		protected void Update()
		{
			if (!this.enabled || this.scrollView == null)
				return;
			
			if (this.scrollView.content.anchoredPosition.y >= this.disableOnOffsetY)
			{
				this.StartAlphaTween(0f, 1f);
				this.enabled = false;
			}
		}
		
		public void SetAlpha(float alpha)
		{
			if (this.m_CanvasGroup == null)
				return;
			
			this.m_CanvasGroup.alpha = alpha;
		}
		
		/// <summary>
		/// Starts a alpha tween on the tooltip.
		/// </summary>
		/// <param name="targetAlpha">Target alpha.</param>
		public void StartAlphaTween(float targetAlpha, float duration)
		{
			if (this.m_CanvasGroup == null)
				return;
			
			var floatTween = new FloatTween { duration = duration, startFloat = this.m_CanvasGroup.alpha, targetFloat = targetAlpha };
			floatTween.AddOnChangedCallback(SetAlpha);
			floatTween.AddOnFinishCallback(OnHide);
			floatTween.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(floatTween);
		}
		
		protected void OnHide()
		{
			if (this.disableOnHide)
				this.gameObject.SetActive(false);
		}
	}
}