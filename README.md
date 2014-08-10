cmdValidator
=========

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
