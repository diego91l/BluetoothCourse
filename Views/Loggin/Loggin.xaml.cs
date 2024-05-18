namespace BluetoothCourse.Views.Loggin;

public partial class Loggin : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public Loggin(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    private async void btnInicio_Clicked(object sender, EventArgs e)
    {
        string usuario = "uisrael";
        string contrasena = "1234";

        if (usuario == txtUserName.Text && contrasena == txtPassword.Text)
        {
            var bluetoothScanner = _serviceProvider.GetRequiredService<BluetoothCourse.Bluetooth.BluetoothScanner>();
            await Navigation.PushAsync(new Scan.ScanResults(bluetoothScanner));
        }
        else
        {
            await DisplayAlert("Alert", "Usuario o contraseña incorrecta", "Ok");
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
    }
}