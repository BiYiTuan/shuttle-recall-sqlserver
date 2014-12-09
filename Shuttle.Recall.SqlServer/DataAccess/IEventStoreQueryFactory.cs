using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer
{
	public interface IEventStoreQueryFactory
	{
		IQuery Get(Guid id);
		IQuery GetVersion(Guid id);
		IQuery Add(Guid id, Event @event, byte[] data);
	}
}