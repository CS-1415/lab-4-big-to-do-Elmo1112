using System;
using System.Collections.Generic;

enum CompletionStatus
{
    NotDone,
    Done
}

class Task
{
    private string _title;
    private CompletionStatus _status;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public CompletionStatus Status
    {
        get { return _status; }
    }

    public Task(string title)
    {
        _title = title;
        _status = CompletionStatus.NotDone;
    }

    public void ToggleStatus()
    {
        if (_status == CompletionStatus.NotDone)
            _status = CompletionStatus.Done;
        else
            _status = CompletionStatus.NotDone;
    }
}

class TodoList
{
    private List<Task> _tasks = new List<Task>();
    private int _selected = 0;

    public int Length
    {
        get { return _tasks.Count; }
    }

    public Task CurrentTask
    {
        get
        {
            if (_tasks.Count == 0) return null;
            return _tasks[_selected];
        }
    }

    public Task GetTask(int index)
    {
        return _tasks[index];
    }

    private int WrappedIndex(int index)
    {
        if (_tasks.Count == 0) return 0;

        if (index < 0)
            return _tasks.Count - 1;

        if (index >= _tasks.Count)
            return 0;

        return index;
    }

    public void SelectPrevious()
    {
        _selected = WrappedIndex(_selected - 1);
    }

    public void SelectNext()
    {
        _selected = WrappedIndex(_selected + 1);
    }

    public void SwapWithPrevious()
    {
        if (_tasks.Count < 2) return;

        int prev = WrappedIndex(_selected - 1);
        Task temp = _tasks[_selected];
        _tasks[_selected] = _tasks[prev];
        _tasks[prev] = temp;

        _selected = prev;
    }

    public void SwapWithNext()
    {
        if (_tasks.Count < 2) return;

        int next = WrappedIndex(_selected + 1);
        Task temp = _tasks[_selected];
        _tasks[_selected] = _tasks[next];
        _tasks[next] = temp;

        _selected = next;
    }

    public void Insert(string title)
    {
        if (_tasks.Count == 0)
        {
            _tasks.Add(new Task(title));
            _selected = 0;
        }
        else
        {
            _tasks.Insert(_selected + 1, new Task(title));
            _selected++;
        }
    }

    public void DeleteSelected()
    {
        if (_tasks.Count == 0) return;

        _tasks.RemoveAt(_selected);

        if (_selected >= _tasks.Count)
            _selected = _tasks.Count - 1;

        if (_selected < 0)
            _selected = 0;
    }
}

class TodoListApp
{
    private TodoList _tasks;
    private bool _showHelp = true;
    private bool _insertMode = true;
    private bool _quit = false;

    public TodoListApp(TodoList tasks)
    {
        _tasks = tasks;
    }

    public void Run()
    {
        while (!_quit)
        {
            Console.Clear();
            Display();
            ProcessUserInput();
        }
    }

    public void Display()
    {
        DisplayTasks();
        if (_showHelp)
        {
            DisplayHelp();
        }
    }

    public void DisplayBar()
    {
        Console.WriteLine("-----------------------------------");
    }

    public string MakeRow(int i)
    {
        Task task = _tasks.GetTask(i);
        string arrow = "  ";
        if (task == _tasks.CurrentTask) arrow = "->";

        string check = " ";
        if (task.Status == CompletionStatus.Done) check = "X";

        return $"{arrow} [{check}] {task.Title}";
    }

    public void DisplayTasks()
    {
        DisplayBar();
        Console.WriteLine("Tasks");
        for (int i = 0; i < _tasks.Length; i++)
        {
            Console.WriteLine(MakeRow(i));
        }
        DisplayBar();
    }

    public void DisplayHelp()
    {
        Console.WriteLine(
@"Instructions:
   h: show hide instructions
   arrow up or down: select previous or next task
   arrow right or left: reorder task
   space: toggle completion
   e: edit title
   i: insert new tasks
   delete backspace: delete task
   esc: quit");
        DisplayBar();
    }

    private string GetTitle()
    {
        Console.WriteLine("enter task title or press enter to stop inserting: ");
        return Console.ReadLine();
    }

    public void ProcessUserInput()
    {
        if (_insertMode)
        {
            string taskTitle = GetTitle();
            if (taskTitle.Length == 0)
            {
                _insertMode = false;
            }
            else
            {
                _tasks.Insert(taskTitle);
            }
        }
        else
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    _quit = true;
                    break;
                case ConsoleKey.UpArrow:
                    _tasks.SelectPrevious();
                    break;
                case ConsoleKey.DownArrow:
                    _tasks.SelectNext();
                    break;
                case ConsoleKey.LeftArrow:
                    _tasks.SwapWithPrevious();
                    break;
                case ConsoleKey.RightArrow:
                    _tasks.SwapWithNext();
                    break;
                case ConsoleKey.I:
                    _insertMode = true;
                    break;
                case ConsoleKey.E:
                    if (_tasks.CurrentTask != null)
                        _tasks.CurrentTask.Title = GetTitle();
                    break;
                case ConsoleKey.H:
                    _showHelp = !_showHelp;
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    if (_tasks.CurrentTask != null)
                        _tasks.CurrentTask.ToggleStatus();
                    break;
                case ConsoleKey.Delete:
                case ConsoleKey.Backspace:
                    _tasks.DeleteSelected();
                    break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        new TodoListApp(new TodoList()).Run();
    }
}
