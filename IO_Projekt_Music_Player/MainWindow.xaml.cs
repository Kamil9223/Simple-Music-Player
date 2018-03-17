using System.Windows;

namespace IO_Projekt_Music_Player
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
            //Create Objects
            AudioPlayer audioPlayer = new AudioPlayer();
            Sequence sequence = new Sequence();
            Timer timer = new Timer(audioPlayer, sequence);
            PlayListManager playListManager = new PlayListManager(audioPlayer, timer);
            PieceManager pieceManager = new PieceManager(FilesInPlayList, audioPlayer, playListManager, timer);

            

            //assigment datacontext properties to appropriate objects
            Load.DataContext = playListManager;
            FilesInPlayList.DataContext = playListManager;
            Remove.DataContext = playListManager;
            Clear.DataContext = playListManager;
            Window.DataContext = playListManager;

            Play.DataContext = pieceManager;
            Pause.DataContext = pieceManager;
            Stop.DataContext = pieceManager;
            Next.DataContext = pieceManager;
            

            Loop.DataContext = sequence;
            Rand.DataContext = sequence;

            ProgressSlider.DataContext = audioPlayer;
            VolumeSlider.DataContext = audioPlayer;
            tytul.DataContext = audioPlayer;
        }
    }
}