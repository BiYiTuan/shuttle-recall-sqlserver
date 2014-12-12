using System;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public class UniqueHashStoreQueryFactory : IUniqueHashStoreQueryFactory
	{
		public IQuery Get(int indexType, byte[] hash)
		{
			return RawQuery.Create(@"select Id from dbo.UniqueHash where [IndexType] = @IndexType and [Hash] = @Hash")
					.AddParameterValue(UniqueHashColumns.IndexType, indexType)
					.AddParameterValue(UniqueHashColumns.Hash, hash);
		}

		public IQuery Remove(int indexType, byte[] hash)
		{
			return RawQuery.Create(@"delete from dbo.UniqueHash where [IndexType] = @IndexType and [Hash] = @Hash")
					.AddParameterValue(UniqueHashColumns.IndexType, indexType)
					.AddParameterValue(UniqueHashColumns.Hash, hash);
		}
	
		public IQuery Add(Guid id, int indexType, byte[] hash)
		{
			return
				RawQuery.Create(@"
insert into dbo.UniqueHash
	(
		Id,
		IndexType,
		Hash
	)
values
	(
		@Id,
		@IndexType,
		@Hash
	)
")
					.AddParameterValue(UniqueHashColumns.Id, id)
					.AddParameterValue(UniqueHashColumns.IndexType, indexType)
					.AddParameterValue(UniqueHashColumns.Hash, hash);
		}
	}
}