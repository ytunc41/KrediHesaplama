using System;

namespace KrediHesaplama.Cekirdek
{
    public class KrediHesaplayici
    {
        /// <summary>
        /// Kredinin aylık taksit tutarını hesaplar.
        /// </summary>
        /// <param name="anaPara">Kredi Tutarı</param>
        /// <param name="aylikFaizOrani">Aylık Faiz Oranı (Yüzde olarak, örn: 2.79)</param>
        /// <param name="vadeAy">Vade (Ay)</param>
        /// <returns>Aylık Taksit Tutarı</returns>
        public decimal TaksitHesapla(decimal anaPara, decimal aylikFaizOrani, int vadeAy)
        {
            if (anaPara <= 0 || aylikFaizOrani <= 0 || vadeAy <= 0)
                throw new ArgumentException("Tüm giriş değerleri pozitif olmalıdır.");

            double r = (double)(aylikFaizOrani / 100m);
            double P = (double)anaPara;
            double n = (double)vadeAy;

            // Formül: P * (r * (1+r)^n) / ((1+r)^n - 1)
            double pay = r * Math.Pow(1 + r, n);
            double payda = Math.Pow(1 + r, n) - 1;

            double aylikTaksit = P * (pay / payda);

            return Math.Round((decimal)aylikTaksit, 2);
        }

        /// <summary>
        /// Toplam geri ödeme tutarını hesaplar.
        /// </summary>
        public decimal ToplamOdemeHesapla(decimal aylikTaksit, int vadeAy)
        {
             return aylikTaksit * vadeAy;
        }

        /// <summary>
        /// Toplam ödenen faiz tutarını hesaplar.
        /// </summary>
        public decimal ToplamFaizHesapla(decimal toplamOdeme, decimal anaPara)
        {
            return toplamOdeme - anaPara;
        }
    }
}
