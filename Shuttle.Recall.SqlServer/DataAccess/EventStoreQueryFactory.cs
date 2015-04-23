using System;
using Shuttle.Core.Data;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public class EventStoreQueryFactory : IEventStoreQueryFactory
	{
		public IQuery Get(Guid id, int afterVersion)
		{
			return
				RawQuery.Create(
					@"select Version, AssemblyQualifiedName, Data from [dbo].[EventStore] es inner join [dbo].[TypeStore] ts on (es.TypeId = ts.Id) where es.Id = @Id and es.Version > @Version order by Version")
					.AddParameterValue(EventStoreColumns.Id, id)
					.AddParameterValue(EventStoreColumns.Version, afterVersion);
		}

		public IQuery GetVersion(Guid id)
		{
			return
				RawQuery.Create(@"select isnull(max(Version),0) from [dbo].[EventStore] where Id = @Id")
					.AddParameterValue(EventStoreColumns.Id, id);
		}

		public IQuery AddEvent(Guid id, Event @event, byte[] data)
		{
			return
				EventQuery(@"
insert into[dbo].[EventStore]
	(
		Id,
		Version,
		TypeId,
		Data
	)
values
	(
		@Id,
		@Version,
		@typeId,
		@Data
	)")
				.AddParameterValue(EventStoreColumns.Id, id)
				.AddParameterValue(EventStoreColumns.Version, @event.Version)
				.AddParameterValue(EventStoreColumns.AssemblyQualifiedName, @event.AssemblyQualifiedName)
				.AddParameterValue(EventStoreColumns.Data, data);
		}

		public IQuery GetSnapshot(Guid id)
		{
			return
				RawQuery.Create(
					@"select Version, AssemblyQualifiedName, Data from [dbo].[EventStore] es inner join [dbo].[TypeStore] ts on (es.TypeId = ts.Id) where es.Id = @Id")
					.AddParameterValue(EventStoreColumns.Id, id);
		}

		public IQuery SaveSnapshot(Guid id, Event @event, byte[] data)
		{
			return
				EventQuery(@"
delete from [dbo].[SnapshotStore] where Id = @Id

insert into [dbo].[SnapshotStore]
	(
		Id,
		Version,
		TypeId,
		Data
	)
values
	(
		@Id,
		@Version,
		@typeId,
		@Data
	)")
				.AddParameterValue(SnapshotStoreColumns.Id, id)
				.AddParameterValue(SnapshotStoreColumns.Version, @event.Version)
				.AddParameterValue(SnapshotStoreColumns.AssemblyQualifiedName, @event.AssemblyQualifiedName)
				.AddParameterValue(SnapshotStoreColumns.Data, data);
		}

		private IQuery EventQuery(string sql)
		{
			return RawQuery.Create(string.Format(@"
declare @typeId uniqueidentifier

select @typeId = id from [dbo].[TypeStore] where AssemblyQualifiedName = @AssemblyQualifiedName

if (@typeId is null)
begin
	set @typeId = newid()
	
	begin try
		insert into [dbo].[TypeStore] (Id, AssemblyQualifiedName) values (@typeId, @AssemblyQualifiedName)
	end try
	begin catch
		if (ERROR_NUMBER() = 2601)
			select @typeId = id from [dbo].[TypeStore] where AssemblyQualifiedName = @AssemblyQualifiedName
	end catch
end

{0}
", sql));
		}
	}
}