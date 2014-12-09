using NUnit.Framework;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer.Tests
{
	[TestFixture]
	public class Fixture
	{
		public readonly DataSource EventStoreSource = new DataSource("EventStore", new SqlDbDataParameterFactory());

		public DatabaseGateway DatabaseGateway { get; private set; }
		public DatabaseConnectionFactory DatabaseConnectionFactory { get; private set; }

		[SetUp]
		public void TestSetUp()
		{
			var databaseConnectionCache = new ThreadStaticDatabaseConnectionCache();
			DatabaseGateway = new DatabaseGateway(databaseConnectionCache);
			DatabaseConnectionFactory = new DatabaseConnectionFactory(DbConnectionFactory.Default(), new DbCommandFactory(), databaseConnectionCache);
		}
	}
}