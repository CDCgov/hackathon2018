using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foundation.Example.WebUI.Models;

namespace Foundation.Example.WebUI.Converters
{
    /// <summary>
    /// Class for converting a Csv file into Customers
    /// </summary>
    public sealed class CsvToCustomersConverter
    {
        /// <summary>
        /// Converts a line from a Csv file into a Customer
        /// </summary>
        /// <param name="csvLines">The Csv to be converted</param>
        /// <returns>List of Customer</returns>
        public List<Customer> Convert(string csvLines)
        {
            var lines = csvLines.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            var customers = new List<Customer>();

            foreach (var line in lines)
            {
                var customer = ConvertLine(line);
                customers.Add(customer);
            }

            return customers;
        }

        /// <summary>
        /// Converts a line from a Csv file into a Customer
        /// </summary>
        /// <param name="line">The line to be converted</param>
        /// <returns>Customer</returns>
        private Customer ConvertLine(string line)
        {
            // Just a really, really basic CSV parser for Customers... don't use this in production

            var values = line.Split(',');
            var customer = new Customer
            {
                Id = values[0].Replace("\"", string.Empty),
                FirstName = values[1].Replace("\"", string.Empty),
                LastName = values[2].Replace("\"", string.Empty),
                Age = values.Length >= 3 && int.TryParse(values[3], out _) ? int.Parse(values[3]) : default(int),
                StreetAddress = values.Length >= 5 ? values[4].Replace("\"", string.Empty) : string.Empty,
                DateOfBirth = values.Length >= 6 && DateTime.TryParse(values[5], out _) ? DateTime.Parse(values[5]) : default(DateTime),
            };
            return customer;
        }
    }
}