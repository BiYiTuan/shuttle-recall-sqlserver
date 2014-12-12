namespace Shuttle.Recall.SqlServer
{
	public class EventStoreConfiguration : IEventStoreConfiguration
	{
		public EventStoreConfiguration()
		{
			ConnectionStringName = "EventStore";
		}

		public string ConnectionStringName { get; set; }
	}
}