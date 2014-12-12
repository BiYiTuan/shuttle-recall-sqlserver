using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public class EventStore : IEventStore
	{
		private readonly ISerializer _serializer;
		private readonly IDatabaseGateway _databaseGateway;
		private readonly IEventStoreQueryFactory _queryFactory;
		private readonly DataSource _eventStoreDataSource;

		public EventStore(IEventStoreConfiguration eventStoreConfiguration, ISerializer serializer, IDatabaseGateway databaseGateway, IEventStoreQueryFactory queryFactory)
		{
			Guard.AgainstNull(serializer, "serializer");
			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(queryFactory, "queryFactory");

			_serializer = serializer;
			_databaseGateway = databaseGateway;
			_queryFactory = queryFactory;

			_eventStoreDataSource = new DataSource(eventStoreConfiguration.ConnectionStringName, new SqlDbDataParameterFactory());
		}

		public EventStream Get(Guid id)
		{
			var table = _databaseGateway.GetDataTableFor(_eventStoreDataSource, _queryFactory.Get(id));
			var version = 0;
			var events = new List<Event>();

			foreach (DataRow row in table.Rows)
			{
				version = EventStoreColumns.Version.MapFrom(row);
				var assemblyQualifiedName = EventStoreColumns.AssemblyQualifiedName.MapFrom(row);

				using (var stream = new MemoryStream(EventStoreColumns.Data.MapFrom(row)))
				{
					events.Add(new Event(version, assemblyQualifiedName, _serializer.Deserialize(Type.GetType(assemblyQualifiedName), stream)));
				}
			}

			return new EventStream(id, version, events);
		}

		public void Save(EventStream eventStream)
		{
			Guard.AgainstNull(eventStream,"eventStream");

			eventStream.ConcurrencyInvariant(_databaseGateway.GetScalarUsing<int>(_eventStoreDataSource, _queryFactory.GetVersion(eventStream.Id)));

			foreach (var @event in eventStream.NewEvents())
			{
				using (var stream = _serializer.Serialize(@event.Data))
				{
					_databaseGateway.ExecuteUsing(_eventStoreDataSource, _queryFactory.Add(eventStream.Id, @event, stream.ToBytes()));
				}
			}
		}
	}
}