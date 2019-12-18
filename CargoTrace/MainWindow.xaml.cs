using System;
using System.Windows;
using System.Windows.Input;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Text.RegularExpressions;

namespace CargoTrace
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtLoginAuth.Focus();
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "HPxTaIfXR1VbpOKn15NBcip45TWQk9LTDi5dxx8L",
            BasePath = "https://kargotakip-86f0d.firebaseio.com/"

        };

        IFirebaseClient firebaseClient;

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            CargoRegistrationPage cargoRegistrationPage = new CargoRegistrationPage();
            try
            {
                if (String.IsNullOrWhiteSpace(txtLoginAuth.Text) || String.IsNullOrWhiteSpace(txtPass.Password))
                {
                    MessageBox.Show("A required field was not filled", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    firebaseClient = new FireSharp.FirebaseClient(config);
                    FirebaseResponse firebaseResponse = firebaseClient.Get("Users/" + txtLoginAuth.Text);
                    User responseUser = firebaseResponse.ResultAs<User>(); // DB Result
                    User currentUser = new User() // User gives information
                    {
                        UserLoginAuthID = txtLoginAuth.Text,
                        Password = txtPass.Password,
                    };
                    cargoRegistrationPage.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Sorry, We could not find an account with this Login Auth Key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
