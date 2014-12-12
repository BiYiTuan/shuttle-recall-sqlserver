using System.Configuration;

namespace Shuttle.Recall.SqlServer
{
	public class EventStoreSection : ConfigurationSection
	{
		public static EventStoreSection Open(string file)
		{
			return Open<EventStoreSection>("eventStore", file);
		}

		[ConfigurationProperty("connectionStringName", IsRequired = false, DefaultValue = "EventStore")]
		public string ConnectionStringName
		{
			get
			{
				return (string)this["connectionStringName"];
			}
		}

		private static T Open<T>(string name) where T : class
		{
			return (ConfigurationManager.GetSection(string.Format("shuttle/{0}", name)) ?? ConfigurationManager.GetSection(name)) as T;
		}

		private static T Open<T>(string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var group = configuration.GetSectionGroup("shuttle");

			var section = group == null ? configuration.GetSection(name) as T : group.Sections[name] as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(SqlServerResources.OpenSectionException, name, file, typeof(T).FullName));
			}

			return section;
		}

		public static IEventStoreConfiguration Configuration()
		{
			var section = Open<EventStoreSection>("eventStore");
			var configuration = new EventStoreConfiguration();

			if (section != null)
			{
				configuration.ConnectionStringName = section.ConnectionStringName;
			}

			return configuration;
		}
	}
}