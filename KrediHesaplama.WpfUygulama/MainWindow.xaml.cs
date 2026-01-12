using System;
using System.Windows;
using KrediHesaplama.Cekirdek;

namespace KrediHesaplama.WpfUygulama
{
    public partial class MainWindow : Window
    {
        private readonly KrediHesaplayici _hesaplayici;

        public MainWindow()
        {
            InitializeComponent();
            _hesaplayici = new KrediHesaplayici();
            
            this.Title = $"Kredi Hesaplama v{Versiyon.GuncelVersiyon}";
            // lblBaslik.Text güncellenmiyor, XAML'daki varsayılan değer (Konut Kredisi Hesaplama) kalıyor.
        }

        private void btnHesapla_Click(object sender, RoutedEventArgs e)
        {
            lblHata.Visibility = Visibility.Collapsed;
            lblAylikTaksit.Text = "- TL";
            lblToplamOdeme.Text = "- TL";
            lblToplamFaiz.Text = "- TL";

            try
            {
                // Girişleri Kontrol Et ve Dene
                if (!decimal.TryParse(txtAnaPara.Text, out decimal anaPara))
                    throw new ArgumentException("Lütfen geçerli bir Ana Para giriniz.");

                if (!decimal.TryParse(txtFaizOrani.Text, out decimal faizOrani))
                    throw new ArgumentException("Lütfen geçerli bir Faiz Oranı giriniz.");

                if (!int.TryParse(txtVade.Text, out int vade))
                    throw new ArgumentException("Lütfen geçerli bir Vade (Ay) giriniz.");

                // Hesapla
                decimal aylikTaksit = _hesaplayici.TaksitHesapla(anaPara, faizOrani, vade);
                decimal toplamOdeme = _hesaplayici.ToplamOdemeHesapla(aylikTaksit, vade);
                decimal toplamFaiz = _hesaplayici.ToplamFaizHesapla(toplamOdeme, anaPara);

                // Sonuçları Göster (Para birimi formatı: C2)
                lblAylikTaksit.Text = aylikTaksit.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                lblToplamOdeme.Text = toplamOdeme.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                lblToplamFaiz.Text = toplamFaiz.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            catch (Exception ex)
            {
                lblHata.Text = "Hata: " + ex.Message;
                lblHata.Visibility = Visibility.Visible;
            }
        }
    }
}