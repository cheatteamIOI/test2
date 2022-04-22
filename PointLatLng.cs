using System;

namespace WindowsFormsApp1
{
    internal class PointLatLng
    {
        internal object Lat;
        internal object Lng;
        

        public static implicit operator GMap.NET.PointLatLng(PointLatLng v)
        {
            throw new NotImplementedException();
        }
    }
}