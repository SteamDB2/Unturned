using System;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitTransactionUtility
	{
		public static void beginGenericTransaction()
		{
			TranslatedText translatedText = new TranslatedText(new TranslationReference("SDG", "Devkit.Transactions.Generic"));
			translatedText.format();
			DevkitTransactionManager.beginTransaction(translatedText);
		}

		public static void endGenericTransaction()
		{
			DevkitTransactionManager.endTransaction();
		}

		public static void recordInstantiation(GameObject go)
		{
			DevkitTransactionManager.recordTransaction(new DevkitGameObjectInstantiationTransaction(go));
		}

		public static void recordObjectDelta(object instance)
		{
			DevkitTransactionManager.recordTransaction(new DevkitObjectDeltaTransaction(instance));
		}

		public static void recordDestruction(GameObject go)
		{
			DevkitTransactionManager.recordTransaction(new DevkitGameObjectDestructionTransaction(go));
		}

		public static void recordTransformChangeParent(Transform transform, Transform parent)
		{
			DevkitTransactionManager.recordTransaction(new DevkitTransformChangeParentTransaction(transform, parent));
		}
	}
}
