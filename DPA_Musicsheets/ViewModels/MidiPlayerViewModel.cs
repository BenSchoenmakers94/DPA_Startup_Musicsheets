using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models.Events;
using DPA_Musicsheets.ViewModels.States.Player;

namespace DPA_Musicsheets.ViewModels
{
    /// <summary>
    /// The viewmodel for playing midi sequences.
    /// It supports starting, stopping and restarting.
    /// </summary>
    public class MidiPlayerViewModel : ViewModelBase
    {
        private OutputDevice _outputDevice;
        private bool _running;
        private Dictionary<string, PlayerState> states;
        private PlayerState current;

        // This sequencer creates a possibility to play a sequence.
        // It has a timer and raises events on the right moments.
        private Sequencer _sequencer;

        public Sequence MidiSequence
        {
            get => _sequencer.Sequence;
            set
            {
                StopCommand.Execute(null);
                _sequencer.Sequence = value;
                UpdateButtons();
            }
        }

        public bool Running
        {
            get => _running;
            set
            {
                _running = value;
                RaisePropertyChanged(() => Running);
                UpdateButtons();
            }
        }

        public MidiPlayerViewModel(FileHandleFacade fileHandleFacade)
        {
            // The OutputDevice is a midi device on the midi channel of your computer.
            // The audio will be streamed towards this output.
            // DeviceID 0 is your computer's audio channel.
            _outputDevice = new OutputDevice(0);
            _sequencer = new Sequencer();

            _sequencer.ChannelMessagePlayed += ChannelMessagePlayed;

            // Wanneer de sequence klaar is moeten we alles closen en stoppen.
            _sequencer.PlayingCompleted += (playingSender, playingEvent) =>
            {
                _sequencer.Stop();
                _running = false;
            };

            states = new Dictionary<string, PlayerState>
            {
                {"Playing", new PlayingState(_sequencer) },
                {"Paused", new PausedState(_sequencer) },
                {"Stopped", new StoppedState(_sequencer) }
            };

            ChangeState("Stopped");
            OwnEventmanager.Manager.Subscribe("changePlayerState", ChangeState);
            OwnEventmanager.Manager.Subscribe("setSequence", SetSequence);
        }

        private void SetSequence(object obj)
        {
            MidiSequence = (Sequence) obj;
        }

        public void ChangeState(object obj)
        {
            current = states[(string)obj];
            current.GoInto(this);
        }

        public void UpdateButtons()
        {
            PlayCommand.RaiseCanExecuteChanged();
            PauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        // Wanneer een channelmessage langskomt sturen we deze direct door naar onze audio.
        // Channelmessages zijn tonen met commands als NoteOn en NoteOff
        // In midi wordt elke noot gespeeld totdat NoteOff is benoemd. Wanneer dus nooit een NoteOff komt nadat die een NoteOn heeft gehad
        // zal deze note dus oneindig lang blijven spelen.
        private void ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            try
            {
                _outputDevice.Send(e.Message);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is OutputDeviceException)
            {
                // Don't crash when we can't play
                // We have to do it this way because IsDisposed on
                // _outDevice may be false when it is being disposed
                // so this is the only safe way to prevent race conditions
            }
        }

        #region buttons for play, stop, pause
        public RelayCommand PlayCommand => new RelayCommand(() =>
        {
            current.Play(this);
        }, () => !_running && _sequencer.Sequence != null);

        public RelayCommand StopCommand => new RelayCommand(() =>
        {
            current.Stop(this);
        }, () => _running);

        public RelayCommand PauseCommand => new RelayCommand(() =>
        {
            current.Pause(this);
        }, () => _running);

        #endregion buttons for play, stop, pause

        /// <summary>
        /// Stop the player and clear the sequence on cleanup.
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();

            _sequencer.Stop();
            _sequencer.Dispose();
            _outputDevice.Dispose();
        }
    }
}
