﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv
{
    public partial class SelectableObservableCollection<T> : ObservableCollection<T>
    {
        private T selectedItem;
        public T SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem")); } }
    }
}