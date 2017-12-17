using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCardScanner.Cognitive.Entities;
using BusinessCardScanner.Services.Interfaces;
using SQLite;
using Xamarin.Forms;

namespace BusinessCardScanner
{
    public class UserDataWrapper : IUserDataWrapper
    {
        private readonly SQLiteConnection _connection;

        public UserDataWrapper()
        {
            _connection?.Close();

            _connection = new SQLiteConnection(DependencyService.Get<IDeviceInfoService>().CreateCommonDatabase());

            //Turning on WAL mode is important if you're going to be
            //doing any concurrent access to the database.
            _connection?.Query<object>("PRAGMA journal_mode=WAL");

            //These pragma statements are very important to ensure
            //that Zumero can track changes correctly.
            _connection?.Execute("PRAGMA foreign_keys=ON");
            _connection?.Execute("PRAGMA recursive_triggers=ON");

            // In a normal SQLite-Net app, you would call CreateTable() here.
            // Do not do that!! Zumero will be in charge of creating tables
            // and updating schemas.

            _connection.CreateTable<ContactCard>();

        }

        private void StartReadTransaction() => _connection?.Execute("BEGIN TRANSACTION");

        private void StartWriteTransaction()
        {
            //This method handles cases where a Zumero Sync may be going on in a
            //background thread.  It will block until the database is unlocked after
            //the sync completes. The other benefit is that starting a write
            //transaction will cause any future Sync calls to block until
            //the write transaction is complete.
            var txBegun = false;

            while (!txBegun)
            {
                try
                {
                    if (_connection.IsInTransaction)
                    {
                        continue;
                    }

                    _connection?.Execute("BEGIN IMMEDIATE TRANSACTION");
                    txBegun = true;
                }
                catch (SQLiteException e)
                {
                    if (e.Result == SQLite3.Result.Busy || e.Result == SQLite3.Result.Locked)
                        //This is a portable way of doing Thread.Sleep().
                        Task.Delay(TimeSpan.FromMilliseconds(100))?.Wait();
                    else
                        throw;
                }
            }
        }

        private void Commit() => _connection?.Execute("COMMIT");

        private void Rollback() => _connection?.Execute("ROLLBACK");

        /// <summary>
        /// To delete an object from table
        /// </summary>
        /// <param name="item"></param>
        public void Delete(object item)
        {
            StartWriteTransaction();
            try
            {
                _connection?.Delete(item);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Loads the list of all records from the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> LoadAll<T>() where T : new()
        {
            StartReadTransaction();
            try
            {
                var results = _connection?.Table<T>()?.ToList();
                Commit();
                return results;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="item"></param>
        public void Update(object item)
        {
            StartWriteTransaction();
            try
            {
                _connection?.Update(item);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Inserts a row into the table
        /// </summary>
        /// <param name="item"></param>
        public void Insert(object item)
        {
            StartWriteTransaction();
            try
            {
                _connection?.Insert(item);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }
    }
}