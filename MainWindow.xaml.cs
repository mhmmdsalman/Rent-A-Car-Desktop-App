using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Xml.Linq;
using System.Net;
using System.Net.Mail;

namespace Wpf_Rent_A_Car
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Customer> customers;
        ObservableCollection<Search> cars;

        string filter = "";
        private bool storeData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customers = CustomerData.ReadXml<ObservableCollection<Customer>>("CustomersInfo.xml");

            if (customers == null)
            {
                customers = new ObservableCollection<Customer>();
                customers = GenerateCustomers(20);
            }

            Lbx_customers.ItemsSource = customers;

            cars = CustomerData.ReadXml<ObservableCollection<Search>>("SearchInfo.xml");

            if (cars == null)
            {
                cars = new ObservableCollection<Search>();
                cars = CarsList(20);
            }

            Lbx_cars.ItemsSource = cars;
        }

        private ObservableCollection<Search> CarsList(int cnt)
        {
            var lst1 = new ObservableCollection<Search>();
            for (int j = 0; j < cnt; j++)
            {
                lst1.Add(new Search
                {
                    CarNumber = "CarNumber" + j,
                    CarName = "CarName" + j,
                    Rate = "Rate" + j,
                    CarType = "CarType" + j,
                    Location = "Location" + j,
                    PickupDate = "PickupDate" + j,
                    DropoffDate = "DropoffDate" + j
                });
            }
            return lst1;
        }

        private ObservableCollection<Customer> GenerateCustomers(int cnt)
        {
            var lst = new ObservableCollection<Customer>();

            for (int i = 0; i < cnt; i++)
            {
                Customer cust = new Customer();
                lst.Add(cust);
            }

            return lst;
        }

        private void Btn_create_new_customer_Click(object sender, RoutedEventArgs e)
        {
            var cust = new Customer { customerName = "Please edit", emailId = "Please edit" };
            customers.Add(cust);
            Lbx_customers.SelectedItem = cust;
            Lbx_customers.ScrollIntoView(cust);
        }

        private void Tbx_filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter = Tbx_filter.Text.ToLower();

            if (filter == "")
            {
                Lbx_customers.ItemsSource = customers;
            }

            else
            {
                var results = from c in customers where c.customerName.ToLower().Contains(filter) select c;
                Lbx_customers.ItemsSource = results;
            }
        }

        private void Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            storeData = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (storeData)
                CustomerData.WriteXml<ObservableCollection<Customer>>(customers, "CustomersInfo.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            {
                if (Cbx_cartype.SelectionBoxItem.Equals("Hatchback") && Cbx_location.SelectionBoxItem.Equals("Hauptbahnhof"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "Hatchback" && n.Location == "Hauptbahnhof") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else if (Cbx_cartype.SelectionBoxItem.Equals("Hatchback") && Cbx_location.SelectionBoxItem.Equals("Wieblingen"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "Hatchback" && n.Location == "Wieblingen") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else if (Cbx_cartype.SelectionBoxItem.Equals("Sedan") && Cbx_location.SelectionBoxItem.Equals("Hauptbahnhof"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "Sedan" && n.Location == "Hauptbahnhof") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else if (Cbx_cartype.SelectionBoxItem.Equals("Sedan") && Cbx_location.SelectionBoxItem.Equals("Wieblingen"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "Sedan" && n.Location == "Wieblingen") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else if (Cbx_cartype.SelectionBoxItem.Equals("SUV") && Cbx_location.SelectionBoxItem.Equals("Hauptbahnhof"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "SUV" && n.Location == "Hauptbahnhof") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else if (Cbx_cartype.SelectionBoxItem.Equals("SUV") && Cbx_location.SelectionBoxItem.Equals("Wieblingen"))
                {
                    var lstInput1 = CustomerData.ReadXml<List<Search>>("SearchInfo.xml");
                    var results = (from n in lstInput1 where (n.CarType == "SUV" && n.Location == "Wieblingen") select n);
                    Lbx_cars.ItemsSource = results;
                }

                else
                {
                    MessageBox.Show("Please select all fields");
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedCar = Lbx_cars.SelectedItem as Search;
            dataGrid2.Items.Add(selectedCar);
            Lbx_cars.ItemsSource = cars;
            var fromAddress = new MailAddress("mhmmdsalman91@gmail.com", "Rental Cars Agency Heidelberg");
            var toAddress = new MailAddress("customerheidelberg@gmail.com", "To Name");
            const string fromPassword = "";
            const string subject = "Rental Car Booked Confirmation";
            const string body = "Hello Customer, Your Rental Car has been booked successfully.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            MessageBox.Show("Rental Car Booked Successfully");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                Search item = (Search)dataGrid2.SelectedItem;
                item.CarNumber = Tbx_selectedcar.Text;

                (dataGrid2.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text = Dt_selectedpickup.SelectedDate.ToString();
                (dataGrid2.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text = Dt_selecteddropoff.SelectedDate.ToString();

                var fromAddress = new MailAddress("mhmmdsalman91@gmail.com", "Rental Cars Agency Heidelberg");
                var toAddress = new MailAddress("customerheidelberg@gmail.com", "To Name");
                const string fromPassword = "";
                const string subject = "Rental Car Update Booking Confirmation";
                const string body = "Hello Customer, Your Rental Car has been updated and Re-booked successfully.";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                MessageBox.Show("Rental Car Updated Successfully");

            }
            else
            {
                MessageBox.Show("Please Select the Car to update");
            }

        }

        private void DataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var c = dataGrid2.SelectedItem;
            {
                string CarNumber = (dataGrid2.SelectedCells[0].Column.GetCellContent(c) as TextBlock).Text;
                Tbx_selectedcar.Text = CarNumber;
                string PickupDate = (dataGrid2.SelectedCells[1].Column.GetCellContent(c) as TextBlock).Text;
                Dt_selectedpickup.Text = PickupDate;
                string DropoffDate = (dataGrid2.SelectedCells[2].Column.GetCellContent(c) as TextBlock).Text;
                Dt_selecteddropoff.Text = DropoffDate;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem == null)
            {
                MessageBox.Show("Please select the car to remove");
                return;
            }
            else
            {
                var d = dataGrid2.SelectedItem;
                cars.Add(d as Search);
                object item = dataGrid2.SelectedItem;
                (dataGrid2.SelectedCells[4].Column.GetCellContent(item) as TextBlock).Text = "Cancelled";

                var fromAddress = new MailAddress("mhmmdsalman91@gmail.com", "Rental Cars Agency Heidelberg");
                var toAddress = new MailAddress("customerheidelberg@gmail.com", "To Name");
                const string fromPassword = "";
                const string subject = "Rental Car Cancellation Confirmation";
                const string body = "Hello Customer, Your Rental Car has been cancelled successfully. We look forward to see you booking a rental car with us.";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })

                {
                    smtp.Send(message);
                }

                MessageBox.Show("Rental Car Cancelled Successfully");
            }
        }        
    }
}

