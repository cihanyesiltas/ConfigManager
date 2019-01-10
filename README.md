# ConfigManager

Projenin amacı dinamik bir konfigürasyon yapısı ile web.config, app.config gibi dosyalarda tutulan appkey’lerin ortak bir yapıyla erişilebilir olması ve deployment veya restart, recycle gerektirmeden güncellemelerin yapılabilir olmasıdır. Kütüphane veritabanına ulaşamadığı durumda son başarılı konfigürasyon kayıtları ile çalışmaktadır.

*ConfigManager.Core* kütüphanesi dll olarak eklenip kullanılabilmektedir. Farklı framework'leri desteklemesi açısından .net standard 2.0 olarak  oluşturulmuştur. Verilerin tutulması için `MongoDB` ve `PostgreSQL` desteklemektedir. Ayrıca cache için de 
`Redis` kullanılmaktadır.

## Kullanım
Kütüphane projeye eklendikten sonra aşağıdaki gibi kullanılabilir. 
```
IConfigurationReaderFactory readerFactory = new ConfigurationReaderFactory();
IConfigurationReader reader = readerFactory.Create("ApplicationName", connection, refreshTimeIntervalInMs));
var value = reader.GetValue<string>("TestKey");
```
**ApplicationName:** Uygulamanızın adı. Her uygulama kendi verilerine ulaşabilmesi için verilecek tekil ad.

**Connection:** Verilerin tutulacağı depolama alanı bilgileri. Örn: ```new Connection("connectionString", StorageProviderType.MongoDb))```

**RefreshTimeIntervalInMs:** ConfigurationReader bu süre aralığıyla veritabanına yeni eklenen veya güncellenen bilgiler varsa cache'i günceller.

## Test Veritabanı Ortamları

Testler aşamasında veritabanları için aşağıdaki `docker` komutları kullanılmıştır.

PostgreSQL docker container
```
docker run -p 1500:5432 --name cont-postgres -e POSTGRES_PASSWORD=pass -d postgres
```

Mongodb docker container
```
docker run --name database -d -p 27017:27017 mongo --noauth --bind_ip=0.0.0.0
```
