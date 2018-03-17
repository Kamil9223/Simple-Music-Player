using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IO_Projekt_Music_Player
{
    public class Sequence : BindableBase
    {
        //commands
        public ICommand LoopCommand { get; private set; }
        public ICommand RandomCommand { get; private set; }

        //states give info about sequence of playing pieces of musics
        public enum PlaybackOrder { Default, Random, Loop }
        public PlaybackOrder sequence { get; private set; }

        public void setSequence(PlaybackOrder state)
        {
            this.sequence = state;
        }

        private SolidColorBrush _colourLoop;
        public SolidColorBrush colourLoop
        {
            get
            {
                return _colourLoop;
            }
            private set
            {
                _colourLoop = value;
                RaisePropertyChanged("colourLoop");
            }
        }

        private SolidColorBrush _colourRand;
        public SolidColorBrush colourRand
        {
            get
            {
                return _colourRand;
            }
            private set
            {
                _colourRand = value;
                RaisePropertyChanged("colourRand");
            }
        }

        //Constructor
        public Sequence()
        {
            this.sequence = PlaybackOrder.Default;
            this.colourLoop = Brushes.Black;
            this.colourRand = Brushes.Black;

            LoopCommand = new DelegateCommand(LoopAction);
            RandomCommand = new DelegateCommand(RandomAction);
        }


        public void LoopAction()
        {
            if (this.sequence != PlaybackOrder.Loop)
            {
                this.sequence = PlaybackOrder.Loop;
                this.colourLoop = Brushes.Green;
                this.colourRand = Brushes.Black;
            }

            else
            {
                this.sequence = PlaybackOrder.Default;
                this.colourLoop = Brushes.Black;
            }
        }

        public void RandomAction()
        {
            if (this.sequence != PlaybackOrder.Random)
            {
                this.sequence = PlaybackOrder.Random;
                this.colourRand = Brushes.Green;
                this.colourLoop = Brushes.Black;
            }
                
            else
            {
                this.sequence = PlaybackOrder.Default;
                this.colourRand = Brushes.Black;
            }
        }

    }
}
