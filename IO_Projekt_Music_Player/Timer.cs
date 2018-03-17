using System;
using System.Windows.Threading;

namespace IO_Projekt_Music_Player
{
    public class Timer
    {

        public DispatcherTimer timer { get; private set; }
        AudioPlayer audioPlayer;
        Sequence sequence;

        public Timer(AudioPlayer audioPlayer, Sequence sequence)
        {
            this.audioPlayer = audioPlayer;
            this.sequence = sequence;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(0.5);
            this.timer.Tick += Timer_Tick;
            this.timer.Stop();
        }


        public void nextSong()
        {
            switch (sequence.sequence)
            {
                case Sequence.PlaybackOrder.Default:
                    PieceManager.currentlyPlaing++;
                    if (PieceManager.currentlyPlaing >= PlayListManager.listOfLoadedPathsFiles.Count)
                        PieceManager.currentlyPlaing = 0;
                    break;
                case Sequence.PlaybackOrder.Random:
                    Random rand = new Random();
                    PieceManager.currentlyPlaing = rand.Next(0, PlayListManager.listOfLoadedPathsFiles.Count);
                    break;
                case Sequence.PlaybackOrder.Loop:
                    
                    break;
                default:
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            audioPlayer.setElapsedTime(audioPlayer.audioFileReader.CurrentTime.TotalSeconds);

            if (audioPlayer.elapsedTime >= audioPlayer.maximum)
            {

                nextSong();

                if (audioPlayer.output != null &&
                    audioPlayer.output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    timer.Stop();
                    audioPlayer.setElapsedTime(0);
                    audioPlayer.output.Stop();
                }

                audioPlayer.init(PlayListManager.listOfLoadedPathsFiles[PieceManager.currentlyPlaing]);
                audioPlayer.setMaximum(audioPlayer.audioFileReader.TotalTime.TotalSeconds);

                audioPlayer.setTitle(System.IO.Path.GetFileName
                    (
                        PlayListManager.listOfLoadedPathsFiles[PieceManager.currentlyPlaing]
                    ));

                audioPlayer.output.Play();
                timer.Start();
            }
        }
    }
}
