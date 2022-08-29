using System;
using congestion.calculator;
public static class CongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    public static int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
       
        bool first = true;
        int fee = 0, tempFee = 0;
        int day = 0;
        int month = 0;
        int secs = 0, tempSecs = 0;

        foreach (DateTime date in dates)
        {
            // save day and month to qpply rule of passing more than once within 60 minutes
            // (assuming year is always the same)
            if(first == false)
            {
                if(day == date.Day && month == date.Month && fee != 0)
                {
                    tempSecs = date.Hour * 3600 + date.Minute * 60 + date.Second;
                    if((tempSecs - secs) >= 00 && (tempSecs - secs) < 3600)
                    {
                        tempFee = GetTollFee(date, vehicle);
                        fee = tempFee > fee ? fee : tempFee;
                    }   
                    else
                        fee = GetTollFee(date, vehicle);
                }
                else
                {
                    fee = GetTollFee(date, vehicle);
                }
                day = date.Day;
                month = date.Month;
                secs = date.Hour * 3600 + date.Minute * 60 + date.Second;
                totalFee += fee;
            }
            else
            {
                day = date.Day;
                month = date.Month;
                secs = date.Hour * 3600 + date.Minute * 60 + date.Second;
                fee = GetTollFee(date, vehicle);
                first = false;
            }            
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private static bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Bus.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public static int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int time = date.Hour * 3600 + date.Minute * 60 + date.Second;

        if (time >= 6 * 3600 && time <= 6 * 3600 + 29 * 60) return 8;
        else if (time >= 6 * 3600 + 30 * 60 && time <= 6 * 3600 + 59 * 60) return 13;
        else if (time >= 7 * 3600 && time <= 7 * 3600 + 59 * 60) return 18;
        else if (time >= 8 * 3600 && time <= 8 * 3600 + 29 * 60) return 13;
        else if (time >= 8 * 3600 + 30 * 60 && time <= 14 * 3600 + 59 * 60) return 8;
        else if (time >= 15 * 3600 && time <= 15 * 3600 + 29 * 60) return 13;
        else if (time >= 15 * 3600 + 30 * 60 && time <= 16 * 3600 + 59 * 60) return 18;
        else if (time >= 17 * 3600 && time <= 17 * 3600 + 59 * 60) return 13;
        else if (time >= 18 * 3600 && time <= 18 * 3600 + 29 * 60) return 8;
        else if (time >= 18 * 3600 + 30 * 60) return 0;
        else return 0;
    }

    private static Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Bus = 1,   // changed from tractor
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}