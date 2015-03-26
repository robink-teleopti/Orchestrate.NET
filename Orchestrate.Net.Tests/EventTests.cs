using System;
using System.Linq;
using NUnit.Framework;
using Orchestrate.Net.Tests.Helpers;

namespace Orchestrate.Net.Tests
{
	[TestFixture]
	public class EventTests
	{
		private const string CollectionName = "EventTestCollection";
		private EventClient _eventClient;

		[TestFixtureSetUp]
		public static void ClassInitialize()
		{
			var orchestrate = new CollectionClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
			var item = new TestData { Id = 1, Value = "Inital Test Item" };

			orchestrate.CreateCollection(CollectionName, "1", item);
		}

		[TestFixtureTearDown]
		public static void ClassCleanUp()
		{
			var orchestrate = new CollectionClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
			orchestrate.DeleteCollection(CollectionName);
		}

		[SetUp]
		public void TestInitialize()
		{
			_eventClient = new EventClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
		}

		[TearDown]
		public void TestCleanup()
		{
			// nothing to see here...
		}

		[Test]
		public void PutEventNowTimeStamp()
		{
			var result = _eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the PutEventNowTimeStamp comment.");

			Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
		}

		[Test]
		public void PostEventNowTimeStamp()
		{
			var result = _eventClient.PostEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the PutEventNowTimeStamp comment.");

			Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
		}

        [Test]
        public void PutEventNowTimeStampAsync()
        {
            var result = _eventClient.PutEventAsync(CollectionName, "1", "comment", DateTime.UtcNow, "This is the PutEventNowTimeStamp comment.").Result;

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
		public void PutEventNoTimeStamp()
		{
			var result = _eventClient.PutEvent(CollectionName, "1", "comment", null, "This is the PutEventNoTimeStamp comment.");

			Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
		}

        [Test]
        public void PutEventNoTimeStampAsync()
        {
            var result = _eventClient.PutEventAsync(CollectionName, "1", "comment", null, "This is the PutEventNoTimeStamp comment.").Result;

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
		public void PutEventWithNoCollectionName()
		{
			try
			{
				_eventClient.PutEvent(string.Empty, "1", "comment", null, "This is the PutEventWithNoCollectionName comment.");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "collectionName");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void PutEventWithNoCollectionNameAsync()
        {
            try
            {
                var result = _eventClient.PutEventAsync(string.Empty, "1", "comment", null, "This is the PutEventWithNoCollectionName comment.").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
		public void PutEventWithNoKey()
		{
			try
			{
				_eventClient.PutEvent(CollectionName, string.Empty, "comment", null, "This is the PutEventWithNoKey comment.");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "key");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void PutEventWithNoKeyAsync()
        {
            try
            {
                var result = _eventClient.PutEventAsync(CollectionName, string.Empty, "comment", null, "This is the PutEventWithNoKey comment.").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "key");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
		public void PutEventWithNoType()
		{
			try
			{
				_eventClient.PutEvent(CollectionName, "1", string.Empty, null, "This is the PutEventWithNoType comment.");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "type");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void PutEventWithNoTypeAsync()
        {
            try
            {
                var result = _eventClient.PutEventAsync(CollectionName, "1", string.Empty, null, "This is the PutEventWithNoType comment.").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "type");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
		public void GetEventsNoStartEnd()
		{
			_eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsNoStartEnd comment.");
			var result = _eventClient.GetEvents(CollectionName, "1", "comment");

			Assert.IsTrue(result.Count > 0);
		}

        [Test]
        public void GetEventsNoStartEndAsync()
        {
            _eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsNoStartEnd comment.");
            var result = _eventClient.GetEventsAsync(CollectionName, "1", "comment").Result;

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
		public void GetEventsWithStartDate()
		{
			_eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithStartDate comment.");
			var result = _eventClient.GetEvents(CollectionName, "1", "comment", DateTime.UtcNow.AddHours(-1));

			Assert.IsTrue(result.Count > 0);
		}

        [Test]
        public void GetEventsWithStartDateAsync()
        {
            _eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithStartDate comment.");
            var result = _eventClient.GetEventsAsync(CollectionName, "1", "comment", DateTime.UtcNow.AddHours(-1)).Result;

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
		public void GetEventsWithEndDate()
		{
			_eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithEndDate comment.");
			var result = _eventClient.GetEvents(CollectionName, "1", "comment", null, DateTime.UtcNow.AddHours(1));

			Assert.IsTrue(result.Count > 0);
		}

        [Test]
        public void GetEventsWithEndDateAsync()
        {
            _eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithEndDate comment.");
            var result = _eventClient.GetEventsAsync(CollectionName, "1", "comment", null, DateTime.UtcNow.AddHours(1)).Result;

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
		public void GetEventsWithStartAndEndDate()
		{
			_eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithStartAndEndDate comment.");
			var result = _eventClient.GetEvents(CollectionName, "1", "comment", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

			Assert.IsTrue(result.Count > 0);
		}

        [Test]
        public void GetEventsWithStartAndEndDateAsync()
        {
            _eventClient.PutEvent(CollectionName, "1", "comment", DateTime.UtcNow, "This is the GetEventsWithStartAndEndDate comment.");
            var result = _eventClient.GetEventsAsync(CollectionName, "1", "comment", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1)).Result;

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
		public void GetEventsWithNoCollectionName()
		{
			try
			{
				_eventClient.GetEvents(string.Empty, "1", "comment");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "collectionName");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void GetEventsWithNoCollectionNameAsync()
        {
            try
            {
                var result = _eventClient.GetEventsAsync(string.Empty, "1", "comment").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
		public void GetEventsWithNoKey()
		{
			try
			{
				_eventClient.GetEvents(CollectionName, string.Empty, "comment");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "key");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void GetEventsWithNoKeyAsync()
        {
            try
            {
                var result = _eventClient.GetEventsAsync(CollectionName, string.Empty, "comment").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "key");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
		public void GetEventsWithNoType()
		{
			try
			{
				_eventClient.GetEvents(CollectionName, "1", string.Empty);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "type");
				return;
			}

			Assert.Fail("No Exception Thrown");
		}

        [Test]
        public void GetEventsWithNoTypeAsync()
        {
            try
            {
                var result = _eventClient.GetEventsAsync(CollectionName, "1", string.Empty).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "type");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }
    }
}