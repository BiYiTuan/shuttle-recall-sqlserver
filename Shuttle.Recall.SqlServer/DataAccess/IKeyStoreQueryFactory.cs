using System;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public interface IKeyStoreQueryFactory
	{
		IQuery Get(byte[] md5Hash, byte[] sha256Hash);
		IQuery Add(Guid id, byte[] md5Hash, byte[] sha256Hash);
		IQuery Remove(byte[] md5Hash, byte[] sha256Hash);
	}
}