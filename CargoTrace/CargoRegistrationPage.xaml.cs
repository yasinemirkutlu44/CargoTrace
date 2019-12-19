using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Globalization;
using System.Device.Location;
using System.Net;
using System.Net.Mail;

namespace CargoTrace
{
    public partial class CargoRegistrationPage : Window
    {
        private GeoCoordinateWatcher Watcher = null;
        public CargoRegistrationPage()
        {
            InitializeComponent();
            btnApply.IsEnabled = false;
            txtLatitude.IsEnabled = false;
            txtLongitude.IsEnabled = false;
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "HPxTaIfXR1VbpOKn15NBcip45TWQk9LTDi5dxx8L",
            BasePath = "https://kargotakip-86f0d.firebaseio.com/"

        };

        IFirebaseClient firebaseClient;

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                firebaseClient = new FireSharp.FirebaseClient(config);
                if (String.IsNullOrWhiteSpace(txtOwnerName.Text) || String.IsNullOrWhiteSpace(txtOwnerSurname.Text) || String.IsNullOrWhiteSpace(txtOwnerEmail.Text) || String.IsNullOrWhiteSpace(txtRFIDNumber.Text) || String.IsNullOrEmpty(txtLatitude.Text) ||
                    String.IsNullOrEmpty(txtLongitude.Text))
                {
                    MessageBox.Show("A required field was not filled", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else // inserting a cargo
                {
                    Cargo cargo = new Cargo()
                    {
                        OwnerName = txtOwnerName.Text,
                        OwnerSurname = txtOwnerSurname.Text,
                        OwnerEmail = txtOwnerEmail.Text,
                        Latitude = double.Parse(txtLatitude.Text.Trim(), CultureInfo.InvariantCulture.NumberFormat),
                        Longitude = double.Parse(txtLongitude.Text.Trim(), CultureInfo.InvariantCulture.NumberFormat),
                        CargoRFIDNumber = txtRFIDNumber.Text
                    };
                    SetResponse setResponse = firebaseClient.Set("Cargo" + "/" + txtRFIDNumber.Text, cargo);
                    MessageBox.Show("You have registered a new cargo into registration system", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    sendEmail(cargo);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("You are not able to register a cargo in to registration system", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e) // The watcher's status has change. See if it is ready
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                // Display the latitude and longitude.
                if (Watcher.Position.Location.IsUnknown)
                {
                    txtLatitude.Text = "Cannot find location data";
                }
                else
                {
                    GeoCoordinate location = Watcher.Position.Location;
                    txtLatitude.Text = location.Latitude.ToString().Replace(",", ".");
                    txtLongitude.Text = location.Longitude.ToString().Replace(",", ".");
                }
            }
        }

        private void sendEmail(Cargo cargo)
        {
            var fromAddress = new MailAddress("kargotakipkou@gmail.com", "From Name");
            var toAddress = new MailAddress(cargo.OwnerEmail, "To Name");
            string fromPassword = "KargoTakip1234";
            string subject = "Kargo Takip Numaranız";
            string body = "Kargo Takip Numaranız " + cargo.CargoRFIDNumber + "\n Bizi tercih ettiğiniz için teşekkür ederiz.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }

        private void btnCurrentLocation_Click(object sender, RoutedEventArgs e)
        {
            Watcher = new GeoCoordinateWatcher(); // Create the watcher.
            Watcher.StatusChanged += Watcher_StatusChanged; // Catch the StatusChanged event.
            Watcher.Start(); // Start the watcher.
            txtLatitude.Text = Watcher.Position.Location.Latitude.ToString();
            txtLongitude.Text = Watcher.Position.Location.Longitude.ToString();
            btnApply.IsEnabled = true;
        }
    }
}