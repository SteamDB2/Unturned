using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MythicLocker : MonoBehaviour
	{
		public bool isMythic
		{
			get
			{
				return this._isMythic;
			}
			set
			{
				this._isMythic = value;
				if (base.gameObject.activeInHierarchy)
				{
					this.system.gameObject.SetActive(this.isMythic);
				}
			}
		}

		private void Update()
		{
			if (this.system == null)
			{
				return;
			}
			this.system.transform.position = base.transform.position;
			this.system.transform.rotation = base.transform.rotation;
		}

		private void LateUpdate()
		{
			if (this.system == null)
			{
				return;
			}
			this.system.transform.position = base.transform.position;
			this.system.transform.rotation = base.transform.rotation;
		}

		private void OnEnable()
		{
			if (this.system == null)
			{
				return;
			}
			this.system.gameObject.SetActive(this.isMythic);
		}

		private void OnDisable()
		{
			if (this.system == null)
			{
				return;
			}
			this.system.gameObject.SetActive(false);
		}

		private void Start()
		{
			if (this.system == null)
			{
				return;
			}
			this.system.transform.parent = Level.effects;
		}

		private void OnDestroy()
		{
			if (this.system == null)
			{
				return;
			}
			Object.Destroy(this.system.gameObject);
		}

		public MythicLockee system;

		private bool _isMythic = true;
	}
}
