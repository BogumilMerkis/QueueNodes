using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Queue
{
    interface IQueue<T>
    {
        bool IsFull();
        bool IsEmpty();
        int Size();
        void Add(T item);
        T Remove();
        T Peek();
    }
    class Stack<T>
    {
        private int _sp;
        private T[] _list;
        public Stack()
        {
            _sp = -1;
            _list = new T[10];
        }
        public T Pop() { return _list[_sp--]; }
        public void Push(T item) { _list[++_sp] = item; }
        public T Peek() { return _list[_sp]; }
    }

    class Queue<T>
    {
        public const int MAX_SIZE = 5;
        private int _rear;
        protected T[] _list = new T[MAX_SIZE];

        public Queue()
        {
            _list = new T[5];
            _rear = 0;
        }
        public int Size { get { return _rear; } }
        public bool IsEmpty { get { return Size == 0; } }
        public bool IsFull { get { return _rear == MAX_SIZE - 1; } }
        public T Remove()
        {
            T item;
            if (IsEmpty)
            {
                throw new Exception();
            }
            else
            {
                item = _list[0];
                _rear--;
                for (int i = 1; i <= _rear; i++)
                {
                    _list[i - 1] = _list[i];
                }
            }
            return item;
        }
        public void Add(T item) { _list[_rear++] = item; }
        public T Peek()
        {

            if (IsEmpty)
                throw new Exception();
            else
                return _list[0];

        }
    }

    class CircularQueue<T>
    {
        public const int MAX_SIZE = 5;
        private int _rear, _front, _size;
        protected T[] _list = new T[MAX_SIZE];

        public CircularQueue() : base()
        {
            _list = new T[MAX_SIZE];
            _rear = 0;
            _front = 0;
            _size = 0;
        }
        public int Size { get { return _rear; } }
        public bool IsEmpty { get { return Size == 0; } }
        public bool IsFull { get { return _rear == MAX_SIZE - 1; } }
        public T Remove()
        {
            if (IsEmpty)
            {
                throw new Exception();
            }
            else
            {
                T temp = _list[_front++ % _list.Length];
                _size--;
                return temp;
            }
        }
        public void Add(T item)
        {
            if (IsFull)
            {
                throw new Exception();
            }
            else
            {
                _list[_rear++ % _list.Length] = item;
                _size++;
            }
        }
        public T Peek()
        {
            if (IsEmpty)
                throw new Exception();
            else
                return _list[0];

        }
    }

    class DynamicQueue<T> : IQueue<T>
    {
        protected class QNode
        {
            public T _data;
            public QNode _pointer;
            public int _priority;
            public QNode(T data, int priority = 0)
            {
                _data = data;
                _pointer = null;
            }
        }
        protected QNode _front, _rear;
        protected int _size;

        public DynamicQueue() : base()
        {
            _size = 0;
            _front = null;
            _rear = null;
        }
        public int Size() { return _size; }
        public bool IsFull() { return false; }
        public bool IsEmpty() { return _front == null; }
        public virtual void Add(T inItem)
        {
            if (_size == 0)
            {
                _rear = new QNode(inItem);
                _front = _rear;
            }
            else
            {
                _rear._pointer = new QNode(inItem);
                _rear = _rear._pointer;
            }
            _size++;
        }
        public T Remove()
        {
            T item;
            if (IsEmpty())
            {
                throw new Exception();
            }
            else
            {
                _size--;
                item = _front._data;
                _front = _front._pointer;
                return item;
            }

        }
        public T Peek()
        {
            T item;
            if (IsEmpty())
            {
                throw new Exception();
            }
            else
            {
                item = _front._data;
                return item;
            }

        }
    }

    class PriorityQueue<T> : DynamicQueue<T>
    {
        private new class QNode : DynamicQueue<T>.QNode
        {
            public QNode(T Data, int inPriority) : base(Data)
            {
                _priority = inPriority;
            }
        }

        private DynamicQueue<T>.QNode _previous, _current;
        public bool _inserted;
        public PriorityQueue() : base()
        {
            _inserted = false;
            _previous = null;
            _current = null;
        }

        public void Add(T inItem, int inPriority)
        {
            var NewNode = new QNode(inItem, inPriority);
            NewNode._data = inItem;
            NewNode._priority = inPriority;

            if (_front == null)
            {
                _front = NewNode;
                _rear = NewNode;
                NewNode._pointer = null;
            }
            else if (_front._priority < NewNode._priority)
            {
                NewNode._pointer = _front;
                _front = NewNode;
            }
            else
            {
                _previous = _front;
                _current = _front._pointer;

                while (!_inserted)
                {
                    if (_current == null)
                    {
                        _previous._pointer = NewNode;
                        _rear = NewNode;
                        _inserted = true;
                    }
                    else if (_current._priority < NewNode._priority)
                    {
                        _previous._pointer = NewNode;
                        NewNode._pointer = _current;
                        _inserted = true;
                    }
                    else
                    {
                        _previous = _current;
                        _current = _current._pointer;
                    }
                }
            }

            _size++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PriorityQueue<string> fifo = new PriorityQueue<string>();
            string name;
            int priority;
            int choice = -1;

            Console.WriteLine("Queue Test Program");
            do
            { 
                Console.WriteLine("\n1. Add an item");
                Console.WriteLine("2. Remove an item");
                Console.WriteLine("3. Peek ahead");
                Console.WriteLine("0. Quit");
                Console.Write("Enter menu option: ");
                try { choice = int.Parse(Console.ReadLine()); }
                catch { choice = -1; }
                switch (choice)
                {
                    case 1:
                        if (fifo.IsFull())
                            Console.WriteLine("Queue is full.");
                        else
                        {
                            Console.Write("Enter name: ");
                            name = Console.ReadLine();
                            Console.Write("Enter priority: ");
                            priority = int.Parse(Console.ReadLine());
                            ((PriorityQueue<string>)fifo).Add(name, priority);
                            Console.WriteLine("{0} added.\nSize = {1}.", name, fifo.Size());
                        }
                        break;
                    case 2:
                        if (fifo.IsEmpty())
                            Console.WriteLine("Queue is empty.");
                        else
                        {
                            name = fifo.Remove();
                            Console.WriteLine("{0} removed.\nSize = {1}.", name, fifo.Size());
                        }
                        break;
                    case 3:
                        if (fifo.IsEmpty())
                            Console.WriteLine("Queue is empty.");
                        else
                        {
                            name = fifo.Peek();
                            Console.WriteLine("{0} is next item.\nSize = {1}.", name, fifo.Size());
                        }
                        break;
                }
            } while (choice != 0);
        }
    }
}



