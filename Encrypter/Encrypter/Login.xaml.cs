using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Encrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(UsernameInput);
            Source.Connect();
        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)NewUserButton.Content == "New User")
            {
                ((Storyboard)FindResource("ShowNewUserFields")).Begin();

                LoginButton.Content = "Create";
                NewUserButton.Content = "Cancel";

                Keyboard.Focus(UsernameInput);
                ReEnterPasswordInput.Focusable = true;
            }
            else // Cancel
            {
                ((Storyboard)FindResource("HideNewUserFields")).Begin();

                LoginButton.Content = "Login";
                NewUserButton.Content = "New User";

                Keyboard.Focus(UsernameInput);
                ReEnterPasswordInput.Focusable = false;
            }
        }

        //---------2---------3---------4---------5---------6---------7---------8---------9
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)LoginButton.Content == "Login")
            {
                User user;
                bool userExists = Source.TryGetUser(UsernameInput.Text, out user);

                bool? isValidPassword = null;

                if (userExists)
                {
                    using (SecureString entry = PasswordInput.SecurePassword)
                    {
                        isValidPassword = Censorship.IsValidPassword(user, entry);
                    }
                }

                if (userExists & isValidPassword ?? false)
                {
                    Home window = new Home(user);
                    window.Show();
                }
                else
                {
                    MessageBox.Show("FAILED");
                }
            }
            else // Create
            {
                Guid salt = Guid.NewGuid();
                Password firstEntry;
                Password secondEntry;

                using (SecureString password = PasswordInput.SecurePassword)
                {
                    Censorship security = new Censorship();
                    firstEntry = security.Sha256Encrypt(password, salt);
                }

                using (SecureString password = ReEnterPasswordInput.SecurePassword)
                {
                    Censorship security = new Censorship();
                    secondEntry = security.Sha256Encrypt(password, salt);
                }

                bool passwordsMatch = firstEntry.Hash.SequenceEqual(secondEntry.Hash);
                bool usernameExists = Source.UsernameExists(UsernameInput.Text);

                if (passwordsMatch && !usernameExists)
                {
                    User user = new User();
                    user.Username = UsernameInput.Text;
                    user.Password = firstEntry;

                    Source.AddUser(user);

                    Home window = new Home(user);
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Passwords do not match or user already exists.");
                }
            }
        }
        
        private void UsernameInput_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
