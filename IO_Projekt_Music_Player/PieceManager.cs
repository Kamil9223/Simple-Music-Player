using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IO_Projekt_Music_Player
{
    public class PieceManager : BindableBase
    {
        //commands
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }

        //properties

        static ListBox FilesInPlayList;

        AudioPlayer audioPlayer;
        PlayListManager playListManager;
        Timer timer;
        public static int currentlyPlaing = 0;


        //Constructor

        public PieceManager(ListBox FilesInPlayList, AudioPlayer connector, PlayListManager playListManager, Timer timer)
        {
            PieceManager.FilesInPlayList = FilesInPlayList;

            this.audioPlayer = connector;
            this.playListManager = playListManager;
            this.timer = timer;

            PlayCommand = new DelegateCommand(clickPlayAction);
            PauseCommand = new DelegateCommand(clickPauseAction);
            StopCommand = new DelegateCommand(clickStopAction);
            NextSongCommand = new DelegateCommand(nextSongAction);
        }

        //Methods

        public static int checkFocus()
        {
            for (int i = 0; i < PieceManager.FilesInPlayList.Items.Count; i++)
            {
                object yourObject = PieceManager.FilesInPlayList.Items[i];
                ListBoxItem lbi = (ListBoxItem)PieceManager.FilesInPlayList.ItemContainerGenerator.ContainerFromItem(yourObject);
                if (lbi.IsFocused)
                    return i;
            }
            return -1;
        }



        public void clickPlayAction()
        {
            if (PlayListManager.listOfLoadedPathsFiles.Count == 0)
                return;

            if (audioPlayer.output != null &&
                    audioPlayer.output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                timer.timer.Stop();
                audioPlayer.setElapsedTime(0);
                audioPlayer.output.Stop();
            }
            try
            {
                audioPlayer.init(PlayListManager.listOfLoadedPathsFiles[currentlyPlaing]);
                audioPlayer.setMaximum(audioPlayer.audioFileReader.TotalTime.TotalSeconds);
                audioPlayer.setTitle(playListManager.listOfLoadedFiles[PieceManager.currentlyPlaing]);

                audioPlayer.output.Play();
                timer.timer.Start();
            }
            catch
            {
                MessageBox.Show("Nie istnieje plik o podanej sciezce");
            }
        }



        public void clickPauseAction()
        {
            if (audioPlayer.output != null &&
                audioPlayer.output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                audioPlayer.output.Pause();
                timer.timer.Stop();
            }

            else if (audioPlayer.output != null &&
                    audioPlayer.output.PlaybackState == NAudio.Wave.PlaybackState.Paused)
            {
                audioPlayer.output.Play();
                timer.timer.Start();
            }

        }

        public void clickStopAction()
        {
            if (audioPlayer.output != null)
            {
                audioPlayer.setElapsedTime(0);
                timer.timer.Stop();
                audioPlayer.output.Stop();
            }
        }

        public void nextSongAction()
        {
            timer.nextSong();
            clickPlayAction();
        }
    }
}
