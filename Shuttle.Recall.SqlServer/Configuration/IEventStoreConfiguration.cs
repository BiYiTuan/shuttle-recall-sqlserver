namespace Shuttle.Recall.SqlServer
{
	public interface IEventStoreConfiguration
	{
		string ConnectionStringName { get; }
	}
}