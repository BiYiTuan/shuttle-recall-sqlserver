using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer.Tests
{
	public class UniqueHasStoreTests : Fixture
	{
		[Test]
		public void Should_be_able_to_use_unqique_hash_store()
		{
			var store = new KeyStore(EventStoreSection.Configuration(), DatabaseGateway, new KeyStoreQueryFactory());

			var id = Guid.NewGuid();

			var value = string.Concat("value=", id.ToString());
			var anotherValue = string.Concat("anotherValue=", id.ToString());

			using (DatabaseConnectionFactory.Create(EventStoreSource))
			{
				store.Add(id, value);

				Assert.Throws<SqlException>(() => store.Add(id, value));

				var idGet = store.Get(value);

				Assert.IsNotNull(idGet);
				Assert.AreEqual(id, idGet);

				idGet = store.Get(anotherValue);

				Assert.IsNull(idGet);
			}
		}
	}
}