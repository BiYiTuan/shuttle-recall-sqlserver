using NUnit.Framework;
using Shuttle.ESB.SqlServer.Tests;

namespace Shuttle.Recall.SqlServer.Tests
{
	[TestFixture]
	public class EventStoreSectionTest : EventStoreSectionFixture
	{
		[Test]
		[TestCase("event-store.config")]
		[TestCase("event-store-grouped.config")]
		public void Should_be_able_to_load_a_full_configuration(string file)
		{
			var section = GetSection(file);

			Assert.IsNotNull(section);

			Assert.AreEqual("event-store-connection-string-name", section.ConnectionStringName);
		}
	}
}