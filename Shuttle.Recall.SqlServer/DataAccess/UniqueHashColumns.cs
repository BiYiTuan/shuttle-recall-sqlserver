using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public class UniqueHashColumns
	{
		public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
		public static readonly MappedColumn<byte[]> Hash = new MappedColumn<byte[]>("Hash", DbType.Binary);
		public static readonly MappedColumn<int> IndexType = new MappedColumn<int>("IndexType", DbType.Int32);
	}
}