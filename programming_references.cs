// Author: Wyatt Murray
// Collaborators: Zebulon
// Date: 9/7/2023
// Description:  This document serves as a brief but extensive reference file for programmers,
//     encompassing a wide range of commands, syntax, and concepts in both C# and Unity.
//     It offers a comprehensive repository of valuable references for both new learners
//     and experienced developers.
// Version: v3.1


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Media;

#region 6 pillars of Object Oriented Programming (OOP)

    // 1. Abstraction: Simplify complex systems using classes and objects to model real-world entities and behaviors.

    // 2. Encapsulation: Bundle data and methods into classes, ensuring data integrity and access control.

    // 3. Inheritance: Create new classes based on existing ones, promoting code reuse and maintainability.

    // 4. Polymorphism: Treat objects of different classes as if they share a common base class, enhancing code flexibility.

    // 5. Composition: Compose complex objects from simpler ones, fostering modularity and reusability.

    // 6. Interface: Define contracts that classes must adhere to, enabling code extensibility and polymorphism.

    // These principles guide the design and implementation of C# programs, making them more organized and maintainable.
#endregion


#region Click the plus on the left of this message!

    //  this is a region, a way to quickly hide bulk sections of code when you are working so you dont scroll 2000 miles to find the method you wrote 3 minutes ago.
    // hovering your cursor over the plus icon and reveal the contents for a brief look
    //  this is a collaspable button!
#endregion


#region Accessing Private Variables in Unity Inspector:

// In Unity, use [SerializeField] to expose private variables in the Inspector.
// It lets you edit them without making them public, which follows the discipline of encasulation

[SerializeField] private int myVariable = 10;

// Now 'myVariable' is visible in the Inspector and can be adjusted there.
#endregion


#region Types of Access Modifiers:

// private:
// - Available only within the class or method it is declared in.
// - Variables and methods marked as private are not accessible from outside the class.
private int myPrivateVariable;

    // public:
    // - Publicly available to every script.
    // - Variables, methods, and classes marked as public are accessible from any other part of the code.
    public int myPublicVariable;

    // protected:
    // - Accessible within the class it is declared in and its subclasses.
    // - Variables and methods marked as protected can be used or modified by derived classes.
    protected int myProtectedVariable;

    // internal:
    // - Accessible within the same assembly (a compiled unit, e.g., a DLL).
    // - Variables, methods, and classes marked as internal are not accessible outside the assembly.
    internal int myInternalVariable;
#endregion


#region Getters and Setters (Properties):

// Declare a public variable 'variableA'.
public int variableA;

// Declare a public variable 'canSet' to control whether 'variableA' can be set.
public bool canSet = true;

// Use properties to control access to 'variableA'.
public int PropertyA
{
    // 'get' allows you to retrieve the value of 'variableA'.
    get { return variableA; }

    // 'set' allows you to modify 'variableA' with additional logic.
    set
    {
        // Check if 'canEdit' is true before modifying 'variableA'.
        if (canEdit)
        {
            variableA = value;
        }
        else
        {
            Debug.Log("The variable can't be edited.");
        }
    }
}
#endregion


#region Variables!

    // Variables are structured by access modifier, type, name, and then any properties youd like to set.
    // The order follows: AccessModifier Type name
    // one can apply [SerializeField] in front of any standard data type.
    // Any standard datatype can be uninitialized, or set to null.

    int myInteger; // Integers!
    double myDouble; // A float, but has double the precision.
    float myFloat; // A 32-bit floating-point number.
    bool myBoolean; // Boolean expression, can only be true or false.
    string myString; // An array of characters.
    char myChar; // A single ASCII character.
    object myObject; // A reference to any type (including user-defined types).
    null myNull; // Represents an invalid or empty value.
#endregion


#region Type Casting

    // Type Casting in C#:
    
    // Type casting converts data from one type to another.
    
    // Implicit Casting: Automatically converts smaller types to larger types.
    int num = 10;
    double decimalNum = num; // Implicit cast from int to double.
    
    // Explicit Casting: Manually converts larger types to smaller types.
    double bigDecimal = 15.75;
    int smallInt = (int)bigDecimal; // Explicit cast from double to int.
    
    // Casting with Reference Types: Allows treating objects as different types.
    class Animal { }
    class Dog : Animal { }
    
    Animal animal = new Dog();
    Dog dog = (Dog)animal; // Explicit cast for accessing Dog-specific members.
#endregion


#region Boolean Expressions and Operators:

// ? (Conditional Operator):
// The conditional operator (ternary operator) is a concise way to express an if-else statement.
// Syntax: (condition) ? trueExpression : falseExpression
bool isTrue = true;
    bool isFalse = false;
    bool result = (isTrue) ? trueExpression : falseExpression; // If isTrue is true, result is trueExpression; otherwise, it's falseExpression.

    // > (Greater Than):
    // Checks if the value on the left is greater than the value on the right.
    bool greaterResult = (5 > 3); // true, because 5 is greater than 3.

    // >= (Greater Than or Equal To):
    // Checks if the value on the left is greater than or equal to the value on the right.
    bool greaterOrEqualResult = (5 >= 5); // true, because 5 is equal to 5.

    // < (Less Than):
    // Checks if the value on the left is less than the value on the right.
    bool lessResult = (3 < 5); // true, because 3 is less than 5.

    // <= (Less Than or Equal To):
    // Checks if the value on the left is less than or equal to the value on the right.
    bool lessOrEqualResult = (3 <= 3); // true, because 3 is equal to 3.

    // == (Equality Operator):
    // Checks if the values on both sides are equal.
    bool equalResult = (5 == 5); // true, because 5 is equal to 5.
    #endregion


#region Creating Structs in C#:

    // A struct is a lightweight data structure in C# that groups together related variables.
    // Unlike classes, structs are value types and are typically used for small, immutable data.
    
    // Defining a Struct:
    // You can define a struct using the 'struct' keyword followed by the struct's name.
    // Inside the struct, you declare its fields, which are variables that hold data.
    public struct Player
    {
        // Fields for the Player struct.
        public string playerName;
        public int playerScore;
    }
    
    // Using a Struct:
    // You can create instances of a struct like any other data type.
    Player newPlayer = new Player();
    newPlayer.playerName = "John";
    newPlayer.playerScore = 100;
    
    // Accessing Struct Fields:
    string name = newPlayer.playerName; // Accessing the playerName field.
    int score = newPlayer.playerScore;  // Accessing the playerScore field.
    
    // Advantages of Structs:
    // - Structs are value types, which means they are stored on the stack and can be more memory-efficient.
    // - They are suitable for representing small, self-contained data.
    
    // System.Serializable Attribute:
    
    // The 'System.Serializable' attribute is used to mark classes and structs as serializable in C#.
    // Serialization is the process of converting objects or data structures into a format that can be easily stored or transmitted.
    
    // Marking a Struct as Serializable:
    // To make a struct serializable, you apply the [System.Serializable] attribute to it.
    [System.Serializable]
    public struct Item
    {
        public string itemName;
        public int itemID;
    }
    
    // Using Serializable Structs:
    // Once marked as serializable, you can use the struct in various contexts, such as saving and loading game data.
    
    // Example: Serialize and Deserialize Using JSON:
    // JSON (JavaScript Object Notation) is a common format for data serialization.
    
    // Serialization (Convert to JSON):
    Item myItem = new Item
    {
        itemName = "Health Potion",
        itemID = 1
    };
    
    string json = JsonUtility.ToJson(myItem);
    
    // Deserialization (Convert from JSON):
    Item deserializedItem = JsonUtility.FromJson<Item>(json);
#endregion


#region Arrays lists and dictionaries

    //Arrays, lists, and dictionaries can be initialised with the 'new' keyword, but must be initialised to use in the inspector

    //Array Syntax:

    // Declaring an array and allocating memory.
    int[] Array1;
    Array1 = new int[10]; // Creating an integer array of size 10.

    // A different way to declare and allocate an array in one line.
    int[] Array2 = new int[10];


    // List Syntax:

    // Declaring a generic List and creating an instance.
    List<dataType> List1;
    List1 = new List<dataType>(); // Creating an empty List.

    // Another way to declare and create a List in one line.
    List<dataType> List2 = new List<dataType>();


    // Dictionary Syntax:

    // Declaring a generic Dictionary without creating an instance.
    Dictionary<int, string> Dictionary1;


// Additional Context:
// Arrays, Lists, Dictionaries, and other data types are reference data types.
// They store data and have their own memory allocation.
// When you declare multiple variables of these types, each one gets its own memory space.
// Changes to one variable do not affect others, as they have different memory addresses.
#endregion


#region Loops:

    // For Loop Syntax:
    for (int counter = 0; counter< 10; counter++)
        {
            // Code to be executed repeatedly.
        }

    // While Loop Syntax:
    int whileLoop = 0;
    while (whileLoop< 10)
    {
        // Code to be executed while the condition is true.
        whileLoop++;
    }

    // Do-While Loop Syntax:
    do
    {
        // Code to be executed at least once, then repeated if the condition is true.
    } while (true);

    // Foreach Loop Syntax:
    int[] array1 = { 1, 2, 3, 4, 5 };
    foreach (int item in array1)
    {
        // Code to be applied to each element in the array.
    }
#endregion


#region Classes:

    // Class1 with a public field named 'name'.
    public class Class1
    {
        public string name = "WyattMurray";
    
        // Constructors can be defined to initialize objects.
        public Class1()
        {
            // Constructor logic can be added here.
        }
    }
    
    // Inheritance:
    
    // Class2 inherits from Class1.
    public class Class2 : Class1
    {
        // You can access the 'name' field from the base class (Class1) since it's public.
        public void SomeMethod()
        {
            Debug.Log(name); // Accesses 'name' from Class1.
        }
    }
    
    // Methods:
    
    // Private Method1:
    private void Method1()
    {
        // Private methods are only accessible within the declaring class.
        // They cannot be accessed from outside the class.
    }
    
    // Public Method2:
    public object Method2()
    {
        // Public methods are accessible from any part of the code.
        // 'object' is the return type, which means this method can return any object.
        return null; // You can return an object or value of the specified type.
    }
#endregion


#region Interfaces in C#:

    // An interface is a contract that defines a set of methods and properties that a class must implement.
    // It allows you to define a common set of behaviors that multiple classes can adhere to.
    
    // Defining an Interface:
    // You define an interface using the 'interface' keyword followed by the interface's name.
    public interface I_Find
    {
        // Interface members (methods or properties) are declared without implementation.
        void Find();
    }
    
    // Implementing an Interface:
    // To implement an interface, a class must provide concrete implementations of all the interface's members.
    public class FindMe : I_Find
    {
        public void Find()
        {
            // Implement the 'Find' method according to the interface's contract.
        }
    }
    
    // Using Interfaces with foreach:
    
    // You can use interfaces to find components or objects that implement a specific interface.
    
    // Example: Search for Objects with the 'I_Find' Interface:
    foreach (I_Find findableComponent in components)
    {
        // 'findableComponent' is an object that implements the 'I_Find' interface.
        // You can call the 'Find' method or access any other interface members here.
    }
#endregion


#region All miscellaneous stuff!

#region Reading and Writing Files in Unity:

    // In Unity, the "StreamingAssets" folder is a special folder that will be included in the build.
    // You can use it to read and write files that are bundled with your game.
    
    // Write Content to a File:
    
    void WriteToFile()
    {
        // Create a StreamWriter to write to a file in the StreamingAssets folder.
        StreamWriter newFile = new StreamWriter(Application.streamingAssetsPath + "/target.txt");
        // You can also use Application.persistentDataPath to store data that persists between sessions.
    
        // Write content to the file.
        newFile.WriteLine("Hello!");
    
        // Close the StreamWriter to save changes.
        newFile.Close();
    }
    
    // Read Content from a File:
    
    void ReadFromFile()
    {
        // Create a StreamReader to read from a file in the StreamingAssets folder.
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/target.txt");
        // You can also use Application.persistentDataPath to read data that persists between sessions.
    
        // Read lines from the file sequentially.
        string firstLine = reader.ReadLine();
        string secondLine = reader.ReadLine();
    
        // Close the StreamReader when you're done reading.
        reader.Close();
    }
    
    // Check if We've Reached the End of the File:
    
    bool IsEndOfFile()
    {
        // The StreamReader.EndOfStream property returns true if we've reached the end of the file.
        return reader.EndOfStream;
    }
#endregion


#region Error Handling in C# and Unity
    //  with Try-Catch Statements in C#:
    
    // Try-catch statements are used for handling exceptions (errors) in C#.
    // They ensure your program continues running even when unexpected issues occur.
    
    try
    {
        // Code that may cause an exception goes here.
        int result = 10 / 0; // Example: Division by zero will throw an exception.
    }
    catch (Exception ex)
    {
        // Catch the exception and handle it gracefully.
        Debug.LogError("An error occurred: " + ex.Message);
        // You can log an error message, notify the user, or take appropriate actions.
    }
    
    // Unity Debugging:
    
    // In Unity, the 'Debug' class provides methods for displaying messages in the console during runtime.
    
    // Debug.Log: Used for general logging and information.
    Debug.Log("This is a regular log message.");
    
    // Debug.LogWarning: Used for non-fatal warnings.
    Debug.LogWarning("This is a warning message.");
    // Warnings indicate potential issues but won't stop the program.
    
    // Debug.LogError: Used for critical errors.
    Debug.LogError("This is an error message.");
    // Errors indicate serious problems that should be addressed.
    
    // Debug.Assert: Used for conditional debugging.
    bool condition = false;
    Debug.Assert(condition, "This condition is false.");
    // Asserts help catch logical errors during development.
    
    // Debug.DrawLine and Debug.DrawRay: Used for visual debugging.
    Vector3 start = Vector3.zero;
    Vector3 end = Vector3.forward;
    Debug.DrawLine(start, end, Color.red);
// DrawLine and DrawRay visualize vectors and lines in the scene view.
#endregion


#region Reading Exceptions in C#:

// When an error occurs, exceptions provide vital information for debugging:

//1.Exception Message:
//-Use `ex.Message` to get a description of the error.

//2. Stack Trace:
//-Access `ex.StackTrace` for a detailed report of the call stack leading to the error.

//By examining these details, you can quickly identify and fix issues in your code.
#endregion


#region Creating File Menus in Unity:

// In Unity, you can use the [CreateAssetMenu] attribute to define custom file creation menus for ScriptableObjects.

// Example Usage:
[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon/RangedWeapon")]

// - 'fileName' specifies the default file name when creating an asset.
// - 'menuName' determines where the asset will appear in the "Create Asset" menu.
#endregion

#endregion
