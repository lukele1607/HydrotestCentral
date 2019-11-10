using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HydrotestCentral.Infrastructure;

namespace HydrotestCentral.ViewModels
{
    public class QuoteItemsVM : PropertyChangedNotifier
    {
        private ObservableCollection<QuoteItemVM> quoteItems;
        public ObservableCollection<QuoteItemVM> QuoteItems
        {
            get => quoteItems;
            set
            {
                if (quoteItems != value)
                {
                    quoteItems = value;
                    TriggerPropertyChangedEvent (nameof (QuoteItems));
                }
            }
        }

        private QuoteItemVM selectedQuote;
        public QuoteItemVM SelectedQuote
        {
            get => selectedQuote;
            set
            {
                if (selectedQuote != value)
                {
                    selectedQuote = value;
                    TriggerPropertyChangedEvent (nameof (SelectedQuote));
                }
            }
        }

        public QuoteItemsVM (IEnumerable<QuoteItemVM> quoteItemsVM)
        {
            QuoteItems = new ObservableCollection<QuoteItemVM> ();
            foreach(QuoteItemVM quoteItemVM in quoteItemsVM)
            {
                QuoteItems.Add (quoteItemVM);
            }
        }

    }
}
