﻿using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace Robot
{
    /// <summary>
    /// Interaction logic for AddNewCommand.xaml
    /// </summary>
    public partial class AddNewCommandWindow : Window
    {
        HContext db = new HContext();
        ListCommand objToAdd;

        public AddNewCommandWindow()
        {          
            InitializeComponent();
        }

        private void saveNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ss = db.ListCommand
                      .AsEnumerable()
                      .Where(a => a.command == objToAdd.command &&
                       a.helpPrint == objToAdd.helpPrint &&
                       a.monitorPrint == objToAdd.monitorPrint &&
                       a.scenario == objToAdd.scenario)
                      .ToList();

                if (objToAdd.id == 0)
                {
                    db.ListCommand.Add(new ListCommand
                    {
                        command = objToAdd.command,
                        monitorPrint = objToAdd.monitorPrint,
                        helpPrint = objToAdd.helpPrint,
                        scenario = objToAdd.scenario
                    });
                }

                db.SaveChanges();
                saveNewHelpCommandBtn.IsEnabled = false;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка данные могут не сохранится");
            }          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             listHelpDg.ItemsSource = getDataFromListCommand();
            //db.ListCommand.Load();
            //this.DataContext = db.ListCommand.Local.ToBindingList();
            // SizeToContent = SizeToContent.Width;
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
        }

        private void listHelpDg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show("ggggg!");
        }

        private void rejectNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            if (objToAdd == null) return;

            try
            {
                db.ListCommand.Remove(objToAdd);
                db.SaveChanges();
            }
            catch
            { }
           
        }            

        private void listHelpDg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
          
        }

        private void listHelpDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            objToAdd = listHelpDg.SelectedItem as ListCommand;
            saveNewHelpCommandBtn.IsEnabled = true;
        }

        public List<ListCommand> getDataFromListCommand()
        {
           return db.ListCommand.AsEnumerable().ToList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            db.Dispose();
        }
    }
    
}
