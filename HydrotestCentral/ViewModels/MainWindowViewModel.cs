using HydrotestCentral.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using HydrotestCentral.Model;
using System.Data;
using System.Windows;

namespace HydrotestCentral.ViewModels
{
    public partial class MainWindowViewModel: PropertyChangedNotifier
    {
        //static string connectionString = Properties.Settings.Default.connString;
        SQLiteConnection connection;
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;
        DataSet ds;
        string connection_String = System.Configuration.ConfigurationManager.ConnectionStrings["connection_String"].ConnectionString;


        private ObservableCollection<QuoteHeader> quote_header_data = null;

        public ObservableCollection<QuoteHeader> quote_headers
        {
            get
            {
                if (quote_header_data != null)
                {
                    return quote_header_data;
                }
                return null;
            }
            set
            {
                if (quote_header_data != value)
                {
                    quote_header_data = value;
                    OnPropertyChanged("quote_headers");
                }
            }
        }

        private QuoteItemsVM quoteItemsVM;
        public QuoteItemsVM QuoteItemsVM
        {
            get => quoteItemsVM;
            set
            {
                if (quoteItemsVM != value)
                {
                    quoteItemsVM = value;
                    TriggerPropertyChangedEvent (nameof (QuoteItemsVM));
                }
            }
        }
        //public ObservableCollection<QuoteItem> quote_items { get; set; }
        public ObservableCollection<InventoryItem> inventory_items { get; set; }

        public MainWindowViewModel()
        {
            InitializeComponent();

            quote_headers = new ObservableCollection<QuoteHeader>();
            quote_headers = LoadQuoteHeaderData();

            var quoteItems = LoadQuoteItemData ();

            QuoteItemsVM = new QuoteItemsVM (quoteItems.Select (x => new QuoteItemVM { Qty = x.qty, Item = x.item, Rate = x.rate, Descr = x.descr, Group = x.group, Taxable = x.taxable, Discountable = x.discountable, Printable = x.printable, LineTotal = x.line_total, TaxTotal = x.tax_total, JobNo = x.jobno, TabIndex = x.tab_index, RowIndex = x.row_index }));
        }

        public ObservableCollection<QuoteHeader> LoadQuoteHeaderData()
        {
            var headers = new ObservableCollection<QuoteHeader>();

            try
            {
                connection = new SQLiteConnection(connection_String);
                connection.Open();
                cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM QTE_HDR");
                adapter = new SQLiteDataAdapter(cmd);

                ds = new DataSet();

                adapter.Fill(ds, "QTE_HDR");


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int cleaned_days = 0;
                    double cleaned_value = 0.00;

                    if (Int32.TryParse(dr[8].ToString(), out cleaned_days)) { }

                    if (Double.TryParse(dr[17].ToString(), out cleaned_value)) { }

                    headers.Add(new QuoteHeader
                    {
                            quoteno = dr[0].ToString(),
                            jobno = dr[0].ToString(),
                            qt_date = dr[1].ToString(),
                            cust = dr[2].ToString(),
                            cust_contact = dr[3].ToString(),
                            cust_phone = dr[4].ToString(),
                            cust_email = dr[5].ToString(),
                            loc = dr[6].ToString(),
                            salesman = dr[7].ToString(),
                            days_est = cleaned_days,
                            status = dr[9].ToString(),
                            pipe_line_size = dr[10].ToString(),
                            pipe_length = dr[11].ToString(),
                            pressure = dr[12].ToString(),
                            endclient = dr[13].ToString(),
                            supervisor = dr[14].ToString(),
                            est_startdate = dr[15].ToString(),
                            est_enddate = dr[16].ToString(),
                            value = cleaned_value
                    });
                     Console.WriteLine(dr[0].ToString() + " created in quote_headers");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return headers;
        }

        public ObservableCollection<QuoteItem> LoadQuoteItemData()
        {
            var items = new ObservableCollection<QuoteItem>();

            try
            {
                connection = new SQLiteConnection(connection_String);
                connection.Open();
                cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM QTE_ITEMS");
                adapter = new SQLiteDataAdapter(cmd);

                ds = new DataSet();

                adapter.Fill(ds, "QTE_ITEM");


                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int cleaned_qty = 0;
                        double cleaned_rate = 0.00;
                        int cleaned_group = 1;
                        bool cleaned_taxable = false;
                        bool cleaned_discountable = false;
                        bool cleaned_printable = false;
                        double cleaned_line_total = 0.00;
                        double cleaned_tax_total = 0.00;
                        int cleaned_tab_index = 0;
                        int cleaned_row_index = 0;

                        if (Int32.TryParse(dr[0].ToString(), out cleaned_qty)) { }
                        if (Double.TryParse(dr[2].ToString(), out cleaned_rate)) { }
                        if (Int32.TryParse(dr[4].ToString(), out cleaned_group)) { }
                        if (Boolean.TryParse(dr[5].ToString(), out cleaned_taxable)) { }
                        if (Boolean.TryParse(dr[6].ToString(), out cleaned_discountable)) { }
                        if (Boolean.TryParse(dr[7].ToString(), out cleaned_printable)) { }
                        if (Double.TryParse(dr[9].ToString(), out cleaned_line_total)) { }
                        if (Double.TryParse(dr[10].ToString(), out cleaned_tax_total)) { }
                        if (Int32.TryParse(dr[11].ToString(), out cleaned_tab_index)) { }
                        if (Int32.TryParse(dr[12].ToString(), out cleaned_row_index)) { }

                        items.Add(new QuoteItem
                        {
                            qty = cleaned_qty,
                            item = dr[1].ToString(),
                            rate = cleaned_rate,
                            descr = dr[3].ToString(),
                            group = cleaned_group,
                            taxable = cleaned_taxable,
                            discountable = cleaned_discountable,
                            printable = cleaned_printable,
                            jobno = dr[8].ToString(),
                            line_total = cleaned_line_total,
                            tax_total = cleaned_tax_total,
                            tab_index = cleaned_tab_index,
                            row_index = cleaned_row_index
                        });
                        //Console.WriteLine(dr[1].ToString() + " created in quote_items");
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return items;
        }

        public void DeleteQuoteItem(String jobno, int tab_index)
        {
            try
            {
                var start_collection = new ObservableCollection<QuoteItem>();
                connection = new SQLiteConnection(connection_String);
                connection.Open();
                cmd = connection.CreateCommand();
                cmd.CommandText = String.Format("DELETE FROM QTE_ITEMS WHERE jobno=\"{0}\" AND tab_index = {1}", jobno, tab_index);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public void DeleteHeaderItem(String jobno)
        {
            try
            {
                var start_collection = new ObservableCollection<QuoteItem>();
                connection = new SQLiteConnection(connection_String);
                connection.Open();
                cmd = connection.CreateCommand();
                cmd.CommandText = String.Format("DELETE FROM QTE_HDR WHERE jobno=\"{0}\"", jobno);
                cmd.ExecuteNonQuery();

                quote_headers = LoadQuoteHeaderData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        public void updateQuoteItemsByJob(string jobno)
        {
            var quoteItems = LoadQuoteItemData ();

            QuoteItemsVM = new QuoteItemsVM (quoteItems.Where (x => x.jobno == jobno).Select (x => new QuoteItemVM { Qty = x.qty, Item = x.item, Rate = x.rate, Descr = x.descr, Group = x.group, Taxable = x.taxable, Discountable = x.discountable, Printable = x.printable, LineTotal = x.line_total, TaxTotal = x.tax_total, JobNo = x.jobno, TabIndex = x.tab_index, RowIndex = x.row_index }));
        }

        public void updateQuoteItemsByJob_And_Tab(string jobno, int tabIndex)
        {
            var quoteItems = LoadQuoteItemData ();

            QuoteItemsVM = new QuoteItemsVM (quoteItems.Where (x => x.jobno == jobno && x.tab_index == tabIndex).Select (x => new QuoteItemVM { Qty = x.qty, Item = x.item, Rate = x.rate, Descr = x.descr, Group = x.group, Taxable = x.taxable, Discountable = x.discountable, Printable = x.printable, LineTotal = x.line_total, TaxTotal = x.tax_total, JobNo = x.jobno, TabIndex = x.tab_index, RowIndex = x.row_index }));
        }

    }
}
