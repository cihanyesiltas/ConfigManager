using System.Collections.Generic;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Managers;
using NSubstitute;
using NUnit.Framework;

namespace ConfigManager.UnitTest
{
    [TestFixture]
    public class ConfigurationReaderTest
    {
        private const string ApplicationName = "services";
        private ICacheManager _mockCacheManager;
        private IStorageProvider _mockStorageProvider;

        [SetUp]
        public void Start()
        {
            _mockCacheManager = Substitute.For<ICacheManager>();
            _mockStorageProvider = Substitute.For<IStorageProvider>();
        }

        [Test]
        public void Add_EmptyRequest_NotAdded()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Add(null);

            Assert.False(result);
        }

        [Test]
        public void Add_EmptyName_NotAdded()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = string.Empty,
                Type = "Type",
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Add_EmptyValue_NotAdded()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = string.Empty
            });

            Assert.False(result);
        }

        [Test]
        public void Add_EmptyType_NotAdded()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = string.Empty,
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Add_ExistingName_NotAdded()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.Exists(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Add_ValidRequest_Added()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.Exists(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            _mockStorageProvider.Add(Arg.Any<AddStorageConfigurationDTO>()).Returns(true);

            var result = configurationReader.Add(new AddConfigurationDTO
            {
                IsActive = true,
                Name = "Name",
                Type = "Type",
                Value = "Value"
            });

            Assert.True(result);
        }

        [Test]
        public void Update_EmtpyRequest_NotUpdated()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Update(null);

            Assert.False(result);
        }

        [Test]
        public void Update_EmtpyType_NotUpdated()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = string.Empty,
                Value = "Value"
            });

            Assert.False(result);
        }

        [Test]
        public void Update_EmtpyValue_NotUpdated()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = "Type",
                Value = string.Empty
            });

            Assert.False(result);
        }

        [Test]
        public void Update_ValidRequest_Updated()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.Update(Arg.Any<UpdateConfigurationDTO>()).Returns(true);
          
            var result = configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = "1",
                IsActive = true,
                Type = "Type",
                Value = "Value"
            });

            Assert.True(result);
        }

        [Test]
        public void GetById_EmptyId_ReturnNull()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.GetById(string.Empty);

            Assert.Null(result);
        }

        [Test]
        public void GetById_ValidRequest_ReturnConfiguration()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.Get(Arg.Any<string>()).Returns(new ConfigurationDTO());

            var result = configurationReader.GetById("id");

            Assert.NotNull(result);
        }

        [Test]
        public void GetList_ReturnAllConfigurationList()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.GetList(Arg.Any<string>()).Returns(new List<ConfigurationDTO>
            {
                new ConfigurationDTO()
            });

            var result = configurationReader.GetAll();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void SearchByName_EmptyName_ReturnEmptyList()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            var result = configurationReader.SearchByName(string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void SearchByName_ExistingName_ReturnNotEmptyList()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockStorageProvider.Search(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<ConfigurationDTO>
            {
                new ConfigurationDTO()
            });

            var result = configurationReader.SearchByName("name");

            Assert.Greater(result.Count, 0);
        }

        [Test]
        public void GetValue_ExistingInCache_ReturnValue()
        {
            var keyValue = "value";

            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockCacheManager.Get<List<CacheConfigurationDTO>>(Arg.Any<string>()).Returns(
                new List<CacheConfigurationDTO>
                {
                    new CacheConfigurationDTO
                    {
                        Name = "name",
                        ApplicationName = ApplicationName,
                        Value = keyValue,
                        Type = "String"
                    }
                });

            var result = configurationReader.GetValue<string>("name");

            Assert.AreEqual(result, keyValue);
        }

        [Test]
        public void GetValue_NotExistingInCache_ReturnDefaultValue()
        {
            var configurationReader = new ConfigurationReader(_mockCacheManager, _mockStorageProvider, ApplicationName, 1);

            _mockCacheManager.Get<List<CacheConfigurationDTO>>(Arg.Any<string>()).Returns(
                new List<CacheConfigurationDTO>
                {
                    new CacheConfigurationDTO
                    {
                        Name = "name",
                        ApplicationName = ApplicationName,
                        Value = "test",
                        Type = "String"
                    }
                });

            var result = configurationReader.GetValue<string>("name2");

            Assert.AreEqual(result, default(string));
        }
    }
}
