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

namespace Encrypter
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private User profile;

        private List<Deposit> encryptedFiles
        {
            get { return profile.Files.Where(x => x.IsEncrypted == true).ToList(); }
        }

        private List<Deposit> decryptedFiles
        {
            get { return profile.Files.Where(x => x.IsEncrypted == false).ToList(); }
        }

        public Home(User user)
        {
            InitializeComponent();

            #region Test Code
            user.Files = new List<Deposit>();

            user.Files.Add(new Deposit());
            user.Files[0].Directory = "C:\\Users\\Bob\\Documents";
            user.Files[0].FileName = "Notes";
            user.Files[0].Extension = ".txt";
            user.Files[0].IsEncrypted = true;

            user.Files.Add(new Deposit());
            user.Files[1].Directory = "C:\\Users\\Bob\\Pictures";
            user.Files[1].FileName = "Scenery";
            user.Files[1].Extension = ".jpg";
            user.Files[1].IsEncrypted = false;

            user.Files.Add(new Deposit());
            user.Files[2].Directory = "C:\\Users\\Bob\\Music";
            user.Files[2].FileName = "House Of Rock";
            user.Files[2].Extension = ".mp3";
            user.Files[2].IsEncrypted = true;

            user.Files.Add(new Deposit());
            user.Files[3].Directory = "C:\\Users\\Bob\\Documents";
            user.Files[3].FileName = "Budget";
            user.Files[3].Extension = ".xlsx";
            user.Files[3].IsEncrypted = true;

            user.Files.Add(new Deposit());
            user.Files[4].Directory = "C:\\Users\\Bob\\Documents";
            user.Files[4].FileName = "Notes";
            user.Files[4].Extension = ".txt";
            user.Files[4].IsEncrypted = false;

            user.Files.Add(new Deposit());
            user.Files[5].Directory = "C:\\Users\\Bob\\Pictures";
            user.Files[5].FileName = "Vacation";
            user.Files[5].Extension = ".jpg";
            user.Files[5].IsEncrypted = false;

            user.Files.Add(new Deposit());
            user.Files[6].Directory = "C:\\Users\\Bob\\Desktop";
            user.Files[6].FileName = "Speech";
            user.Files[6].Extension = ".mp4";
            user.Files[6].IsEncrypted = true;

            user.Files.Add(new Deposit());
            user.Files[7].Directory = "C:\\Users\\Bob\\";
            user.Files[7].FileName = "Meeting Minutes";
            user.Files[7].Extension = ".docx";
            user.Files[7].IsEncrypted = true;
            #endregion

            profile = user;

            EncryptedFileCabinet.ItemsSource = encryptedFiles;
            DecryptedFileCabinet.ItemsSource = decryptedFiles;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
