using GeoCoordinatePortable;
using System;

namespace WebApplication_attw114.Helper
{
    public class GeoHelper
    {
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Bán kính Trái Đất (km)

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // khoảng cách tính bằng km
        }

        private double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }
        public double CalculateDistanceMetter(double lat1, double lon1, double lat2, double lon2)
        {
            var p1 = new GeoCoordinate(lat1, lon1);
            var p2 = new GeoCoordinate(lat2, lon2);
            return p1.GetDistanceTo(p2);
        }
    }
}
