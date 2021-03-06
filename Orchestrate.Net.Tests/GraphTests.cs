﻿using System;
using System.Linq;
using NUnit.Framework;
using Orchestrate.Net.Tests.Helpers;

namespace Orchestrate.Net.Tests
{
    [TestFixture]
    public class GraphTests
    {
        private const string CollectionName = "GraphTestCollection";
        private GraphClient _graphClient;
		private CollectionClient _orchestrate;

	    [TestFixtureSetUp]
        public static void ClassInitialize()
        {
			var orchestrate = new CollectionClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
            var item = new TestData {Id = 1, Value = "Inital Test Item"};

            orchestrate.CreateCollection(CollectionName, "1", item);
        }

        [TestFixtureTearDown]
        public static void ClassCleanUp()
        {
			var orchestrate = new CollectionClient(new Communication(new OrchestrateCredentials(TestHelper.ApiKey)));
            orchestrate.DeleteCollection(CollectionName);
            orchestrate.DeleteCollection("GraphTestCollection2");
            orchestrate.DeleteCollection("GraphTestCollection3");
        }

        [SetUp]
        public void TestInitialize()
        {
	        var communication = new Communication(new OrchestrateCredentials(TestHelper.ApiKey));
	        _graphClient = new GraphClient(communication);
			_orchestrate = new CollectionClient(communication);
        }

	    [TearDown]
        public void TestCleanup()
        {
        }

        [Test]
        public void AddGraph()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);

            var result = _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
        public void AddGraphAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);

            var result =
                _graphClient.PutGraphAsync(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2").Result;

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
        public void AddGraphWithNoCollectionName()
        {
            try
            {
                _graphClient.PutGraph(string.Empty, "1", "toplevelgraph", "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoCollectionNameAsync()
        {
            try
            {
                var result =
                    _graphClient.PutGraphAsync(string.Empty, "1", "toplevelgraph", "GraphTestCollection2", "2").Result;
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
        public void AddGraphWithNoKey()
        {
            try
            {
                _graphClient.PutGraph(CollectionName, string.Empty, "toplevelgraph", "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "key");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoKeyAsync()
        {
            try
            {
                var result = _graphClient.PutGraphAsync(CollectionName, string.Empty, "toplevelgraph", "GraphTestCollection2", "2").Result;
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
        public void AddGraphWithNoKind()
        {
            try
            {
                _graphClient.PutGraph(CollectionName, "1", string.Empty, "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "kind");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoKindAsync()
        {
            try
            {
                var result = _graphClient.PutGraphAsync(CollectionName, "1", string.Empty, "GraphTestCollection2", "2").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "kind");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoToCollectionName()
        {
            try
            {
                _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", string.Empty, "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "toCollectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoToCollectionNameAsync()
        {
            try
            {
                var result = _graphClient.PutGraphAsync(CollectionName, "1", "toplevelgraph", string.Empty, "2").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "toCollectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoToKey()
        {
            try
            {
                _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "toKey");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void AddGraphWithNoToKeyAsync()
        {
            try
            {
                var result = _graphClient.PutGraphAsync(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", string.Empty).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "toKey");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void GetSingleGraph()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            var result = _graphClient.GetGraph(CollectionName, "1", kinds);

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void GetSingleGraphAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            var result = _graphClient.GetGraphAsync(CollectionName, "1", kinds).Result;

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void GetMultipleLevleGraph()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            data = new TestData {Id = 3, Value = "This is collection 3 data"};
            _orchestrate.CreateCollection("GraphTestCollection3", "3", data);
            _graphClient.PutGraph("GraphTestCollection2", "2", "sublevelgraph", "GraphTestCollection3", "3");

            var kinds = new[] {"toplevelgraph", "sublevelgraph"};

            var result = _graphClient.GetGraph(CollectionName, "1", kinds);

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void GetMultipleLevleGraphAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            data = new TestData {Id = 3, Value = "This is collection 3 data"};
            _orchestrate.CreateCollection("GraphTestCollection3", "3", data);
            _graphClient.PutGraph("GraphTestCollection2", "2", "sublevelgraph", "GraphTestCollection3", "3");

            var kinds = new[] {"toplevelgraph", "sublevelgraph"};

            var result = _graphClient.GetGraphAsync(CollectionName, "1", kinds).Result;

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void GetSingleGraphWithNoCollectionName()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            try
            {
                _graphClient.GetGraph(string.Empty, "1", kinds);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void GetSingleGraphWithNoCollectionNameAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            try
            {
                var result = _graphClient.GetGraphAsync(string.Empty, "1", kinds).Result;
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
        public void GetSingleGraphWithNoKey()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            try
            {
                _graphClient.GetGraph(CollectionName, string.Empty, kinds);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "key");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void GetSingleGraphWithNoKeyAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            try
            {
                var result = _graphClient.GetGraphAsync(CollectionName, string.Empty, kinds).Result;
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
        public void GetSingleGraphWithNoKinds()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            try
            {
                _graphClient.GetGraph(CollectionName, "1", null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "kinds");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void GetSingleGraphWithNoKindsAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            try
            {
                var result = _graphClient.GetGraphAsync(CollectionName, "1", null).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "kinds");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraph()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            _graphClient.DeleteGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var kinds = new[] {"toplevelgraph"};

            try
            {
                _graphClient.GetGraph(CollectionName, "1", kinds);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("404"));
            }
        }

        [Test]
        public void DeleteGraphAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var result = _graphClient.DeleteGraphAsync(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2").Result;
            var kinds = new[] {"toplevelgraph"};

            try
            {
                _graphClient.GetGraph(CollectionName, "1", kinds);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("404"));
            }
        }

        [Test]
        public void DeleteGraphPurge()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var result = _graphClient.DeleteGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
        public void DeleteGraphPurgeAsync()
        {
            var data = new TestData {Id = 2, Value = "This is collection 2 data"};
            _orchestrate.CreateCollection("GraphTestCollection2", "2", data);
            _graphClient.PutGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2");
            var result = _graphClient.DeleteGraphAsync(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", "2").Result;

            Assert.IsTrue(result.Value == null || result.Value.ToString() == string.Empty);
        }

        [Test]
        public void DeleteGraphWithNoCollectionName()
        {
            try
            {
                _graphClient.DeleteGraph(string.Empty, "1", "toplevelgraph", "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoCollectionNameAsync()
        {
            try
            {
                var result = _graphClient.DeleteGraphAsync(string.Empty, "1", "toplevelgraph", "GraphTestCollection2", "2").Result;
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
        public void DeleteGraphWithNoKey()
        {
            try
            {
                _graphClient.DeleteGraph(CollectionName, string.Empty, "toplevelgraph", "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "key");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoKeyAsync()
        {
            try
            {
                var result = _graphClient.DeleteGraphAsync(CollectionName, string.Empty, "toplevelgraph", "GraphTestCollection2", "2").Result;
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
        public void DeleteGraphWithNoKind()
        {
            try
            {
                _graphClient.DeleteGraph(CollectionName, "1", string.Empty, "GraphTestCollection2", "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "kind");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoKindAsync()
        {
            try
            {
                var result = _graphClient.DeleteGraphAsync(CollectionName, "1", string.Empty, "GraphTestCollection2", "2").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "kind");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoToCollectionName()
        {
            try
            {
                _graphClient.DeleteGraph(CollectionName, "1", "toplevelgraph", string.Empty, "2");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "toCollectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoToCollectionNameAsync()
        {
            try
            {
                var result = _graphClient.DeleteGraphAsync(CollectionName, "1", "toplevelgraph", string.Empty, "2").Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "toCollectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoToKey()
        {
            try
            {
                _graphClient.DeleteGraph(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "toKey");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [Test]
        public void DeleteGraphWithNoToKeyAsync()
        {
            try
            {
                var result = _graphClient.DeleteGraphAsync(CollectionName, "1", "toplevelgraph", "GraphTestCollection2", string.Empty).Result;
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerExceptions.First() as ArgumentNullException;
                Assert.IsTrue(inner.ParamName == "toKey");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }
    }
}