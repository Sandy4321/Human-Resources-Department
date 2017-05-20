﻿using System.Collections.Generic;

using Human_Resources_Department.classes.db;

namespace Human_Resources_Department.classes.employees.db
{
    class MainModel : Database
    {
        public static IEnumerable<MainTable> QueryEmployees(string q, object[] args = null)
        {
            try
            {
                if ( args == null )
                    return con.Query<MainTable>(q);

                return con.Query<MainTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static void CreateTableEmployees()
        {
            try
            {
                CreateTable<MainTable>();
            }
            catch { }
        }

        public static IEnumerable<MainTable> GetAllData(bool allEmployees = false)
        {
            if (allEmployees)
                return QueryEmployees("SELECT * FROM " + typeof(MainTable).Name
                    + " WHERE IsActivity = 1");

            return QueryEmployees("SELECT * FROM " + typeof(MainTable).Name);
        }

        public static IEnumerable<MainTable> GetOneData(int id)
        {
            return QueryEmployees("SELECT * FROM " + typeof(MainTable).Name
                + " WHERE id = ?", new object[] { id });
        }

        public static int GetCountRecords()
        {
            return con.Table<MainTable>().Count();
        }
    }
}
