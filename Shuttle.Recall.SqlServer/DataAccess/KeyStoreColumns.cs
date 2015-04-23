using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public class KeyStoreColumns
	{
		public static readonly MappedColumn<Guid> Id = new MappedColumn<Guid>("Id", DbType.Guid);
		public static readonly MappedColumn<byte[]> MD5Hash = new MappedColumn<byte[]>("MD5Hash", DbType.Binary);
		public static readonly MappedColumn<byte[]> SHA256Hash = new MappedColumn<byte[]>("SHA256Hash", DbType.Binary);
	}
}