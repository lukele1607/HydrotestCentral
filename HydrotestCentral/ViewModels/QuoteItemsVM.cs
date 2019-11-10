using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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

            QuoteItems.CollectionChanged += QuoteItems_CollectionChanged;
        }

        private void QuoteItems_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var itemRemove = e.OldItems[0] as QuoteItemVM;

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connection_String"].ConnectionString;
                using (SQLiteConnection connection = new SQLiteConnection ())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open ();
                    using (SQLiteCommand command = new SQLiteCommand (connection))
                    {
                        command.CommandText =
                            "DELETE FROM QTE_ITEMS where jobno = @jobno AND tab_index = @tab_index AND row_index= @row_index";

                        command.Parameters.Add (new SQLiteParameter ("@jobno", itemRemove.JobNo));
                        command.Parameters.Add (new SQLiteParameter ("@tab_index", itemRemove.TabIndex));
                        command.Parameters.Add (new SQLiteParameter ("@row_index", itemRemove.RowIndex));


                        command.ExecuteNonQuery ();
                    }
                }
            }
        }
    }
}
