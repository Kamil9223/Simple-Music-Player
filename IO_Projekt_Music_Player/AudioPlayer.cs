using NAudio.Wave;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows.Input;

namespace IO_Projekt_Music_Player
{
    public class AudioPlayer : BindableBase
    {
        //commands
        public ICommand TrackControlMouseDownCommand { get; private set; }
        public ICommand TrackControlMouseUpCommand { get; private set; }
        public ICommand VolumeControlValueChangedCommand { get; private set; }

        //properties
        private double _elapsedTime;
        public double elapsedTime
        {
            get{ return _elapsedTime; }

            set
            {
                _elapsedTime = value;
                RaisePropertyChanged("elapsedTime");
            }
        }

        private double _maximum;
        public double maximum
        {
            get{ return _maximum; }

            set
            {
                _maximum = value;
                RaisePropertyChanged("maximum");
            }
        }

        private double _currentVolume;
        public double currentVolume
        {
            get{ return _currentVolume; }

            set
            {
                _currentVolume = value;
                RaisePropertyChanged("currentVolume");
            }
        }

        private string _title;
        public string title
        {
            get{ return _title; }

            private set
            {
                _title = value;
                RaisePropertyChanged("title");
            }
        }

        public void setTitle(string value)
        {
            this.title = value;
        }

        public void setMaximum(double value)
        {
            this.maximum = value;
        }

        public void setElapsedTime(double value)
        {
            this.elapsedTime = value;
        }


        //AudioReader from NAudio libraty
        public AudioFileReader audioFileReader { get;  set; }
        public DirectSoundOut output { get; private set; }
        private bool isPaused;

        //Constructor
        public AudioPlayer()
        {
            this.elapsedTime = 0;
            this.currentVolume = 0.5;
            this.isPaused = false;

            TrackControlMouseDownCommand = new DelegateCommand(TrackControlMouseDownAction);
            TrackControlMouseUpCommand = new DelegateCommand(TrackControlMouseUpAction);
            VolumeControlValueChangedCommand = new DelegateCommand(VolumeControlValueChangedAction);
        }


        //methods
        public void init(string filepath)
        {
            try
            {
                audioFileReader = new AudioFileReader(filepath);
                audioFileReader.Volume = (float)this.currentVolume;
                output = new DirectSoundOut(100);
                var wc = new WaveChannel32(audioFileReader);
                wc.PadWithZeroes = false;
                output.Init(wc);
            }
            catch
            {

            }
           
        }


        public void TrackControlMouseDownAction()
        {
            try
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    this.output.Pause();
                    this.isPaused = true;
                }
            }
            catch
            {

            }
            
        }


        public void TrackControlMouseUpAction()
        {
            try
            {
                if (this.output.PlaybackState == PlaybackState.Paused && this.isPaused == true)
                {
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(this.elapsedTime);
                    this.output.Play();
                    this.isPaused = false;
                }
                else if (this.output.PlaybackState == PlaybackState.Paused && this.isPaused == false)
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(this.elapsedTime);
            }
            catch
            {

            }
            

        }

        public void VolumeControlValueChangedAction()
        {
            try
            {
                audioFileReader.Volume = (float)this.currentVolume;
            }
            catch
            {

            }
        }

    }
}
