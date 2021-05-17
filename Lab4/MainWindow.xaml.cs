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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace Lab4

{
    public partial class MainWindow
    {
        private readonly Microsoft.Win32.OpenFileDialog _fileDialog = new Microsoft.Win32.OpenFileDialog();
        private LFSR _lfsr;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            try
            {
                Preparation();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var destFile = TxtBoxPathToFile.Text + ".lfsr";
            try
            {
                _lfsr.Processing(TxtBoxPathToFile.Text, destFile);
            }
            catch
            {
                Result.AppendText("Failed to encrypt file...\n");
                return;
            }

            Result.AppendText("File successfully encrypted.\n");
        }


        private void Decrypt(object sender, RoutedEventArgs e)
        {
            try
            {
                Preparation();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var index = TxtBoxPathToFile.Text.LastIndexOf('.');
            var destFile = TxtBoxPathToFile.Text.Substring(0, index);
            try
            {
                _lfsr.Processing(TxtBoxPathToFile.Text, destFile);
            }
            catch
            {
                Result.AppendText("Failed to decrypt file...\n");
                return;
            }

            Result.AppendText("File successfully decrypted.\n");
        }

        private void Preparation()
        {
            if (!File.Exists(TxtBoxPathToFile.Text))
            {
                MessageBox.Show("File doesn't exists.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                throw new Exception();
            }

            _lfsr = new LFSR(ulong.Parse(Key.Text));
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            if (_fileDialog != null && (bool)_fileDialog.ShowDialog())
            {
                var fileName = _fileDialog.FileName;
                TxtBoxPathToFile.Text = fileName;
            }
        }
    }
}
