using System.ComponentModel;
using System.Data.SQLite;
using HydrotestCentral.Infrastructure;
using HydrotestCentral.Model;
using Microsoft.Win32;

namespace HydrotestCentral.ViewModels
{
    public class QuoteItemVM : PropertyChangedNotifier
    {
        private int qty;
        public int Qty
        {
            get => qty;
            set
            {
                if (qty != value)
                {
                    qty = value;
                    TriggerPropertyChangedEvent (nameof (Qty));
                }
            }
        }

        private string item;
        public string Item
        {
            get => item;
            set
            {
                if (item != value)
                {
                    item = value;
                    TriggerPropertyChangedEvent (nameof (Item));
                }
            }
        }

        private double rate;
        public double Rate
        {
            get => rate;
            set
            {
                if (rate != value)
                {
                    rate = value;
                    TriggerPropertyChangedEvent (nameof (Rate));
                }
            }
        }

        private string descr;
        public string Descr
        {
            get => descr;
            set
            {
                if (descr != value)
                {
                    descr = value;
                    TriggerPropertyChangedEvent (nameof (Descr));
                    Save ();
                }
            }
        }

        private int group;
        public int Group
        {
            get => group;
            set
            {
                if (group != value)
                {
                    group = value;
                    TriggerPropertyChangedEvent (nameof (Group));
                }
            }
        }


        private bool taxable;
        public bool Taxable
        {
            get => taxable;
            set
            {
                if (taxable != value)
                {
                    taxable = value;
                    TriggerPropertyChangedEvent (nameof (Taxable));
                }
            }
        }

        private bool discountable;
        public bool Discountable
        {
            get => discountable;
            set
            {
                if (discountable != value)
                {
                    discountable = value;
                    TriggerPropertyChangedEvent (nameof (Discountable));
                }
            }
        }

        private bool printable;
        public bool Printable
        {
            get => printable;
            set
            {
                if (printable != value)
                {
                    printable = value;
                    TriggerPropertyChangedEvent (nameof (Printable));
                }
            }
        }

        private string jobNo;
        public string JobNo
        {
            get => jobNo;
            set
            {
                if (jobNo != value)
                {
                    jobNo = value;
                    TriggerPropertyChangedEvent (nameof (jobNo));
                }
            }
        }

        private double line_total;
        public double LineTotal
        {
            get => line_total;
            set
            {
                if (line_total != value)
                {
                    line_total = value;
                    TriggerPropertyChangedEvent (nameof (LineTotal));
                }
            }
        }

        private double tax_total;
        public double TaxTotal
        {
            get => tax_total;
            set
            {
                if (tax_total != value)
                {
                    tax_total = value;
                    TriggerPropertyChangedEvent (nameof (TaxTotal));
                }
            }
        }

        private int tabIndex;
        public int TabIndex
        {
            get => tabIndex;
            set
            {
                if (tabIndex != value)
                {
                    tabIndex = value;
                    TriggerPropertyChangedEvent (nameof (TabIndex));
                }
            }
        }

        private int rowIndex;
        public int RowIndex
        {
            get => rowIndex;
            set
            {
                if (rowIndex != value)
                {
                    rowIndex = value;
                    TriggerPropertyChangedEvent (nameof (rowIndex));
                }
            }
        }

        private void Save ()
        {
            //Only save descr.
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connection_String"].ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection ())
            {
                connection.ConnectionString = connectionString;
                connection.Open ();
                using (SQLiteCommand command = new SQLiteCommand (connection))
                {
                    command.CommandText =
                        "update QTE_ITEMS set descr = @descr where jobno = @jobno AND tab_index = @tab_index AND row_index= @row_index";

                    command.Parameters.Add (new SQLiteParameter ("@descr", Descr));
                    command.Parameters.Add (new SQLiteParameter ("@jobno", JobNo));
                    command.Parameters.Add (new SQLiteParameter ("@tab_index", TabIndex));
                    command.Parameters.Add (new SQLiteParameter ("@row_index", RowIndex));

                    
                    command.ExecuteNonQuery ();
                }
            }
        }
    }
}
