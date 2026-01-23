# Lab-4-Big-To-Do

Todo-List: User-Defined Datatypes 
General Learning Objectives

Use a modern operating system and utilities.

Use an integrated development environment to develop a program.

Solve problems and develop programs using the control structures of sequence, selection, and repetition, following a disciplined approach.

Objectives:

Practice defining your own data types.
Create a nifty, useful app (that you can expand later).
Step 1: Get Big Picture Requirements

This is a simplified version of the extended lab you started last time. You will implement a todo-list program to keep track of a list of tasks.

Here is an example of the program running.

-----------------------------------
Tasks:
   [X] Get car fixed
-> [ ] Buy groceries
   [ ] Paint house
-----------------------------------
Instructions:
   h: show/hide instructions
   ↕: select previous or next task (wrapping around at the top and bottom)
   ↔: reorder task (swap selected task with previous or next task)
   space: toggle completion of selected task
   e: edit title
   i: insert new tasks
   delete/backspace: delete task
-----------------------------------
Step 2: Consider the classes

You will create the final solution via the following user-defined data types:

enum CompletionStatus: NotDone, Finished
class Task: responsible for knowing the name of a task its completion status; it is also responsible for toggling its own completion status when asked to
class TodoList: responsible for knowing all tasks and which is selected; it is also responsible for reporting the number of tasks it has, retrieving a task from by index, swapping the selected task with the previous or next task, selecting the previous or next task, inserting a new task after the selected position, or deleting the task at the selected position when requested to.
class TodoListApp: responsible for knowing the todolist, whether or not the user is in insert mode, and whether or not the help should be displayed; also responsible for repeatedly displaying it and updating it in response to user input.
Class diagram:




Step 3: Setup Your Repo
Accept the [GitHub Classroom Assignment]().
Open a terminal.
Navigate to your code directory for the course (e.g.: cd my-cs1415-dir).
Run git clone {yourUrl} to make a local copy of your repository.
Run code {nameOfYourRepo} to open that directory in VS Code.
Open the integrated terminal in VS Code (Ctrl+`, or the Terminal menu -> New Terminal).
Run dotnet new console to create a new project in the current folder.
Run dotnet build to compile your code.
Run dotnet run to execute your code.
Step 4: Read the App class and Main Driver

You main use the following UI code. Familiarize yourself with it and notice what classes and methods you will need to make to get it to work. (Can you spot the Properties?)

  class TodoListApp {
    private TodoList _tasks;
    private bool _showHelp = true;
    private bool _insertMode = true;
    private bool _quit = false;

    public TodoListApp(TodoList tasks) {
        _tasks = tasks;
    }

    public void Run() {
        while (!_quit) {
            Console.Clear();
            Display();
            ProcessUserInput();
        }
    }

    public void Display() {
        DisplayTasks();
        if (_showHelp) {
            DisplayHelp();
        }
    }

    public void DisplayBar() {
        Console.WriteLine("----------------------------");
    }

    public string MakeRow(int i) {
        Task task = _tasks.GetTask(i);
        string arrow = "  ";
        if (task == _tasks.CurrentTask) arrow = "->";
        string check = " ";
        if (task.Status == CompletionStatus.Done) check = "X";
        return $"{arrow} [{check}] {task.Title}";
    }

    public void DisplayTasks() {
        DisplayBar();
        Console.WriteLine("Tasks:");
        for (int i = 0; i < _tasks.Length; i++) {
            Console.WriteLine(MakeRow(i));
        }
        DisplayBar();
    }

    public void DisplayHelp() {
        Console.WriteLine(
@"Instructions:
   h: show/hide instructions
   ↕: select previous or next task (wrapping around at the top and bottom)
   ↔: reorder task (swap selected task with previous or next task)
   space: toggle completion of selected task
   e: edit title
   i: insert new tasks
   delete/backspace: delete task");
        DisplayBar();
    }

    private string GetTitle() {
        Console.WriteLine("Please enter task title (or [enter] for none): ");
        return Console.ReadLine()!;
    }

    public void ProcessUserInput() {
        if (_insertMode) {
            string taskTitle = GetTitle();
            if (taskTitle.Length == 0) {
                _insertMode = false;
            } else {
                _tasks.Insert(taskTitle);
            }
        } else {
            switch (Console.ReadKey(true).Key) {
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
                    _tasks.CurrentTask.Title = GetTitle();
                    break;
                case ConsoleKey.H:
                    _showHelp = !_showHelp;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    _tasks.CurrentTask.ToggleStatus();
                    break;
                case ConsoleKey.Delete:
                case ConsoleKey.Backspace:
                    _tasks.DeleteSelected();
                    break;
                default:
                    break;
            }
        }
    }
  }

  class Program {
    static void Main() {
        new TodoListApp(new TodoList()).Run();
    }
  }

Step 5: Complete the features

Comment out as much code as necessary to get the program to compile. Then incrementally uncomment lines of the UI and create the necessary supporting types. Each time you succeed in getting more functionality that compiles, commit a checkpoint to your git repository.  




Do not forget to test each section of the code to make sure it is functioning as you desire. 

Submission

Commit your working code along with diagrams you used to design the classes.  You may submit a PDF through Canvas or attach it to the repo you upload to me whichever is easier for you.

Grading

Grading must be done in person or via Teams with a TA or instructor.


