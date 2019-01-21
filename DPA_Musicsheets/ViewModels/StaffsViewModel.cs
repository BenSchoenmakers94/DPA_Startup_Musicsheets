using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using PSAMControlLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
        // These staffs will be bound to.
        public ObservableCollection<MusicalSymbol> Staffs { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StaffsViewModel()
        {
            Staffs = new ObservableCollection<MusicalSymbol>();
            OwnEventmanager.Manager.Subscribe("setStaffs", SetStaffs);
        }

        /// <summary>
        /// SetStaffs fills the observablecollection with new symbols. 
        /// We don't want to reset the collection because we don't want other classes to create an observable collection.
        /// </summary>
        /// <param name="obj">The new symbols to show.</param>
        public void SetStaffs(object obj)
        {
            //TODO convert incoming Domain staffs to MusicalSymbols
            IList<MusicalSymbol> symbols = (IList<MusicalSymbol>) obj;
            Staffs.Clear();
            foreach (var symbol in symbols)
            {
                Staffs.Add(symbol);
            }
        }
    }
}
