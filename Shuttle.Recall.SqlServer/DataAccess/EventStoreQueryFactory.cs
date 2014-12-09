using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public class EventStoreQueryFactory : IEventStoreQueryFactory
	{
		public IQuery Get(Guid id)
		{
			return
				RawQuery.Create(@"select Version, AssemblyQualifiedName, Data from dbo.EventStore where Id = @Id order by Version")
					.AddParameterValue(EventStoreColumns.Id, id);
		}

		public IQuery GetVersion(Guid id)
		{
			return
				RawQuery.Create(@"select isnull(max(Version),0) from dbo.EventStore where Id = @Id")
					.AddParameterValue(EventStoreColumns.Id, id);
		}

		public IQuery Add(Guid id, Event @event, byte[] data)
		{
			return
				RawQuery.Create(@"
insert into dbo.EventStore
	(
		Id,
		Version,
		AssemblyQualifiedName,
		Data
	)
values
	(
		@Id,
		@Version,
		@AssemblyQualifiedName,
		@Data
	)
")
					.AddParameterValue(EventStoreColumns.Id, id)
					.AddParameterValue(EventStoreColumns.Version, @event.Version)
					.AddParameterValue(EventStoreColumns.AssemblyQualifiedName, @event.AssemblyQualifiedName)
					.AddParameterValue(EventStoreColumns.Data, data);
		}
	}
}