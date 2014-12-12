using Shuttle.Recall.SqlServer;

namespace Shuttle.ESB.SqlServer.Tests
{
	public class EventStoreSectionFixture
	{
		protected EventStoreSection GetSection(string file)
		{
			return EventStoreSection.Open(string.Format(@".\Section\files\{0}", file));
		}
	}
}