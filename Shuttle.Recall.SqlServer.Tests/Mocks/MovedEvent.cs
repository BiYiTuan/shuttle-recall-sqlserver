﻿using System;

namespace Shuttle.Recall.SqlServer.Tests
{
	public class MovedEvent
	{
		public string Address { get; set; }
		public DateTime DateMoved { get; set; }
		public DateTime DateRegistered { get; set; }
	}
}