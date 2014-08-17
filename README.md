Argard
===

An validator/parser for commandline arguments in .NET programmes.

Important: The wiki is at the moment obsolete! It will be updated soon!
---

Example programme
---

This programme doesn't demonstrate every feature of Argard. But it will help you to get an idea for what this programme is good for.

```cs

//don't forget to reference the namespace
using Argard;
using System;

namespace DebugProgramme
{
    class Program
    {
        static void Main()
        {
            //the parser is the core of Argard. You can add parameter sets to it
            //which will trigger an event when they match.
            ParameterSetParser parser = new ParameterSetParser(false);

            //now we add the parameter sets

            //matches "helloworld" and triggers the helloWorld event
            //surrounding brackets [ and ] introduce an optional word
            //instead of "helloworld" you could also only write "hello"
            parser.AddParameterSet("hello[world]", helloWorld);

            //matches "hello -name string"
            //if the string has whitespaces you must qoute it like
            //the following: "string with whitespaces"
            parser.AddParameterSet("hello, name:^s", helloName);

            //matches "add string1 string2 string3...", ergo a list
            parser.AddParameterSet("add:^l", add);

            //matches "list -src string -dst string"
            //instead of src you could use source and for dst you could use destination
            //by the way, the order of the options doesn't matter ;)
            parser.AddParameterSet("list,src|source:^s,dst|destination:^s", list);

            //here we will set the to parsing arguments.
            string args = "add 5, 10, 25";

            //call the check args method to start the matching process
            //if it matches with a parameter set, it will trigger the respective event
            //otherwise it will return false and computes the if-statement
            if (parser.CheckArgs(args) == false)
                Console.WriteLine("Nothing matched. :(");
        }

        static void helloWorld(ParameterSetArgs args)
        {
            Console.WriteLine("Hello world!");
        }

        static void helloName(ParameterSetArgs args)
        {
            //print out the passed string
            Console.WriteLine("Hello {0}", args.Options["name"].Values.FirstValue);
        }

        static void add(ParameterSetArgs args)
        {
            try
            {
                int sum = 0;
                foreach (var x in args.CMD.Values.ParsedValues)
                    sum += Convert.ToInt32(x);
                
                Console.WriteLine("Sum: {0}", sum);
            }
            catch
            {
                Console.WriteLine("All arguments must be numbers!");
            }
        }

        static void list(ParameterSetArgs args)
        {
            Console.WriteLine("source: {0}",
                args.Options["src"].Values.FirstValue);

            //.Values.ParsedValues[0] equals .Values.FirstValue
            //just for clarifying :)
            Console.WriteLine("destination: {0}",
                args.Options["dst"].Values.ParsedValues[0]);
        }
    }
}
    
```
