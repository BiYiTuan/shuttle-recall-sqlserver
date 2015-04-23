using System;
using System.Security.Cryptography;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public class KeyStore : IKeyStore
	{
		private readonly IDatabaseGateway _databaseGateway;
		private readonly IKeyStoreQueryFactory _queryFactory;
		private readonly DataSource _eventStoreDataSource;
		private readonly SHA256 _sha256 = SHA256.Create();
		private readonly MD5 _md5 = MD5.Create();

		public KeyStore(IEventStoreConfiguration eventStoreConfiguration, IDatabaseGateway databaseGateway, IKeyStoreQueryFactory queryFactory)
		{
			Guard.AgainstNull(eventStoreConfiguration, "eventStoreConfiguration");
			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(queryFactory, "queryFactory");

			_databaseGateway = databaseGateway;
			_queryFactory = queryFactory;

			_eventStoreDataSource = new DataSource(eventStoreConfiguration.ConnectionStringName, new SqlDbDataParameterFactory());
		}

		private static byte[] GetBytes(string value)
		{
			var result = new byte[value.Length * sizeof(char)];

			Buffer.BlockCopy(value.ToCharArray(), 0, result, 0, result.Length);

			return result;
		}

		public Guid? Get(string key)
		{
			var buffer = GetBytes(key);

			return _databaseGateway.GetScalarUsing<Guid?>(_eventStoreDataSource, _queryFactory.Get(_md5.ComputeHash(buffer), _sha256.ComputeHash(buffer)));
		}

		public void Remove(string key)
		{
			var buffer = GetBytes(key);

			_databaseGateway.ExecuteUsing(_eventStoreDataSource, _queryFactory.Remove(_md5.ComputeHash(buffer), _sha256.ComputeHash(buffer)));
		}

		public void Add(Guid id, string key)
		{
			var buffer = GetBytes(key);

			_databaseGateway.ExecuteUsing(_eventStoreDataSource, _queryFactory.Add(id, _md5.ComputeHash(buffer), _sha256.ComputeHash(buffer)));
		}
	}
}