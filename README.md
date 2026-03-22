# JetBrains Trial Reset 🔄

[![.NET](https://img.shields.io/badge/.NET-v10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platforms-Windows%20%7C%20macOS%20%7C%20Linux-green)]()

**JetBrains Trial Reset**, JetBrains(R) IDE ürünleri (IntelliJ IDEA, WebStorm, PyCharm vb.) için 30 günlük başlangıç deneme sürecini (evaluation period) temizleyen ve sıfırlayan otomatik bir araçtır. 

Orijinal Java uygulamasından yola çıkılarak, esnek çapraz platform (cross-platform) yetenekleriyle C# `.NET` ekosistemine uyarlanmıştır. Aşağıdaki doküman, platform bağımsız çalışabilen **Console (Terminal)** varyantına göre hazırlanmıştır.

---

## 🚀 Özellikler (Features)

* **Çapraz Platform Desteği (Cross-Platform):** Sadece Windows'ta değil; **macOS** ve **Linux** işletim sistemlerinde de Terminal üzerinden sorunsuz çalışır.
* **Seçmeli (Selective) Sıfırlama:** Sistemdeki her şeyi bozmadan yalnızca sıfırlamak veya süre yenilemek istediğiniz (örneğin yalnızca PyCharm ve GoLand) belirli ürünleri seçebilirsiniz.
---

## 🛠 Desteklenen Ürünler (Supported Products)
Aşağıdaki ürünler test edilmiş ve scriptlerin (`eval` klasörleri ve `other.xml`) işleyişine dahil edilmiştir:
- IntelliJ IDEA
- CLion
- DataGrip
- GoLand
- PhpStorm
- PyCharm
- ReSharper / ReSharperC
- Rider
- RubyMine
- WebStorm
- Datalore

---

## 💻 Kurulum & Kullanım (How to use)

Bu uygulamayı çalıştırabilmek için bilgisayarınızda (Windows, Mac veya Linux fark etmeksizin) **.NET SDK** yüklü olmalıdır.  
[Buradan .NET SDK indirebilirsiniz.](https://dotnet.microsoft.com/download)

Projeyi kendi ortamınıza klonladıktan veya indirdikten sonra terminal / CMD açın ve C# Console projesi dizinine gidin:

```bash
# Proje dizinine giriş yapın
cd JetBrainsResetTrialConsole

# Projeyi Derleyin (Opsiyonel)
dotnet build

# Projeyi Başlatın
dotnet run
```

### Klavye Kısayolları (Keybinds)
C# Console uygulamasını çalıştırdığınızda aşağıdaki kontroller geçerli olur:
- `Yukarı / Aşağı Oklar` **:** Menüde ürünler arasında gezinin.
- `Space (Boşluk)` **:** Üzerinde bulunduğunuz ürünü seçin veya kaldırın.
- `A Tuşu` **:** Listedeki bütün ürünleri aynı anda tek tuşla seçin.
- `Enter` **:** Seçimlerinizi bitirip arka plan sıfırlama (bat/bash) komutlarını uygulayın.
- `ESC` **:** Programdan çıkış yapın.

---

## ⚠️ Yasal Uyarı (Disclaimer)
Bu yazılım sadece **eğitim (Educational) ve test (Testing) amaçlı** bir yan proje (Proof of Concept) niyetiyle yazılmıştır. Açık kaynak kodları ve işletim sistemi registry/dosya yapılarının programatik olarak nasıl entegre edilebileceği mantığını ve `.NET API'larını` göstermeyi hedefler. Gerçek iş ortamlarında (Production) kullanılması, ticari ihlallere yol açabilir. Sorumluluk tamamen kullanıcıya aittir; lütfen mümkünse lisans satın alarak geliştiricileri (JetBrains) destekleyiniz.