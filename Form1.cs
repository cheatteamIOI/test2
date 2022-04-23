using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        private GMapMarker _selectedMarker;

        DB db = new DB();

        List<object> id = new List<object>();
       
        object mark;
        object lat = 0;
        object len = 0;
        int col_markers = 0;

        
        public Form1()
        {
            InitializeComponent();
            
            gMapControl1.MouseUp += _gMapControl_MouseUp;
            gMapControl1.MouseDown += _gMapControl_MouseDown;
            
        }


        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.Position = new GMap.NET.PointLatLng(55.030488, 82.925218);

            gMapControl1.ShowCenter = false;
            gMapControl1.MinZoom = 2; 
            gMapControl1.MaxZoom = 16; 
            gMapControl1.Zoom = 4; 

            GMapOverlay markers = new GMapOverlay("markers");

            db.openconnection();

            SqlCommand command = new SqlCommand("SELECT * FROM markers", db.getconnection());
            
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {

                while ( reader.Read() )
                {
                col_markers++;

                id.Add(reader.GetValue(0));
                mark = reader.GetValue(1);
                lat = reader.GetValue(2);
                len = reader.GetValue(3);


                GMapMarker marker1 = new GMarkerGoogle(
                    new GMap.NET.PointLatLng((double)lat, (double)len),
                    GMarkerGoogleType.lightblue);

                marker1.Tag = id[col_markers-1];
                marker1.ToolTipText = (string)mark;    
                markers.Markers.Add(marker1);

                gMapControl1.Overlays.Add(markers);
                }
            }

            reader.Close();
            db.closeconnection();
           
        }

        private void _gMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            _selectedMarker = gMapControl1.Overlays
                .SelectMany(o => o.Markers)
                .FirstOrDefault(m => m.IsMouseOver == true);
        }

        private void _gMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_selectedMarker is null)
                return;

            var latlng = gMapControl1.FromLocalToLatLng(e.X, e.Y);

            _selectedMarker.Position = latlng;

            db.openconnection();

            SqlCommand command2 = new SqlCommand("UPDATE markers SET lat = @lat, len = @len  WHERE id = @id", db.getconnection());
            command2.Parameters.Add("@lat", SqlDbType.Float).Value = (double)latlng.Lat;
            command2.Parameters.Add("@len", SqlDbType.Float).Value = (double)latlng.Lng;
            command2.Parameters.Add("@id", SqlDbType.Int).Value = (int)_selectedMarker.Tag;

            if (command2.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка обновления координат");

            db.closeconnection();

            _selectedMarker = null;
        }

        
        private void gMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void gMapControl1_OnMapZoomChanged()
        {

        }
    }
}
