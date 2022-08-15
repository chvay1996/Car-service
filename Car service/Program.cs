using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        static void Main ( string [] args )
        {
            CarService carService = new CarService ();
            carService.OpenMenu ();
        }
    }

    class CarService
    {
        private Queue<Driver> _car = new Queue<Driver> ();
        private Storage _details = new Storage ();
        private int _money;

        public CarService ()
        {
            _money = 1000;
            CreateCars ( RandomNumberCar() );
        }

        public void OpenMenu ()
        {
            bool isWork = true;
            string userInput;

            while ( ShowConditions ( isWork ) )
            {
                _details.ShowStorage ();
                ShowMoney ();
                Console.WriteLine ( $"В автосервисе {_car.Count} машин стоят на ремонт. " +
                    $"\n1 - починить машины " +
                    $"\n2 - закрыть автосервис" );
                Console.Write ( "Введите число:\t" );
                userInput = Console.ReadLine ();

                switch ( userInput )
                {
                    case "1":
                        ServiceCar ();
                        break;
                    case "2":
                        isWork = false;
                        break;
                    default:
                        Console.WriteLine ( "Не верный ввод" );
                        break;
                }
            }
        }

        private int RandomNumberCar ()
        {
            int minNumberCar = 4;
            int maxNumberCar = 7;
            Random random = new Random ();
            int rand = random.Next ( minNumberCar, maxNumberCar );
            return rand;
        }

        private bool ShowConditions ( bool isWork )
        {
            if ( isWork == true && _car.Count > 0 )
            {
                return true;
            }
            else
            {
                Console.WriteLine ( $"Вы обслужищи всех клиентов. \nВаш баланс: {_money} монет." );
                return false;
            }
        }

        private void CreateCars ( int numberOfDrivers )
        {
            for ( int i = 0; i < numberOfDrivers; i++ )
            {
                _car.Enqueue ( new Driver () );
            }
        }

        private void ShowCar ( Driver driver )
        {
            Console.WriteLine ( $"В машине сломалась {driver.BrokenPart.Name}" );
            Console.WriteLine ( $"Цена за работу - {_details.GetTotalPrice ( driver )} монет." );
        }

        private void ServiceCar ()
        {
            string userInput;
            Console.Clear ();
            var car = _car.Dequeue ();
            ShowCar ( car );
            Console.WriteLine ( "Что выберете: \n1 - починить авто. \n2 - отказать." );
            Console.Write ( "Введите число:\t" );
            userInput = Console.ReadLine ();

            switch ( userInput )
            {
                case "1":
                    RepairCar ( car );
                    break;
                case "2":
                    RefuseDriver ();
                    break;
                default:
                    Console.WriteLine ( "Не верный ввод" );
                    break;
            }
            Console.ReadKey ();
            Console.Clear ();
        }

        private int ShowMoney ()
        {
            if ( _money >= 0 )
            {
                Console.WriteLine ( $"\nБаланс автосервиса  - {_money} монет.\n" );

                return _money;
            }
            else
            {
                Console.WriteLine ( $"\nВы ушли в минус, ваш автосервис обонкротился!\n" );
                return _money;
            }
        }

        private void RepairCar ( Driver driver )
        {
            int totalMoney = _details.GetTotalPrice ( driver );

            if ( _details.TryRepairCar ( driver ) )
            {
                Console.WriteLine ( $"Вы успешно починили автомобиль и заработали {totalMoney} монет" );
                _money += totalMoney;
            }
            else
            {
                Console.WriteLine ( $"Клиент разочарован!!! Вы установили нету деталь и вам придётся выплатить полную компенсацию в размере {totalMoney}" );
                _money -= totalMoney;
            }
        }

        private void RefuseDriver ()
        {
            int compensation = 500;
            Console.WriteLine ( $"Вы отказали клиенту и за это вам нужно выплатить компенсацию в размере {compensation} монет" );
            _money -= compensation;
        }
    }

    class Driver
    {
        private Random _random = new Random ();
        private List<Detail> _details = new List<Detail> ();
        public Detail BrokenPart { get; private set; }

        public Driver ()
        {
            _details.Add ( new Detail ( "Обдув салона", 700 ) );
            _details.Add ( new Detail ( "Лобач", 2800 ) );
            _details.Add ( new Detail ( "Глушитель", 500 ) );
            _details.Add ( new Detail ( "Термостав", 150 ) );
            _details.Add ( new Detail ( "Стартер", 300 ) );
            _details.Add ( new Detail ( "Пусковое реле", 600 ) );
            _details.Add ( new Detail ( "Патрубки", 800 ) );
            _details.Add ( new Detail ( "Фара", 400 ) );
            _details.Add ( new Detail ( "Двигатель", 4000 ) );
            _details.Add ( new Detail ( "Бензонанос", 1500 ) );
            CreateBrokenPart ();
        }

        private Detail CreateBrokenPart ()
        {
            BrokenPart = _details [ _random.Next ( 0, _details.Count - 1 ) ];
            return BrokenPart;
        }
    }

    class Storage
    {
        private List<Detail> _storage = new List<Detail> ();
        private List<Detail> _details = new List<Detail> ();
        private Random _random = new Random ();

        public Storage ()
        {
            _details.Add ( new Detail ( "Обдув салона", 700 ) );
            _details.Add ( new Detail ( "Лобач", 2800 ) );
            _details.Add ( new Detail ( "Глушитель", 500 ) );
            _details.Add ( new Detail ( "Термостав", 150 ) );
            _details.Add ( new Detail ( "Стартер", 300 ) );
            _details.Add ( new Detail ( "Пусковое реле", 600 ) );
            _details.Add ( new Detail ( "Патрубки", 800 ) );
            _details.Add ( new Detail ( "Фара", 400 ) );
            _details.Add ( new Detail ( "Двигатель", 4000 ) );
            _details.Add ( new Detail ( "Бензонанос", 1500 ) );
            CreateDetail ();
        }

        public void ShowStorage ()
        {
            Console.WriteLine ( "На складе есть такие детали:" );

            for ( int i = 0; i < _storage.Count; i++ )
            {
                Console.WriteLine ( $"{i + 1}. Название - {_storage [ i ].Name}, цена - {_storage [ i ].Price}." );
            }
        }

        public bool TryRepairCar ( Driver driver )
        {
            bool isNumber;
            ShowStorage ();
            Console.Write ( "\nКакую деталь вы хотитель установить: " );
            isNumber = int.TryParse ( Console.ReadLine (), out int inputNumberDetail );

            if ( isNumber == false )
            {
                Console.WriteLine ( "Ошибка! Не верный ввод." );
                return false;
            }
            else if ( inputNumberDetail > 0 && inputNumberDetail - 1 < _storage.Count && driver.BrokenPart.Name == _storage [ inputNumberDetail - 1 ].Name )
            {
                int indexDetail = inputNumberDetail - 1;
                _storage.RemoveAt ( indexDetail );
                return true;
            }
            else
            {
                Console.WriteLine ( "Кажется это не та деталь." );
                return false;
            }
        }

        public int GetTotalPrice ( Driver driver )
        {
            int totalPrice = 0;
            int pricePerJob = 500;

            foreach ( var detail in _details )
            {
                if ( driver.BrokenPart.Name == detail.Name )
                {
                    totalPrice += detail.Price + pricePerJob;
                    break;
                }
            }

            return totalPrice;
        }

        private void CreateDetail ()
        {
            int maximumDetails;
            int minimumDetails;
            int numderOfDetail;
            maximumDetails = 20;
            minimumDetails = 10;
            numderOfDetail = _random.Next ( minimumDetails, maximumDetails );

            for ( int i = 0; i < numderOfDetail; i++ )
            {
                _storage.Add ( _details [ _random.Next ( 0, _details.Count - 1 ) ] );
            }
        }
    }

    class Detail
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Detail ( string name, int price = 0 )
        {
            Name = name;
            Price = price;
        }
    }
}
/*Задача:
У вас есть автосервис в который приезжают люди чтобы починить свои автомобили.
У вашего автосервиса есть баланс денег и склад деталей.
Когда приезжает автомобиль у него сразу ясна его поломка и эта поломка отображается у вас в консоли вместе с ценой за починку(цена за починку складывается из цены детали + цена за работу).
Поломка всегда чинится заменой детали, но количество деталей ограничено тем, что находится на вашем складе деталей.
Если у вас нет нужной детали на складе, то вы можете отказать клиенту и в этом случае вам придется выплатить штраф.
Если вы замените не ту деталь, то вам придется возместить ущерб клиенту.
За каждую удачную починку вы получаете выплату за ремонт, которая указана в чек-листе починки.*/