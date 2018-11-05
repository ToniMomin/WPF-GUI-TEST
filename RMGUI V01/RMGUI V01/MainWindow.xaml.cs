using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RMGUI_V01
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();


        }

        private void btn_Suche_Click(object sender, RoutedEventArgs e)
        {
            tbi_suche.Focus();
        }

        private void btn_depot_Click(object sender, RoutedEventArgs e)
        {
            tbi_depot.Focus();
        }

        private void btn_verwaltung_Click(object sender, RoutedEventArgs e)
        {
            tbi_verwalten.Focus();
        }

        private void btn_Historie_Click(object sender, RoutedEventArgs e)
        {
            tbi_historie.Focus();
        }

        private void txb_Name_KeyDown(object sender, KeyEventArgs e)
        {
            string Name = txb_Name.Text;
            if (e.Key == Key.Enter)
                {
                using (var c = new SqlConnection("Server=goliath.wi.fh-flensburg.de;Database=ws1819_spford;User ID=ws1819_spford;Password=kpe_1921"))
                {
                    SqlCommand cmd = c.CreateCommand();

                    cmd.CommandText = "SELECT * FROM Besitzer WHERE Name LIKE @NameFilter";
                    cmd.Parameters.Add("@NameFilter", SqlDbType.VarChar, 50);
                    cmd.Parameters["@NameFilter"].Value = Name;

                    c.Open();

                    IDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        txb_vorname.Text =Convert.ToString(r["Vorname"]);
                        txb_KdNr.Text = Convert.ToString(r["KundenID"]);
                    }

                    c.Close();

                }
            }
            }

        private void btn_auslösersuche_Click(object sender, RoutedEventArgs e)
        {
            var data = tB_Suche.Text;

            using (var c = new SqlConnection("Server=goliath.wi.fh-flensburg.de;Database=ws1819_spford;User ID=ws1819_spford;Password=kpe_1921"))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = "select *from Besitzer b full outer join PKW p on b.KundenID = p.KundenID full outer join Reifensatz r on p.PKWID = r.PKWID full outer join Lagerung l on r.ReifenID = l.ReifenID full outer join Lagerplatz lagp on l.LPID = lagp.LPID where b.Name like @item or p.Kennzeichen like @item";
                cmd.Parameters.Add("@item", SqlDbType.VarChar, 50);
                cmd.Parameters["@item"].Value = data;

                c.Open();

                IDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    //ausgabe.Items.Add(Name = Convert.ToString((r["b.Vorname"])),Vor);
                    lV_ausgabe.Items.Add(new Eintrag { Name = Convert.ToString(r["Name"]), Vorname = Convert.ToString(r["Vorname"]), Kennzeichen = Convert.ToString(r["Kennzeichen"]) });
                }
            }
        }

        private void btn_speichern_Click(object sender, RoutedEventArgs e)
        {
            string name = txb_Name.Text;
            string vorname = txb_vorname.Text;
            int kdnr = Convert.ToInt32(txb_KdNr.Text);


            string query = " Insert into dbo.Besitzer(KundenID, Name, Vorname)"+
                            "Values(@filterkdnr, @filtername, @filtervorname)";

            using (var c = new SqlConnection("Server=goliath.wi.fh-flensburg.de;Database=ws1819_spford;User ID=ws1819_spford;Password=kpe_1921"))
            using (SqlCommand cmd = new SqlCommand(query, c))
            {
                
                cmd.CommandText = query;
                cmd.Parameters.Add("@filtername", SqlDbType.VarChar, 50);
                cmd.Parameters["@filtername"].Value = name;
                cmd.Parameters.Add("@filtervorname", SqlDbType.VarChar, 50);
                cmd.Parameters["@filtervorname"].Value = vorname;
                cmd.Parameters.Add("@filterkdnr", SqlDbType.Int);
                cmd.Parameters["@filterkdnr"].Value = kdnr;

                c.Open();

                cmd.ExecuteNonQuery();

                c.Close();
            }
        }
        public class Eintrag
        {
            public string Name { get; set; }
            public string Vorname { get; set; }
            public string Kennzeichen { get; set; }
        }
        

    }
}