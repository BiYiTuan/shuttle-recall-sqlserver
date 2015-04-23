using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public class SnapshotStoreColumns
	{
		public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
		public static readonly MappedColumn<int> Version = new MappedColumn<int>("Version", DbType.Int32);
		public static readonly MappedColumn<string> AssemblyQualifiedName = new MappedColumn<string>("AssemblyQualifiedName", DbType.AnsiString, 512);
		public static readonly MappedColumn<byte[]> Data = new MappedColumn<byte[]>("Data", DbType.Binary);
		public static readonly MappedColumn<DateTime> DateRegistered = new MappedColumn<DateTime>("DateRegistered", DbType.DateTime);
	}
}