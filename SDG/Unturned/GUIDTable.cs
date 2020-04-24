using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class GUIDTable
	{
		public static event GUIDTableMappingAddedHandler mappingAdded;

		public static event GUIDTableCleared cleared;

		public static Guid resolveIndex(GUIDTableIndex index)
		{
			Guid result;
			if (GUIDTable.indexToGUIDDictionary.TryGetValue(index, out result))
			{
				return result;
			}
			return Guid.Empty;
		}

		public static GUIDTableIndex resolveGUID(Guid GUID)
		{
			GUIDTableIndex result;
			if (GUIDTable.GUIDToIndexDictionary.TryGetValue(GUID, out result))
			{
				return result;
			}
			return GUIDTableIndex.invalid;
		}

		public static List<KeyValuePair<GUIDTableIndex, Guid>> toList()
		{
			return GUIDTable.tableList;
		}

		public static void addMapping(GUIDTableIndex index, Guid GUID)
		{
			GUIDTable.indexToGUIDDictionary.Add(index, GUID);
			GUIDTable.GUIDToIndexDictionary.Add(GUID, index);
			GUIDTable.tableList.Add(new KeyValuePair<GUIDTableIndex, Guid>(index, GUID));
			GUIDTable.triggerMappingAdded(index, GUID);
		}

		public static GUIDTableIndex add(Guid GUID)
		{
			GUIDTableIndex guidtableIndex = GUIDTable.createIndex();
			GUIDTable.addMapping(guidtableIndex, GUID);
			return guidtableIndex;
		}

		public static void clear()
		{
			GUIDTable.indexToGUIDDictionary.Clear();
			GUIDTable.GUIDToIndexDictionary.Clear();
			GUIDTable.tableList.Clear();
			GUIDTable.currentTableIndex = 0;
			GUIDTable.triggerCleared();
		}

		private static void triggerMappingAdded(GUIDTableIndex index, Guid GUID)
		{
			if (GUIDTable.mappingAdded != null)
			{
				GUIDTable.mappingAdded(index, GUID);
			}
		}

		private static void triggerCleared()
		{
			if (GUIDTable.cleared != null)
			{
				GUIDTable.cleared();
			}
		}

		private static GUIDTableIndex createIndex()
		{
			GUIDTableIndex result = GUIDTable.currentTableIndex;
			GUIDTable.currentTableIndex.index = GUIDTable.currentTableIndex.index + 1;
			return result;
		}

		private static Dictionary<GUIDTableIndex, Guid> indexToGUIDDictionary = new Dictionary<GUIDTableIndex, Guid>();

		private static Dictionary<Guid, GUIDTableIndex> GUIDToIndexDictionary = new Dictionary<Guid, GUIDTableIndex>();

		private static List<KeyValuePair<GUIDTableIndex, Guid>> tableList = new List<KeyValuePair<GUIDTableIndex, Guid>>();

		private static GUIDTableIndex currentTableIndex = 0;
	}
}
