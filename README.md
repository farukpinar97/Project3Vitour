# 🌍 Vitour - Tur & Seyahat Yönetim Sistemi

Bu proje **Murat Yücedağ** eğitmenliğinde geliştirilen eğitim süreci kapsamında oluşturulmuştur.

**Vitour**, modern web mimarisi ile geliştirilen, tur & seyahat acenteleri için kapsamlı bir yönetim ve rezervasyon platformudur.

---

## 📋 Proje Hakkında

**Vitour**, kullanıcıların turları keşfedebildiği, rezervasyon yapabildiği ve yorum bırakabildiği; yöneticilerin ise tüm bu süreçleri admin panel üzerinden yönetebildiği full-stack bir web uygulamasıdır.

Frontend'de tur listesi, tur detay sayfası, rehber tanıtımları ve iletişim formu gibi kullanıcı odaklı sayfalar yer alırken; admin panelde tur yönetimi, rezervasyon takibi, yorum moderasyonu, kategori ve rehber yönetimi gibi kapsamlı araçlar sunulmaktadır.

---

## 🎯 Öne Çıkan Özellikler

### 🌐 Frontend (Kullanıcı Tarafı)
- Tur listesi ve detay sayfaları
- Tur filtreleme ve arama
- Rezervasyon formu ve online booking
- Müşteri yorum & değerlendirme sistemi (4 kategorili puanlama)
- Rehber tanıtım sayfaları
- İletişim formu

### 🔧 Admin Panel
- **Tur Yönetimi** — Tur ekleme, düzenleme, silme; günlük tur planı (DayPlan) yönetimi
- **Rezervasyon Yönetimi** — Durum takibi (Bekleyen / Onaylı / İptal), rezervasyon numarası sistemi (`RES-YYYY-XXXXX`)
- **Yorum Moderasyonu** — Claude AI destekli içerik moderasyonu, onaylama/reddetme
- **Rehber Yönetimi** — Rehber CRUD işlemleri
- **Kategori Yönetimi** — Kategori CRUD işlemleri
- **Mesaj Yönetimi** — Müşteri mesajları, okundu/okunmadı takibi
- **Dashboard** — İstatistik kartları, son rezervasyonlar, son yorumlar, gelir özeti
- **Raporlama** — PDF ve Excel export (tur bazlı & tüm rezervasyonlar)
- **Server-side Pagination** — Tüm listelerde sayfalama ve filtreleme

  ## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|-----------|----------|
| **ASP.NET Core 9** | Web framework, MVC mimarisi |
| **MongoDB** | NoSQL veritabanı, tüm veri depolama |
| **AutoMapper** | Entity ↔ DTO dönüşümleri |
| **iText7** | PDF rapor oluşturma ve export |
| **EPPlus** | Excel rapor oluşturma ve export |
| **SweetAlert2** | Modern modal ve bildirim sistemi |
| **Font Awesome** | İkon kütüphanesi |
| **Bootstrap** | Responsive tasarım |
| **Anthropic Claude API** | AI destekli yorum moderasyonu (otomatik içerik analizi) |

## :camera: Projenin Ekran Görüntüleri

<img width="1350" height="6268" alt="01_Homepage" src="https://github.com/user-attachments/assets/c7852468-d363-4afb-94c1-660d3ce22612" />

<img width="1350" height="1848" alt="02_Homepage_en" src="https://github.com/user-attachments/assets/4f7e0f16-cb89-4021-9c21-f3f09d6a2d44" />

<img width="1350" height="2570" alt="03_TourList" src="https://github.com/user-attachments/assets/582bb962-8633-40b0-9229-0de843525bb2" />

<img width="2000" height="2000" alt="04_GuideList_Contact" src="https://github.com/user-attachments/assets/af85e781-420c-4aec-84e8-a89eea3160b9" />

<img width="1920" height="3900" alt="05_TourDetail" src="https://github.com/user-attachments/assets/ff89599f-44f5-44c7-8aa2-70796c0f408a" />

<img width="1920" height="3288" alt="06_TourDayPlan" src="https://github.com/user-attachments/assets/143db4ab-1622-492d-8d3a-8dbd9ad0be8b" />

<img width="1920" height="2400" alt="07_TourGallery" src="https://github.com/user-attachments/assets/78434b28-bdfa-40db-a6b0-1df591f8bbf4" />

<img width="1920" height="3498" alt="08_TourReview" src="https://github.com/user-attachments/assets/d7f8b775-7977-42b9-bd70-9326f8c2f2f7" />

<img width="2000" height="2000" alt="09_ReviewApprove" src="https://github.com/user-attachments/assets/d433544a-7e6d-4ce8-b736-f480a9a1a7c5" />

<img width="1920" height="2017" alt="10_TourBooking" src="https://github.com/user-attachments/assets/3c76286a-9f37-46dd-ae9d-dc35bceb2174" />

<img width="1920" height="1716" alt="11_TourBookingFull" src="https://github.com/user-attachments/assets/8e07b8d0-b1f0-41c8-9524-732616493d67" />

<img width="1920" height="1658" alt="12_TourBookingApprove" src="https://github.com/user-attachments/assets/796df5f6-4883-4a82-85b9-46014a83b76e" />

<img width="2000" height="2000" alt="13_TourBookingMail" src="https://github.com/user-attachments/assets/dbc47798-133c-41ca-9c68-7655b5bc65d8" />

<img width="1920" height="1420" alt="14_AdminDashboard" src="https://github.com/user-attachments/assets/eb651aa0-c962-4264-9e85-727331d67495" />

<img width="1920" height="1027" alt="15_AdminTourList" src="https://github.com/user-attachments/assets/7d0cd382-3115-4c6e-852a-29f33fc42f33" />

<img width="1920" height="3769" alt="16_AdminTourUpdate" src="https://github.com/user-attachments/assets/b300275a-87be-47bc-8110-8e35e4c8ebc6" />

<img width="1920" height="1097" alt="17_AdminReservation" src="https://github.com/user-attachments/assets/df0fdac7-d518-4243-891c-a9a88d28eca2" />

<img width="1920" height="1884" alt="18_AdminReview" src="https://github.com/user-attachments/assets/19f061cc-5658-4009-808f-7c80c44be7f7" />

<img width="1920" height="1089" alt="19_AdminMessages" src="https://github.com/user-attachments/assets/37ddf084-ca56-47a1-b9ac-c33de257b028" />

<img width="1920" height="945" alt="20_AdminMessageDetail" src="https://github.com/user-attachments/assets/a6474493-15ca-4ef5-a50f-2d1e9264390d" />

