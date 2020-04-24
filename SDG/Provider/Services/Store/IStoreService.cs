using System;
using SDG.Provider.Services.Economy;

namespace SDG.Provider.Services.Store
{
	public interface IStoreService : IService
	{
		bool canOpenStore { get; }

		void open(IStorePackageID packageID);

		void open(IEconomyItemDefinition itemDefinitionID);

		void open();
	}
}
