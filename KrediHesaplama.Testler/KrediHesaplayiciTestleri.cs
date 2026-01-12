using Xunit;
using KrediHesaplama.Cekirdek;
using System;

namespace KrediHesaplama.Testler
{
    public class KrediHesaplayiciTestleri
    {
        private readonly KrediHesaplayici _hesaplayici;

        public KrediHesaplayiciTestleri()
        {
            _hesaplayici = new KrediHesaplayici();
        }

        [Fact]
        public void TaksitHesapla_StandartDegerler_DogruSonucDonmeli()
        {
            // Senaryo: 100.000 TL, %1.20 Faiz, 12 Ay Vade
            // Beklenen Taksit: Yaklaşık 9.000 TL civarı (Online hesaplayıcılardan teyit edilebilir veya formül)
            // PMT Formül sonucu: 8997.54 TL
            
            decimal anaPara = 100000m;
            decimal oran = 1.20m;
            int vade = 12;

            decimal sonuc = _hesaplayici.TaksitHesapla(anaPara, oran, vade);

            Assert.Equal(8997.54m, sonuc);
        }

        [Theory]
        [InlineData(0, 1.20, 12)]
        [InlineData(100000, 0, 12)]
        [InlineData(100000, 1.20, 0)]
        [InlineData(-500, 1.20, 12)]
        public void TaksitHesapla_GecersizDegerler_HataFirlatmali(decimal anaPara, decimal oran, int vade)
        {
            Assert.Throws<ArgumentException>(() => _hesaplayici.TaksitHesapla(anaPara, oran, vade));
        }

        [Fact]
        public void ToplamOdemeHesapla_DogruHesaplamali()
        {
            decimal taksit = 1000m;
            int vade = 12;

            decimal toplam = _hesaplayici.ToplamOdemeHesapla(taksit, vade);

            Assert.Equal(12000m, toplam);
        }

        [Fact]
        public void ToplamFaizHesapla_DogruHesaplamali()
        {
            decimal toplamOdeme = 12000m;
            decimal anaPara = 10000m;

            decimal faiz = _hesaplayici.ToplamFaizHesapla(toplamOdeme, anaPara);

            Assert.Equal(2000m, faiz);
        }
    }
}
