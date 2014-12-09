using System;
using NUnit.Framework;
using Shuttle.Recall.Core;

namespace Shuttle.Recall.SqlServer.Tests
{
	public class EventStoreTests : Fixture
	{
		[Test]
		public void Should_be_able_to_use_event_stream()
		{
			var store = new EventStore(new DefaultSerializer(), DatabaseGateway, new EventStoreQueryFactory());
			var aggregate = new Aggregate(Guid.NewGuid());
			EventStream eventStream;

			using (Timer.Time("get empty"))
			using (DatabaseConnectionFactory.Create(EventStoreSource))
			{
				eventStream = store.Get(aggregate.Id);
			}

			var moveCommand = new MoveCommand();

			using (Timer.Time("adding events"))
				for (var i = 0; i < 10000; i++)
				{
					moveCommand = new MoveCommand
					{
						Address = string.Format("Address-{0}", i),
						DateMoved = DateTime.Now
					};

					eventStream.Add(aggregate.Move(moveCommand));
				}

			using (Timer.Time("saving event stream"))
			using (DatabaseConnectionFactory.Create(EventStoreSource))
			{
				store.Save(eventStream);
			}

			using (DatabaseConnectionFactory.Create(EventStoreSource))
			{
				aggregate = new Aggregate(aggregate.Id);

				using (Timer.Time("reading event stream"))
				{
					eventStream = store.Get(aggregate.Id);
				}

				using (Timer.Time("apply events"))
				{
					eventStream.Apply(aggregate);
				}
			}

			Assert.AreEqual(moveCommand.Address, aggregate.State.Location.Address);
			Assert.AreEqual(moveCommand.DateMoved, aggregate.State.Location.DateMoved);
		}
	}
}