using System;
using Shuttle.Core.Data;

namespace Shuttle.Recall.SqlServer
{
	public class KeyStoreQueryFactory : IKeyStoreQueryFactory
	{
		public IQuery Get(byte[] md5Hash, byte[] sha256Hash)
		{
			return RawQuery.Create(@"select Id from dbo.KeyStore where [MD5Hash] = @MD5Hash and [SHA256Hash] = @SHA256Hash")
					.AddParameterValue(KeyStoreColumns.MD5Hash, md5Hash)
					.AddParameterValue(KeyStoreColumns.SHA256Hash, sha256Hash);
		}

		public IQuery Add(Guid id, byte[] md5Hash, byte[] sha256Hash)
		{
			return
				RawQuery.Create(@"
insert into dbo.KeyStore
	(
		Id,
		[MD5Hash],
		[SHA256Hash]
	)
values
	(
		@Id,
		@MD5Hash,
		@SHA256Hash
	)
")
					.AddParameterValue(KeyStoreColumns.Id, id)
					.AddParameterValue(KeyStoreColumns.MD5Hash, md5Hash)
					.AddParameterValue(KeyStoreColumns.SHA256Hash, sha256Hash);
		}

		public IQuery Remove(byte[] md5Hash, byte[] sha256Hash)
		{
			return RawQuery.Create(@"delete from dbo.KeyStore where [MD5Hash] = @MD5Hash and [SHA256Hash] = @SHA256Hash")
					.AddParameterValue(KeyStoreColumns.MD5Hash, md5Hash)
					.AddParameterValue(KeyStoreColumns.SHA256Hash, sha256Hash);
		}
	}
}