using System;

namespace SDG.Unturned
{
	public class ModeConfigData
	{
		public ModeConfigData(EGameMode mode)
		{
			this.Items = new ItemsConfigData(mode);
			this.Vehicles = new VehiclesConfigData(mode);
			this.Zombies = new ZombiesConfigData(mode);
			this.Animals = new AnimalsConfigData(mode);
			this.Barricades = new BarricadesConfigData(mode);
			this.Structures = new StructuresConfigData(mode);
			this.Players = new PlayersConfigData(mode);
			this.Objects = new ObjectConfigData(mode);
			this.Events = new EventsConfigData(mode);
			this.Gameplay = new GameplayConfigData(mode);
		}

		public ItemsConfigData Items;

		public VehiclesConfigData Vehicles;

		public ZombiesConfigData Zombies;

		public AnimalsConfigData Animals;

		public BarricadesConfigData Barricades;

		public StructuresConfigData Structures;

		public PlayersConfigData Players;

		public ObjectConfigData Objects;

		public EventsConfigData Events;

		public GameplayConfigData Gameplay;
	}
}
