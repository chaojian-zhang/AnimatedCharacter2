using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace AnimatedCharacter.Windows
{
    public partial class HistoryWindow : Window, INotifyPropertyChanged
    {
        #region Constructor
        public HistoryWindow()
        {
            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            ChatHistory = string.Join(Environment.NewLine, mainWindow.ConversationMessages.Select(m => Regex.Replace(m, "^((Reply)|(Question)): ", string.Empty)));

            InitializeComponent();
        }
        #endregion

        #region Public View Properties
        private string _ChatHistory;
        public string ChatHistory { get => _ChatHistory; set => SetField(ref _ChatHistory, value); }
        #endregion

        #region Data Binding
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<TType>(ref TType field, TType value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TType>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
