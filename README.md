# ConfigManager

Projenin amacı dinamik bir konfigürasyon yapısı ile web.config, app.config gibi dosyalarda tutulan appkey’lerin ortak ve dinamik bir yapıyla erişilebilir olması ve deployment veya restart, recycle gerektirmeden güncellemelerin yapılabilir olmasıdır.

*ConfigManager.Core* kütüphanesi dll olarak eklenip kullanılabilmektedir. Farklı framework'leri desteklemesi açısından .net standard 2.0 olarak  oluşturulmuştur. Verilerin tutulması için `MongoDB` ve `PostgreSQL` desteklemektedir. Ayrıca cache için de `Redis` kullanılmaktadır.

## Kullanım
Kütüphaneyi projeye ekledikten sonra aşağıdaki gibi kullanılabilir. 
```
IConfigurationReaderFactory configurationReaderFactory = new ConfigurationReaderFactory();
IConfigurationReader reader = configurationReaderFactory.Create("ApplicationName", connection, refreshTimeIntervalInMs));
```
**ApplicationName:** Uygulamanızın adı. Her uygulama kendi verilerine ulaşabilmesi için verilecek ad.

**Connection:** Verilerin tutulacağı yerin bilgileri. (new Connection("connectionString", StorageProviderType.MongoDb))

**RefreshTimeIntervalInMs:** ConfigurationReader bu süre aralığıyla veritabanına yeni eklenen veya güncellenen bilgiler varsa cache'i günceller.
