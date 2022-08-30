using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using congestion.calculator.DB.DTO;
using Dapper;

namespace congestion.calculator.DB.Repositories
{
    public class TollRepository
    {
        public IEnumerable<TollDataDTO> GetTollData(SqlConnection connection, string vehicleId)
        {
            try
            {
                string sql = string.Format("SELECT [TollRegistry].PassData" +
                                            " FROM [TollRegistry]" +
                                            " INNER JOIN [Vehicles]" +
                                            " ON [Vehicles].Id = [TollRegistry].VehicleId" +
                                            " WHERE [Vehicles].RegistrationNo = '{0}'", vehicleId);
                return connection.Query<TollDataDTO>(sql);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IEnumerable<VehicleDTO> GetVehicleInfo(SqlConnection connection, string vehicleId)
        {
            try
            {
                string sql = string.Format("SELECT [Vehicles].RegistrationNo, [Vehicles].Type" +
                                            " FROM [Vehicles]" +
                                            " WHERE [Vehicles].RegistrationNo = '{0}'", vehicleId);
                return connection.Query<VehicleDTO>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
