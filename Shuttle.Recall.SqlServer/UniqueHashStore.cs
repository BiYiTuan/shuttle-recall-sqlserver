using System;
using System.Security.Cryptography;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public class UniqueHashStore : IUniqueHashStore
	{
		private readonly IDatabaseGateway _databaseGateway;
		private readonly IUniqueHashStoreQueryFactory _queryFactory;
		private readonly DataSource _eventStoreDataSource;
		private readonly SHA256 _sha = SHA256.Create();

		public UniqueHashStore(IEventStoreConfiguration eventStoreConfiguration, IDatabaseGateway databaseGateway, IUniqueHashStoreQueryFactory queryFactory)
		{
			Guard.AgainstNull(eventStoreConfiguration, "eventStoreConfiguration");
			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(queryFactory, "queryFactory");

			_databaseGateway = databaseGateway;
			_queryFactory = queryFactory;

			_eventStoreDataSource = new DataSource(eventStoreConfiguration.ConnectionStringName, new SqlDbDataParameterFactory());
		}

		public Guid? Get(int indexType, string value)
		{
			return _databaseGateway.GetScalarUsing<Guid?>(_eventStoreDataSource, _queryFactory.Get(indexType, _sha.ComputeHash(GetBytes(value))));
		}

		public bool Remove(int indexType, string value)
		{
			return _databaseGateway.ExecuteUsing(_eventStoreDataSource, _queryFactory.Remove(indexType, _sha.ComputeHash(GetBytes(value)))) > 0;
		}

		public void Add(Guid id, int indexType, string value)
		{
			_databaseGateway.ExecuteUsing(_eventStoreDataSource, _queryFactory.Add(id, indexType, _sha.ComputeHash(GetBytes(value))));
		}

		private static byte[] GetBytes(string value)
		{
			var result = new byte[value.Length * sizeof(char)];

			Buffer.BlockCopy(value.ToCharArray(), 0, result, 0, result.Length);

			return result;
		}
	}
}