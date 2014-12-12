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
			var store = new UniqueHashStore(EventStoreSection.Configuration(), DatabaseGateway, new UniqueHashStoreQueryFactory());

			var id = Guid.NewGuid();

			const string value = "name=SomeName;surname=SomeSurname";
			const string anotherValue = "name=AnotherName;surname=SomeSurname";

			using (DatabaseConnectionFactory.Create(EventStoreSource))
			{
				store.Remove(0, value); // clear

				store.Add(id, 0, value);

				Assert.Throws<SqlException>(() => store.Add(id, 0, value));

				var idGet = store.Get(0, value);

				Assert.IsNotNull(idGet);
				Assert.AreEqual(id, idGet);

				idGet = store.Get(0, anotherValue);

				Assert.IsNull(idGet);

				idGet = store.Get(1, value);

				Assert.IsNull(idGet);

				Assert.IsFalse(store.Remove(1, anotherValue));
				Assert.IsTrue(store.Remove(0, value));

				idGet = store.Get(0, value);

				Assert.IsNull(idGet);
			}
		}
	}
}