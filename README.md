# MiniCrm API

MiniCrm, müşteri yönetimi, ürün ve stok takibi, sepet ve sipariş akışı, destek talepleri, şikâyet yönetimi ve satış sonrası süreçleri tek bir backend üzerinde yöneten **ASP.NET Core Web API** projesidir.

Proje; katmanlı mimari, SOLID prensipleri, Repository + Service yaklaşımı, Entity Framework Core Code First, ASP.NET Core Identity, JWT tabanlı kimlik doğrulama, rol bazlı yetkilendirme, SMTP e-posta gönderimi ve merkezi hata yönetimi gibi gerçek bir backend uygulamasında kullanılan temel yaklaşımları içerecek şekilde geliştirilmiştir.

> Proje şu anda backend tarafında ana işlevlerini tamamlamış durumdadır. Modüller Swagger üzerinden manuel olarak test edilmiştir. Otomatik unit/integration test altyapısı sonraki geliştirme adımıdır.

---

## İçindekiler

- [Projenin Amacı](#projenin-amacı)
- [Teknolojiler](#teknolojiler)
- [Mimari](#mimari)
- [Proje Yapısı](#proje-yapısı)
- [Temel Özellikler](#temel-özellikler)
- [Kimlik Doğrulama ve Yetkilendirme](#kimlik-doğrulama-ve-yetkilendirme)
- [Sipariş Akışı](#sipariş-akışı)
- [Destek ve Satış Sonrası Süreçler](#destek-ve-satış-sonrası-süreçler)
- [Veritabanı ve EF Core](#veritabanı-ve-ef-core)
- [Hata Yönetimi ve Güvenlik](#hata-yönetimi-ve-güvenlik)
- [Kurulum](#kurulum)
- [User Secrets Yapılandırması](#user-secrets-yapılandırması)
- [Migration İşlemleri](#migration-işlemleri)
- [Projeyi Çalıştırma](#projeyi-çalıştırma)
- [Swagger Kullanımı](#swagger-kullanımı)
- [Önemli API Grupları](#önemli-api-grupları)
- [Test Durumu](#test-durumu)
- [Planlanan Geliştirmeler](#planlanan-geliştirmeler)

---

# Projenin Amacı

MiniCrm'in amacı klasik bir CRUD uygulamasının ötesine geçen, birbirine bağlı gerçek iş süreçleri içeren bir CRM/e-ticaret destek backend'i oluşturmaktır.

Sistem içerisinde:

- kullanıcı kayıt ve giriş işlemleri,
- müşteri profili ve adres yönetimi,
- kategori ve ürün yönetimi,
- stok takibi,
- kalıcı sepet,
- sipariş oluşturma,
- e-posta doğrulama,
- sipariş durum yönetimi,
- ticket sistemi,
- şikâyet yönetimi,
- müşteri-admin destek konuşmaları,
- iade/değişim/garanti/teknik destek süreçleri

birbiriyle ilişkili şekilde çalışmaktadır.

---

# Teknolojiler

Projede kullanılan temel teknolojiler:

- **.NET 10**
- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core 10**
- **EF Core Code First**
- **Microsoft SQL Server**
- **ASP.NET Core Identity**
- **JWT Bearer Authentication**
- **Role-Based Authorization**
- **OpenAPI / Swagger**
- **SMTP**
- **Dependency Injection**
- **Repository Pattern**
- **Service Layer**
- **Global Exception Handling**
- **ProblemDetails**
- **Rate Limiting**
- **Health Checks**

---

# Mimari

Proje 5 ayrı .NET projesinden oluşan katmanlı bir mimariye sahiptir.

```text
MiniCrm.sln
│
├── MiniCrm.Api
├── MiniCrm.Application
├── MiniCrm.Domain
├── MiniCrm.Persistence
└── MiniCrm.Infrastructure
```

Temel bağımlılık yapısı:

```text
MiniCrm.Api
   │
   ├── MiniCrm.Application
   ├── MiniCrm.Persistence
   └── MiniCrm.Infrastructure

MiniCrm.Application
   │
   └── MiniCrm.Domain

MiniCrm.Persistence
   │
   ├── MiniCrm.Application
   └── MiniCrm.Domain

MiniCrm.Infrastructure
   │
   └── MiniCrm.Application

MiniCrm.Domain
   │
   └── Bağımsız
```

## MiniCrm.Domain

Sistemin temel iş nesnelerini ve business rule'larını içerir.

Örnek entity'ler:

- Customer
- CustomerAddress
- Category
- Product
- StockMovement
- Cart
- CartItem
- Order
- OrderItem
- OrderVerification
- Ticket
- TicketMessage
- Complaint
- SupportConversation
- SupportMessage
- AfterSalesRequest
- EmailLog

Domain katmanı ayrıca:

- enum'ları,
- value object'leri,
- `DomainException`,
- entity davranışlarını

barındırır.

---

## MiniCrm.Application

Uygulamanın kullanım senaryolarını ve servis sözleşmelerini içerir.

Bu katmanda:

- DTO'lar,
- Repository interface'leri,
- Service interface'leri,
- Business service implementasyonları,
- Unit of Work interface'i,
- Role sabitleri

bulunur.

Örnek akış:

```text
Controller
   ↓
IOrderService
   ↓
OrderService
   ↓
IOrderRepository
   ↓
Persistence
```

---

## MiniCrm.Persistence

Veritabanı erişim katmanıdır.

İçerdiği ana bileşenler:

- `AppDbContext`
- EF Core entity configuration'ları
- Repository implementasyonları
- ASP.NET Core Identity entegrasyonu
- Migration dosyaları
- Unit of Work
- AuthService
- Identity seeding

---

## MiniCrm.Infrastructure

Dış servis ve teknik altyapı implementasyonlarını içerir.

Örnekler:

- JWT token üretimi
- SMTP e-posta gönderimi
- doğrulama kodu üretimi ve hash işlemleri
- sipariş numarası üretimi

---

## MiniCrm.Api

HTTP katmanıdır.

Bu katmanda:

- Controller'lar
- Authentication / Authorization middleware'i
- Swagger/OpenAPI
- Global exception handling
- ProblemDetails
- Rate Limiting
- Security Headers
- Health Check
- uygulama başlangıç konfigürasyonu

bulunur.

---

# Proje Yapısı

Özet klasör yapısı:

```text
MiniCrm
│
├── MiniCrm.Api
│   ├── Controllers
│   ├── ErrorHandling
│   ├── Middleware
│   ├── OpenApi
│   ├── Program.cs
│   └── appsettings.json
│
├── MiniCrm.Application
│   ├── Common
│   ├── DTOs
│   ├── Interfaces
│   │   ├── Repositories
│   │   └── Services
│   └── Services
│
├── MiniCrm.Domain
│   ├── Common
│   ├── Entities
│   ├── Enums
│   ├── Exceptions
│   └── ValueObjects
│
├── MiniCrm.Persistence
│   ├── Configurations
│   ├── Context
│   ├── Identity
│   ├── Migrations
│   ├── Repositories
│   ├── Seed
│   └── Services
│
├── MiniCrm.Infrastructure
│   ├── Authentication
│   ├── Email
│   ├── Orders
│   └── Security
│
└── MiniCrm.sln
```

---

# Temel Özellikler

## Müşteri Yönetimi

Müşteriler:

- sisteme kayıt olabilir,
- giriş yapabilir,
- profilini görüntüleyebilir,
- ad/telefon/şirket bilgilerini güncelleyebilir,
- birden fazla adres ekleyebilir,
- varsayılan adres belirleyebilir.

Kullanıcı e-posta adresi Identity hesabıyla ilişkili olduğu için profil güncellemesinde doğrudan değiştirilmez.

---

## Kategori Yönetimi

Admin:

- kategori oluşturabilir,
- güncelleyebilir,
- aktif/pasif hale getirebilir,
- soft delete uygulayabilir.

Silinmemiş kategori isimlerinde uniqueness korunur.

---

## Ürün Yönetimi

Ürün alanları:

```text
Name
Description
Price
StockQuantity
IsActive
SKU
Category
ImageUrl
```

Admin:

- ürün ekleyebilir,
- güncelleyebilir,
- aktif/pasif yapabilir,
- stok artırabilir,
- stok azaltabilir.

SKU alanında soft-delete uyumlu unique index kullanılmaktadır.

---

## Stok Yönetimi

Stok değişiklikleri yalnızca ürün üzerindeki güncel quantity değerini değiştirmekle kalmaz.

Her değişiklik ayrıca `StockMovement` tablosunda kayıt altına alınır.

Örnek hareket türleri:

```text
InitialStock
AdminIncrease
AdminDecrease
OrderConfirmed
OrderCancelledRestock
CustomerReturn
```

Böylece stok geçmişi audit edilebilir.

---

## Kalıcı Sepet

Her müşteri için veritabanında kalıcı bir sepet bulunur.

Desteklenen işlemler:

- ürün ekleme,
- quantity artırma,
- quantity değiştirme,
- ürün çıkarma,
- sepeti temizleme.

Sepet yalnızca RAM üzerinde tutulmaz; veritabanında saklanır.

---

# Kimlik Doğrulama ve Yetkilendirme

Sistem ASP.NET Core Identity ve JWT kullanmaktadır.

Roller:

```text
Admin
Customer
```

JWT içerisinde temel olarak:

- User ID
- Email
- Role
- JWT ID

claim'leri bulunur.

API endpoint'leri `[Authorize]` ve role bazlı authorization ile korunmaktadır.

Örnek:

```csharp
[Authorize(Roles = AppRoles.Admin)]
```

## HTTP davranışı

```text
Token yok / geçersiz token
→ 401 Unauthorized

Token geçerli fakat yetki yok
→ 403 Forbidden

Kaynak bulunamadı
→ 404 Not Found

Business rule ihlali
→ 400 Bad Request
```

---

# Sipariş Akışı

Sipariş sistemi aşağıdaki state akışını kullanır:

```text
PendingVerification
        ↓
     Confirmed
        ↓
     Preparing
        ↓
      Shipped
        ↓
     Delivered
```

İptal edilebilir uygun durumlarda:

```text
Confirmed / Preparing
        ↓
     Cancelled
```

## Checkout

Customer checkout yaptığında:

1. Sepet kontrol edilir.
2. Sipariş oluşturulur.
3. Sipariş `PendingVerification` durumuna gelir.
4. 6 haneli doğrulama kodu üretilir.
5. Kod doğrudan saklanmaz.
6. HMAC tabanlı hash saklanır.
7. SMTP üzerinden kullanıcıya kod gönderilir.

Doğrulama başarılı olduktan sonra:

```text
PendingVerification
       ↓
Confirmed
```

olur.

Bu aşamada:

- stok düşürülür,
- StockMovement oluşturulur,
- sepet temizlenir,
- doğrulama kaydı tamamlanır,
- confirmation e-postası gönderilir.

Doğrulama kodlarında:

- süre sonu kontrolü,
- maksimum başarısız deneme limiti,
- yeniden kod gönderme

mekanizmaları bulunmaktadır.

---

## Sipariş İptali

Uygun durumdaki sipariş iptal edildiğinde:

- sipariş `Cancelled` olur,
- cancellation reason tutulur,
- daha önce düşürülen stok geri eklenir,
- stock movement kaydı oluşturulur.

---

# Destek ve Satış Sonrası Süreçler

## Ticket Sistemi

Ticket'larda:

- müşteri ticket açabilir,
- admin cevap verebilir,
- müşteri tekrar mesaj gönderebilir,
- mesaj geçmişi DB'de tutulur.

Status akışı:

```text
Open
 ↓
InProgress
 ↓
WaitingForCustomer
 ↓
Resolved
 ↓
Closed
```

Resolved ticket'a müşteri tekrar mesaj gönderirse uygun durumda yeniden işlem sürecine alınabilir.

Closed ticket'a yeni mesaj gönderilemez.

---

## Complaint Management

Müşteri:

- siparişe bağlı veya bağımsız complaint oluşturabilir,
- kendi complaint kayıtlarını görüntüleyebilir.

Admin akışı:

```text
Open
 ↓
UnderReview
 ├──→ Resolved
 └──→ Rejected
          ↓
        Closed
```

Admin açıklamaları ve ilgili tarih alanları veritabanında saklanır.

---

## Support Conversation

Müşteri ile admin arasında DB tabanlı mesajlaşma sistemi bulunur.

```text
Customer
   ↕
SupportConversation
   ↕
Admin
```

Conversation:

```text
Open
 ↓
Closed
```

durumlarına sahiptir.

Closed conversation'a yeni mesaj gönderilemez.

Aynı müşterinin aynı anda birden fazla açık conversation oluşturması service katmanında engellenir.

---

## After-Sales Request

Satış sonrası süreçler:

```text
Return
Exchange
Warranty
TechnicalSupport
```

desteklenmektedir.

Akış:

```text
Requested
    ↓
UnderReview
   ↙      ↘
Approved  Rejected
   ↓
Completed
```

Customer ayrıca yalnızca `Requested` aşamasında:

```text
Requested
   ↓
Cancelled
```

işlemi yapabilir.

After-sales request yalnızca müşterinin kendisine ait ve `Delivered` durumundaki siparişlerde oluşturulabilir.

Talep quantity değeri satın alınan miktardan fazla olamaz.

---

# E-posta Sistemi

SMTP üzerinden aşağıdaki süreçlerde e-posta gönderilebilir:

- sipariş doğrulama kodu,
- sipariş onayı,
- sipariş durum değişiklikleri,
- ticket güncellemeleri,
- complaint güncellemeleri,
- after-sales güncellemeleri.

E-posta işlemleri ayrıca `EmailLog` tablosunda tutulur.

Delivery status:

```text
Pending
Sent
Failed
```

Başarısız e-posta denemelerinde hata bilgisi loglanır.

Hassas verification code değerleri log içerisinde açık metin olarak saklanmaz.

---

# Veritabanı ve EF Core

Proje **Entity Framework Core Code First** yaklaşımı kullanmaktadır.

DbContext:

```text
AppDbContext
```

ASP.NET Core Identity tabloları da aynı DbContext üzerinden yönetilmektedir.

## Audit Alanları

Domain entity'leri ortak olarak `BaseEntity` üzerinden:

```text
Id
CreatedAtUtc
UpdatedAtUtc
IsDeleted
DeletedAtUtc
```

alanlarını kullanır.

`AppDbContext.SaveChangesAsync` sırasında audit alanları merkezi şekilde yönetilir.

---

## Soft Delete

Birçok business entity fiziksel olarak silinmez.

```text
DELETE
 ↓
IsDeleted = true
DeletedAtUtc = ...
```

haline dönüştürülür.

EF Core global query filter ile silinmiş kayıtlar normal sorgulardan otomatik olarak hariç tutulur.

---

## Concurrency

`DbUpdateConcurrencyException` Unit of Work seviyesinde yakalanır ve uygulama katmanına kontrollü bir hata olarak aktarılır.

---

# Hata Yönetimi ve Güvenlik

## Global Exception Handling

API merkezi `GlobalExceptionHandler` kullanmaktadır.

Hatalar `ProblemDetails` formatında döndürülür.

Örnek:

```json
{
  "title": "Not Found",
  "status": 404,
  "detail": "Product was not found.",
  "instance": "/api/Products/999",
  "traceId": "..."
}
```

Beklenmeyen `500` hatalarında internal exception detayları client'a gönderilmez.

---

## Rate Limiting

Authentication endpoint'lerinde IP bazlı rate limiting bulunmaktadır.

Amaç:

- brute-force denemelerini azaltmak,
- login/register endpoint'lerinin kötüye kullanımını sınırlamak.

Limit aşımında:

```text
429 Too Many Requests
```

döner.

---

## Security Headers

API aşağıdaki güvenlik header'larını ekler:

```text
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: no-referrer
Permissions-Policy: camera=(), microphone=(), geolocation=()
```

Production ortamında HTTPS yönlendirmesi ve HSTS kullanılmaktadır.

---

## Health Check

Uygulama health endpoint'i:

```http
GET /health
```

Başarılı durumda:

```text
200 OK
Healthy
```

döndürür.

---

# Kurulum

## Gereksinimler

Yerel geliştirme ortamında:

- .NET 10 SDK
- SQL Server / SQL Server LocalDB
- EF Core CLI
- Git
- Visual Studio 2022 veya Visual Studio Code

gereklidir.

EF CLI yüklü değilse:

```powershell
dotnet tool install --global dotnet-ef
```

Mevcut kurulumda EF CLI 10.x kullanılmalıdır.

---

## Repoyu Klonlama

```powershell
git clone https://github.com/ahmetcancavdar/CRM.git
```

```powershell
cd CRM
```

---

## Paketleri Restore Etme

```powershell
dotnet restore MiniCrm.sln
```

---

## Build

```powershell
dotnet build MiniCrm.sln
```

Beklenen:

```text
0 Warning(s)
0 Error(s)
```

---

# User Secrets Yapılandırması

Hassas bilgiler Git repository içerisinde tutulmamalıdır.

API projesi için:

```powershell
cd MiniCrm.Api
```

User Secrets initialize edilmemişse:

```powershell
dotnet user-secrets init
```

Örnek secret tanımları:

```powershell
dotnet user-secrets set "Jwt:Key" "YOUR_LONG_RANDOM_JWT_SECRET"
```

```powershell
dotnet user-secrets set "Verification:HashKey" "YOUR_LONG_RANDOM_VERIFICATION_HASH_KEY"
```

```powershell
dotnet user-secrets set "Smtp:Username" "your-email@example.com"
```

```powershell
dotnet user-secrets set "Smtp:Password" "YOUR_SMTP_APP_PASSWORD"
```

Connection string de secret olarak tutulmak istenirse:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_SQL_SERVER_CONNECTION_STRING"
```

Secret listesini kontrol etmek için:

```powershell
dotnet user-secrets list
```

> Gerçek JWT key, SMTP password, verification hash key veya production credential değerlerini GitHub'a commit etmeyin.

---

# Migration İşlemleri

Migration listesini görüntülemek:

```powershell
dotnet ef migrations list `
  --project MiniCrm.Persistence `
  --startup-project MiniCrm.Api
```

Model ile son migration arasında fark olup olmadığını kontrol etmek:

```powershell
dotnet ef migrations has-pending-model-changes `
  --project MiniCrm.Persistence `
  --startup-project MiniCrm.Api
```

Veritabanını güncellemek:

```powershell
dotnet ef database update `
  --project MiniCrm.Persistence `
  --startup-project MiniCrm.Api
```

Yeni bir schema değişikliği yapıldığında migration oluşturmak:

```powershell
dotnet ef migrations add MigrationName `
  --project MiniCrm.Persistence `
  --startup-project MiniCrm.Api
```

Migration'lar doğrudan uygulanmadan önce `Up` ve `Down` metotlarının kontrol edilmesi önerilir.

---

# Projeyi Çalıştırma

Solution root klasöründe:

```powershell
dotnet run --project MiniCrm.Api
```

Development ortamında Swagger/OpenAPI erişime açılır.

Terminalde gösterilen localhost URL'si üzerinden uygulamaya erişilebilir.

---

# Swagger Kullanımı

Swagger üzerinden:

1. `POST /api/Auth/register` ile customer hesabı oluşturulur.
2. `POST /api/Auth/login` ile JWT alınır.
3. Swagger içindeki **Authorize** butonuna JWT girilir.
4. Role uygun endpoint'ler çağrılır.

Admin ve Customer endpoint'leri birbirinden role authorization ile ayrılmıştır.

---

# Önemli API Grupları

## Authentication

```text
/api/Auth
```

Temel işlemler:

- register
- login

---

## Profile

```text
/api/Profile
```

- profil görüntüleme
- profil güncelleme

---

## Addresses

```text
/api/Addresses
```

- listeleme
- oluşturma
- güncelleme
- silme
- varsayılan adres seçme

---

## Categories

```text
/api/Categories
```

Kategori CRUD ve admin yönetimi.

---

## Products

```text
/api/Products
```

Ürün:

- listeleme,
- oluşturma,
- güncelleme,
- aktif/pasif yönetimi,
- stok yönetimi,
- stok hareketleri.

---

## Cart

```text
/api/Cart
```

Kalıcı müşteri sepeti.

---

## Orders

Customer:

```text
/api/Orders
```

Admin:

```text
/api/AdminOrders
```

Sipariş:

- checkout,
- verification,
- resend verification,
- detail/list,
- prepare,
- ship,
- deliver,
- cancel

işlemlerini içerir.

---

## Tickets

Customer:

```text
/api/Tickets
```

Admin:

```text
/api/AdminTickets
```

---

## Complaints

Customer:

```text
/api/Complaints
```

Admin:

```text
/api/AdminComplaints
```

---

## Support Conversations

Customer:

```text
/api/SupportConversations
```

Admin:

```text
/api/AdminSupportConversations
```

---

## After-Sales

Customer:

```text
/api/AfterSalesRequests
```

Admin:

```text
/api/AdminAfterSalesRequests
```

Desteklenen request türleri:

```text
Return
Exchange
Warranty
TechnicalSupport
```

---

# Test Durumu

Mevcut sistem geliştirme sürecinde Swagger üzerinden modül modül manuel olarak test edilmiştir.

Kontrol edilen başlıca akışlar:

- register/login,
- JWT authorization,
- role kontrolü,
- category/product,
- stok hareketleri,
- cart,
- checkout,
- SMTP verification,
- order lifecycle,
- cancellation/restock,
- profile/address ownership,
- ticket workflow,
- complaint workflow,
- support conversation,
- after-sales workflow,
- 400/401/403/404/429 davranışları,
- health check,
- security headers,
- migration/model uyumluluğu.

## Otomatik Test

Henüz repository içerisinde kapsamlı otomatik test suite'i bulunmamaktadır.

Planlanan test yapısı:

```text
tests
├── MiniCrm.Domain.Tests
├── MiniCrm.Application.Tests
└── MiniCrm.Api.IntegrationTests
```

Hedef:

```powershell
dotnet test MiniCrm.sln
```

ile kritik business rule ve API akışlarının otomatik doğrulanmasıdır.

---

# Planlanan Geliştirmeler

Ana backend kapsamı tamamlanmıştır. Bundan sonraki geliştirmeler sistemin v2/production-scale iyileştirmeleri olarak değerlendirilebilir.

Planlanan başlıklar:

- xUnit unit testleri
- service testleri
- API integration testleri
- full business flow testleri
- GitHub Actions CI
- pagination
- gelişmiş filtering/search
- refresh token
- email confirmation
- forgot/reset password
- Redis cache
- SignalR gerçek zamanlı destek mesajlaşması
- Docker
- Docker Compose
- CI/CD
- production deployment

---

# Kısa Sistem Akışı

```text
Client
  ↓
ASP.NET Core Controllers
  ↓
Application Services
  ↓
Repository Interfaces
  ↓
Persistence Repositories
  ↓
Entity Framework Core
  ↓
SQL Server
```

Authentication:

```text
Client
  ↓
Login
  ↓
ASP.NET Core Identity
  ↓
JWT
  ↓
Authorize
  ↓
Protected API
```

Sipariş:

```text
Product
  ↓
Cart
  ↓
Checkout
  ↓
PendingVerification
  ↓
SMTP Verification Code
  ↓
Confirmed
  ↓
Preparing
  ↓
Shipped
  ↓
Delivered
  ↓
After-Sales
```

---

# Sonuç

MiniCrm; yalnızca müşteri CRUD işlemleri yapan basit bir CRM örneği değil, birbiriyle ilişkili iş akışlarını yöneten katmanlı bir backend uygulamasıdır.

Proje kapsamında:

- authentication,
- authorization,
- müşteri yönetimi,
- ürün/stok,
- sepet,
- sipariş,
- e-posta doğrulama,
- ticket,
- complaint,
- support conversation,
- after-sales,
- audit/logging,
- soft delete,
- merkezi exception handling,
- rate limiting

gibi backend geliştirmede kullanılan temel konseptler tek bir sistem içerisinde uygulanmıştır.
