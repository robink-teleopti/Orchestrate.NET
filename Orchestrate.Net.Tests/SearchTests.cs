using System;
using System.Linq;
using NUnit.Framework;
using Orchestrate.Net.Tests.Helpers;

namespace Orchestrate.Net.Tests
{
	[TestFixture]
    public class SearchTests
    {
		    private const string CollectionName = "SearchTestCollection";
        private SearchClient _searchClient;

		[TestFixtureSetUp]
        public static void ClassInitialize()
        {
			var orchestrate = new CollectionClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));

            var item = new TestData { Id = 1, Value = "Inital Test Item" };
            var item2 = new TestData { Id = 2, Value = "Inital Test Item #2" };
            var item3 = new TestData { Id = 3, Value = "Inital Test Item #3" };

            orchestrate.CreateCollection(CollectionName, "1", item);
            orchestrate.Put(CollectionName, "2", item2);
            orchestrate.Put(CollectionName, "3", item3);
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
			_searchClient = new SearchClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
        }

		[TearDown]
        public void TestCleanup()
        {
            // nothing to see here...
        }

        [Test]
        public void SearchSuccess()
        {
            var result = _searchClient.Search(CollectionName, "*");

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void SearchSuccessAsync()
        {
            var result = _searchClient.SearchAsync(CollectionName, "*").Result;

            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void SearchNotFound()
        {
            var result = _searchClient.Search(CollectionName, "Id:9999");

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void SearchNotFoundAsync()
        {
            var result = _searchClient.SearchAsync(CollectionName, "Id:9999").Result;

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void SearchBadKey()
        {
            var result = _searchClient.Search(CollectionName, "NonExistantKey:9999");

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void SearchBadKeyAsync()
        {
            var result = _searchClient.SearchAsync(CollectionName, "NonExistantKey:9999").Result;

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void SearchWithNoCollectionName()
        {
            try
            {
                _searchClient.Search(string.Empty, "9999");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithNoCollectionNameAsync()
        {
            try
            {
                var result = _searchClient.SearchAsync(string.Empty, "9999").Result;
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
        public void SearchWithNoQuery()
        {
            try
            {
                _searchClient.Search(CollectionName, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "query");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithNoQueryAsync()
        {
            try
            {
                var result = _searchClient.SearchAsync(CollectionName, string.Empty).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "query");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithBadLimit()
        {
            try
            {
                _searchClient.Search(CollectionName, "*", -100);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsTrue(ex.ParamName == "limit");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithBadLimitAsync()
        {
            try
            {
                var result = _searchClient.SearchAsync(CollectionName, "*", -100).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentOutOfRangeException;
                Assert.IsTrue(inner.ParamName == "limit");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithBadOffset()
        {
            try
            {
                _searchClient.Search(CollectionName, "*", 10, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsTrue(ex.ParamName == "offset");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void SearchWithBadOffsetAsync()
        {
            try
            {
                var result = _searchClient.SearchAsync(CollectionName, "*", 10, -1).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentOutOfRangeException;
                Assert.IsTrue(inner.ParamName == "offset");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

    }
}
