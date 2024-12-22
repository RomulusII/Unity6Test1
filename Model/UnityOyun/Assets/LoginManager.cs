using UnityEngine;
using TMPro; // TextMeshPro bileşenlerini kullanmak için

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput; // Kullanıcı adı giriş alanı
    public TMP_InputField passwordInput; // Şifre giriş alanı
    public TextMeshProUGUI errorText;    // Hata mesajlarını göstermek için

    // Login butonuna tıklandığında çağrılan fonksiyon
    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text; // Kullanıcı adını al
        string password = passwordInput.text; // Şifreyi al

        // Boş alan kontrolü
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Kullanıcı adı ve şifre boş olamaz!");
            return;
        }

        // Basit bir doğrulama (örnek)
        if (username == "admin" && password == "1234")
        {
            Debug.Log("Giriş başarılı!");
            // Başarılı girişte başka bir sahneye geçmek için:
            // UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
        else
        {
            ShowError("Kullanıcı adı veya şifre yanlış!");
        }
    }

    // Hata mesajını göstermek için kullanılan fonksiyon
    private void ShowError(string message)
    {
        errorText.text = message; // Hata mesajını UI'de göster
        errorText.gameObject.SetActive(true); // Hata mesajını görünür yap
    }
}