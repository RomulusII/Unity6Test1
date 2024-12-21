using UnityEngine;
using UnityEngine.UI; // Unity'nin UI sistemini kullanmak için.
using TMPro; // TextMeshPro kullanıyorsanız.

public class LoginManager : MonoBehaviour
{
    // Kullanıcı arayüzündeki elemanlara referanslar.
    public TMP_InputField usernameInput; // Kullanıcı adı giriş alanı
    public TMP_InputField passwordInput; // Şifre giriş alanı
    public TextMeshProUGUI errorText;    // Hata mesajları için metin

    // Login butonuna tıklandığında çağrılan fonksiyon
    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text; // Kullanıcı adını al
        string password = passwordInput.text; // Şifreyi al

        // Basit bir doğrulama örneği
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Kullanıcı adı ve şifre boş olamaz!");
            return;
        }

        // Yerel doğrulama (örnek)
        if (username == "admin" && password == "1234")
        {
            Debug.Log("Giriş başarılı!");
            // Giriş başarılı olduğunda başka bir sahneye geçebilirsiniz.
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
        errorText.text = message; // Hata mesajını UI'ye yaz
        errorText.gameObject.SetActive(true); // Hata mesajını görünür yap
    }
}