using System;
using Shuttle.Core.Data;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public interface IEventStoreQueryFactory
	{
		IQuery Get(Guid id, int i);
		IQuery GetVersion(Guid id);
		IQuery AddEvent(Guid id, Event @event, byte[] data);
		IQuery GetSnapshot(Guid id);
		IQuery SaveSnapshot(Guid id, Event @event, byte[] data);
	}
}