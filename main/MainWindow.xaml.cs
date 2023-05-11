using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Business.Concrete;
using NAudio.Lame;
using NAudio.Wave;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using MessageBox = System.Windows.MessageBox;

namespace main;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly YoutubeConverter converter = new YoutubeConverter();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void btn_save_Click(object sender, RoutedEventArgs e)
    {
        var result = await converter.DownloadVideo(tbx1.Text);

        MessageBox.Show(result ? "Conversion successful!" : "Conversion failed.");

    }

    private void btn_path_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            converter.SetSelectedFilePath(dialog.SelectedPath);
            lbl_path.Content = dialog.SelectedPath;
        }
    }

}