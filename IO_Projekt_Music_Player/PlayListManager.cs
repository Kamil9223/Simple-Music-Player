using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace IO_Projekt_Music_Player
{
    public class PlayListManager : BindableBase
    {
        //commands
        public ICommand LoadFilesCommand { get;  set; }
        public ICommand DoubleClickCommand { get;  set; }
        public ICommand RemoveFileCommand { get;  set; }
        public ICommand ClearAllCommand { get;  set; }
        public ICommand CloseCommand { get;  private set; }


        //Properties with getters setters
        private ObservableCollection<string> _listOfLoadedFiles;
        public ObservableCollection<string> listOfLoadedFiles
        {
            get
            {
                return _listOfLoadedFiles;
            }
            set
            {
                _listOfLoadedFiles = value;
                RaisePropertyChanged("listOfLoadedFiles");
            }
        }

        public string path { get; private set; }

        public void setPath(string path)
        {
            this.path = path;
        }

        public static ObservableCollection<string> listOfLoadedPathsFiles { get; private set; }
        AudioPlayer audioPlayer;
        Timer timer;

        //Constructor
        public PlayListManager(AudioPlayer audioPlayer, Timer timer)
        {
            this.audioPlayer = audioPlayer;
            this.timer = timer;
            this.path = "history.txt";

            listOfLoadedFiles = new ObservableCollection<string>();
            listOfLoadedPathsFiles = new ObservableCollection<string>();

            LoadFilesCommand = new DelegateCommand(LoadFilesAction);
            RemoveFileCommand = new DelegateCommand(RemoveFilesAction);
            DoubleClickCommand = new DelegateCommand(DoubleClickAction);
            ClearAllCommand = new DelegateCommand(ClearAllAction);
            CloseCommand = new DelegateCommand(closeAction);
            
            this.rememberLastFiles();
        }

        //Methods

        public void rememberLastFiles()
        {
            try
            {
                FileStream fStream = new FileStream("history.txt", FileMode.Open);
                StreamReader reader = new StreamReader(fStream);
                int index = 0;
                string title;
                do
                { 
                    title = reader.ReadLine();
                    if (title == null)
                        break;

                    listOfLoadedPathsFiles.Add(title);
                    this.listOfLoadedFiles.Add(Path.GetFileName(title));
                    index++;
                } while (title != null);
                reader.Close();
                fStream.Close();
            }
            catch
            {
                //MessageBox.Show("Nie istnieje plik, z history");
            }
        }

        public void LoadFilesAction()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    listOfLoadedPathsFiles.Add(filename);
                    listOfLoadedFiles.Add(Path.GetFileName(filename));
                }    
            }
        }

        public void RemoveFilesAction()
        {
            if (PieceManager.checkFocus() != -1)
            {
                listOfLoadedPathsFiles.RemoveAt(PieceManager.checkFocus());
                this.listOfLoadedFiles.RemoveAt(PieceManager.checkFocus());
                PieceManager.currentlyPlaing = 0;
            }
        }

        public void ClearAllAction()
        {
            listOfLoadedPathsFiles.Clear();
            this.listOfLoadedFiles.Clear();
            PieceManager.currentlyPlaing = 0;
        }

        public void DoubleClickAction()
        {
            if (PieceManager.checkFocus() != -1)
            {
                if (audioPlayer.output != null && 
                    audioPlayer.output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    timer.timer.Stop();
                    audioPlayer.setElapsedTime(0);
                    audioPlayer.output.Stop();
                }
                //ustawiamy na aktualnie odtwarzany ten, na ktorym jest focus
                PieceManager.currentlyPlaing = PieceManager.checkFocus();
                audioPlayer.setTitle(this.listOfLoadedFiles[PieceManager.currentlyPlaing]);
                try
                {
                    audioPlayer.init(listOfLoadedPathsFiles[PieceManager.currentlyPlaing]);
                    audioPlayer.setMaximum(audioPlayer.audioFileReader.TotalTime.TotalSeconds);

                    audioPlayer.output.Play();
                    timer.timer.Start();
                }
                catch
                {
                    MessageBox.Show("Nie istnieje plik o podanej sciezce");
                }
            }
        }

        public void closeAction()
        {
            FileStream fStream = new FileStream(this.path, FileMode.Create);
            StreamWriter writter = new StreamWriter(fStream);

            foreach(string filename in listOfLoadedPathsFiles)
            {
                writter.WriteLine(filename);
                writter.Flush();
            }
            writter.Close();
            fStream.Close();
        }
    }
}
