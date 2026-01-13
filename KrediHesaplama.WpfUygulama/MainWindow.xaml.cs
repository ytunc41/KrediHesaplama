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
            
            // Varsayılan değerler
            txtAnaPara.Text = "500000";
            txtFaizOrani.Text = "3,19";
            txtVade.Text = "24";
            
            txtTahsisUcreti.Text = "2500";
            txtEkspertizUcreti.Text = "20000";
            txtIpotekUcreti.Text = "3600";
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
                // txtAnaPara içinde nokta formatı olabilir (örn: 100.000), parsing öncesi temizleyelim.
                string anaParaText = txtAnaPara.Text.Replace(".", ""); 
                if (!decimal.TryParse(anaParaText, out decimal anaPara))
                    throw new ArgumentException("Lütfen geçerli bir Ana Para giriniz.");

                if (!decimal.TryParse(txtFaizOrani.Text, out decimal faizOrani))
                    throw new ArgumentException("Lütfen geçerli bir Faiz Oranı giriniz.");

                if (!int.TryParse(txtVade.Text, out int vade))
                    throw new ArgumentException("Lütfen geçerli bir Vade (Ay) giriniz.");

                // Masrafları Al (Boşsa 0 kabul et)
                decimal tahsisUcreti = 0;
                string tahsisText = txtTahsisUcreti.Text.Replace(".", "");
                if (!string.IsNullOrEmpty(tahsisText) && !decimal.TryParse(tahsisText, out tahsisUcreti))
                     throw new ArgumentException("Geçersiz Tahsis Ücreti.");

                decimal ekspertizUcreti = 0;
                string ekspertizText = txtEkspertizUcreti.Text.Replace(".", "");
                if (!string.IsNullOrEmpty(ekspertizText) && !decimal.TryParse(ekspertizText, out ekspertizUcreti))
                     throw new ArgumentException("Geçersiz Ekspertiz Ücreti.");

                decimal ipotekUcreti = 0;
                string ipotekText = txtIpotekUcreti.Text.Replace(".", "");
                if (!string.IsNullOrEmpty(ipotekText) && !decimal.TryParse(ipotekText, out ipotekUcreti))
                     throw new ArgumentException("Geçersiz İpotek Ücreti.");

                // Hesapla
                decimal aylikTaksit = _hesaplayici.TaksitHesapla(anaPara, faizOrani, vade);
                decimal krediToplamOdeme = _hesaplayici.ToplamOdemeHesapla(aylikTaksit, vade);
                decimal toplamFaiz = _hesaplayici.ToplamFaizHesapla(krediToplamOdeme, anaPara);
                
                // Masrafları Toplam Ödemeye Ekle
                decimal genelToplamOdeme = krediToplamOdeme + tahsisUcreti + ekspertizUcreti + ipotekUcreti;

                // Sonuçları Göster (Para birimi formatı: C2)
                lblAylikTaksit.Text = aylikTaksit.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                lblToplamOdeme.Text = genelToplamOdeme.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                lblToplamFaiz.Text = toplamFaiz.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            catch (Exception ex)
            {
                lblHata.Text = "Hata: " + ex.Message;
                lblHata.Visibility = Visibility.Visible;
            }
        }

        private void txtFaizOrani_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not System.Windows.Controls.TextBox textBox) return;

            string originalText = textBox.Text;
            int selectionStart = textBox.SelectionStart;
            
            // Değişiklik gerekip gerekmediğini anlamak için geçici değişken
            string newText = "";
            int commaCount = 0;
            bool hasChanges = false;

            // 1. Karakter bazlı filtreleme ve düzeltme
            foreach (char c in originalText)
            {
                char ch = c;
                // Noktaları virgüle çevir
                if (ch == '.') 
                {
                    ch = ',';
                    hasChanges = (c != ch); // Nokta geldiyse değişim var demektir
                }

                if (char.IsDigit(ch))
                {
                    // Rakam ise ekle
                    newText += ch;
                }
                else if (ch == ',')
                {
                    // Virgül ise, daha önce eklenmemişse VE en başta değilse (boş stringe eklenemez) ekle
                    if (commaCount == 0 && newText.Length > 0)
                    {
                        newText += ch;
                        commaCount++;
                    }
                    else
                    {
                        // İkinci, sonraki veya en başta olan virgülleri yoksay
                        hasChanges = true;
                    }
                }
                else
                {
                    // Rakam veya virgül dışındaki her şeyi yoksay (harf, sembol vb.)
                    hasChanges = true; 
                }
            }

            // 2. Virgülden sonra en fazla 2 basamak kontrolü
            int commaIndex = newText.IndexOf(',');
            if (commaIndex != -1)
            {
                string afterComma = newText.Substring(commaIndex + 1);
                if (afterComma.Length > 2)
                {
                    newText = newText.Substring(0, commaIndex + 1) + afterComma.Substring(0, 2);
                    hasChanges = true;
                }
            }

            // 3. Maksimum Değer Kontrolü (100'den büyük olamaz)
            if (decimal.TryParse(newText, out decimal currentValue))
            {
                if (currentValue > 100)
                {
                    newText = "99,99";
                    hasChanges = true;
                }
            }
            
            // Eğer string değiştiyse (filtrelendi veya düzeltildiyse) TextBox'ı güncelle
            // 'originalText != newText' kontrolü de yapılabilir ama hasChanges daha granüler
            if (originalText != newText) 
            {
                textBox.Text = newText;
                
                // İmleci mantıklı bir yere koymaya çalış
                // Basitçe: Eğer metnin sonuna ekleme yapıyorsak sona, araya ekliyorsak konumunu koru
                // Ancak silme işlemlerinde bu indeks kayabilir.
                // En güvenli basit yöntem: Uzunluğu aşmıyorsa eski yerinde tut, aşıyorsa sona al.
                if (selectionStart <= newText.Length)
                    textBox.SelectionStart = selectionStart;
                else
                    textBox.SelectionStart = newText.Length;
            }
        }

        private void txtSadeceSayi_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not System.Windows.Controls.TextBox textBox) return;

            string originalText = textBox.Text;
            int caretIndex = textBox.CaretIndex;

            // 1. Sadece rakamları al (Ham Veri) ve İmleçten önceki rakam sayısını bul
            string rawDigits = "";
            int digitsBeforeCaret = 0;
            
            for (int i = 0; i < originalText.Length; i++)
            {
                if (char.IsDigit(originalText[i]))
                {
                    rawDigits += originalText[i];
                    if (i < caretIndex) digitsBeforeCaret++;
                }
            }

            if (rawDigits.Length == 0)
            {
                if (!string.IsNullOrEmpty(originalText)) textBox.Text = "";
                return;
            }

            // 2. Limit Kontrolleri
            long numericValue = 0;
            if (long.TryParse(rawDigits, out long val))
            {
                 if (textBox.Name == "txtVade" && val > 240)
                 {
                     val = 240;
                 }
                 else if (textBox.Name == "txtAnaPara" && val > 100000000)
                 {
                     val = 100000000;
                 }
                 else if ((textBox.Name == "txtTahsisUcreti" || 
                           textBox.Name == "txtEkspertizUcreti" || 
                           textBox.Name == "txtIpotekUcreti") && val > 100000)
                 {
                     val = 100000;
                 }
                 numericValue = val;
            }

            // 3. Formatlama
            string resultText;
            if (textBox.Name == "txtAnaPara" || 
                textBox.Name == "txtTahsisUcreti" || 
                textBox.Name == "txtEkspertizUcreti" || 
                textBox.Name == "txtIpotekUcreti")
            {
                 // tr-TR kültüründe binlik ayracı noktadır.
                 resultText = numericValue.ToString("#,##0", new System.Globalization.CultureInfo("tr-TR"));
            }
            else
            {
                resultText = numericValue.ToString();
            }

            // 4. UI Güncelleme (Eğer değişim varsa)
            if (originalText != resultText)
            {
                textBox.Text = resultText;

                // İmleç Pozisyonunu Geri Yükle
                // Yeni metinde, eski metindeki kadar rakam geçince dur.
                int newCaretIndex = 0;
                int seenDigits = 0;
                
                // Eğer hiç rakam yoksa veya başta isek 0 kalır.
                if (digitsBeforeCaret > 0)
                {
                     foreach (char c in resultText)
                     {
                        newCaretIndex++;
                        if (char.IsDigit(c)) seenDigits++;
                        if (seenDigits >= digitsBeforeCaret) break;
                     }
                }
                
                textBox.CaretIndex = newCaretIndex;
            }
        }
    }
}