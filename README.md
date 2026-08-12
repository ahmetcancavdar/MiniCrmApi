# MiniCrmApi

MiniCrmApi, müşteri yönetimi temelinde geliştirilmiş bir **ASP.NET Core Web API** projesidir. Projede CRM sistemlerinin temel mantığını göstermek amacıyla müşteri kayıtlarının oluşturulması, listelenmesi, güncellenmesi ve silinmesi işlemleri API üzerinden yapılmaktadır.

Proje; **katmanlı mimari**, **Entity Framework Core**, **Code First yaklaşımı**, **Repository Pattern**, **Service Layer**, **DTO kullanımı** ve **SQL Server veritabanı** mantığını öğrenmek ve uygulamak amacıyla hazırlanmıştır.

---

## Projenin Amacı

Bu projenin temel amacı, gerçek hayatta kullanılabilecek küçük ölçekli bir CRM API altyapısı oluşturmaktır.

Bu API üzerinden:

* Müşteri kaydı oluşturulabilir.
* Müşteri listesi alınabilir.
* Belirli bir müşteri detayına ulaşılabilir.
* Müşteri bilgileri güncellenebilir.
* Müşteri kaydı soft delete mantığıyla silinebilir.
* Entity Framework Core ile veritabanı yönetimi yapılabilir.
* Swagger üzerinden endpointler test edilebilir.

---

## Kullanılan Teknolojiler

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **Entity Framework Core SQL Server**
* **Entity Framework Core Tools**
* **SQL Server LocalDB**
* **Swagger / Swashbuckle**
* **Code First Migration**
* **Repository Pattern**
* **Service Layer**
* **DTO yapısı**

---

## Proje Mimarisi

Proje, sorumlulukların ayrıldığı basit ve anlaşılır bir katmanlı mimariyle geliştirilmiştir.

Genel akış şu şekildedir:

```text
Client / Swagger / Postman
        |
        v
CustomersController
        |
        v
ICustomerService / CustomerService
        |
        v
ICustomerRepository / CustomerRepository
        |
        v
AppDbContext
        |
        v
SQL Server Database
```

Bu yapı sayesinde controller doğrudan veritabanı ile konuşmaz. Controller sadece HTTP isteklerini karşılar ve iş mantığını service katmanına devreder. Service katmanı iş kurallarını yönetir. Repository katmanı ise veritabanı işlemlerinden sorumludur.

---

## Klasör Yapısı

```text
MiniCrmApi
│
├── Controllers
│   └── CustomersController.cs
│
├── DTOs
│   ├── CreateCustomerDto.cs
│   ├── CustomerDto.cs
│   └── UpdateCustomerDto.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Domain
│   ├── BaseEntity.cs
│   ├── Customer.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Product.cs
│
├── Migrations
│   └── InitialCreate migration dosyaları
│
├── Repositories
│   ├── ICustomerRepository.cs
│   └── CustomerRepository.cs
│
├── Services
│   ├── ICustomerService.cs
│   └── CustomerService.cs
│
├── Program.cs
├── appsettings.json
└── MiniCrmApi.csproj
```

---

## Katmanların Görevleri

### Controllers

`Controllers` klasörü API endpointlerinin bulunduğu katmandır. HTTP istekleri burada karşılanır.

Bu projede `CustomersController` müşteri işlemlerini yönetir.

Desteklenen temel işlemler:

* `GET /api/customers`
* `GET /api/customers/{id}`
* `POST /api/customers`
* `PUT /api/customers/{id}`
* `DELETE /api/customers/{id}`

Controller katmanı iş mantığını doğrudan içermez. Gelen isteği ilgili service metoduna yönlendirir.

---

### Services

`Services` klasörü uygulamanın iş mantığını içerir.

`CustomerService`, müşteri işlemleri için gerekli kuralları ve dönüşümleri yönetir.

Bu katmanda:

* Entity nesneleri DTO nesnelerine dönüştürülür.
* Create ve update işlemleri yönetilir.
* Silme işlemi fiziksel silme yerine soft delete olarak yapılır.
* Repository katmanı ile controller arasında ara katman görevi görür.

---

### Repositories

`Repositories` klasörü veritabanı işlemlerinden sorumludur.

`CustomerRepository`, `AppDbContext` üzerinden müşteri tablosuna erişir.

Bu katmanda:

* Aktif müşteriler listelenir.
* Id değerine göre müşteri aranır.
* Yeni müşteri eklenir.
* Müşteri güncellenir.
* Değişiklikler veritabanına kaydedilir.

Repository katmanı sayesinde veritabanı erişimi controller ve service katmanlarından ayrılmış olur.

---

### DTOs

`DTOs` klasörü API üzerinden alınan veya döndürülen veri modellerini içerir.

DTO kullanılmasının temel amacı, entity sınıflarını doğrudan dış dünyaya açmamaktır.

Bu projede kullanılan DTO sınıfları:

* `CreateCustomerDto`: Yeni müşteri oluşturmak için kullanılır.
* `UpdateCustomerDto`: Müşteri bilgilerini güncellemek için kullanılır.
* `CustomerDto`: API response olarak müşteri bilgilerini döndürmek için kullanılır.

---

### Domain

`Domain` klasörü veritabanı entity sınıflarını içerir.

Projede bulunan temel entity sınıfları:

* `BaseEntity`
* `Customer`
* `Product`
* `Order`
* `OrderItem`

`BaseEntity`, ortak alanları tutar:

```csharp
Id
CreatedDate
UpdatedDate
IsDeleted
```

Bu sayede tüm entity sınıflarında ortak alanlar tekrar tekrar yazılmaz.

---

### Data

`Data` klasöründe `AppDbContext` sınıfı bulunur.

`AppDbContext`, Entity Framework Core ile veritabanı bağlantısını ve tablo ilişkilerini yönetir.

Tanımlı DbSet yapıları:

```csharp
Customers
Products
Orders
OrderItems
```

Ayrıca müşteri-sipariş, sipariş-sipariş kalemi ve ürün-sipariş kalemi ilişkileri de bu sınıf içinde yapılandırılmıştır.

---

## Veritabanı Yapısı

Proje SQL Server LocalDB kullanacak şekilde ayarlanmıştır.

`appsettings.json` içindeki bağlantı cümlesi:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MiniCrmDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Bu ayar ile proje çalıştırıldığında `MiniCrmDb` isimli bir veritabanı kullanılır.

---

## API Endpointleri

### Müşterileri Listeleme

```http
GET /api/customers
```

Tüm aktif müşterileri listeler.

---

### Id ile Müşteri Getirme

```http
GET /api/customers/{id}
```

Belirtilen id değerine sahip müşteriyi getirir.

Örnek:

```http
GET /api/customers/1
```

---

### Yeni Müşteri Oluşturma

```http
POST /api/customers
```

Örnek request body:

```json
{
  "fullName": "Ahmet Can Çavdar",
  "email": "ahmet@example.com",
  "phone": "05555555555",
  "companyName": "Mini CRM Ltd."
}
```

---

### Müşteri Güncelleme

```http
PUT /api/customers/{id}
```

Örnek request body:

```json
{
  "fullName": "Ahmet Can Çavdar",
  "email": "ahmetcan@example.com",
  "phone": "05551234567",
  "companyName": "Mini CRM Updated"
}
```

---

### Müşteri Silme

```http
DELETE /api/customers/{id}
```

Bu işlem müşteriyi veritabanından fiziksel olarak silmez. Bunun yerine `IsDeleted` alanını `true` yapar. Bu yaklaşıma **soft delete** denir.

---

## Kurulum

### 1. Repoyu Klonlama

```bash
git clone https://github.com/ahmetcancavdar/MiniCrmApi.git
cd MiniCrmApi
```

---

### 2. Gerekli Paketleri Yükleme

```bash
dotnet restore
```

---

### 3. Entity Framework Tool Kurulumu

Bilgisayarınızda EF Core CLI aracı yüklü değilse şu komutla yükleyebilirsiniz:

```bash
dotnet tool install --global dotnet-ef
```

Daha önce yüklüyse güncellemek için:

```bash
dotnet tool update --global dotnet-ef
```

---

### 4. Veritabanını Oluşturma

Projede migration dosyaları bulunduğu için veritabanını oluşturmak için şu komut çalıştırılır:

```bash
dotnet ef database update
```

Bu komut, `appsettings.json` içinde tanımlı connection string üzerinden `MiniCrmDb` veritabanını oluşturur.

---

### 5. Projeyi Çalıştırma

```bash
dotnet run
```

Proje çalıştıktan sonra terminalde uygulamanın hangi portta ayağa kalktığı görünecektir.

Varsayılan geliştirme adresleri:

```text
https://localhost:7219
http://localhost:5238
```

Swagger arayüzüne gitmek için:

```text
https://localhost:7219/swagger
```

veya

```text
http://localhost:5238/swagger
```

---

## Swagger Kullanımı

Proje çalıştırıldığında Swagger arayüzü otomatik olarak açılabilir.

Swagger üzerinden:

* Endpointler görüntülenebilir.
* Request body örnekleri girilebilir.
* API cevapları test edilebilir.
* CRUD işlemleri doğrudan tarayıcı üzerinden denenebilir.

---

## Örnek Kullanım Senaryosu

1. Proje çalıştırılır.
2. Swagger arayüzü açılır.
3. `POST /api/customers` endpointi ile yeni müşteri oluşturulur.
4. `GET /api/customers` endpointi ile müşteri listesi kontrol edilir.
5. `PUT /api/customers/{id}` ile müşteri bilgileri güncellenir.
6. `DELETE /api/customers/{id}` ile müşteri soft delete yapılır.
7. Tekrar listeleme yapıldığında silinen müşteri aktif listede görünmez.

---

## Mimari Avantajlar

Bu projede kullanılan mimari yapı, küçük ölçekli bir API projesi için temiz ve anlaşılır bir temel sağlar.

Başlıca avantajları:

* Controller katmanı sade kalır.
* İş mantığı service katmanında toplanır.
* Veritabanı işlemleri repository katmanına ayrılır.
* DTO kullanımı sayesinde entity sınıfları doğrudan dışarı açılmaz.
* Dependency Injection ile bağımlılıklar yönetilir.
* Entity Framework Core ile Code First yaklaşımı uygulanır.
* Soft delete mantığı ile kayıt geçmişi korunabilir.
* Swagger ile API kolayca test edilebilir.

---

## Geliştirilebilir Özellikler

Bu proje temel bir CRM API altyapısı sunduğu için ileride şu özellikler eklenebilir:

* Ürün yönetimi endpointleri
* Sipariş oluşturma endpointleri
* Sipariş kalemi yönetimi
* Kullanıcı kayıt ve giriş sistemi
* JWT Authentication
* Role-based Authorization
* Admin ve müşteri rolleri
* Global exception handling
* FluentValidation ile gelişmiş validasyon
* Pagination, filtering ve searching
* Unit test ve integration test yapısı
* Docker desteği
* CI/CD pipeline entegrasyonu

---

## Genel Değerlendirme

MiniCrmApi, ASP.NET Core Web API ve Entity Framework Core öğrenmek için geliştirilmiş temiz ve geliştirilebilir bir başlangıç projesidir. Projede müşteri yönetimi üzerinden temel CRUD işlemleri uygulanmış, veritabanı işlemleri katmanlı mimariyle ayrılmış ve API testleri için Swagger desteği eklenmiştir.

Bu yapı, ileride daha kapsamlı bir CRM veya sipariş yönetim sistemine dönüştürülebilecek sağlam bir temel sunar.
