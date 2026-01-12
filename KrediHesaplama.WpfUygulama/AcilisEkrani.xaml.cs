using System;
using System.Threading.Tasks;
using System.Windows;

namespace KrediHesaplama.WpfUygulama
{
    public partial class AcilisEkrani : Window
    {
        public AcilisEkrani()
        {
            InitializeComponent();
            Baslat();
        }

        private async void Baslat()
        {
            // İlerleme simülasyonu (yaklaşık 2 saniye)
            // UI thread'ini her adımda güncellemek yavaşlatabilir.
            // Bu yüzden adımları 1'er 1'er değil, daha büyük adımlarla artıralım.
            
            // Toplam hedef süre: 2000 ms
            // İstenen adım sayısı: 20 (her adım %5 artar)
            // Adım başına bekleme: 2000 / 20 = 100 ms
            
            for (int i = 0; i <= 100; i += 5)
            {
                prgYukleniyor.Value = i;
                lblYuzde.Text = $"%{i}";
                
                await Task.Delay(100); 
            }

            // Ana pencereyi oluştur ve göster
            MainWindow anaPencere = new MainWindow();
            anaPencere.Show();

            // Açılış ekranını kapat
            this.Close();
        }
    }
}
