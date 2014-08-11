cmdValidator
===========

An validator for commandline arguments in .NET programmes.

Installation and usage
-----
1. First you need to compile the cmdValidator project folder. It will result in an Assembly (.dll file) in your debug or release folder.

2. Now you need to reference this assembly to your current project folder.

3. We'll continue in the code:

    ```cs
        //...
        //add the namespace
        using cmdValidator;
        
        namespace DebugProgramme
        {
            class Program
            {
                static void Main(string[] args)
                {
                    //create a new validator
                    //the ignoreUnknownParameters set to false will prevent,
                    //that events will be raised when there are unknown parameters
                    Validator validator = new Validator(false);
                    
                    //add an argument set
                    validator.AddArgumentSet("hello", hello);
                    
                    //add another argument set
                    validator.AddArgumentSet("goodbye", goodBye);
        
                    //pass the commandline args
                    validator.CheckArgs(args);
                }
        
                static void hello(ArgumentSetArgs args)
                {
                    Console.WriteLine("Hello world!");
                }
                
                static void goodbye(ArgumentSetArgs args)
                {
                     Console.WriteLine("Goodbye world!");
                }
            }
        }
    
    ```

4. If we would know pass the argument *hello* to the programme, e.g. via the console, the validator would trigger the *hello event*. That's all. It's easy, isn't it?

Advanced tutorial
=================

In this advanced tutorial you will learn advanced possibilties of the cmdValidator. Follow the steps:

1. Create an instance of the cmdValidator.

2. Add an argument set. Argument sets are objects that describe a command with every possible argument and option. In our example we want an argument set that describes following fantasy command:
    - command name: **install**
    - required option: **name (with single value appended)** (the name of the programme whhich should be installed)
    - required option: **destination (with single value appended)** (the place where we want to install sth.)
    - optional option: **silent (just a flag)** (if somebody wants to install sth. in the background without disturbance)

  The argument set for that is **install, name:^s, destination:^s, (silent)**
  
3. Add an event which will be raised if the passed args match with one argument set. The function must have the same signature as the *GetArguments* delegate:

    ```cs
    public delegate void GetArguments(ArgumentSetArgs args);
  
    ```
4. Pass the args which should be checked on matching to the argument sets. Therefore you need to call the *CheckArgs(...)* method of the *Validator*. There are 2 overloadings of. The first accepts an string array, the second a string. Valid strings could be:

    - **multiply 3.1519 15**
    - **remove someApp.exe someOtherApp.exe yetAnotherApp.exe --quiet**
    - **help -x -y -z**

  
  Here is the code for all three steps:
  
    ```cs
        //...
        
        //1. create the instance
        Validator validator = new Validator(false);
    
        //2. add the argument set
        validator.AddArgumentSet("install, name:^s, destination:^s, (silent)", Install);
        
        //4. create args and call CheckArgs(...)
        string args = "install appXYZ.exe --destination "C:\\folder with spaces" --silent";
        validator.CheckArgs(args);
        
        //3. declare a function
        void Install(GetArgumentArgs args)
        {
            //Do sth...
        }
    ```
    
  If you would run this programme, it would raise the Install function. Remember that the **silent** option could be removed because it's optional. Every thing you write in round brackets will be optional.
  
TODO: complete README, add documentation
