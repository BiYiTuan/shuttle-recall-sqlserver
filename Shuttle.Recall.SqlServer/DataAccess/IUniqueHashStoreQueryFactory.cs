using System;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public interface IUniqueHashStoreQueryFactory
	{
		IQuery Get(int indexType, byte[] hash);
		IQuery Remove(int indexType, byte[] hash);
		IQuery Add(Guid id, int indexType, byte[] hash);
	}
}