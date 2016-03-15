using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;
using System.IO;
using System.Collections;

namespace AndroidLab3
{
    [Activity(Label = "AndroidLab3", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        SQLite.SQLiteConnection conn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            ValidateConnection();
            conn.CreateTable<Person>(CreateFlags.AutoIncPK);
            UpdateView();
            
            //5conn.CreateTable(typeof(Person));
            Button button = FindViewById<Button>(Resource.Id.btn_add);

            button.Click += delegate {
                Person p = new Person();
                p.FirstName = FindViewById<EditText>(Resource.Id.txt_name).Text;
                p.LastName = FindViewById<EditText>(Resource.Id.txt_surname).Text;
                if (p.FirstName != "" && p.LastName != "")
                {
                    conn.Insert(p);
                }
                UpdateView();
            };

        }
        private void ValidateConnection()
        {
            var sqliteFilename = "TodoSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            conn = new SQLite.SQLiteConnection(path);
           // conn.DeleteAll<Person>();
        }
        private void UpdateView()
        {
            Person[] allPeople = conn.Query<Person>("select * from Person").ToArray();
            TextView output = FindViewById<TextView>(Resource.Id.txt_people);
            String outputStr = "";
            for (int i = 0; i < allPeople.Length; i++)
            {
                outputStr += "ID = " + allPeople[i].ID + "\n";
                outputStr += "Name = " + allPeople[i].FirstName + "\n";
                outputStr += "LastName = " + allPeople[i].LastName + "\n";
                outputStr += "*************\n";
            }
            output.Text = outputStr;
        }
    }
}

