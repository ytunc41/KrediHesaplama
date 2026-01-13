using System.Collections.Generic;

namespace KrediHesaplama.Cekirdek
{
    /// <summary>
    /// Uygulama versiyon ve geçmiş bilgilerini tutar.
    /// </summary>
    public static class Versiyon
    {
        public const string GuncelVersiyon = "1.0.3";

        public static readonly List<VersiyonKaydi> VersiyonGecmisi = new List<VersiyonKaydi>
        {
            new VersiyonKaydi
            {
                VersiyonNo = "1.0.3",
                Tarih = "13.01.2026",
                Aciklamalar = new List<string>
                {
                    "Proje dökümantasyonu (README.md) baştan aşağı yenilendi",
                    "Kullanım kılavuzu ve teknik detaylar dökümantasyona eklendi",
                    "Arayüz rehberi ve giriş limitleri dökümante edildi"
                }
            },
            new VersiyonKaydi
            {
                VersiyonNo = "1.0.2",
                Tarih = "13.01.2026",
                Aciklamalar = new List<string>
                {
                    "Koyu Tema (Dark Theme) desteği eklendi",
                    "Yeni masraf kalemleri eklendi (Banka Tahsis, Ort. Ekspertiz, İpotek Ücretleri)",
                    "Masrafların toplam geri ödeme tutarına dahil edilmesi sağlandı",
                    "Uygulama ikonu modernleştirildi, dolar simgesi eklendi ve yüksek çözünürlüklü hale getirildi",
                    "Açılış ekranı ikon boyutu büyütüldü ve görsel uyumluluk iyileştirmeleri yapıldı",
                    "Arayüzdeki metin ve kontrol renkleri koyu temaya uygun olarak optimize edildi"
                }
            },
            new VersiyonKaydi
            {
                VersiyonNo = "1.0.1",
                Tarih = "13.01.2026",
                Aciklamalar = new List<string>
                {
                    "Gelişmiş giriş doğrulama kontrolleri (Sadece sayı, karakter engelleme)",
                    "Açılış ekranı (Splash Screen), ilerleme çubuğu ve animasyon eklendi",
                    "Uygulama ikonu eklendi",
                    "Ana Para alanı için binlik ayırıcı nokta (.) formatı eklendi",
                    "Faiz oranı girişi için otomatik düzeltme ve limit (Max 99,99) getirildi",
                    "Vade (Max 240) ve Ana Para (Max 100M) için üst limitler tanımlandı",
                    "Açılışta varsayılan değerlerin gelmesi sağlandı"
                }
            },
            new VersiyonKaydi
            {
                VersiyonNo = "1.0.0",
                Tarih = "13.01.2026",
                Aciklamalar = new List<string>
                {
                    "ilk versiyon",
                    "Proje oluşturuldu (Cekirdek ve WPF)",
                    "Kredi hesaplama mantığı eklendi",
                    "Türkçe isimlendirme uygulandı",
                    "Arayüz tasarımı yapıldı",
                    "Birim testleri eklendi"
                }
            }
            // Yeni versiyonlar buraya eklenecek (Şablon)
            /*
            new VersiyonKaydi
            {
                VersiyonNo = "1.0.1",
                Tarih = "Tarih Buraya",
                Aciklamalar = new List<string>
                {
                    "Yapılan değişiklik 1",
                    "Yapılan değişiklik 2"
                }
            }
            */
        };
    }

    public class VersiyonKaydi
    {
        public string VersiyonNo { get; set; } = string.Empty;
        public string Tarih { get; set; } = string.Empty;
        public List<string> Aciklamalar { get; set; } = new List<string>();
    }
}
