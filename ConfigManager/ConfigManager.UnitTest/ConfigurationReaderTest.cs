
using ConfigManager.Core.Contracts;
using ConfigManager.Core.Managers;
using NSubstitute;
using NUnit.Framework;

namespace ConfigManager.UnitTest
{
    [TestFixture]
    public class ConfigurationReaderTest
    {
        private const string _applicationName = "services";
        private ICacheManager _mockCacheManager;
        private IStorageProvider _mockStorageProvider;

        [SetUp]
        public void Start()
        {
            _mockCacheManager = Substitute.For<ICacheManager>();
            _mockStorageProvider = Substitute.For<IStorageProvider>();
        }

        [Test]
        public void Add_EmptyName_NotAdded()
        {
            var configurationReader =
                new ConfigurationReader(_mockCacheManager, _mockStorageProvider, _applicationName, 1);

            var result = configurationReader.Add(null);

            Assert.False(result);
        }
    }
}
