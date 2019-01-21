using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using PSAMControlLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
        // These staffs will be bound to.
        public ObservableCollection<MusicalSymbol> Staffs { get; }
        private readonly FileHandleFacade fileHandleFacade;

        /// <summary>
        /// Constructor
        /// </summary>
        public StaffsViewModel(FileHandleFacade fileHandleFacade)
        {
            this.fileHandleFacade = fileHandleFacade;
            Staffs = new ObservableCollection<MusicalSymbol>();
            OwnEventmanager.Manager.Subscribe("setStaffs", SetStaffs);
        }

        /// <summary>
        /// SetStaffs fills the observablecollection with new symbols. 
        /// We don't want to reset the collection because we don't want other classes to create an observable collection.
        /// </summary>
        /// <param name="obj">External score to convert</param>
        public void SetStaffs(object obj)
        {
            IList<MusicalSymbol> symbols = fileHandleFacade.GetWpfMusicalSymbols((Score) obj);
            Staffs.Clear();
            foreach (var symbol in symbols)
            {
                Staffs.Add(symbol);
            }
        }
    }
}
