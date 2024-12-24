using UnityEngine;
using TMPro;
using Model.UnityOyun.Assets.Model;
using Newtonsoft.Json;
using System.Threading.Tasks; // TextMeshPro bileşenlerini kullanmak için

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput; // Kullanıcı adı giriş alanı
    public TMP_InputField passwordInput; // Şifre giriş alanı
    public TextMeshProUGUI errorText;    // Hata mesajlarını göstermek için

    [SerializeField]
    public ClientSocket clientSocket; // ClientSocket bileşeni

    private void Start()
    {
        // Subscribe to events
        clientSocket.LoginResponseReceived += OnLoginResponseReceived;
        clientSocket.ErrorReceived += OnErrorReceived;
    }

    // Login butonuna tıklandığında çağrılan fonksiyon
    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text; // Kullanıcı adını al
        string password = passwordInput.text; // Şifreyi al
        ShowError("Login...");
        // Boş alan kontrolü
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Kullanıcı adı ve şifre boş olamaz!");
            return;
        }

        try
        {
            Login();
        }
        catch (System.Exception ex)
        {
            ShowError("Bir hata oluştu: " + ex.Message);
        }
    }

    private async void Login()
    {
        var loginRequest = new LoginRequest
        {
            Email = usernameInput.text,
            Password = passwordInput.text
        };

        await clientSocket.SendMessage(loginRequest);
    }

    // Hata mesajını göstermek için kullanılan fonksiyon
    private void ShowError(string message)
    {
        errorText.text = message; // Hata mesajını UI'de göster
        errorText.gameObject.SetActive(true); // Hata mesajını görünür yap
    }

    // Event handler for LoginResponseReceived
    private void OnLoginResponseReceived(object sender, LoginResponse loginResponse)
    {
        if (loginResponse.Success)
        {
            Debug.Log("Login successful!");
            // Handle successful login (e.g., navigate to another scene)
        }
        else
        {
            ShowError(loginResponse.Message);
        }
    }

    // Event handler for ErrorReceived
    private void OnErrorReceived(object sender, string errorMessage)
    {
        ShowError(errorMessage);
    }
}